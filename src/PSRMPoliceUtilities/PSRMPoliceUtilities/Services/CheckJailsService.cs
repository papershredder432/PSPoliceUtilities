using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PSRMPoliceUtilities.Models;
using PSRMPoliceUtilities.Storage;
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

        private static IEnumerator CheckJail()
        {
            bool isFound = false;
            JailTime players = new JailTime();
            
            Logger.LogWarning(DateTime.UtcNow.ToString());

            while (PSRMPoliceUtilities.Instance.IsPluginLoaded)
            {
                yield return new WaitForSeconds((float) Convert.ToDouble(PSRMPoliceUtilities.Instance.Configuration.Instance.CheckInterval));

                Logger.LogWarning($"{PSRMPoliceUtilities.Instance.JailTimesDatabase.Data.Count} player(s) in jail!");

                foreach (var jailedPlayers in PSRMPoliceUtilities.Instance.JailTimesDatabase.Data.Where(jailedPlayers => jailedPlayers.ExpireDate <= DateTime.UtcNow))
                {
                    isFound = true;
                    players = jailedPlayers;
                    break;
                }

                if (!isFound) continue;
                PSRMPoliceUtilities.Instance.JailTimesDatabase.Data.Remove(players);
                UnturnedPlayer.FromCSteamID((CSteamID) Convert.ToUInt64(players.PlayerId)).Teleport(new Vector3(PSRMPoliceUtilities.Instance.Configuration.Instance.RelaseLocation.x, PSRMPoliceUtilities.Instance.Configuration.Instance.RelaseLocation.x, PSRMPoliceUtilities.Instance.Configuration.Instance.RelaseLocation.z), 0);

            }
        }
    }
}
