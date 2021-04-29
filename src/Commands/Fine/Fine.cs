using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;

namespace PSPoliceUtilities.Commands.Fine
{
    public class Fine : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, string[] command)
        {
            var unturnedPlayer = (UnturnedPlayer) caller;

            var finedPlayer = UnturnedPlayer.FromName(command[0]);
            var amountBool = decimal.TryParse(command[1], out decimal finedAmount);
            var fineReason = string.Join(" ", command.Skip(2));

            // Return if found no matching player
            if (finedPlayer == null || command.Length < 1)
            {
                UnturnedChat.Say(caller, "That player was not found!", true);
                return;
            }
            
            // Return if specified amount is not a decimal
            if (!amountBool || command.Length < 2)
            {
                UnturnedChat.Say(caller, "Incorrect specification amount.", true);
                return;
            }
            
            // Specify "N/A" if there is no reason
            if (fineReason.Length < 1 || command.Length < 3)
            {
                fineReason = "N/A";
            }
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "fine";
        public string Help => "Fines a player.";
        public string Syntax => "<Player> <Amount> [Reason]";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "ps.policeutils.fine" };
    }
}