using System.Collections.Generic;
using PSRMPoliceUtilities.Models;
using PSRMPoliceUtilities.Storage;

namespace PSRMPoliceUtilities.Database
{
    public class JailTimesDatabase
    {
        private DataStorage<List<JailTime>> DataStorage { get; set; }
        
        public List<JailTime> Data { get; private set; }

        public JailTimesDatabase()
        {
            DataStorage = new DataStorage<List<JailTime>>(PSRMPoliceUtilities.Instance.Directory, "JailTimes.json");
        }

        public void Reload()
        {
            Data = DataStorage.Read();
            if (Data == null)
            {
                Data = new List<JailTime>();
                DataStorage.Save(Data);
            }
        }

        public void AddJailTime(JailTime time)
        {
            Data.Add(time);
            DataStorage.Save(Data);
        }

        public void RemoveJailTime(JailTime time)
        {
            Data.Remove(time);
            DataStorage.Save(Data);
        }
    }
}