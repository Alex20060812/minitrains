using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml.Linq;

namespace minitrains
{
    public partial class Form_login : Form
    {
        private const string RememberFile = "remember.dat";
        
        public int LoggedInUserId { get; private set; }
        public bool RememberMeChecked { get; private set; }

        public int port;
        public Form_login()
        {
            InitializeComponent();
            
        }

        private void Form_login_Shown(object sender, EventArgs e)
        {
            // If auto-login succeeds, set DialogResult and close the dialog.
            if (TryAutoLogin())
            {
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

       

        // ------------------------------- JELSZÓ HASH -------------------------------
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
            }
        }

        // -------------------- FIXED TIME EQUALS (saját verzió) ---------------------
        public static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            int diff = a.Length ^ b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }

        // ------------------------------ VERIFY PASSWORD ----------------------------
        public static bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 2) return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] storedHashBytes = Convert.FromBase64String(parts[1]);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256))
            {
                byte[] computedHash = pbkdf2.GetBytes(32);
                return FixedTimeEquals(storedHashBytes, computedHash);
            }
        }

        // ----------------------- TOKEN GENERÁLÁS LOGIN-HEZ -------------------------
        private static string GenerateToken()
        {
            byte[] token = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(token);

            return Convert.ToBase64String(token);
        }

        // ----------------------------- TOKEN MENTÉSE -------------------------------
        private void SaveRememberFile(string username, string token)
        {
            File.WriteAllLines(RememberFile, new[] { username, token });
        }

        // ----------------------------- TOKEN OLVASÁSA ------------------------------
        private bool TryAutoLogin()
        {
            if (!File.Exists(RememberFile))
                return false;

            string[] lines = File.ReadAllLines(RememberFile);
            if (lines.Length != 2) return false;

            string savedUsername = lines[0];
            string savedToken = lines[1];

            using (var conn = new MySqlConnection($"Server=localhost;Database=modellvasut;user=root;password=;Port={port}"))
            {
                conn.Open();

                var cmd = new MySqlCommand(
                    "SELECT id FROM users WHERE username=@u AND remember_token=@t",
                    conn
                );
                cmd.Parameters.AddWithValue("@u", savedUsername);
                cmd.Parameters.AddWithValue("@t", savedToken);

                var dbId = cmd.ExecuteScalar();
                if (dbId == null)
                    return false;

                // Sikeres automatikus belépés!
                this.LoggedInUserId = Convert.ToInt32(dbId);
                this.RememberMeChecked = true;
                

                this.DialogResult = DialogResult.OK;
                this.Close();
                return true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            button3.Show(); textBox1.Show(); textBox2.Show(); label1.Show(); label2.Show(); checkBoxRememberMe.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            port = (int)numericUpDown1.Value;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Nem lehet üres!");
                return;
            }

            using (var conn = new MySqlConnection($"Server=localhost;Database=modellvasut;user=root;password=;Port={port}"))
            {
                conn.Open();

                
                          // --------------------- LOGIN -------------------------
                {
                    var cmd = new MySqlCommand(
                        "SELECT id, password_hash, remember_token FROM users WHERE username=@u",
                        conn
                    );
                    cmd.Parameters.AddWithValue("@u", username);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            MessageBox.Show("Hibás felhasználónév vagy jelszó!");
                            return;
                        }

                        int userId = reader.GetInt32("id");
                        string storedHash = reader.GetString("password_hash");

                        string storedToken = reader.IsDBNull(reader.GetOrdinal("remember_token"))
                                ? null
                                : reader.GetString("remember_token");

                        if (!VerifyPassword(password, storedHash))
                        {
                            MessageBox.Show("Hibás felhasználónév vagy jelszó!");
                            return;
                        }

                        // Jelszó helyes → új token
                        string newToken = GenerateToken();
                        reader.Close();

                        var cmd2 = new MySqlCommand(
                            "UPDATE users SET remember_token=@t WHERE username=@u",
                            conn
                        );
                        cmd2.Parameters.AddWithValue("@t", newToken);
                        cmd2.Parameters.AddWithValue("@u", username);
                        cmd2.ExecuteNonQuery();

                        if (checkBoxRememberMe.Checked)
                            SaveRememberFile(username, newToken);

                        // 🔥 Itt tároljuk a felhasználói ID-t a fő programnak!
                        this.LoggedInUserId = userId;
                        this.RememberMeChecked = checkBoxRememberMe.Checked;

                        MessageBox.Show("Sikeres bejelentkezés!");

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }

            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(this.DialogResult != DialogResult.OK)
            {
                Application.Exit();
            }
        }
    }
}
