using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace PSRMPoliceUtilities.Commands
{
    public class DeleteJail : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = (UnturnedPlayer) caller;
            
            var jails = PSRMPoliceUtilities.Instance.Configuration.Instance.Jails;
            var findJail = jails.FirstOrDefault(x => x.Name.ToLower() == command[0].ToLower());

            if (command[0].Length < 1)
            {
                ChatManager.serverSendMessage($"Incorrect usage of command.", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }
            
            if (findJail == null)
            {
                ChatManager.serverSendMessage($"Jail doesn't exists", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            PSRMPoliceUtilities.Instance.Configuration.Instance.Jails.Remove(findJail);

            PSRMPoliceUtilities.Instance.Configuration.Save();
            ChatManager.serverSendMessage($"Deleted jail {command[0]}", Color.green, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "deletejail";
        public string Help => "Deletes a jail.";
        public string Syntax => "/deletejail <name>";
        public List<string> Aliases=> new List<string>();
        public List<string> Permissions => new List<string> { "ps.policeutilities.jailmanager" };
    }
}