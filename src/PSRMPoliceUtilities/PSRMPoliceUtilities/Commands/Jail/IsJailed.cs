using System;
using System.Collections.Generic;
using System.Linq;
using PSRMPoliceUtilities.Models;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace PSRMPoliceUtilities.Commands.JailCommands
{
    public class IsJailed : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = (UnturnedPlayer) caller;

            var jailedPlayer = UnturnedPlayer.FromName(command[0]);
            
            if (command.Length < 1)
            {
                ChatManager.serverSendMessage($"Incorrect usage of command.", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            if (jailedPlayer == null)
            {
                ChatManager.serverSendMessage($"Player does not exist.", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }
            
            JailTime jailedTime = new JailTime();
            if (PSRMPoliceUtilities.Instance.JailTimeService.IsPlayerJailed(jailedPlayer.CSteamID.ToString(), out jailedTime))
            {
                if (jailedTime == null) return;
                var jail = PSRMPoliceUtilities.Instance.Configuration.Instance.Jails.FirstOrDefault(x => x.Name == jailedTime.JailName);

                ChatManager.serverSendMessage($"{jailedPlayer.CharacterName} is in jail for {jailedTime.Reason} at {jail.Name} for {jailedTime.ExpireDate - DateTime.UtcNow}", Color.green, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
            }
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "isjailed";
        public string Help => "Check if someone is jailed.";
        public string Syntax => "/isjailed <player>";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "ps.policeutilities.isjailed" };
    }
}