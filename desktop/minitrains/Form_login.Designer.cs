namespace minitrains
{
    partial class Form_login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_login));
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button2 = new Button();
            label1 = new Label();
            label2 = new Label();
            checkBox_RememberMe = new CheckBox();
            label3 = new Label();
            textBox_Port = new TextBox();
            labelZ21Ip = new Label();
            textBox_Z21Ip = new TextBox();
            labelZ21Port = new Label();
            textBox_Z21Port = new TextBox();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(168, 363);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(127, 23);
            textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(168, 403);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(127, 23);
            textBox2.TabIndex = 2;
            // 
            // button2
            // 
            button2.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button2.Location = new Point(341, 403);
            button2.Name = "button2";
            button2.Size = new Size(80, 30);
            button2.TabIndex = 3;
            button2.Text = "OK";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.Control;
            label1.Font = new Font("Bahnschrift", 11.25F, FontStyle.Bold);
            label1.Location = new Point(36, 363);
            label1.Name = "label1";
            label1.Size = new Size(113, 18);
            label1.TabIndex = 4;
            label1.Text = "Felhasználónév:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Bahnschrift", 11.25F, FontStyle.Bold);
            label2.Location = new Point(36, 408);
            label2.Name = "label2";
            label2.Size = new Size(54, 18);
            label2.TabIndex = 5;
            label2.Text = "Jelszó:";
            // 
            // checkBox_RememberMe
            // 
            checkBox_RememberMe.AutoSize = true;
            checkBox_RememberMe.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            checkBox_RememberMe.Location = new Point(341, 363);
            checkBox_RememberMe.Name = "checkBox_RememberMe";
            checkBox_RememberMe.Size = new Size(129, 20);
            checkBox_RememberMe.TabIndex = 6;
            checkBox_RememberMe.Text = "Emlékezzen rám?";
            checkBox_RememberMe.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Bahnschrift", 11.25F, FontStyle.Bold);
            label3.Location = new Point(36, 446);
            label3.Name = "label3";
            label3.Size = new Size(38, 18);
            label3.TabIndex = 7;
            label3.Text = "Port:";
            // 
            // textBox_Port
            // 
            textBox_Port.Location = new Point(168, 446);
            textBox_Port.Name = "textBox_Port";
            textBox_Port.Size = new Size(127, 23);
            textBox_Port.TabIndex = 8;
            textBox_Port.Text = "3306";
            // 
            // labelZ21Ip
            // 
            labelZ21Ip.AutoSize = true;
            labelZ21Ip.Font = new Font("Bahnschrift", 11.25F, FontStyle.Bold);
            labelZ21Ip.Location = new Point(36, 486);
            labelZ21Ip.Name = "labelZ21Ip";
            labelZ21Ip.Size = new Size(50, 18);
            labelZ21Ip.TabIndex = 9;
            labelZ21Ip.Text = "Z21 IP:";
            // 
            // textBox_Z21Ip
            // 
            textBox_Z21Ip.Location = new Point(168, 486);
            textBox_Z21Ip.Name = "textBox_Z21Ip";
            textBox_Z21Ip.Size = new Size(127, 23);
            textBox_Z21Ip.TabIndex = 10;
            textBox_Z21Ip.Text = "192.168.0.111";
            // 
            // labelZ21Port
            // 
            labelZ21Port.AutoSize = true;
            labelZ21Port.Font = new Font("Bahnschrift", 11.25F, FontStyle.Bold);
            labelZ21Port.Location = new Point(36, 526);
            labelZ21Port.Name = "labelZ21Port";
            labelZ21Port.Size = new Size(67, 18);
            labelZ21Port.TabIndex = 11;
            labelZ21Port.Text = "Z21 Port:";
            // 
            // textBox_Z21Port
            // 
            textBox_Z21Port.Location = new Point(168, 526);
            textBox_Z21Port.Name = "textBox_Z21Port";
            textBox_Z21Port.Size = new Size(127, 23);
            textBox_Z21Port.TabIndex = 12;
            textBox_Z21Port.Text = "21105";
            // 
            // Form_login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(484, 581);
            Controls.Add(textBox_Z21Port);
            Controls.Add(labelZ21Port);
            Controls.Add(textBox_Z21Ip);
            Controls.Add(labelZ21Ip);
            Controls.Add(textBox_Port);
            Controls.Add(label3);
            Controls.Add(checkBox_RememberMe);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form_login";
            Text = "Bejelentkezés";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox textBox1;
        private TextBox textBox2;
        private Button button2;
        private Label label1;
        private Label label2;
        private CheckBox checkBox_RememberMe;
        private Label label3;
        private TextBox textBox_Port;
        private Label labelZ21Ip;
        private TextBox textBox_Z21Ip;
        private Label labelZ21Port;
        private TextBox textBox_Z21Port;
    }
}