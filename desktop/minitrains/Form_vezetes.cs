using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.ApplicationServices;
using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;



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
        private Z21Udp z21 = new Z21Udp();
        private Button[] functionButtons;


        public Form_vezetes(int userId, bool rememberMe)
        {

            InitializeComponent();

            CurrentUserId = userId;

            RememberMe = rememberMe;

            try
            {


                pictureBox1.Image = Image.FromFile("..//..//..//Pictures//play.png");



                z21.OnTrackPowerChanged += state =>
                {
                    if (state)
                        pictureBox1.Image = Image.FromFile("..//..//..//Pictures//play.png");
                    else
                        pictureBox1.Image = Image.FromFile("..//..//..//Pictures//pause.png");
                };

            }
            catch (Exception ex)
            {
                MessageBox.Show("Form_vezetes ctor hiba: " + ex.Message);
                throw;
            }
            try
            {

            }
            catch (Exception ex)
            {

                MessageBox.Show("hiba" + ex.Message);
            }


            

        }


        private void Form_vezetes_Load(object? sender, EventArgs e)
        {


            try
            {

                z21.Connect(GlobalConfig.Z21IP, int.Parse(GlobalConfig.Z21Port));

            }
            catch (Exception ex)
            {

                MessageBox.Show($"hiba: {ex.Message}");
            }

            try
            {
                checkBox_login.Visible = RememberMe;
                checkBox_login.Checked = RememberMe;

                LoadUserTrains();

                if (comboBox_vonatvalasztas.Items.Count == 0)
                {
                    comboBox_vonatvalasztas.DisplayMember = "Name";
                    if (comboBox_vonatvalasztas.Items.Count > 0)
                        comboBox_vonatvalasztas.SelectedIndex = 0;
                }

                try
                {
                    BackgroundImage = Image.FromFile("..//..//..//Pictures//20250502-IMG_1315.jpg");
                }
                catch { MessageBox.Show("Hiba a háttér betöltése közben!"); }




            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        private void ToggleFunction(int fnNumber)
        {
            var sel = SelectedTrain();
            if (sel == null) return;
            if (fnNumber < 0 || fnNumber >= functionButtons.Length) return;

            bool isActive = sel.ActiveFunctions.Contains(fnNumber);
            if (isActive)
            {
                z21.SetFunction(SelectedTrain().Address, fnNumber, false);
                sel.ActiveFunctions.Remove(fnNumber);
            }

            else
            {
                z21.SetFunction(SelectedTrain().Address, fnNumber, true);
                sel.ActiveFunctions.Add(fnNumber);
            }



            var btn = functionButtons[fnNumber];
            btn.BackColor = isActive ? Color.Gray : Color.Yellow;


        }


        /// <summary>
        /// Betölti a felhasználóhoz tartozó vonatokat az adatbázisból.
        /// </summary>
        private async void LoadUserTrains()
        {
            if (CurrentUserId <= 0) return;
            string connStr = GlobalConfig.GetConnectionString();

            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    await conn.OpenAsync();
                    var cmd = new MySqlCommand(
                        "SELECT t.id,t.name,td.dcc_address FROM trains t " +
                        "LEFT JOIN train_details td ON td.train_id=t.id " +
                        "WHERE t.user_id=@uid",
                        conn);
                    cmd.Parameters.AddWithValue("@uid", CurrentUserId);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        comboBox_vonatvalasztas.Items.Clear();
                        comboBox_vonatvalasztas.DisplayMember = "Name";
                        trainDbMap.Clear();

                        while (await reader.ReadAsync())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("id"));
                            string name = reader.IsDBNull(reader.GetOrdinal("name"))
                                ? "Unnamed"
                                : reader.GetString(reader.GetOrdinal("name"));
                            int address = reader.IsDBNull(reader.GetOrdinal("dcc_address"))
                                ? 0
                                : reader.GetInt32(reader.GetOrdinal("dcc_address"));

                            var t = new TrainModel
                            {
                                Id = id,
                                Name = name,
                                Address = address,
                                Speed = 0,
                                Direction = true
                            };

                            comboBox_vonatvalasztas.Items.Add(t);
                            trainDbMap[t] = id;
                        }
                    }
                }



                //minden vonatra betöltjük a funkciókat DB-ből
                foreach (var kvp in trainDbMap)
                {
                    var train = kvp.Key;
                    LoadFunctionsForTrain(train);
                }

                //minden vonatra bekapcsoljuk az alapértelmezett funkciókat
                TurnOnDefaultFunctionsForAllTrains();
            }
            catch
            {
                // DB hibák figyelmen kívül hagyása
            }
        }
        /// <summary>
        /// Alkalmazás indításakor minden mozdonyon bekapcsolja
        /// az alapértelmezetten bekapcsolt funkciókat (ActiveFunctions).
        /// </summary>
        private void TurnOnDefaultFunctionsForAllTrains()
        {
            if (z21 == null)
                return;

            foreach (var kvp in trainDbMap)
            {
                var train = kvp.Key;
                if (train.Address <= 0) continue;

                // Végignézzük az összes lehetséges funkciót (0-28)
                for (int fn = 0; fn <= 28; fn++)
                {
                    bool isActive = train.ActiveFunctions.Contains(fn);

                    // Elküldjük a z21-nek az összes funkció állapotát
                    z21.SetFunction(train.Address, fn, isActive);

                    // ha a kijelölt vonat, és van hozzá gomb, állítsuk a gomb színét is
                    if (SelectedTrain() == train && functionButtons != null &&
                        fn >= 0 && fn < functionButtons.Length)
                    {
                        var btn = functionButtons[fn];
                        if (btn != null)
                            btn.BackColor = isActive ? Color.Yellow : Color.Gray;
                    }
                }
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
            string connStr = GlobalConfig.GetConnectionString();


            if (functionButtons == null)
            {
                functionButtons = new[]
                {
                    button_F0, button_F1, button_F2, button_F3, button_F4,
                    button_F5, button_F6, button_F7, button_F8, button_F9,
                    button_F10, button_F11, button_F12, button_F13, button_F14,
                    button_F15, button_F16, button_F17, button_F18, button_F19,
                    button_F20, button_F21, button_F22, button_F23, button_F24,
                    button_F25, button_F26, button_F27, button_F28
                };
            }


            for (int i = 0; i < functionButtons.Length; i++)
            {
                var btn = functionButtons[i];
                if (btn == null) continue;

                btn.Visible = true;
                btn.Text = "F" + i;
                btn.Image = null;
                btn.BackColor = Color.Gray;
            }

            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    var cmd = new MySqlCommand(@"
                            SELECT f.id AS function_id,
                                   f.number,
                                   f.name AS default_name,
                                   f.hidden,
                                   f.icon,
                                   fs.custom_name,
                                   fs.default_state
                            FROM functions f
                            LEFT JOIN functions_settings fs ON fs.function_id = f.id
                            WHERE f.train_id = @tid
                            ORDER BY f.number", conn);
                    cmd.Parameters.AddWithValue("@tid", trainId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int number = reader.GetInt32(reader.GetOrdinal("number"));
                            if (number < 0 || number >= functionButtons.Length)
                                continue;

                            string defaultName = reader.IsDBNull(reader.GetOrdinal("default_name"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("default_name"));
                            string customName = reader.IsDBNull(reader.GetOrdinal("custom_name"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("custom_name"));
                            bool hidden = !reader.IsDBNull(reader.GetOrdinal("hidden")) &&
                                          reader.GetInt32(reader.GetOrdinal("hidden")) == 1;
                            bool defaultState = !reader.IsDBNull(reader.GetOrdinal("default_state")) &&
                                                reader.GetInt32(reader.GetOrdinal("default_state")) == 1;
                            string iconFile = reader.IsDBNull(reader.GetOrdinal("icon"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("icon"));

                            var btn = functionButtons[number];


                            btn.Visible = !hidden;


                            string nameToShow = !string.IsNullOrEmpty(customName)
                                ? customName
                                : (!string.IsNullOrEmpty(defaultName) ? defaultName : "F" + number);
                            btn.Text = nameToShow;

                            // ikon (ugyanaz a mappa, mint az editorban)
                            if (!string.IsNullOrEmpty(iconFile))
                            {
                                string path = Path.Combine(Application.StartupPath, "icons", iconFile);
                                if (File.Exists(path))
                                {
                                    try
                                    {
                                        using (var img = Image.FromFile(path))
                                        {
                                            btn.Image = new Bitmap(img);

                                        }

                                        btn.ImageAlign = ContentAlignment.TopCenter;
                                        btn.TextAlign = ContentAlignment.BottomCenter;
                                    }
                                    catch
                                    {
                                        btn.Image = null;
                                    }
                                }
                                else
                                {
                                    btn.Image = null;
                                }
                            }
                            else
                            {
                                btn.Image = null;
                            }

                            // alapállapot → ActiveFunctions + háttérszín
                            if (defaultState)
                            {
                                sel.ActiveFunctions.Add(number);
                                btn.BackColor = Color.Yellow; // vagy amit szeretnél "ON" állapotnak
                            }
                            else
                            {
                                btn.BackColor = Color.Gray;
                            }
                        }
                    }
                }



            }
            catch
            {

            }
        }

        /// <summary>
        /// Frissíti a sebesség kijelzőt a kiválasztott vonat alapján.
        /// </summary>
        private void UpdateSpeedLabel()
        {
            var sel = SelectedTrain();
            double sebesseg = Math.Round(sel.Speed * 4.1);
            label1.Text = "Sebesség: " + sebesseg.ToString() + " km/h";
        }

        /// <summary>
        /// Növeli a sebességet egy egységgel.
        /// </summary>
        private void button_plus_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value < trackBar1.Maximum) trackBar1.Value += 1;
            SelectedTrain().Speed = trackBar1.Value;
            UpdateSpeedLabel();
            z21.SetLocoDrive(SelectedTrain().Address, SelectedTrain().Speed, SelectedTrain().Direction);
        }

        /// <summary>
        /// Visszaállítja a sebességet nullára.
        /// </summary>
        private void button_stop_Click(object sender, EventArgs e)
        {
            SelectedTrain().Speed = 0;
            trackBar1.Value = 0;
            UpdateSpeedLabel();
            z21.SetLocoDrive(SelectedTrain().Address, 0, SelectedTrain().Direction);
        }

        /// <summary>
        /// Csökkenti a sebességet egy egységgel.
        /// </summary>
        private void button_minus_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value > trackBar1.Minimum) trackBar1.Value -= 1;
            SelectedTrain().Speed = trackBar1.Value;
            UpdateSpeedLabel();
            z21.SetLocoDrive(SelectedTrain().Address, SelectedTrain().Speed, SelectedTrain().Direction);
        }

        /// <summary>
        /// A sebesség csúszka mozgatására frissíti a sebességet.
        /// </summary>
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            SelectedTrain().Speed = trackBar1.Value;
            UpdateSpeedLabel();
            z21.SetLocoDrive(SelectedTrain().Address, SelectedTrain().Speed, SelectedTrain().Direction);
        }

        /// <summary>
        /// Vonat kiválasztásakor frissíti a kijelzőket és betölti a funkciókat.
        /// </summary>


        private void comboBox_vonatvalasztas_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // ide jön, amit csinálni akarsz
            var sel = SelectedTrain();
            if (sel == null) return;
            label3.Text = "Cím: " + sel.Address.ToString();
            label2.Text = "Irány: " + (sel.Direction ? "Előre" : "Hátra");
            label4.Text = "Név: " + sel.Name;
            label1.Text = "Sebesség: " + Math.Round(sel.Speed * 4.1).ToString() + " km/h";
            trackBar1.Value = sel.Speed;

            LoadFunctionsForTrain(sel);
        }

        /// <summary>
        /// Irányváltás gomb: megfordítja a vonat irányát és nullázza a sebességet.
        /// </summary>
        private void button_direction_Click(object sender, EventArgs e)
        {
            bool vesz = false;
            if (trackBar1.Value > 0)
            {
                vesz = true;

            }
            if (vesz)
            {
                z21.EmergencyStopLoco(SelectedTrain().Address, SelectedTrain().Direction);
                trackBar1.Value = 0;
            }
            else
            {
                SelectedTrain().Direction = !SelectedTrain().Direction;

                label2.Text = "Irány: " + (SelectedTrain().Direction ? "Előre" : "Hátra");
                label1.Text = "Sebesség: 0 km/h";

                z21.SetLocoDrive(SelectedTrain().Address, 0, SelectedTrain().Direction);
            }
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
                string connStr = GlobalConfig.GetConnectionString();

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
                                    cmdFunc.Parameters.AddWithValue("@fname", "F" + i);
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
        /// Funkciók szerkesztése gomb: megnyitja a szerkesztő ablakot.
        /// </summary>
        private void button_functions_Click(object sender, EventArgs e)
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
                    sel.Name = editor.UpdatedTrainName;
                    sel.Address = editor.UpdatedTrainAddress;

                    // ComboBox frissítése, hogy a megváltozott név látszódjon
                    int idx = comboBox_vonatvalasztas.SelectedIndex;
                    comboBox_vonatvalasztas.Items[idx] = sel;

                    // Labelek frissítése
                    label4.Text = "Név: " + sel.Name;
                    label3.Text = "Cím: " + sel.Address.ToString();

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
                    using (var conn = new MySqlConnection(GlobalConfig.GetConnectionString()))
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
        private TrainModel? SelectedTrain()
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


        public bool be = true;
        private void pictureBox1_Click(object sender, EventArgs e)
        {

            if (be)
            {
                z21.TrackPowerOff();
                be = false;
            }
            else
            {
                z21.TrackPowerOn();
                be = true;
            }

        }


        // =============================
        // ====== FUNKCIÓGOMBOK ========
        // =============================


        private void button_F0_Click(object sender, EventArgs e)
        {

            ToggleFunction(0);

        }

        private void button_F1_Click(object sender, EventArgs e)
        {
            ToggleFunction(1);
        }

        private void button_F2_Click(object sender, EventArgs e)
        {
            ToggleFunction(2);
        }

        private void button_F3_Click(object sender, EventArgs e)
        {
            ToggleFunction(3);
        }

        private void button_F4_Click(object sender, EventArgs e)
        {
            ToggleFunction(4);
        }

        private void button_F5_Click(object sender, EventArgs e)
        {
            ToggleFunction(5);
        }

        private void button_F6_Click(object sender, EventArgs e)
        {
            ToggleFunction(6);
        }

        private void button_F7_Click(object sender, EventArgs e)
        {
            ToggleFunction(7);
        }
        private void button_F7_Click_1(object sender, EventArgs e)
        {
            ToggleFunction(7);
        }

        private void button_F8_Click(object sender, EventArgs e)
        {
            ToggleFunction(8);
        }

        private void button_F9_Click(object sender, EventArgs e)
        {
            ToggleFunction(9);
        }

        private void button_F10_Click(object sender, EventArgs e)
        {
            ToggleFunction(10);
        }

        private void button_F11_Click(object sender, EventArgs e)
        {
            ToggleFunction(11);
        }

        private void button_F12_Click(object sender, EventArgs e)
        {
            ToggleFunction(12);
        }

        private void button_F13_Click(object sender, EventArgs e)
        {
            ToggleFunction(13);
        }

        private void button_F14_Click(object sender, EventArgs e)
        {
            ToggleFunction(14);
        }

        private void button_F15_Click(object sender, EventArgs e)
        {
            ToggleFunction(15);
        }

        private void button_F16_Click(object sender, EventArgs e)
        {
            ToggleFunction(16);
        }

        private void button_F17_Click(object sender, EventArgs e)
        {
            ToggleFunction(17);
        }

        private void button_F18_Click(object sender, EventArgs e)
        {
            ToggleFunction(18);
        }

        private void button_F19_Click(object sender, EventArgs e)
        {
            ToggleFunction(19);
        }

        private void button_F20_Click(object sender, EventArgs e)
        {
            ToggleFunction(20);
        }

        private void button_F21_Click(object sender, EventArgs e)
        {
            ToggleFunction(21);
        }

        private void button_F22_Click(object sender, EventArgs e)
        {
            ToggleFunction(22);
        }

        private void button_F23_Click(object sender, EventArgs e)
        {
            ToggleFunction(23);
        }

        private void button_F24_Click(object sender, EventArgs e)
        {
            ToggleFunction(24);
        }

        private void button_F25_Click(object sender, EventArgs e)
        {
            ToggleFunction(25);
        }

        private void button_F26_Click(object sender, EventArgs e)
        {
            ToggleFunction(26);
        }

        private void button_F27_Click(object sender, EventArgs e)
        {
            ToggleFunction(27);
        }

        private void button_F28_Click(object sender, EventArgs e)
        {
            ToggleFunction(28);
        }

        private void button_DELTrain_Click(object sender, EventArgs e)
        {
            var sel = SelectedTrain();
            if (sel == null) return;

            if (!trainDbMap.TryGetValue(sel, out int trainId))
                return;

            var confirmResult = MessageBox.Show(
                $"Biztosan törölni szeretnéd a(z) '{sel.Name}' nevű vonatot minden adatával együtt?",
                "Törlés megerősítése",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    using (var conn = new MySqlConnection(GlobalConfig.GetConnectionString()))
                    {
                        conn.Open();
                        using (var tran = conn.BeginTransaction())
                        {
                            // 1. functions_settings törlése (ami a functions-re hivatkozik)
                            using (var cmd = new MySqlCommand("DELETE FROM functions_settings WHERE function_id IN (SELECT id FROM functions WHERE train_id = @tid)", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@tid", trainId);
                                cmd.ExecuteNonQuery();
                            }

                            // 2. functions törlése
                            using (var cmd = new MySqlCommand("DELETE FROM functions WHERE train_id = @tid", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@tid", trainId);
                                cmd.ExecuteNonQuery();
                            }

                            // 3. train_details törlése
                            using (var cmd = new MySqlCommand("DELETE FROM train_details WHERE train_id = @tid", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@tid", trainId);
                                cmd.ExecuteNonQuery();
                            }

                            // 4. maga a vonat törlése
                            using (var cmd = new MySqlCommand("DELETE FROM trains WHERE id = @tid", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@tid", trainId);
                                cmd.ExecuteNonQuery();
                            }

                            tran.Commit();
                        }
                    }

                    // Eltávolítás a felületről és a memóriából
                    trainDbMap.Remove(sel);
                    comboBox_vonatvalasztas.Items.Remove(sel);

                    if (comboBox_vonatvalasztas.Items.Count > 0)
                    {
                        comboBox_vonatvalasztas.SelectedIndex = 0;
                    }
                    else
                    {
                        // Ha nincs több vonat, kiürítjük a UI-t
                        label3.Text = "Cím: ";
                        label2.Text = "Irány: ";
                        label4.Text = "Név: ";
                        label1.Text = "Sebesség: ";
                        trackBar1.Value = 0;
                        if (functionButtons != null)
                        {
                            foreach (var btn in functionButtons) 
                            {
                                if (btn != null) btn.Visible = false;
                            }
                        }
                    }

                    MessageBox.Show("A vonat sikeresen törölve lett.", "Törlés", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba a vonat törlése közben: " + ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
