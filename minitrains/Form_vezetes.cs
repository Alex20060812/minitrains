using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace minitrains
{
    public partial class Form_vezetes : Form
    {
        train t1;
        train t2;
        
        private readonly List<Button> functionButtons = new List<Button>();

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

            t1 = new train("Express", 0, true, 40);
            t2 = new train("Local", 0, false, 41);

            comboBox1.DisplayMember = "name";
            comboBox1.Items.Add(t1);
            comboBox1.Items.Add(t2);
            
            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;

            for (int i = 0; i < 29; i++)
            {
                int idx = i;
                Button btn = new Button
                {
                    Text = $"F{idx}",
                    Size = new Size(flowLayoutPanel1.Width / 11, flowLayoutPanel1.Height / 5),
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
                    if (selTrain.activeFunctions.Contains(idx))
                    {
                        selTrain.activeFunctions.Remove(idx);
                        gombfrissites(btn, false);
                    }
                    else
                    {
                        selTrain.activeFunctions.Add(idx);
                        gombfrissites(btn, true);
                    }
                };

                flowLayoutPanel1.Controls.Add(btn);
                functionButtons.Add(btn);
            }

            RefreshFunctionButtons(SelectedTrain());
        }

        public int CurrentUserId { get; }
        public bool RememberMe { get; }

        private train SelectedTrain()
        {
            return comboBox1.SelectedItem as train ?? t1;
        }

        private void sebessegkiiras()
        {
            var sel = SelectedTrain();
            double sebesseg = Math.Round(sel.speed * 4.1);
            label1.Text = "Sebesség: " + sebesseg.ToString() + " km/h";
        }

        private void gombfrissites(Button btn, bool aktiv)
        {
            btn.BackColor = aktiv ? Color.DarkBlue : Color.DimGray;
        }

        private void RefreshFunctionButtons(train sel)
        {
            if (sel == null) return;

            foreach (var btn in functionButtons)
            {
                if (btn.Tag is int idx)
                {
                    bool active = sel.activeFunctions.Contains(idx);
                    gombfrissites(btn, active);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value < trackBar1.Maximum) trackBar1.Value += 1;
            SelectedTrain().speed = trackBar1.Value;
            sebessegkiiras();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SelectedTrain().speed = 0;
            trackBar1.Value = 0;
            sebessegkiiras();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value > trackBar1.Minimum) trackBar1.Value -= 1;
            SelectedTrain().speed = trackBar1.Value;
            sebessegkiiras();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            SelectedTrain().speed = trackBar1.Value;
            sebessegkiiras();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sel = SelectedTrain();
            label3.Text = "Cím: " + sel.adress.ToString();
            label2.Text = "Irány: " + (sel.direction ? "Előre" : "Hátra");
            label4.Text = "Név: " + sel.name;
            label1.Text = "Sebesség: " + Math.Round(sel.speed * 4.1).ToString() + " km/h";
            trackBar1.Value = sel.speed;

            RefreshFunctionButtons(sel);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 0;
            SelectedTrain().direction = !SelectedTrain().direction;
            label2.Text = "Irány: " + (SelectedTrain().direction ? "Előre" : "Hátra");
            label1.Text = "Sebesség: 0 km/h";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (var kisAblak = new Form_vonathozzaadas())
            {
                if (kisAblak.ShowDialog() == DialogResult.OK)
                {
                    decimal szam = kisAblak.NumericErtek;
                    string szoveg = kisAblak.TextErtek;
                    var newTrain = new train(szoveg, 0, true, (int)szam);
                    comboBox1.Items.Add(newTrain);
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
    }

    class train
    {
        public string name;
        public int speed;
        public bool direction;
        public int adress;
        public HashSet<int> activeFunctions;

        public train(string n, int s, bool d, int adr)
        {
            name = n;
            speed = s;
            direction = d;
            adress = adr;
            activeFunctions = new HashSet<int>();
        }

        public override string ToString() => name;
    }
}