using System;
using System.Collections.Generic;
using fr34kyn01535.Uconomy;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace PSRMPoliceUtilities.Commands.Fine
{
    public class RemoveFine : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = (UnturnedPlayer) caller;
            
            if (command.Length < 1)
            {
                ChatManager.serverSendMessage($"Incorrect usage of command.", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }
            
            if (!Decimal.TryParse(command[0], out var caseId) || !PSRMPoliceUtilities.Instance.FinesDatabase.Collection.Exists(x => x.CaseID == caseId))
            {
                ChatManager.serverSendMessage($"Invalid case ID.", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            Models.Fine selectedFine = PSRMPoliceUtilities.Instance.FinesDatabase.Collection.FindOne(x => x.Active && x.CaseID == caseId);
            PSRMPoliceUtilities.Instance.FinesDatabase.DeactivateFine(selectedFine);
            ChatManager.serverSendMessage($"Deactivated fine with the Case ID of {selectedFine.CaseID}.", Color.blue, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
            ChatManager.serverSendMessage($"Your fine with the Case ID of {selectedFine.CaseID} has been revoked.", Color.blue, null, UnturnedPlayer.FromCSteamID((CSteamID) Convert.ToUInt64(selectedFine.PlayerId)).SteamPlayer(), EChatMode.SAY, null, true);

        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "removefine";
        public string Help => "Deactives a fine.";
        public string Syntax => "/removefine <FineID>";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "ps.policeutilities.removefine" };
    }
}