namespace minitrains
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            // Provide required arguments for Form_vezetes constructor
            int userId = 0; // Replace with actual user id as needed
            bool rememberMe = false; // Replace with actual value as needed
            Application.Run(new Form_vezetes(userId, rememberMe));
        }
    }
}