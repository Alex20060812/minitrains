using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace minitrains
{
    public partial class Form_vonathozzaadas : Form
    {
        public Form_vonathozzaadas()
        {
            InitializeComponent();
        }
        public decimal NumericErtek
        {
            get { return numericUpDown1.Value; }
        }

        public string TextErtek
        {
            get { return textBox1.Text; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Kérem, adjon meg egy nevet!");
                return;
            }
            if (numericUpDown1.Value <= 1)
            {
                MessageBox.Show("Kérem, adjon meg érvényes értéket");
                return;

            }






            this.DialogResult = DialogResult.OK; // ezzel jelzed a fő ablaknak, hogy kész
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
