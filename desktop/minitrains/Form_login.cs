using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace minitrains
{
    public partial class Form_login : Form
    {
        public int CurrentUserId { get; private set; }
        public bool RememberMe { get; private set; }

        private const string RememberFile = "remember.dat";

        public Form_login()
        {
            InitializeComponent();
            textBox_Port.Text = GlobalConfig.DbPort;
            this.AcceptButton = button2;
            this.Shown += Form_login_Shown;

            try
            {
                BackgroundImage = Image.FromFile("..//..//..//Pictures//20250502-IMG_1315.jpg");
            }
            catch
            {
                // opcionálisan hagyd üresen, ha a kép hiányzik
            }
        }

        private void Form_login_Shown(object sender, EventArgs e)
        {
            if (!File.Exists(RememberFile))
                return;

            TryAutoLogin();
        }

        // ----------------- JELSZÓ HASH / VERIFY -----------------
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

        public static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            int diff = a.Length ^ b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }

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

        // ----------------- REMEMBER TOKEN -----------------
        private static string GenerateToken()
        {
            byte[] token = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(token);

            return Convert.ToBase64String(token);
        }

        private void SaveRememberFile(string username, string token)
        {
            File.WriteAllLines(RememberFile, new[] { username, token });
        }

        private bool TryAutoLogin()
        {
            try
            {
                string[] lines = File.ReadAllLines(RememberFile);
                if (lines.Length != 2) return false;

                string savedUsername = lines[0];
                string savedToken = lines[1];

                using (var conn = new MySqlConnection(GlobalConfig.GetConnectionString()))
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

                    CurrentUserId = Convert.ToInt32(dbId);
                    
                    RememberMe = true;

                    DialogResult = DialogResult.OK;
                    Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // ----------------- BEJELENTKEZÉS GOMB (button2) -----------------
        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            string port = textBox_Port.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(port))
                return;

            GlobalConfig.Save(port);

            try
            {
                using (var conn = new MySqlConnection(GlobalConfig.GetConnectionString()))
                {
                    conn.Open();

                    var cmd = new MySqlCommand(
                        "SELECT id, password_hash FROM users WHERE username=@u",
                        conn
                    );
                    cmd.Parameters.AddWithValue("@u", username);

                    int userId;
                    string storedHash;

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return;

                        userId = reader.GetInt32("id");
                        storedHash = reader.GetString("password_hash");

                        if (!VerifyPassword(password, storedHash))
                            return;
                    }

                    if (checkBox_RememberMe.Checked)
                    {
                        string newToken = GenerateToken();
                        var cmd2 = new MySqlCommand(
                            "UPDATE users SET remember_token=@t WHERE username=@u",
                            conn
                        );
                        cmd2.Parameters.AddWithValue("@t", newToken);
                        cmd2.Parameters.AddWithValue("@u", username);
                        cmd2.ExecuteNonQuery();

                        SaveRememberFile(username, newToken);
                    }
                    else
                    {
                        var cmd2 = new MySqlCommand(
                            "UPDATE users SET remember_token=NULL WHERE username=@u",
                            conn
                        );
                        cmd2.Parameters.AddWithValue("@u", username);
                        cmd2.ExecuteNonQuery();

                        if (File.Exists(RememberFile))
                            File.Delete(RememberFile);
                    }

                    CurrentUserId = userId;
                    RememberMe = checkBox_RememberMe.Checked;

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Login error");
            }
        }
    }
}
