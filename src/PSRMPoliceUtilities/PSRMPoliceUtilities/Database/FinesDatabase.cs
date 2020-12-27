using System.Collections.Generic;
using PSRMPoliceUtilities.Models;
using PSRMPoliceUtilities.Storage;

namespace PSRMPoliceUtilities.Database
{
    public class FinesDatabase
    {
        private DataStorage<List<Fine>> DataStorage { get; set; }
        
        public List<Fine> Data { get; private set; }

        public FinesDatabase()
        {
            DataStorage = new DataStorage<List<Fine>>(PSRMPoliceUtilities.Instance.Directory, "Fines.json");
        }

        public void Reload()
        {
            Data = DataStorage.Read();
            if (Data == null)
            {
                Data = new List<Fine>();
                DataStorage.Save(Data);
            }
        }

        public void AddFine(Fine fine)
        {
            Data.Add(fine);
            DataStorage.Save(Data);
        }

        public void RemoveFine(Fine fine)
        {
            Data.Remove(fine);
            DataStorage.Save(Data);
        }
    }
}