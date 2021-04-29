using System;
using System.Collections.Generic;
using System.Linq;
using PSRMPoliceUtilities.Models;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace PSRMPoliceUtilities.Commands.JailCommands
{
    public class Jail : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = (UnturnedPlayer) caller;

            var jailedPlayer = UnturnedPlayer.FromName(command[0]);
            var jailName = PSRMPoliceUtilities.Instance.Configuration.Instance.Jails.FirstOrDefault(x => x.Name.ToLower() == command[1].ToLower());
            var jailTime = Convert.ToDouble(command[2]);
            var jailReason = string.Join(" ", command[3]);
            
            if (command.Length <= 1)
            {
                ChatManager.serverSendMessage($"Incorrect usage of command.", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            if (jailedPlayer == null)
            {
                ChatManager.serverSendMessage($"Player does not exist.", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            if (jailedPlayer.IsAdmin || jailedPlayer.HasPermission("ps.policeutilities.jailimmune"))
            {
                ChatManager.serverSendMessage($"You cannot jail that player!", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            if (jailName == null)
            {
                ChatManager.serverSendMessage($"Jail does not exist.", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            if (jailTime <= 1)
            {
                ChatManager.serverSendMessage($"Invalid time in seconds.", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            if (jailReason.Length < 1)
            {
                jailReason = "N/A";
            }
            
            JailTime jailedTime = new JailTime();
            if (PSRMPoliceUtilities.Instance.JailTimeService.IsPlayerJailed(jailedPlayer.CSteamID.ToString(), out jailedTime))
            {
                if (jailedTime == null) return;
                var jail = PSRMPoliceUtilities.Instance.Configuration.Instance.Jails.FirstOrDefault(x => x.Name == jailedTime.JailName);
                ChatManager.serverSendMessage($"{jailedPlayer} is already in jail at {jailedTime.JailName} for {jailedTime.Reason}", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            PSRMPoliceUtilities.Instance.JailTimeService.RegisterJailedUser(jailedPlayer.CSteamID.ToString(), jailName, jailTime, jailReason);
            jailedPlayer.Teleport(new Vector3(jailName.X, jailName.Y, jailName.Z), 0f);
            ChatManager.serverSendMessage($"{unturnedPlayer.CharacterName} jailed {jailedPlayer.CharacterName} for {jailReason} at {jailName.Name} for {jailTime}!", Color.yellow, null, null, EChatMode.GLOBAL, null, true);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "jail";
        public string Help => "Sends a player to jail.";
        public string Syntax => "/jail <player> <jailname> <length> [Reason]";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "ps.policeutilities.jail" };
    }
}