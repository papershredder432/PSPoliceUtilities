using System;
using System.Collections.Generic;
using System.Linq;
using PSRMPoliceUtilities.Database;
using PSRMPoliceUtilities.Models;
using PSRMPoliceUtilities.Services;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace PSRMPoliceUtilities
{
    public class PSRMPoliceUtilities : RocketPlugin<PSRMPoliceUtilitiesConfiguration>
    {
        public static PSRMPoliceUtilities Instance { get; private set; }
        public JailTimesDatabase JailTimesDatabase { get; private set; }
        public JailTimeService JailTimeService { get; private set; }
        
        public CheckJailsService CheckJailsService { get; private set; }

        public Dictionary<string, DateTime> JailTimes { get; private set; }
        
        public bool IsPluginLoaded { get; private set; }

        protected override void Load()
        {
            Instance = this;
            IsPluginLoaded = true;

            Logger.LogWarning($"{Name} {Assembly.GetName().Version} loaded! Made by papershredder432, join the support Discord here: https://discord.gg/ydjYVJ2");

            UnturnedPlayerEvents.OnPlayerDeath += OnPlayerDeath;
            UnturnedPlayerEvents.OnPlayerUpdatePosition += OnPlayerUpdatePosition;

            JailTimes = new Dictionary<string, DateTime>();
            JailTimesDatabase = new JailTimesDatabase();
            JailTimesDatabase.Reload();

            JailTimeService = gameObject.AddComponent<JailTimeService>();
            CheckJailsService = gameObject.AddComponent<CheckJailsService>();
        }

        private void OnPlayerUpdatePosition(UnturnedPlayer player, Vector3 position)
        {
            JailTime jailTime = new JailTime();
            if (!Instance.JailTimeService.IsPlayerJailed(player.CSteamID.ToString(), out jailTime)) return;
            var jail = Instance.Configuration.Instance.Jails.FirstOrDefault(x => x.Name == jailTime.JailName);

            float radius = Vector3.Distance(new Vector3(jail.X, jail.Y, jail.Z), position);

            if (!(radius > Instance.Configuration.Instance.JailRadius)) return;
            Logger.LogWarning($"{player.CharacterName} tried to get of their jail so they were teleported back.");
            player.Teleport(new Vector3(jail.X, jail.Y, jail.Z), 0f);
        }

        private void OnPlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer)
        {
            JailTime jailTime = new JailTime();
            if (Instance.JailTimeService.IsPlayerJailed(player.CSteamID.ToString(), out jailTime))
            {
                var jail = Instance.Configuration.Instance.Jails.FirstOrDefault(x => x.Name == jailTime.JailName);
                
                Logger.LogWarning($"{player.CharacterName} died while jailed so they were teleported back to their jail");
                player.Teleport(new Vector3(jail.X, jail.Y, jail.Z), 0f);
            }
        }

        protected override void Unload()
        {
            Instance = null;
            IsPluginLoaded = false;
            
            Logger.LogWarning($"{Name} {Assembly.GetName().Version} unloaded.");

            UnturnedPlayerEvents.OnPlayerDeath -= OnPlayerDeath;
            UnturnedPlayerEvents.OnPlayerUpdatePosition -= OnPlayerUpdatePosition;

            Destroy(JailTimeService);
            Destroy(CheckJailsService);
        }

        public override TranslationList DefaultTranslations => new TranslationList
        {
            
        };
    }
}