using System;
using System.Threading;
using PSPoliceUtilities.Models;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace PSPoliceUtilities
{
    public class PSPoliceUtilities : RocketPlugin<PSPoliceUtilitiesConfiguration>
    {
        public static PSPoliceUtilities Instance { get; private set; }
        public DatabaseManager DatabaseManager { get; private set; }

        protected override void Load()
        {
            Instance = this;
            
            DatabaseManager = new DatabaseManager();

            UnturnedPlayerEvents.OnPlayerUpdatePosition += PlayerUpdatePosition;
            UnturnedPlayerEvents.OnPlayerDeath += UnturnedPlayerEventsOnOnPlayerDeath;
        }

        private void PlayerUpdatePosition(UnturnedPlayer player, Vector3 position)
        {
            if (!DatabaseManager.IsPlayerJailed(long.Parse(player.CSteamID.ToString()))) return;

            var jailPos = DatabaseManager.GetJailPos(long.Parse(player.CSteamID.ToString())).Split(',');
            Vector3 jailVector = new Vector3(float.Parse(jailPos[0]), float.Parse(jailPos[1]), float.Parse(jailPos[2]));

            float.TryParse(DatabaseManager.GetJailRadius(long.Parse(player.CSteamID.ToString())), out float radius);
            float jailRadius = Vector3.Distance(jailVector, position);

            if (!(jailRadius > radius)) return;
            player.Teleport(jailVector, 0f);
        }
        
        private void UnturnedPlayerEventsOnOnPlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer)
        {
            if (!DatabaseManager.IsPlayerJailed(long.Parse(player.CSteamID.ToString()))) return;

            var jailPos = DatabaseManager.GetJailPos(long.Parse(player.CSteamID.ToString())).Split(',');
            Vector3 jailVector = new Vector3(float.Parse(jailPos[0]), float.Parse(jailPos[1]), float.Parse(jailPos[2]));
            
            player.Teleport(jailVector, 0f);
        }

        protected override void Unload()
        {
            Instance = null;

            UnturnedPlayerEvents.OnPlayerUpdatePosition -= PlayerUpdatePosition;
            UnturnedPlayerEvents.OnPlayerDeath += UnturnedPlayerEventsOnOnPlayerDeath;
        }

        #region Jailing Events
        public delegate void EventPlayerJailed(UnturnedPlayer unturnedPlayer, int duration, int jailId, string reason);
        public event EventPlayerJailed OnPlayerJailed;

        public delegate void EventPlayerUnjailed(UnturnedPlayer unturnedPlayer, int duration, int jailId, string reason);
        public event EventPlayerUnjailed OnPlayerUnjailed;
        #endregion
        
        #region Jailing
        public void JailPlayer(UnturnedPlayer unturnedPlayer, int duration, int jailId)
        {
            JailPlayer(unturnedPlayer, duration, jailId, "N/A");
        }
        
        public void JailPlayer(UnturnedPlayer unturnedPlayer, int duration, int jailId, string reason)
        {
            var jailPos = DatabaseManager.GetJailPos(jailId).Split(',');
            var releasePos = DatabaseManager.GetReleasePos(jailId).Split(',');
            
            Vector3 jailVector = new Vector3(float.Parse(jailPos[0]), float.Parse(jailPos[1]), float.Parse(jailPos[2]));
            Vector3 releaseVector = new Vector3(float.Parse(releasePos[0]), float.Parse(releasePos[1]), float.Parse(releasePos[2]));

            new Thread(() =>
            {
                var jailTime = new JailTime()
                {
                    //CaseID = 1,
                    Active = true,
                    PlayerID = long.Parse(unturnedPlayer.CSteamID.ToString()),
                    ExpireDate = DateTime.Now.AddSeconds(duration),
                    Reason = reason
                };

                OnPlayerJailed?.Invoke(unturnedPlayer, duration, jailId, reason);
                
                UnturnedChat.Say(unturnedPlayer, $"You have been jailed for {duration} seconds!");
                unturnedPlayer.Teleport(jailVector, 0);
                
                DatabaseManager.JailPlayer(jailTime.Active, jailTime.PlayerID, jailTime.JailID, jailTime.ExpireDate, jailTime.Reason);
                
                Thread.Sleep(duration * 1000);

                OnPlayerUnjailed?.Invoke(unturnedPlayer, duration, jailId, reason);
                
                UnturnedChat.Say(unturnedPlayer, $"You have been released from jail.");
                unturnedPlayer.Teleport(releaseVector, 0);
            })
            {
                IsBackground = true
            }.Start();
        }
        #endregion

        public override TranslationList DefaultTranslations => new TranslationList
        {
            
        };
    }
}