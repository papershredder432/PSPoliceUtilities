using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

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
            while (PSRMPoliceUtilities.Instance.IsPluginLoaded)
            {
                yield return new WaitForSeconds((float) Convert.ToDouble(PSRMPoliceUtilities.Instance.Configuration.Instance.CheckInterval));

                //Logger.Log("Checking jails database...");
                //Logger.Log($"{PSRMPoliceUtilities.Instance.JailTimesDatabase.Data.Count} player(s) found in jail.");

                foreach (var jailedPlayer in PSRMPoliceUtilities.Instance.JailTimesDatabase.Data.ToList().Where(jailedPlayer => jailedPlayer.ExpireDate <= DateTime.Now))
                {
                    PSRMPoliceUtilities.Instance.JailTimeService.RemoveJailedUser(jailedPlayer.PlayerId);
                    ChatManager.serverSendMessage($"{UnturnedPlayer.FromCSteamID((CSteamID) Convert.ToUInt64(jailedPlayer.PlayerId)).CharacterName} was automatically released from {jailedPlayer.JailName}.", Color.blue, null, null, EChatMode.GLOBAL, null, true);
                    UnturnedPlayer.FromCSteamID((CSteamID) Convert.ToUInt64(jailedPlayer.PlayerId)).Teleport(new Vector3(PSRMPoliceUtilities.Instance.Configuration.Instance.RelaseLocation.x, PSRMPoliceUtilities.Instance.Configuration.Instance.RelaseLocation.x, PSRMPoliceUtilities.Instance.Configuration.Instance.RelaseLocation.z), 0);
                }
            }
        }
    }
}