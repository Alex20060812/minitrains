using System.Net.Sockets;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Z21;

namespace minitrains
{
    public partial class Form_vezetes : Form
    {
        
        private class TrainModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Address { get; set; }
            public int Speed { get; set; }
            public bool Direction { get; set; }
            public HashSet<int> ActiveFunctions { get; } = new HashSet<int>();
            public override string ToString() => Name;
        }

        
        private readonly Dictionary<TrainModel, int> trainDbMap = new Dictionary<TrainModel, int>();
        
        public int CurrentUserId { get; }
        public bool RememberMe { get; }
       

        // Paraméter nélküli konstruktor
        

        // Fõ konstruktor, inicializálja a formot és betölti a vonatokat
        public Form_vezetes(int userId, bool rememberMe)
        {
            InitializeComponent();
            CurrentUserId = userId;
            RememberMe = rememberMe;

            checkBox_login.Visible = RememberMe;
            checkBox_login.Checked = RememberMe;

            LoadUserTrains();

            if (comboBox_vonatvalasztas.Items.Count == 0)
            {
                comboBox_vonatvalasztas.DisplayMember = "Name";
                if (comboBox_vonatvalasztas.Items.Count > 0) comboBox_vonatvalasztas.SelectedIndex = 0;
                
            }

            
            
            
            BackgroundImage = Image.FromFile("..//..//Pictures//20250502-IMG_1315.jpg");
        }

        /// <summary>
        /// Funkciógomb állapotának átváltása a kiválasztott vonathoz.
        /// </summary>
        private void ToggleFunctionButton(Button btn, int idx)
        {
            var selTrain = SelectedTrain();
            if (selTrain == null) return;
            if (selTrain.ActiveFunctions.Contains(idx))
            {
                selTrain.ActiveFunctions.Remove(idx);
                UpdateButtonState(btn, false);
            }
            else
            {
                selTrain.ActiveFunctions.Add(idx);
                UpdateButtonState(btn, true);
            }
        }

        /// <summary>
        /// Betölti a felhasználóhoz tartozó vonatokat az adatbázisból.
        /// </summary>
        private void LoadUserTrains()
        {
            if (CurrentUserId <= 0) return;
            string connStr = "Server=localhost;Database=modellvasut;user=root;password=;";

            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    var cmd = new MySqlCommand(
                        "SELECT t.id,t.name,td.dcc_address FROM trains t LEFT JOIN train_details td ON td.train_id=t.id WHERE t.user_id=@uid",
                        conn);
                    cmd.Parameters.AddWithValue("@uid", CurrentUserId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        comboBox_vonatvalasztas.Items.Clear();
                        comboBox_vonatvalasztas.DisplayMember = "Name";
                        trainDbMap.Clear();

                        while (reader.Read())
                        {
                            int id = reader.GetInt32("id");
                            string name = reader.IsDBNull(reader.GetOrdinal("name")) ? "Unnamed" : reader.GetString("name");
                            int address = reader.IsDBNull(reader.GetOrdinal("dcc_address")) ? 0 : reader.GetInt32("dcc_address");

                            var t = new TrainModel { Id = id, Name = name, Address = address, Speed = 0, Direction = true };
                            comboBox_vonatvalasztas.Items.Add(t);
                            trainDbMap[t] = id;
                        }
                    }
                }

                if (comboBox_vonatvalasztas.Items.Count > 0)
                {
                    comboBox_vonatvalasztas.SelectedIndex = 0;
                    LoadFunctionsForTrain(SelectedTrain());
                }
            }
            catch
            {
                // DB hibák figyelmen kívül hagyása
            }
        }

        /// <summary>
        /// Betölti a kiválasztott vonathoz tartozó funkciókat az adatbázisból.
        /// </summary>
        private void LoadFunctionsForTrain(TrainModel sel)
        {
            if (sel == null) return;
            if (!trainDbMap.TryGetValue(sel, out int trainId))
                return;

            sel.ActiveFunctions.Clear();
            string connStr = "Server=localhost;Database=modellvasut;user=root;password=;";

            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    var cmd = new MySqlCommand(@"
                            SELECT f.id AS function_id, f.number, f.name AS default_name, f.hidden,
                                   fs.custom_name, fs.default_state
                            FROM functions f
                            LEFT JOIN functions_settings fs ON fs.function_id = f.id
                            WHERE f.train_id = @tid
                        ", conn);
                    cmd.Parameters.AddWithValue("@tid", trainId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        

                        while (reader.Read())
                        {
                            int number = reader.GetInt32("number");
                            string defaultName = reader.IsDBNull(reader.GetOrdinal("default_name")) ? null : reader.GetString("default_name");
                            string customName = reader.IsDBNull(reader.GetOrdinal("custom_name")) ? null : reader.GetString("custom_name");
                            bool hidden = !reader.IsDBNull(reader.GetOrdinal("hidden")) && reader.GetInt32("hidden") == 1;
                            bool defaultState = !reader.IsDBNull(reader.GetOrdinal("default_state")) && reader.GetInt32("default_state") == 1;

                            
                            

                            if (defaultState)
                            {
                                sel.ActiveFunctions.Add(number);
                            }
                        }

                        
                    }
                }
            }
            catch
            {
                // DB hibák figyelmen kívül hagyása
            }
        }

        /// <summary>
        /// Frissíti a sebesség kijelzõt a kiválasztott vonat alapján.
        /// </summary>
        private void UpdateSpeedLabel()
        {
            var sel = SelectedTrain();
            double sebesseg = Math.Round(sel.Speed * 4.1);
            label1.Text = "Sebesség: " + sebesseg.ToString() + " km/h";
        }

        /// <summary>
        /// Frissíti egy gomb színét az aktív állapot alapján.
        /// </summary>
        private void UpdateButtonState(Button btn, bool aktiv)
        {
            btn.BackColor = aktiv ? Color.DarkBlue : Color.DimGray;
        }

        /// <summary>
        /// Frissíti az összes funkciógomb állapotát a kiválasztott vonat alapján.
        /// </summary>
        

        /// <summary>
        /// Növeli a sebességet egy egységgel.
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value < trackBar1.Maximum) trackBar1.Value += 1;
            SelectedTrain().Speed = trackBar1.Value;
            UpdateSpeedLabel();
        }

        /// <summary>
        /// Visszaállítja a sebességet nullára.
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            SelectedTrain().Speed = 0;
            trackBar1.Value = 0;
            UpdateSpeedLabel();
        }

        /// <summary>
        /// Csökkenti a sebességet egy egységgel.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value > trackBar1.Minimum) trackBar1.Value -= 1;
            SelectedTrain().Speed = trackBar1.Value;
            UpdateSpeedLabel();
        }

        /// <summary>
        /// A sebesség csúszka mozgatására frissíti a sebességet.
        /// </summary>
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            SelectedTrain().Speed = trackBar1.Value;
            UpdateSpeedLabel();
        }

        /// <summary>
        /// Vonat kiválasztásakor frissíti a kijelzõket és betölti a funkciókat.
        /// </summary>
        private void comboBox_vonatvalasztas_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sel = SelectedTrain();
            if (sel == null) return;
            label3.Text = "Cím: " + sel.Address.ToString();
            label2.Text = "Irány: " + (sel.Direction ? "Elõre" : "Hátra");
            label4.Text = "Név: " + sel.Name;
            label1.Text = "Sebesség: " + Math.Round(sel.Speed * 4.1).ToString() + " km/h";
            trackBar1.Value = sel.Speed;

            LoadFunctionsForTrain(sel);
            
        }

        /// <summary>
        /// Irányváltás gomb: megfordítja a vonat irányát és nullázza a sebességet.
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 0;
            SelectedTrain().Direction = !SelectedTrain().Direction;
            label2.Text = "Irány: " + (SelectedTrain().Direction ? "Elõre" : "Hátra");
            label1.Text = "Sebesség: 0 km/h";
        }

        /// <summary>
        /// Új vonat hozzáadása az adatbázishoz és a felülethez.
        /// </summary>
        private void button_hozzaad_Click(object sender, EventArgs e)
        {
            AddNewTrain();
        }

        /// <summary>
        /// Új vonat hozzáadása logika külön metódusba szervezve.
        /// </summary>
        private void AddNewTrain()
        {
            using (var kisAblak = new Form_vonathozzaadas())
            {
                if (kisAblak.ShowDialog() != DialogResult.OK)
                    return;

                if (CurrentUserId <= 0)
                {
                    MessageBox.Show("No current user. Cannot create train in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                decimal szam = kisAblak.NumericErtek;
                string szoveg = kisAblak.TextErtek;
                string connStr = $"Server=localhost;Database=modellvasut;user=root;password=";

                try
                {
                    using (var conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        using (var tran = conn.BeginTransaction())
                        {
                            using (var cmd = new MySqlCommand("INSERT INTO trains (user_id, name) VALUES (@uid, @name)", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@uid", CurrentUserId);
                                cmd.Parameters.AddWithValue("@name", szoveg);
                                cmd.ExecuteNonQuery();
                            }

                            long newId;
                            using (var idCmd = new MySqlCommand("SELECT LAST_INSERT_ID()", conn, tran))
                            {
                                newId = Convert.ToInt64(idCmd.ExecuteScalar());
                            }

                            using (var cmdDet = new MySqlCommand("INSERT INTO train_details (train_id, dcc_address) VALUES (@tid, @addr)", conn, tran))
                            {
                                cmdDet.Parameters.AddWithValue("@tid", newId);
                                cmdDet.Parameters.AddWithValue("@addr", (int)szam);
                                cmdDet.ExecuteNonQuery();
                            }

                            using (var cmdFunc = new MySqlCommand("", conn, tran))
                            {
                                for (int i = 0; i <= 28; i++)
                                {
                                    cmdFunc.CommandText = "INSERT INTO functions (train_id, number, name, icon, hidden) VALUES (@tid,@num,@fname,NULL,0)";
                                    cmdFunc.Parameters.Clear();
                                    cmdFunc.Parameters.AddWithValue("@tid", newId);
                                    cmdFunc.Parameters.AddWithValue("@num", i);
                                    cmdFunc.Parameters.AddWithValue("@fname", "f" + i);
                                    cmdFunc.ExecuteNonQuery();
                                }
                            }

                            using (var cmdFs = new MySqlCommand("", conn, tran))
                            {
                                for (int i = 0; i <= 28; i++)
                                {
                                    cmdFs.CommandText = @"SELECT id FROM functions WHERE train_id=@tid AND number=@num LIMIT 1";
                                    cmdFs.Parameters.Clear();
                                    cmdFs.Parameters.AddWithValue("@tid", newId);
                                    cmdFs.Parameters.AddWithValue("@num", i);
                                    var fidObj = cmdFs.ExecuteScalar();
                                    if (fidObj != null && fidObj != DBNull.Value)
                                    {
                                        long fid = Convert.ToInt64(fidObj);
                                        using (var up = new MySqlCommand("INSERT INTO functions_settings (function_id, custom_name, default_state) VALUES (@fid, NULL, 0)", conn, tran))
                                        {
                                            up.Parameters.AddWithValue("@fid", fid);
                                            up.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }

                            tran.Commit();

                            var newTrain = new TrainModel { Id = (int)newId, Name = szoveg, Address = (int)szam, Speed = 0, Direction = true };
                            comboBox_vonatvalasztas.Items.Add(newTrain);
                            trainDbMap[newTrain] = (int)newId;
                            comboBox_vonatvalasztas.SelectedItem = newTrain;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not create train: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Funkciók szerkesztése gomb: megnyitja a szerkesztõ ablakot.
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {
            var sel = SelectedTrain();
            if (sel == null) return;
            if (!trainDbMap.TryGetValue(sel, out int trainId))
            {
                MessageBox.Show("This train has no database record to edit.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var editor = new Form_function_editor(trainId))
            {
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    LoadFunctionsForTrain(sel);
                }
            }
        }

        /// <summary>
        /// "Remember me" checkbox változásának kezelése.
        /// </summary>
        private void checkBox_login_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox_login.Checked)
            {
                checkBox_login.Visible = false;
                const string rememberFile = "remember.dat";
                try
                {
                    if (File.Exists(rememberFile))
                        File.Delete(rememberFile);
                }
                catch { }

                try
                {
                    using (var conn = new MySqlConnection("Server=localhost;Database=modellvasut;user=root;password=;"))
                    {
                        conn.Open();
                        var cmd = new MySqlCommand("UPDATE users SET remember_token = NULL WHERE id = @id", conn);
                        cmd.Parameters.AddWithValue("@id", CurrentUserId);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Visszaadja a kiválasztott vonatot, vagy null-t ha nincs.
        /// </summary>
        private TrainModel SelectedTrain()
        {
            if (comboBox_vonatvalasztas?.SelectedItem is TrainModel sel)
                return sel;

            for (int i = 0; comboBox_vonatvalasztas != null && i < comboBox_vonatvalasztas.Items.Count; i++)
            {
                if (comboBox_vonatvalasztas.Items[i] is TrainModel first)
                {
                    comboBox_vonatvalasztas.SelectedIndex = i;
                    return first;
                }
            }
            return null;
        }
        
    }
}
