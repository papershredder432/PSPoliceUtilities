using System;
using System.Collections.Generic;
using System.Linq;
using PSRMPoliceUtilities.Database;
using PSRMPoliceUtilities.Models;
using UnityEngine;

namespace PSRMPoliceUtilities.Services
{
    public class FinesService : MonoBehaviour
    {
        public Dictionary<string, DateTime> Fines { get; private set; }

        private FinesDatabase Database => PSRMPoliceUtilities.Instance.FinesDatabase;

        void Awake()
        {
            Fines = new Dictionary<string, DateTime>();
        }

        void Start()
        {
            
        }

        private void OnDestroy()
        {
            
        }

        public void RegisterFine(string playerId, int amount, string reason)
        {
            var fine = new Fine()
            {
                PlayerId = playerId,
                FinedDate = DateTime.Now,
                FinedAmount = amount,
                Reason = reason
            };
            
            Database.AddFine(fine);
        }

        public void RemoveFine(string playerId)
        {
            var fine = Database.Data.Single(x => x.PlayerId == playerId);

            Database.RemoveFine(fine);
        }
        
        public bool ActiveFine(string playerId, out Fine fine)
        {
            if (Database.Data.Exists(x => x.PlayerId == playerId))
            {
                fine = Database.Data.FirstOrDefault(x => x.PlayerId == playerId);
                return true;
            }

            fine = null;
            return false;
        }

        public int AmountFines(string playerId, out int amount)
        {
            if (Database.Data.Exists(x => x.PlayerId == playerId))
            {
                amount = Database.Data.FindAll(x => x.PlayerId == playerId).Count;
                return amount;
            }

            amount = 0;
            return amount;
        }
    }
}