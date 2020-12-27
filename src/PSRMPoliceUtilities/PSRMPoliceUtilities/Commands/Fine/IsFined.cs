using System;
using System.Collections.Generic;
using System.Linq;
using fr34kyn01535.Uconomy;
using PSRMPoliceUtilities.Models;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace PSRMPoliceUtilities.Commands.Fine
{
    public class IsFined : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = (UnturnedPlayer) caller;

            var finedPlayer = UnturnedPlayer.FromName(command[0]);
            
            if (command.Length < 1)
            {
                ChatManager.serverSendMessage($"Incorrect usage of command.", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            if (finedPlayer == null)
            {
                ChatManager.serverSendMessage($"Player does not exist.", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            int amount = 0;
            Models.Fine fine = new Models.Fine();
            if (!PSRMPoliceUtilities.Instance.FinesService.ActiveFine(finedPlayer.CSteamID.ToString(), out fine)) return;
            if (fine == null) return;
            if (PSRMPoliceUtilities.Instance.FinesService.AmountFines(finedPlayer.CSteamID.ToString(), out amount) > 1)
            {
                ChatManager.serverSendMessage($"{finedPlayer} has {amount} active fine(s).", Color.blue, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }
                
            ChatManager.serverSendMessage($"{finedPlayer} was fined on {fine.FinedDate} for {fine.FinedAmount} {Uconomy.Instance.Configuration.Instance.MoneyName} for {fine.Reason}", Color.blue, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
        }

        public AllowedCaller AllowedCaller { get; }
        public string Name { get; }
        public string Help { get; }
        public string Syntax { get; }
        public List<string> Aliases { get; }
        public List<string> Permissions { get; }
    }
}