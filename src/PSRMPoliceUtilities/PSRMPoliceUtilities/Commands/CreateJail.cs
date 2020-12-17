using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace PSRMPoliceUtilities.Commands
{
    public class CreateJail : IRocketCommand
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
            
            if (findJail != null)
            {
                ChatManager.serverSendMessage($"Jail already exists.", Color.red, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            PSRMPoliceUtilities.Instance.Configuration.Instance.Jails.Add(new Models.Jail() 
            { 
                Name = command[0],
                X = unturnedPlayer.Position.x,
                Y = unturnedPlayer.Position.y,
                Z = unturnedPlayer.Position.z 
            });

            PSRMPoliceUtilities.Instance.Configuration.Save();
            ChatManager.serverSendMessage($"Created jail {command[0]} at X: {unturnedPlayer.Position.x}, Y: {unturnedPlayer.Position.y}, Z: {unturnedPlayer.Position.z}", Color.green, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, null, true);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "createjail";
        public string Help => "Creates a jail.";
        public string Syntax => "/createjail <name>";
        public List<string> Aliases=> new List<string>();
        public List<string> Permissions => new List<string> { "ps.policeutilities.jailmanager" };
    }
}