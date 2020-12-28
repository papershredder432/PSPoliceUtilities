using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using PSRMPoliceUtilities.Models;
using PSRMPoliceUtilities.Storage;

namespace PSRMPoliceUtilities.Database
{
    public class FinesDatabase : Database<Fine>
    {
        public void AddFine(Fine fine)
        {
            Collection.Insert(fine);
        }

        public void DeactivateFine(Fine fine)
        {
            fine.Active = false;

            Collection.Update(fine);
        }

        public List<Fine> FindActiveFines(string id)
        {
            return Collection.Find(x => x.Active && x.PlayerId == id).ToList();
        }

        public List<Fine> FindInactiveFines(string id)
        {
            return Collection.Find(x => !x.Active && x.PlayerId == id).ToList();
        }

        public void FinePlayer(string playerid, decimal amount, string reason = "")
        {
            Collection.Insert(new Fine()
            {
                PlayerId = playerid,
                Active = true,
                FinedAmount = amount,
                FinedDate = DateTime.Now,
                FineID = ObjectId.NewObjectId(),
                Reason = reason,
                CaseID = Collection.Count() + 1
            });
        }
    }
}