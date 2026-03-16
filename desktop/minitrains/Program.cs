using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Windows.Forms;



namespace minitrains
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            GlobalConfig.Load();

            using (var loginForm = new Form_login())
            {
                var result = loginForm.ShowDialog();
                


                if (result == DialogResult.OK)
                {
                    
                    Application.Run(
                        new Form_vezetes(
                            loginForm.CurrentUserId,
                            loginForm.RememberMe));

                }
                else
                {
                    
                    Application.Exit();
                }
            }
        }
    }
    
}