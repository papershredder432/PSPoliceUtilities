using Rocket.API;

namespace PSPoliceUtilities
{
    public class PSPoliceUtilitiesConfiguration : IRocketPluginConfiguration
    {
        public MySqlConnection mySqlConnection = new MySqlConnection();
        public class MySqlConnection
        {
            public string DatabaseAddress { get; set; }
            public string DatabaseName { get; set; }
            public string DatabaseUsername { get; set; }
            public string DatabasePassword { get; set; }
            public int DatabasePort { get; set; }
        }

        public Jail jail = new Jail();
        public class Jail
        {
            public string DatabaseTableName { get; set; }
            public int MinJailDuration { get; set; }
            public int MaxJailDuration { get; set; }
        }

        public JailedPlayers jailedPlayers = new JailedPlayers();
        public class JailedPlayers
        {
            public string DatabaseTableName { get; set; }
        }

        public Fine fine = new Fine();
        public class Fine
        {
            public bool UseUconomy { get; set; }
            public string DatabaseTableName { get; set; }
            public decimal MinFineAmount { get; set; }
            public decimal MaxFineAmount { get; set; }
        }
        
        public void LoadDefaults()
        {
            mySqlConnection.DatabaseAddress = "127.0.0.1";
            mySqlConnection.DatabaseName = "unturned";
            mySqlConnection.DatabaseUsername = "unturned";
            mySqlConnection.DatabasePassword = "password";
            mySqlConnection.DatabasePort = 3306;

            jail.DatabaseTableName = "Jails";
            jail.MinJailDuration = 30;
            jail.MaxJailDuration = 60000;

            jailedPlayers.DatabaseTableName = "JailedPlayers";

            fine.UseUconomy = true;
            fine.DatabaseTableName = "Fines";
            fine.MinFineAmount = 20;
            fine.MaxFineAmount = 10000;
        }
    }
}