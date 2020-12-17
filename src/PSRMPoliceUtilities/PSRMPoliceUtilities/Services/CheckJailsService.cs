using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PSRMPoliceUtilities.Models;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace PSRMPoliceUtilities.Services
{
    public class CheckJailsService : MonoBehaviour
    {
        public Dictionary<string, DateTime> JailTimes { get; private set; }
        
        private void Start()
        {
            StartCoroutine(CheckJail());
        }
        
        private void OnDestroy()
        {
            
        }

        IEnumerator CheckJail()
        {
            JailTime jailedTime = new JailTime();
            Logger.LogWarning(DateTime.UtcNow.ToString());
            
            yield return new WaitForSeconds(Convert.ToSingle(PSRMPoliceUtilities.Instance.Configuration.Instance.CheckInterval));
            
            foreach (var jailedPlayers in PSRMPoliceUtilities.Instance.JailTimes)
            {
                if (PSRMPoliceUtilities.Instance.JailTimeService.IsPlayerJailed(jailedPlayers.Key, out jailedTime))
                {
                    if (jailedTime == null) yield break;
                    var jail = PSRMPoliceUtilities.Instance.Configuration.Instance.Jails.FirstOrDefault(x => x.Name == jailedTime.JailName);
                    
                    if (jailedTime.ExpireDate >= DateTime.UtcNow)
                    {
                        PSRMPoliceUtilities.Instance.JailTimesDatabase.RemoveJailTime(jailedTime);
                        Logger.LogWarning($"Removed {jailedTime.PlayerId} from {jailedTime.JailName}");
                    }
                }
            }
            
            Logger.LogWarning(DateTime.UtcNow.ToString());
        }
    }
}