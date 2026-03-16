using System.IO;

namespace minitrains
{
    public static class GlobalConfig
    {
        public static string DbPort = "3306";
        public static string Z21IP = "192.168.0.111";
        public static string Z21Port = "21105";

        private const string DbConfigFile = "db_port.dat";
        private const string Z21IpConfigFile = "z21_ip.dat";
        private const string Z21PortConfigFile = "z21_port.dat";

        public static void Load()
        {
            if (File.Exists(DbConfigFile))
            {
                DbPort = File.ReadAllText(DbConfigFile).Trim();
            }
            if (File.Exists(Z21IpConfigFile))
            {
                Z21IP = File.ReadAllText(Z21IpConfigFile).Trim();
            }
            if (File.Exists(Z21PortConfigFile))
            {
                Z21Port = File.ReadAllText(Z21PortConfigFile).Trim();
            }
        }

        public static void Save(string dbPort, string z21Ip, string z21Port)
        {
            DbPort = dbPort;
            File.WriteAllText(DbConfigFile, dbPort);

            Z21IP = z21Ip;
            File.WriteAllText(Z21IpConfigFile, z21Ip);

            Z21Port = z21Port;
            File.WriteAllText(Z21PortConfigFile, z21Port);
        }

        public static string GetConnectionString()
        {
            return $"Server=localhost;Port={DbPort};Database=modellvasut;user=root;password=;";
        }
    }
}
