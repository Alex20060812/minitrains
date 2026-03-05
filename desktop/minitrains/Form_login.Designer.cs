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
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(168, 360);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(127, 23);
            textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(168, 413);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(127, 23);
            textBox2.TabIndex = 2;
            // 
            // button2
            // 
            button2.Font = new Font("Bahnschrift", 9.75F, FontStyle.Bold);
            button2.Location = new Point(341, 408);
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
            label1.Location = new Point(49, 363);
            label1.Name = "label1";
            label1.Size = new Size(113, 18);
            label1.TabIndex = 4;
            label1.Text = "Felhasználónév:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Bahnschrift", 11.25F, FontStyle.Bold);
            label2.Location = new Point(49, 413);
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
            // Form_login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 461);
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
    }
}