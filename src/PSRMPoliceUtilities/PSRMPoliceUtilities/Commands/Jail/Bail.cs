using System;
using System.Collections.Generic;
using fr34kyn01535.Uconomy;
using PSRMPoliceUtilities.Models;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace PSRMPoliceUtilities.Commands.JailCommands
{
    public class Bail : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = (UnturnedPlayer) caller;

            var jailedPlayer = UnturnedPlayer.FromName(command[0]);

            if (command[0].Length < 1) jailedPlayer = unturnedPlayer;

                if (jailedPlayer == null)
            {
                ChatManager.serverSendMessage($"Player does not exist.", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }
            
            JailTime jailedTime = new JailTime();
            if (!PSRMPoliceUtilities.Instance.JailTimeService.IsPlayerJailed(jailedPlayer.CSteamID.ToString(), out jailedTime)) return;
            if (jailedTime == null)
            {
                ChatManager.serverSendMessage($"{jailedPlayer} is not in jail.", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            var currentCredits = Uconomy.Instance.Database.GetBalance(jailedPlayer.CSteamID.ToString());
            var requiredCredits = PSRMPoliceUtilities.Instance.Configuration.Instance.CreditsPerMinute * (jailedTime.ExpireDate - DateTime.UtcNow).Minutes;
            if (currentCredits < requiredCredits)
            {
                ChatManager.serverSendMessage($"You need {requiredCredits} {Uconomy.Instance.Configuration.Instance.MoneyName}, but you only have {currentCredits} {Uconomy.Instance.Configuration.Instance.MoneyName}!", Color.red, null, null, EChatMode.GLOBAL, null, true);
                return;
            }

            Uconomy.Instance.Database.IncreaseBalance(unturnedPlayer.CSteamID.ToString(), -requiredCredits);
            ChatManager.serverSendMessage($"You bailed {jailedPlayer.CharacterName} for {requiredCredits} {Uconomy.Instance.Configuration.Instance.DatabaseName}.", Color.red, null, null, EChatMode.GLOBAL, null, true);

            ChatManager.serverSendMessage($"{unturnedPlayer.CharacterName} bailed {jailedPlayer.CharacterName} from {jailedTime.JailName}", Color.blue, null, null, EChatMode.GLOBAL, null, true);
            
            jailedPlayer.Teleport(new Vector3(PSRMPoliceUtilities.Instance.Configuration.Instance.RelaseLocation.x, PSRMPoliceUtilities.Instance.Configuration.Instance.RelaseLocation.x, PSRMPoliceUtilities.Instance.Configuration.Instance.RelaseLocation.z), 0);
            PSRMPoliceUtilities.Instance.JailTimesDatabase.Data.Remove(jailedTime);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "isjailed";
        public string Help => "Check if someone is jailed.";
        public string Syntax => "/bail [player]";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "ps.policeutilities.bail" };
    }
}