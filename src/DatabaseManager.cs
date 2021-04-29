using System;
using I18N.West;
using MySql.Data.MySqlClient;
using Rocket.Core.Logging;

namespace PSPoliceUtilities
{
    public class DatabaseManager
    {
        internal DatabaseManager()
        {
            new CP1250();
            CheckSchema();
        }

        #region Jail Positions
        public string GetJailPos(int jailId)
        {
            string output = "";

            var result = ExecuteQuery(true,
                $"select `location` from `{PSPoliceUtilities.Instance.Configuration.Instance.jail.DatabaseTableName}` where `id` = `{jailId}`");
            
            return output;
        }

        public string GetJailPos(long playerId)
        {
            string output = "";

            var result = ExecuteQuery(true,
                $"select `location` from `{PSPoliceUtilities.Instance.Configuration.Instance.jail.DatabaseTableName}` where `playerId` = `{playerId}`");
            
            return output;
        }
        #endregion

        #region Release Positions
        public string GetReleasePos(int jailId)
        {
            string output = "";

            var result = ExecuteQuery(true,
                $"select `releaseLocation` from `{PSPoliceUtilities.Instance.Configuration.Instance.jail.DatabaseTableName}` where `id` = `{jailId}`");
            
            return output;
        }

        public string GetReleasePos(long playerId)
        {
            string output = "";

            var result = ExecuteQuery(true,
                $"select `releaseLocation` from `{PSPoliceUtilities.Instance.Configuration.Instance.jail.DatabaseTableName}` where `playerId` = `{playerId}`");
            
            return output;
        }
        #endregion
        
        #region Jail Radii
        public string GetJailRadius(int jailId)
        {
            string output = "";

            var result = ExecuteQuery(true,
                $"select `radius` from `{PSPoliceUtilities.Instance.Configuration.Instance.jail.DatabaseTableName}` where `id` = `{jailId}`");
            
            return output;
        }

        public string GetJailRadius(long playerId)
        {
            string output = "";

            var result = ExecuteQuery(true,
                $"select `radius` from `{PSPoliceUtilities.Instance.Configuration.Instance.jail.DatabaseTableName}` where `playerId` = `{playerId}`");
            
            return output;
        }
        #endregion

        public void JailPlayer(bool active, long playerId, int jailId, DateTime expireDate, string reason)
        {
            var result = ExecuteQuery(true,
                $"insert ignore into `{PSPoliceUtilities.Instance.Configuration.Instance.jailedPlayers.DatabaseTableName}` " +
                $"(active,playerId,jailId,expireDate,reason)" +
                $"values({active},{playerId},{jailId},{expireDate},{reason})");
        }

        public bool IsPlayerJailed(long playerId)
        {
            var exists = 0;

            var result = ExecuteQuery(true,
                $"SELECT EXISTS(SELECT 1 FROM `{PSPoliceUtilities.Instance.Configuration.Instance.jailedPlayers.DatabaseTableName}` WHERE `playerId` = '{playerId}' AND `active` = '1');");

            if (result != null) int.TryParse(result.ToString(), out exists);
            
            if (exists == 0) return false;

            return true;
        }

        private MySqlConnection CreateConnection()
        {
            MySqlConnection connection = null;

            try
            {
                connection = new MySqlConnection(
                    $"SERVER={PSPoliceUtilities.Instance.Configuration.Instance.mySqlConnection.DatabaseAddress};" +
                    $"DATABASE={PSPoliceUtilities.Instance.Configuration.Instance.mySqlConnection.DatabaseName};" +
                    $"UID={PSPoliceUtilities.Instance.Configuration.Instance.mySqlConnection.DatabaseUsername};" +
                    $"PASSWORD={PSPoliceUtilities.Instance.Configuration.Instance.mySqlConnection.DatabasePassword};" +
                    $"PORT={PSPoliceUtilities.Instance.Configuration.Instance.mySqlConnection.DatabasePort}");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return connection;
        }

        private void CheckSchema()
        {
            var jailTable = ExecuteQuery(true,
                $"show tables like '{PSPoliceUtilities.Instance.Configuration.Instance.jail.DatabaseTableName}'");

            var jailedPlayersTable = ExecuteQuery(true,
                $"show tables like '{PSPoliceUtilities.Instance.Configuration.Instance.jailedPlayers.DatabaseTableName}'");

            var fineTable = ExecuteQuery(true,
                $"show tables like '{PSPoliceUtilities.Instance.Configuration.Instance.fine.DatabaseTableName}'");

            if (jailTable == null)
            {
                ExecuteQuery(false,
                    $"CREATE TABLE `{PSPoliceUtilities.Instance.Configuration.Instance.jail.DatabaseTableName}` (" +
                    $"`id` INT NOT NULL AUTO_INCREMENT," +
                    $"`name` VARCHAR(64) NOT NULL," +
                    $"`maxCapacity` INT(99) NOT NULL," +
                    $"`radius` FLOAT(24) NOT NULL," +
                    $"`location` VARCHAR(128) NOT NULL," +
                    $"`releaseLocation` VARCHAR(128) NOT NULL," +
                    $"PRIMARY KEY (`id`));");
                
                Logger.Log($"Created table: `{PSPoliceUtilities.Instance.Configuration.Instance.jail.DatabaseTableName}`");
            }

            if (jailedPlayersTable == null)
            {
                ExecuteQuery(false,
                    $"CREATE TABLE `{PSPoliceUtilities.Instance.Configuration.Instance.jailedPlayers.DatabaseTableName}` (" +
                    $"`caseId` INT NOT NULL AUTO_INCREMENT," +
                    $"`active` BOOLEAN NOT NULL," +
                    $"`playerId` VARCHAR(32) NOT NULL," +
                    $"`jailId` INT(99) NOT NULL," +
                    $"`expireDate` DATETIME NOT NULL," +
                    $"`reason` VARCHAR(255) NOT NULL," +
                    $"PRIMARY KEY (`playerId`));");
                
                Logger.Log($"Created table: `{PSPoliceUtilities.Instance.Configuration.Instance.jailedPlayers.DatabaseTableName}`");
            }

            if (fineTable == null)
            {
                ExecuteQuery(false,
                    $"CREATE TABLE `{PSPoliceUtilities.Instance.Configuration.Instance.fine.DatabaseTableName}` (" +
                    $"`caseId` INT NOT NULL AUTO_INCREMENT," +
                    $"`active` BOOLEAN NOT NULL," +
                    $"`playerId` VARCHAR(32) NOT NULL," +
                    $"`date` DATETIME NOT NULL," +
                    $"`amount` DECIMAL(64) NOT NULL," +
                    $"`reason` VARCHAR(255) NOT NULL," +
                    $"PRIMARY KEY (`playerId`));");
                
                Logger.Log($"Created table: `{PSPoliceUtilities.Instance.Configuration.Instance.fine.DatabaseTableName}`");
            }
        }

        private object ExecuteQuery(bool isScalar, string query)
        {
            var connection = CreateConnection();
            object result = null;

            try
            {
                // Initialize command within try context, and execute within it as well.
                var command = connection.CreateCommand();
                command.CommandText = query;

                connection.Open();
                if (isScalar)
                    result = command.ExecuteScalar();
                else
                    command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Catch and log any errors during execution, like connection or similar.
                Logger.LogException(ex);
            }
            finally
            {
                // No matter what happens, close the connection at the end of execution.
                connection.Close();
            }

            return result;
        }
    }
}