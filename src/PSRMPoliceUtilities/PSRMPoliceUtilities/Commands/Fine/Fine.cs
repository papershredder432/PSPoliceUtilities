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
    public class Fine : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = (UnturnedPlayer) caller;

            var finedPlayer = UnturnedPlayer.FromName(command[0]);
            var fineReason = string.Join(" ", command.Skip(2));
            
            if (command.Length <= 1)
            {
                ChatManager.serverSendMessage($"Incorrect usage of command.", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            if (finedPlayer == null)
            {
                ChatManager.serverSendMessage($"Player does not exist.", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            if (finedPlayer.IsAdmin || finedPlayer.HasPermission("ps.policeutilities.fineimmune"))
            {
                ChatManager.serverSendMessage($"You cannot fine that player!", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            if (!Decimal.TryParse(command[1], out var fineAmount))
            {
                ChatManager.serverSendMessage($"Invalid amount to fine.", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            if (fineReason.Length < 1)
            {
                fineReason = "N/A";
            }

            PSRMPoliceUtilities.Instance.FinesDatabase.FinePlayer(finedPlayer.CSteamID.ToString(), fineAmount, fineReason);
            ChatManager.serverSendMessage($"{unturnedPlayer.CharacterName} fined {finedPlayer.CharacterName} for {fineReason} for {fineAmount} {Uconomy.Instance.Configuration.Instance.MoneyName}(s)!", Color.yellow, null, null, EChatMode.GLOBAL, null, true);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "fine";
        public string Help => "Fines a player.";
        public string Syntax => "/fine <Player> <Amount> [Reason]";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "ps.policeutilities.fine" };
    }
}