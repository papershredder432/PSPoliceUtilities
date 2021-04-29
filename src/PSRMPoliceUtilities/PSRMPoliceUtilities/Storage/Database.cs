using System;
using System.IO;
using LiteDB;
using Rocket.Core.Logging;

namespace PSRMPoliceUtilities.Storage
{
    public class Database<T>
    {
        public string Path;
        public ILiteCollection<T> Collection;

        private LiteDatabase _database;
        
        public Database()
        {
            var databaseFolderPath = string.Concat(new string[]
            {
                Directory.GetCurrentDirectory(),
                System.IO.Path.DirectorySeparatorChar.ToString(),
                "Database",
                System.IO.Path.DirectorySeparatorChar.ToString()
            });
            if (!Directory.Exists(databaseFolderPath))
            {
                Directory.CreateDirectory(databaseFolderPath);
            }
            Path = databaseFolderPath + GetType().Name;
            _database = new LiteDatabase(Path);
            Collection = _database.GetCollection<T>(GetType().Name);
            Logger.Log($"{GetType().Name} database loaded.", ConsoleColor.Green);
        }

        ~Database()
        {
            _database.Dispose();
        }
    }
}