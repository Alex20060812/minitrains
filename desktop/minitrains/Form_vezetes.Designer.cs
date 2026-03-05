namespace minitrains
{
    partial class Form_vezetes
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_vezetes));
            comboBox_vonatvalasztas = new ComboBox();
            button_hozzaad = new Button();
            trackBar1 = new TrackBar();
            button_plus = new Button();
            button_stop = new Button();
            button_minus = new Button();
            button_direction = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            button_F0 = new Button();
            button_F1 = new Button();
            button_F2 = new Button();
            button_F3 = new Button();
            button_F4 = new Button();
            button_F5 = new Button();
            button_F6 = new Button();
            button_F7 = new Button();
            button_F8 = new Button();
            button_F9 = new Button();
            button_F10 = new Button();
            button_F11 = new Button();
            button_F12 = new Button();
            button_F13 = new Button();
            button_F14 = new Button();
            button_F15 = new Button();
            button_F16 = new Button();
            button_F17 = new Button();
            button_F18 = new Button();
            button_F19 = new Button();
            button_F20 = new Button();
            button_F21 = new Button();
            button_F22 = new Button();
            button_F23 = new Button();
            button_F24 = new Button();
            button_F25 = new Button();
            button_F26 = new Button();
            button_F27 = new Button();
            button_F28 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            checkBox_login = new CheckBox();
            button_functions = new Button();
            pictureBox1 = new PictureBox();
            label9 = new Label();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // comboBox_vonatvalasztas
            // 
            comboBox_vonatvalasztas.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_vonatvalasztas.Font = new Font("Bahnschrift", 18F, FontStyle.Bold, GraphicsUnit.Point, 238);
            comboBox_vonatvalasztas.FormattingEnabled = true;
            comboBox_vonatvalasztas.ItemHeight = 29;
            comboBox_vonatvalasztas.Location = new Point(45, 12);
            comboBox_vonatvalasztas.Name = "comboBox_vonatvalasztas";
            comboBox_vonatvalasztas.Size = new Size(146, 37);
            comboBox_vonatvalasztas.TabIndex = 0;
            comboBox_vonatvalasztas.SelectedIndexChanged += comboBox_vonatvalasztas_SelectedIndexChanged;
            // 
            // button_hozzaad
            // 
            button_hozzaad.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 238);
            button_hozzaad.Location = new Point(4, 12);
            button_hozzaad.Name = "button_hozzaad";
            button_hozzaad.Size = new Size(35, 36);
            button_hozzaad.TabIndex = 1;
            button_hozzaad.Text = "+";
            button_hozzaad.UseVisualStyleBackColor = true;
            button_hozzaad.Click += button_hozzaad_Click;
            // 
            // trackBar1
            // 
            trackBar1.BackColor = Color.Black;
            trackBar1.LargeChange = 1;
            trackBar1.Location = new Point(195, 375);
            trackBar1.Maximum = 29;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(477, 45);
            trackBar1.TabIndex = 2;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // button_plus
            // 
            button_plus.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_plus.Location = new Point(551, 283);
            button_plus.Name = "button_plus";
            button_plus.Size = new Size(120, 72);
            button_plus.TabIndex = 3;
            button_plus.Text = "+";
            button_plus.UseVisualStyleBackColor = true;
            button_plus.Click += button_plus_Click;
            // 
            // button_stop
            // 
            button_stop.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_stop.Location = new Point(374, 283);
            button_stop.Name = "button_stop";
            button_stop.Size = new Size(120, 72);
            button_stop.TabIndex = 4;
            button_stop.Text = "STOP";
            button_stop.UseVisualStyleBackColor = true;
            button_stop.Click += button_stop_Click;
            // 
            // button_minus
            // 
            button_minus.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_minus.Location = new Point(195, 283);
            button_minus.Name = "button_minus";
            button_minus.Size = new Size(120, 72);
            button_minus.TabIndex = 5;
            button_minus.Text = "-";
            button_minus.UseVisualStyleBackColor = true;
            button_minus.Click += button_minus_Click;
            // 
            // button_direction
            // 
            button_direction.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 238);
            button_direction.Location = new Point(374, 439);
            button_direction.Name = "button_direction";
            button_direction.Size = new Size(120, 72);
            button_direction.TabIndex = 6;
            button_direction.Text = "Irányváltás <->";
            button_direction.UseVisualStyleBackColor = true;
            button_direction.Click += button_direction_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 10;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.Controls.Add(button_F0, 0, 0);
            tableLayoutPanel1.Controls.Add(button_F1, 1, 0);
            tableLayoutPanel1.Controls.Add(button_F2, 2, 0);
            tableLayoutPanel1.Controls.Add(button_F3, 3, 0);
            tableLayoutPanel1.Controls.Add(button_F4, 4, 0);
            tableLayoutPanel1.Controls.Add(button_F5, 5, 0);
            tableLayoutPanel1.Controls.Add(button_F6, 6, 0);
            tableLayoutPanel1.Controls.Add(button_F7, 7, 0);
            tableLayoutPanel1.Controls.Add(button_F8, 8, 0);
            tableLayoutPanel1.Controls.Add(button_F9, 9, 0);
            tableLayoutPanel1.Controls.Add(button_F10, 0, 1);
            tableLayoutPanel1.Controls.Add(button_F11, 1, 1);
            tableLayoutPanel1.Controls.Add(button_F12, 2, 1);
            tableLayoutPanel1.Controls.Add(button_F13, 3, 1);
            tableLayoutPanel1.Controls.Add(button_F14, 4, 1);
            tableLayoutPanel1.Controls.Add(button_F15, 5, 1);
            tableLayoutPanel1.Controls.Add(button_F16, 6, 1);
            tableLayoutPanel1.Controls.Add(button_F17, 7, 1);
            tableLayoutPanel1.Controls.Add(button_F18, 8, 1);
            tableLayoutPanel1.Controls.Add(button_F19, 9, 1);
            tableLayoutPanel1.Controls.Add(button_F20, 0, 2);
            tableLayoutPanel1.Controls.Add(button_F21, 1, 2);
            tableLayoutPanel1.Controls.Add(button_F22, 2, 2);
            tableLayoutPanel1.Controls.Add(button_F23, 3, 2);
            tableLayoutPanel1.Controls.Add(button_F24, 4, 2);
            tableLayoutPanel1.Controls.Add(button_F25, 5, 2);
            tableLayoutPanel1.Controls.Add(button_F26, 6, 2);
            tableLayoutPanel1.Controls.Add(button_F27, 7, 2);
            tableLayoutPanel1.Controls.Add(button_F28, 8, 2);
            tableLayoutPanel1.Location = new Point(134, 559);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Size = new Size(604, 213);
            tableLayoutPanel1.TabIndex = 7;
            // 
            // button_F0
            // 
            button_F0.BackColor = Color.Gray;
            button_F0.BackgroundImageLayout = ImageLayout.Stretch;
            button_F0.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F0.ForeColor = Color.Black;
            button_F0.ImageAlign = ContentAlignment.TopCenter;
            button_F0.Location = new Point(3, 3);
            button_F0.Name = "button_F0";
            button_F0.Size = new Size(54, 64);
            button_F0.TabIndex = 0;
            button_F0.Text = "F0";
            button_F0.TextAlign = ContentAlignment.BottomCenter;
            button_F0.UseVisualStyleBackColor = false;
            button_F0.Click += button_F0_Click;
            // 
            // button_F1
            // 
            button_F1.BackColor = Color.Gray;
            button_F1.BackgroundImageLayout = ImageLayout.Stretch;
            button_F1.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F1.ForeColor = Color.Black;
            button_F1.ImageAlign = ContentAlignment.TopCenter;
            button_F1.Location = new Point(63, 3);
            button_F1.Name = "button_F1";
            button_F1.Size = new Size(54, 64);
            button_F1.TabIndex = 1;
            button_F1.Text = "F1";
            button_F1.TextAlign = ContentAlignment.BottomCenter;
            button_F1.UseVisualStyleBackColor = false;
            button_F1.Click += button_F1_Click;
            // 
            // button_F2
            // 
            button_F2.BackColor = Color.Gray;
            button_F2.BackgroundImageLayout = ImageLayout.Stretch;
            button_F2.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F2.ForeColor = Color.Black;
            button_F2.ImageAlign = ContentAlignment.TopCenter;
            button_F2.Location = new Point(123, 3);
            button_F2.Name = "button_F2";
            button_F2.Size = new Size(54, 64);
            button_F2.TabIndex = 2;
            button_F2.Text = "F2";
            button_F2.TextAlign = ContentAlignment.BottomCenter;
            button_F2.UseVisualStyleBackColor = false;
            button_F2.Click += button_F2_Click;
            // 
            // button_F3
            // 
            button_F3.BackColor = Color.Gray;
            button_F3.BackgroundImageLayout = ImageLayout.Stretch;
            button_F3.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F3.ForeColor = Color.Black;
            button_F3.ImageAlign = ContentAlignment.TopCenter;
            button_F3.Location = new Point(183, 3);
            button_F3.Name = "button_F3";
            button_F3.Size = new Size(54, 64);
            button_F3.TabIndex = 3;
            button_F3.Text = "F3";
            button_F3.TextAlign = ContentAlignment.BottomCenter;
            button_F3.UseVisualStyleBackColor = false;
            button_F3.Click += button_F3_Click;
            // 
            // button_F4
            // 
            button_F4.BackColor = Color.Gray;
            button_F4.BackgroundImageLayout = ImageLayout.Stretch;
            button_F4.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F4.ForeColor = Color.Black;
            button_F4.ImageAlign = ContentAlignment.TopCenter;
            button_F4.Location = new Point(243, 3);
            button_F4.Name = "button_F4";
            button_F4.Size = new Size(54, 64);
            button_F4.TabIndex = 4;
            button_F4.Text = "F4";
            button_F4.TextAlign = ContentAlignment.BottomCenter;
            button_F4.UseVisualStyleBackColor = false;
            button_F4.Click += button_F4_Click;
            // 
            // button_F5
            // 
            button_F5.BackColor = Color.Gray;
            button_F5.BackgroundImageLayout = ImageLayout.Stretch;
            button_F5.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F5.ForeColor = Color.Black;
            button_F5.ImageAlign = ContentAlignment.TopCenter;
            button_F5.Location = new Point(303, 3);
            button_F5.Name = "button_F5";
            button_F5.Size = new Size(54, 64);
            button_F5.TabIndex = 5;
            button_F5.Text = "F5";
            button_F5.TextAlign = ContentAlignment.BottomCenter;
            button_F5.UseVisualStyleBackColor = false;
            button_F5.Click += button_F5_Click;
            // 
            // button_F6
            // 
            button_F6.BackColor = Color.Gray;
            button_F6.BackgroundImageLayout = ImageLayout.Stretch;
            button_F6.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F6.ForeColor = Color.Black;
            button_F6.ImageAlign = ContentAlignment.TopCenter;
            button_F6.Location = new Point(363, 3);
            button_F6.Name = "button_F6";
            button_F6.Size = new Size(54, 64);
            button_F6.TabIndex = 6;
            button_F6.Text = "F6";
            button_F6.TextAlign = ContentAlignment.BottomCenter;
            button_F6.UseVisualStyleBackColor = false;
            button_F6.Click += button_F6_Click;
            // 
            // button_F7
            // 
            button_F7.BackColor = Color.Gray;
            button_F7.BackgroundImageLayout = ImageLayout.Stretch;
            button_F7.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F7.ForeColor = Color.Black;
            button_F7.ImageAlign = ContentAlignment.TopCenter;
            button_F7.Location = new Point(423, 3);
            button_F7.Name = "button_F7";
            button_F7.Size = new Size(54, 64);
            button_F7.TabIndex = 7;
            button_F7.Text = "F7";
            button_F7.TextAlign = ContentAlignment.BottomCenter;
            button_F7.UseVisualStyleBackColor = false;
            button_F7.Click += button_F7_Click;
            // 
            // button_F8
            // 
            button_F8.BackColor = Color.Gray;
            button_F8.BackgroundImageLayout = ImageLayout.Stretch;
            button_F8.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F8.ForeColor = Color.Black;
            button_F8.ImageAlign = ContentAlignment.TopCenter;
            button_F8.Location = new Point(483, 3);
            button_F8.Name = "button_F8";
            button_F8.Size = new Size(54, 64);
            button_F8.TabIndex = 8;
            button_F8.Text = "F8";
            button_F8.TextAlign = ContentAlignment.BottomCenter;
            button_F8.UseVisualStyleBackColor = false;
            button_F8.Click += button_F8_Click;
            // 
            // button_F9
            // 
            button_F9.BackColor = Color.Gray;
            button_F9.BackgroundImageLayout = ImageLayout.Stretch;
            button_F9.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F9.ForeColor = Color.Black;
            button_F9.ImageAlign = ContentAlignment.TopCenter;
            button_F9.Location = new Point(543, 3);
            button_F9.Name = "button_F9";
            button_F9.Size = new Size(58, 64);
            button_F9.TabIndex = 9;
            button_F9.Text = "F9";
            button_F9.TextAlign = ContentAlignment.BottomCenter;
            button_F9.UseVisualStyleBackColor = false;
            button_F9.Click += button_F9_Click;
            // 
            // button_F10
            // 
            button_F10.BackColor = Color.Gray;
            button_F10.BackgroundImageLayout = ImageLayout.Stretch;
            button_F10.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F10.ForeColor = Color.Black;
            button_F10.ImageAlign = ContentAlignment.TopCenter;
            button_F10.Location = new Point(3, 73);
            button_F10.Name = "button_F10";
            button_F10.Size = new Size(54, 64);
            button_F10.TabIndex = 10;
            button_F10.Text = "F10";
            button_F10.TextAlign = ContentAlignment.BottomCenter;
            button_F10.UseVisualStyleBackColor = false;
            button_F10.Click += button_F10_Click;
            // 
            // button_F11
            // 
            button_F11.BackColor = Color.Gray;
            button_F11.BackgroundImageLayout = ImageLayout.Stretch;
            button_F11.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F11.ForeColor = Color.Black;
            button_F11.ImageAlign = ContentAlignment.TopCenter;
            button_F11.Location = new Point(63, 73);
            button_F11.Name = "button_F11";
            button_F11.Size = new Size(54, 64);
            button_F11.TabIndex = 11;
            button_F11.Text = "F11";
            button_F11.TextAlign = ContentAlignment.BottomCenter;
            button_F11.UseVisualStyleBackColor = false;
            button_F11.Click += button_F11_Click;
            // 
            // button_F12
            // 
            button_F12.BackColor = Color.Gray;
            button_F12.BackgroundImageLayout = ImageLayout.Stretch;
            button_F12.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F12.ForeColor = Color.Black;
            button_F12.ImageAlign = ContentAlignment.TopCenter;
            button_F12.Location = new Point(123, 73);
            button_F12.Name = "button_F12";
            button_F12.Size = new Size(54, 64);
            button_F12.TabIndex = 12;
            button_F12.Text = "F12";
            button_F12.TextAlign = ContentAlignment.BottomCenter;
            button_F12.UseVisualStyleBackColor = false;
            button_F12.Click += button_F12_Click;
            // 
            // button_F13
            // 
            button_F13.BackColor = Color.Gray;
            button_F13.BackgroundImageLayout = ImageLayout.Stretch;
            button_F13.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F13.ForeColor = Color.Black;
            button_F13.ImageAlign = ContentAlignment.TopCenter;
            button_F13.Location = new Point(183, 73);
            button_F13.Name = "button_F13";
            button_F13.Size = new Size(54, 64);
            button_F13.TabIndex = 13;
            button_F13.Text = "F13";
            button_F13.TextAlign = ContentAlignment.BottomCenter;
            button_F13.UseVisualStyleBackColor = false;
            button_F13.Click += button_F13_Click;
            // 
            // button_F14
            // 
            button_F14.BackColor = Color.Gray;
            button_F14.BackgroundImageLayout = ImageLayout.Stretch;
            button_F14.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F14.ForeColor = Color.Black;
            button_F14.ImageAlign = ContentAlignment.TopCenter;
            button_F14.Location = new Point(243, 73);
            button_F14.Name = "button_F14";
            button_F14.Size = new Size(54, 64);
            button_F14.TabIndex = 14;
            button_F14.Text = "F14";
            button_F14.TextAlign = ContentAlignment.BottomCenter;
            button_F14.UseVisualStyleBackColor = false;
            button_F14.Click += button_F14_Click;
            // 
            // button_F15
            // 
            button_F15.BackColor = Color.Gray;
            button_F15.BackgroundImageLayout = ImageLayout.Stretch;
            button_F15.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F15.ForeColor = Color.Black;
            button_F15.ImageAlign = ContentAlignment.TopCenter;
            button_F15.Location = new Point(303, 73);
            button_F15.Name = "button_F15";
            button_F15.Size = new Size(54, 64);
            button_F15.TabIndex = 15;
            button_F15.Text = "F15";
            button_F15.TextAlign = ContentAlignment.BottomCenter;
            button_F15.UseVisualStyleBackColor = false;
            button_F15.Click += button_F15_Click;
            // 
            // button_F16
            // 
            button_F16.BackColor = Color.Gray;
            button_F16.BackgroundImageLayout = ImageLayout.Stretch;
            button_F16.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F16.ForeColor = Color.Black;
            button_F16.ImageAlign = ContentAlignment.TopCenter;
            button_F16.Location = new Point(363, 73);
            button_F16.Name = "button_F16";
            button_F16.Size = new Size(54, 64);
            button_F16.TabIndex = 16;
            button_F16.Text = "F16";
            button_F16.TextAlign = ContentAlignment.BottomCenter;
            button_F16.UseVisualStyleBackColor = false;
            button_F16.Click += button_F16_Click;
            // 
            // button_F17
            // 
            button_F17.BackColor = Color.Gray;
            button_F17.BackgroundImageLayout = ImageLayout.Stretch;
            button_F17.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F17.ForeColor = Color.Black;
            button_F17.ImageAlign = ContentAlignment.TopCenter;
            button_F17.Location = new Point(423, 73);
            button_F17.Name = "button_F17";
            button_F17.Size = new Size(54, 64);
            button_F17.TabIndex = 17;
            button_F17.Text = "F17";
            button_F17.TextAlign = ContentAlignment.BottomCenter;
            button_F17.UseVisualStyleBackColor = false;
            button_F17.Click += button_F17_Click;
            // 
            // button_F18
            // 
            button_F18.BackColor = Color.Gray;
            button_F18.BackgroundImageLayout = ImageLayout.Stretch;
            button_F18.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F18.ForeColor = Color.Black;
            button_F18.ImageAlign = ContentAlignment.TopCenter;
            button_F18.Location = new Point(483, 73);
            button_F18.Name = "button_F18";
            button_F18.Size = new Size(54, 64);
            button_F18.TabIndex = 18;
            button_F18.Text = "F18";
            button_F18.TextAlign = ContentAlignment.BottomCenter;
            button_F18.UseVisualStyleBackColor = false;
            button_F18.Click += button_F18_Click;
            // 
            // button_F19
            // 
            button_F19.BackColor = Color.Gray;
            button_F19.BackgroundImageLayout = ImageLayout.Stretch;
            button_F19.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F19.ForeColor = Color.Black;
            button_F19.ImageAlign = ContentAlignment.TopCenter;
            button_F19.Location = new Point(543, 73);
            button_F19.Name = "button_F19";
            button_F19.Size = new Size(58, 64);
            button_F19.TabIndex = 19;
            button_F19.Text = "F19";
            button_F19.TextAlign = ContentAlignment.BottomCenter;
            button_F19.UseVisualStyleBackColor = false;
            button_F19.Click += button_F19_Click;
            // 
            // button_F20
            // 
            button_F20.BackColor = Color.Gray;
            button_F20.BackgroundImageLayout = ImageLayout.Stretch;
            button_F20.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F20.ForeColor = Color.Black;
            button_F20.ImageAlign = ContentAlignment.TopCenter;
            button_F20.Location = new Point(3, 143);
            button_F20.Name = "button_F20";
            button_F20.Size = new Size(54, 67);
            button_F20.TabIndex = 20;
            button_F20.Text = "F20";
            button_F20.TextAlign = ContentAlignment.BottomCenter;
            button_F20.UseVisualStyleBackColor = false;
            button_F20.Click += button_F20_Click;
            // 
            // button_F21
            // 
            button_F21.BackColor = Color.Gray;
            button_F21.BackgroundImageLayout = ImageLayout.Stretch;
            button_F21.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F21.ForeColor = Color.Black;
            button_F21.ImageAlign = ContentAlignment.TopCenter;
            button_F21.Location = new Point(63, 143);
            button_F21.Name = "button_F21";
            button_F21.Size = new Size(54, 67);
            button_F21.TabIndex = 21;
            button_F21.Text = "F21";
            button_F21.TextAlign = ContentAlignment.BottomCenter;
            button_F21.UseVisualStyleBackColor = false;
            button_F21.Click += button_F21_Click;
            // 
            // button_F22
            // 
            button_F22.BackColor = Color.Gray;
            button_F22.BackgroundImageLayout = ImageLayout.Stretch;
            button_F22.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F22.ForeColor = Color.Black;
            button_F22.ImageAlign = ContentAlignment.TopCenter;
            button_F22.Location = new Point(123, 143);
            button_F22.Name = "button_F22";
            button_F22.Size = new Size(54, 67);
            button_F22.TabIndex = 22;
            button_F22.Text = "F22";
            button_F22.TextAlign = ContentAlignment.BottomCenter;
            button_F22.UseVisualStyleBackColor = false;
            button_F22.Click += button_F22_Click;
            // 
            // button_F23
            // 
            button_F23.BackColor = Color.Gray;
            button_F23.BackgroundImageLayout = ImageLayout.Stretch;
            button_F23.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F23.ForeColor = Color.Black;
            button_F23.ImageAlign = ContentAlignment.TopCenter;
            button_F23.Location = new Point(183, 143);
            button_F23.Name = "button_F23";
            button_F23.Size = new Size(54, 67);
            button_F23.TabIndex = 23;
            button_F23.Text = "F23";
            button_F23.TextAlign = ContentAlignment.BottomCenter;
            button_F23.UseVisualStyleBackColor = false;
            button_F23.Click += button_F23_Click;
            // 
            // button_F24
            // 
            button_F24.BackColor = Color.Gray;
            button_F24.BackgroundImageLayout = ImageLayout.Stretch;
            button_F24.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F24.ForeColor = Color.Black;
            button_F24.ImageAlign = ContentAlignment.TopCenter;
            button_F24.Location = new Point(243, 143);
            button_F24.Name = "button_F24";
            button_F24.Size = new Size(54, 67);
            button_F24.TabIndex = 24;
            button_F24.Text = "F24";
            button_F24.TextAlign = ContentAlignment.BottomCenter;
            button_F24.UseVisualStyleBackColor = false;
            button_F24.Click += button_F24_Click;
            // 
            // button_F25
            // 
            button_F25.BackColor = Color.Gray;
            button_F25.BackgroundImageLayout = ImageLayout.Stretch;
            button_F25.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F25.ForeColor = Color.Black;
            button_F25.ImageAlign = ContentAlignment.TopCenter;
            button_F25.Location = new Point(303, 143);
            button_F25.Name = "button_F25";
            button_F25.Size = new Size(54, 67);
            button_F25.TabIndex = 25;
            button_F25.Text = "F25";
            button_F25.TextAlign = ContentAlignment.BottomCenter;
            button_F25.UseVisualStyleBackColor = false;
            button_F25.Click += button_F25_Click;
            // 
            // button_F26
            // 
            button_F26.BackColor = Color.Gray;
            button_F26.BackgroundImageLayout = ImageLayout.Stretch;
            button_F26.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F26.ForeColor = Color.Black;
            button_F26.ImageAlign = ContentAlignment.TopCenter;
            button_F26.Location = new Point(363, 143);
            button_F26.Name = "button_F26";
            button_F26.Size = new Size(54, 67);
            button_F26.TabIndex = 26;
            button_F26.Text = "F26";
            button_F26.TextAlign = ContentAlignment.BottomCenter;
            button_F26.UseVisualStyleBackColor = false;
            button_F26.Click += button_F26_Click;
            // 
            // button_F27
            // 
            button_F27.BackColor = Color.Gray;
            button_F27.BackgroundImageLayout = ImageLayout.Stretch;
            button_F27.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F27.ForeColor = Color.Black;
            button_F27.ImageAlign = ContentAlignment.TopCenter;
            button_F27.Location = new Point(423, 143);
            button_F27.Name = "button_F27";
            button_F27.Size = new Size(54, 67);
            button_F27.TabIndex = 27;
            button_F27.Text = "F27";
            button_F27.TextAlign = ContentAlignment.BottomCenter;
            button_F27.UseVisualStyleBackColor = false;
            button_F27.Click += button_F27_Click;
            // 
            // button_F28
            // 
            button_F28.BackColor = Color.Gray;
            button_F28.BackgroundImageLayout = ImageLayout.Stretch;
            button_F28.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button_F28.ForeColor = Color.Black;
            button_F28.ImageAlign = ContentAlignment.TopCenter;
            button_F28.Location = new Point(483, 143);
            button_F28.Name = "button_F28";
            button_F28.Size = new Size(54, 67);
            button_F28.TabIndex = 28;
            button_F28.Text = "F28";
            button_F28.TextAlign = ContentAlignment.BottomCenter;
            button_F28.UseVisualStyleBackColor = false;
            button_F28.Click += button_F28_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Bahnschrift", 18F, FontStyle.Bold);
            label1.Location = new Point(197, 112);
            label1.Name = "label1";
            label1.Size = new Size(59, 29);
            label1.TabIndex = 8;
            label1.Text = "Név:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Bahnschrift", 18F, FontStyle.Bold);
            label2.Location = new Point(457, 112);
            label2.Name = "label2";
            label2.Size = new Size(60, 29);
            label2.TabIndex = 9;
            label2.Text = "Cím:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Bahnschrift", 18F, FontStyle.Bold);
            label3.Location = new Point(197, 157);
            label3.Name = "label3";
            label3.Size = new Size(122, 29);
            label3.TabIndex = 10;
            label3.Text = "Sebesség:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Bahnschrift", 18F, FontStyle.Bold);
            label4.Location = new Point(457, 157);
            label4.Name = "label4";
            label4.Size = new Size(72, 29);
            label4.TabIndex = 11;
            label4.Text = "Irány:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Bahnschrift", 18F, FontStyle.Bold);
            label5.Location = new Point(325, 112);
            label5.Name = "label5";
            label5.Size = new Size(0, 29);
            label5.TabIndex = 12;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Bahnschrift", 18F, FontStyle.Bold);
            label6.Location = new Point(326, 157);
            label6.Name = "label6";
            label6.Size = new Size(0, 29);
            label6.TabIndex = 13;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Bahnschrift", 18F, FontStyle.Bold);
            label7.Location = new Point(568, 112);
            label7.Name = "label7";
            label7.Size = new Size(0, 29);
            label7.TabIndex = 14;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Bahnschrift", 18F, FontStyle.Bold);
            label8.Location = new Point(568, 157);
            label8.Name = "label8";
            label8.Size = new Size(0, 29);
            label8.TabIndex = 15;
            // 
            // checkBox_login
            // 
            checkBox_login.AutoSize = true;
            checkBox_login.Location = new Point(709, 22);
            checkBox_login.Name = "checkBox_login";
            checkBox_login.Size = new Size(167, 19);
            checkBox_login.TabIndex = 16;
            checkBox_login.Text = "Automatikus bejelentkezés";
            checkBox_login.UseVisualStyleBackColor = true;
            checkBox_login.CheckedChanged += checkBox_login_CheckedChanged;
            // 
            // button_functions
            // 
            button_functions.Location = new Point(377, 793);
            button_functions.Name = "button_functions";
            button_functions.Size = new Size(117, 42);
            button_functions.TabIndex = 17;
            button_functions.Text = "Szerkesztés";
            button_functions.UseVisualStyleBackColor = true;
            button_functions.Click += button_functions_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(374, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(120, 83);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 18;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(733, 66);
            label9.Name = "label9";
            label9.Size = new Size(0, 15);
            label9.TabIndex = 19;
            // 
            // Form_vezetes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(884, 861);
            Controls.Add(label9);
            Controls.Add(pictureBox1);
            Controls.Add(button_functions);
            Controls.Add(checkBox_login);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(button_direction);
            Controls.Add(button_minus);
            Controls.Add(button_stop);
            Controls.Add(button_plus);
            Controls.Add(trackBar1);
            Controls.Add(button_hozzaad);
            Controls.Add(comboBox_vonatvalasztas);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form_vezetes";
            Text = "Minitrains";
            Load += Form_vezetes_Load;
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBox_vonatvalasztas;
        private Button button_hozzaad;
        private TrackBar trackBar1;
        private Button button_plus;
        private Button button_stop;
        private Button button_minus;
        private Button button_direction;
        private TableLayoutPanel tableLayoutPanel1;
        private Button button_F0;
        private Button button_F1;
        private Button button_F2;
        private Button button_F3;
        private Button button_F4;
        private Button button_F5;
        private Button button_F6;
        private Button button_F7;
        private Button button_F8;
        private Button button_F9;
        private Button button_F10;
        private Button button_F11;
        private Button button_F12;
        private Button button_F13;
        private Button button_F14;
        private Button button_F15;
        private Button button_F16;
        private Button button_F17;
        private Button button_F18;
        private Button button_F19;
        private Button button_F20;
        private Button button_F21;
        private Button button_F22;
        private Button button_F23;
        private Button button_F24;
        private Button button_F25;
        private Button button_F26;
        private Button button_F27;
        private Button button_F28;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private CheckBox checkBox_login;
        private Button button_functions;
        private PictureBox pictureBox1;
        private Label label9;
    }
}
