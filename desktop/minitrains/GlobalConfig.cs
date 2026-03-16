using System.IO;

namespace minitrains
{
    public static class GlobalConfig
    {
        public static string DbPort = "3306";
        private const string ConfigFile = "db_port.dat";

        public static void Load()
        {
            if (File.Exists(ConfigFile))
            {
                DbPort = File.ReadAllText(ConfigFile).Trim();
            }
        }

        public static void Save(string port)
        {
            DbPort = port;
            File.WriteAllText(ConfigFile, port);
        }

        public static string GetConnectionString()
        {
            return $"Server=localhost;Port={DbPort};Database=modellvasut;user=root;password=;";
        }
    }
}
