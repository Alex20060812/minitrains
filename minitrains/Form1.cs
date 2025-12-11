using minitrains;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;


namespace minitrains
{

    public partial class Form1 : Form
    {
        train t1;
        train t2;
        private Image backgroundImage;
        public Form1()
        {
            InitializeComponent();
            t1 = new train("Express", 0, true, 40);
            t2 = new train("Local", 0, false, 41);
            Form3 loginForm = new Form3();
            loginForm.ShowDialog();
            backgroundImage = Image.FromFile("20220402-IMG_1242.jpg");
            this.DoubleBuffered = true;
            this.Resize += (s, e) => this.Invalidate();
            comboBox1.DisplayMember = "name";
            comboBox1.Items.Add(t1);
            comboBox1.Items.Add(t2);

            
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
            for (int i = 0; i < 29; i++)
            {
                Button btn = new Button();
                btn.Text = $"F{i}";
                btn.Size = new Size(flowLayoutPanel1.Width/11, flowLayoutPanel1.Height/5);
                btn.Name = i.ToString();
                btn.FlatStyle = FlatStyle.Flat;
                btn.BackColor = Color.DimGray;
                btn.ForeColor = Color.White;
                btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.ImageAlign = ContentAlignment.MiddleCenter;
                //funkciogombok kiolvasasa, betoltese
                //gombfrissites(btn, SelectedTrain().);
                //button click esemény kezelő hozzáadása
                btn.Click += (s, e) =>
                {
                    //gomb funkció váltás
                    //SelectedTrain().ToggleFunction(btn.Name);
                    //gomb frissítése
                    //gombfrissites(btn, SelectedTrain().IsFunctionActive(btn.Name));
                };


                flowLayoutPanel1.Controls.Add(btn);
            }
        }

        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (backgroundImage == null)
                return;

            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            float imageAspect = (float)backgroundImage.Width / backgroundImage.Height;
            float formAspect = (float)formWidth / formHeight;

            float scale;

            //Ha a form szélesebb arányban, mint a kép, akkor növeljük a magasságot (zoomolunk)
            if (formAspect > imageAspect)
            {
                // a szélességhez igazítjuk, magasságot növeljük (zoom)
                scale = (float)formWidth / backgroundImage.Width;
            }
            else
            {
                // a magassághoz igazítjuk (normál eset)
                scale = (float)formHeight / backgroundImage.Height;
            }

            int drawWidth = (int)(backgroundImage.Width * scale);
            int drawHeight = (int)(backgroundImage.Height * scale);

            // középre igazítás
            int x = (formWidth - drawWidth) / 2;
            int y = (formHeight - drawHeight) / 2;

            Rectangle destRect = new Rectangle(x, y, drawWidth, drawHeight);

            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImage(backgroundImage, destRect);
        }
        private train SelectedTrain()
        {
            return comboBox1.SelectedItem as train ?? t1;
            
        }

        private void sebessegkiiras()
        {
            
            var sel = SelectedTrain();
            double sebesseg = sel.speed * 4.1;
            sebesseg = Math.Round(sebesseg);
            label1.Text = "Sebesség: " + sebesseg.ToString() + " km/h";
        }
        private void gombfrissites(Button btn, bool aktiv)
        {
            if (aktiv)
            {
                btn.BackColor = Color.Green;
            }
            else
            {
                btn.BackColor = Color.DimGray;
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            // Increase current trackbar and apply to selected train
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
        }

        

        private void button4_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 0;
            if (SelectedTrain().direction)
            {
                SelectedTrain().direction = false;
            }
            else
            {
                SelectedTrain().direction = true;
            }

            label2.Text = "Irány: " + (SelectedTrain().direction ? "Előre" : "Hátra");
            label1.Text = "Sebesség: 0 km/h"; 
        }
          

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 kisAblak = new Form2();

            // megnyitod modálisan
            var result = kisAblak.ShowDialog();

            // ha az OK gombbal zárta be (pl. DialogResult.OK)
            if (result == DialogResult.OK)
            {
                // elérheted az értékeket a Form2-ből
                decimal szam = kisAblak.NumericErtek;
                string szoveg = kisAblak.TextErtek;

                train t1 = new train(szoveg,0,true,(int)szam);
                comboBox1.Items.Add(t1);
            }
        }


    }
    
    class train
    {
        public string name;
        public int speed;
        public bool direction;
        public int adress;
        public train(string n, int s, bool d, int adr)
        {
            name = n;
            speed = s;
            direction = d;
            adress = adr;
        }

        
        public override string ToString()
        {
            return name;
        }
    }
}