using System;
using System.Windows.Forms;

namespace minitrains
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Show login first; only open main form when login returns OK.
            using (var login = new Form_login())
            {
                var result = login.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Application.Run(new Form_vezetes(login.LoggedInUserId, login.RememberMeChecked));
                }
                // otherwise exit — do not open Form_vezetes
            }
        }
    }
}
