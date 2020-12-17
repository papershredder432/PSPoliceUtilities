using System;
using System.Collections.Generic;
using System.Linq;
using PSRMPoliceUtilities.Database;
using PSRMPoliceUtilities.Models;
using UnityEngine;

namespace PSRMPoliceUtilities.Services
{
    public class JailTimeService : MonoBehaviour
    {
        public Dictionary<string, DateTime> JailTimes { get; private set; }

        private JailTimesDatabase Database => PSRMPoliceUtilities.Instance.JailTimesDatabase;

        void Awake()
        {
            JailTimes = new Dictionary<string, DateTime>();
        }

        void Start()
        {
            
        }

        private void OnDestroy()
        {
            
        }

        public void RegisterJailedUser(string playerId, Jail jail, double time, string reason)
        {
            var jailTime = new JailTime()
            {
                PlayerId = playerId,
                JailName = jail.Name,
                ExpireDate = DateTime.UtcNow.AddSeconds(time),
                Reason = reason
            };
            
            Database.AddJailTime(jailTime);
            JailTimes[playerId] = DateTime.UtcNow.AddSeconds(time);
        }

        public void RemoveJailedUser(string playerId)
        {
            var jailTime = new JailTime()
            {
                PlayerId = playerId
            };
            
            Database.RemoveJailTime(jailTime);
        }
        
        public bool IsPlayerJailed(string playerId, out JailTime jail)
        {
            if (Database.Data.Exists(x => x.PlayerId == playerId))
            {
                jail = Database.Data.FirstOrDefault(x => x.PlayerId == playerId);
                return true;
            }

            jail = null;
            return false;
        }
    }
}