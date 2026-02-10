using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace minitrains
{
        public partial class Form_vezetes : Form
    {
        // internal DB-backed train model (different name than the old 'train' class)
        private class TrainModel
        {
            public int Id { get; set; } // DB id (0 = not persistent)
            public string Name { get; set; }
            public int Address { get; set; }
            public int Speed { get; set; }
            public bool Direction { get; set; }
            public HashSet<int> ActiveFunctions { get; } = new HashSet<int>();
            public override string ToString() => Name;
        }

        

        private readonly List<Button> functionButtons = new List<Button>();
        private readonly Dictionary<TrainModel, int> trainDbMap = new Dictionary<TrainModel, int>();

        // Parameterless constructor added to allow calls that supply no arguments.
        public Form_vezetes() : this(0, false)
        {
        }

        public Form_vezetes(int userId, bool rememberMe)
        {
            InitializeComponent();
            CurrentUserId = userId;
            RememberMe = rememberMe;

            // show the remember checkbox only if the user previously chose "Remember me"
            checkBox1.Visible = RememberMe;
            checkBox1.Checked = RememberMe;

            // create UI buttons
            for (int i = 0; i < 29; i++)
            {
                int idx = i;
                Button btn = new Button
                {
                    Text = $"F{idx}",
                    Size = new Size(Math.Max(24, flowLayoutPanel1.Width / 7), Math.Max(40, flowLayoutPanel1.Height / 6)),
                    Name = idx.ToString(),
                    Tag = idx,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.DimGray,
                    ForeColor = Color.White,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    ImageAlign = System.Drawing.ContentAlignment.MiddleCenter
                };

                btn.Click += (s, e) =>
                {
                    var selTrain = SelectedTrain();
                    if (selTrain == null) return;
                    if (selTrain.ActiveFunctions.Contains(idx))
                    {
                        selTrain.ActiveFunctions.Remove(idx);
                        gombfrissites(btn, false);
                    }
                    else
                    {
                        selTrain.ActiveFunctions.Add(idx);
                        gombfrissites(btn, true);
                    }
                };

                flowLayoutPanel1.Controls.Add(btn);
                functionButtons.Add(btn);
            }

            // load trains and functions from DB for the current user
            LoadUserTrains();

            // fallback if no trains found
            if (comboBox1.Items.Count == 0)
            {
               

                comboBox1.DisplayMember = "Name";

                if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;

                RefreshFunctionButtons(SelectedTrain());
            }
        }

        public int CurrentUserId { get; }
        public bool RememberMe { get; }

        

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
                        comboBox1.Items.Clear();
                        comboBox1.DisplayMember = "Name";
                        trainDbMap.Clear();

                        while (reader.Read())
                        {
                            int id = reader.GetInt32("id");
                            string name = reader.IsDBNull(reader.GetOrdinal("name")) ? "Unnamed" : reader.GetString("name");
                            int address = reader.IsDBNull(reader.GetOrdinal("dcc_address")) ? 0 : reader.GetInt32("dcc_address");

                            var t = new TrainModel { Id = id, Name = name, Address = address, Speed = 0, Direction = true };
                            comboBox1.Items.Add(t);
                            trainDbMap[t] = id;
                        }
                    }
                }

                if (comboBox1.Items.Count > 0)
                {
                    comboBox1.SelectedIndex = 0;
                    // load functions for the first train
                    LoadFunctionsForTrain(SelectedTrain());
                }
            }
            catch
            {
                // ignore DB errors for now — keep offline fallback
            }
        }

        private void LoadFunctionsForTrain(TrainModel sel)
        {
            if (sel == null) return;
            if (!trainDbMap.TryGetValue(sel, out int trainId))
                return; // no DB data for this train

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
                        // reset any customizations on buttons
                        foreach (var btn in functionButtons)
                        {
                            btn.Visible = true;
                            btn.Text = $"F{btn.Tag}";
                        }

                        while (reader.Read())
                        {
                            int number = reader.GetInt32("number");
                            string defaultName = reader.IsDBNull(reader.GetOrdinal("default_name")) ? null : reader.GetString("default_name");
                            string customName = reader.IsDBNull(reader.GetOrdinal("custom_name")) ? null : reader.GetString("custom_name");
                            bool hidden = !reader.IsDBNull(reader.GetOrdinal("hidden")) && reader.GetInt32("hidden") == 1;
                            bool defaultState = !reader.IsDBNull(reader.GetOrdinal("default_state")) && reader.GetInt32("default_state") == 1;

                            // find the button with Tag == number
                            var btn = functionButtons.Find(b => (b.Tag as int?) == number);
                            if (btn != null)
                            {
                                btn.Visible = !hidden;
                                if (!string.IsNullOrEmpty(customName)) btn.Text = customName;
                                else if (!string.IsNullOrEmpty(defaultName)) btn.Text = defaultName;
                            }

                            if (defaultState)
                            {
                                sel.ActiveFunctions.Add(number);
                            }
                        }

                        RefreshFunctionButtons(sel);
                    }
                }
            }
            catch
            {
                // ignore DB errors
            }
        }

        private void sebessegkiiras()
        {
            var sel = SelectedTrain();
            double sebesseg = Math.Round(sel.Speed * 4.1);
            label1.Text = "Sebesség: " + sebesseg.ToString() + " km/h";
        }

        private void gombfrissites(Button btn, bool aktiv)
        {
            btn.BackColor = aktiv ? Color.DarkBlue : Color.DimGray;
        }

        private void RefreshFunctionButtons(TrainModel sel)
        {
            if (sel == null) return;

            foreach (var btn in functionButtons)
            {
                if (btn.Tag is int idx)
                {
                    bool active = sel.ActiveFunctions.Contains(idx);
                    gombfrissites(btn, active);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value < trackBar1.Maximum) trackBar1.Value += 1;
            SelectedTrain().Speed = trackBar1.Value;
            sebessegkiiras();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SelectedTrain().Speed = 0;
            trackBar1.Value = 0;
            sebessegkiiras();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value > trackBar1.Minimum) trackBar1.Value -= 1;
            SelectedTrain().Speed = trackBar1.Value;
            sebessegkiiras();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            SelectedTrain().Speed = trackBar1.Value;
            sebessegkiiras();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sel = SelectedTrain();
            if (sel == null) return;
            label3.Text = "Cím: " + sel.Address.ToString();
            label2.Text = "Irány: " + (sel.Direction ? "Előre" : "Hátra");
            label4.Text = "Név: " + sel.Name;
            label1.Text = "Sebesség: " + Math.Round(sel.Speed * 4.1).ToString() + " km/h";
            trackBar1.Value = sel.Speed;

            // load functions for the newly selected train
            LoadFunctionsForTrain(sel);

            RefreshFunctionButtons(sel);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 0;
            SelectedTrain().Direction = !SelectedTrain().Direction;
            label2.Text = "Irány: " + (SelectedTrain().Direction ? "Előre" : "Hátra");
            label1.Text = "Sebesség: 0 km/h";
        }

        private void button5_Click(object sender, EventArgs e)
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
                string connStr = "Server=localhost;Database=modellvasut;user=root;password=;";

                try
                {
                    using (var conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        using (var tran = conn.BeginTransaction())
                        {
                            // Insert train
                            using (var cmd = new MySqlCommand("INSERT INTO trains (user_id, name) VALUES (@uid, @name)", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@uid", CurrentUserId);
                                cmd.Parameters.AddWithValue("@name", szoveg);
                                cmd.ExecuteNonQuery();
                            }

                            // Get inserted train id
                            long newId;
                            using (var idCmd = new MySqlCommand("SELECT LAST_INSERT_ID()", conn, tran))
                            {
                                newId = Convert.ToInt64(idCmd.ExecuteScalar());
                            }

                            // Insert train_details
                            using (var cmdDet = new MySqlCommand("INSERT INTO train_details (train_id, dcc_address) VALUES (@tid, @addr)", conn, tran))
                            {
                                cmdDet.Parameters.AddWithValue("@tid", newId);
                                cmdDet.Parameters.AddWithValue("@addr", (int)szam);
                                cmdDet.ExecuteNonQuery();
                            }

                            // Insert functions f0..f28, visible, names f0..f28
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

                            // Optionally create default functions_settings rows (empty custom_name, default_state 0)
                            // This block is safe if functions_settings exists; if not, it's fine to omit.
                            using (var cmdFs = new MySqlCommand("", conn, tran))
                            {
                                for (int i = 0; i <= 28; i++)
                                {
                                    // Find the function id just inserted for this train+number
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

                            // Add to UI and select it
                            var newTrain = new TrainModel { Id = (int)newId, Name = szoveg, Address = (int)szam, Speed = 0, Direction = true };
                            comboBox1.Items.Add(newTrain);
                            trainDbMap[newTrain] = (int)newId;
                            comboBox1.SelectedItem = newTrain;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not create train: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

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
                    // reload functions to reflect saved changes
                    LoadFunctionsForTrain(sel);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // If user unchecks remember -> hide checkbox and clear stored remember token + file,
            // so next start they must login again.
            if (!checkBox1.Checked)
            {
                checkBox1.Visible = false;

                // delete local remember file if present
                const string rememberFile = "remember.dat";
                try
                {
                    if (File.Exists(rememberFile))
                        File.Delete(rememberFile);
                }
                catch
                {
                    // ignore file delete errors
                }

                // clear token in database for this user
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
                catch
                {
                    // ignore DB errors here or log as needed
                }
            }
        }

        private TrainModel SelectedTrain()
        {
            // Prefer the ComboBox selected item if it is a TrainModel.
            if (comboBox1?.SelectedItem is TrainModel sel)
                return sel;

            // If nothing is selected but the ComboBox has items, select the first TrainModel.
            for (int i = 0; comboBox1 != null && i < comboBox1.Items.Count; i++)
            {
                if (comboBox1.Items[i] is TrainModel first)
                {
                    comboBox1.SelectedIndex = i;
                    return first;
                }
            }

            // No train available.
            return null;
        }
    }
}