using System.Collections.Generic;
using Rocket.API;

namespace PSRMPoliceUtilities.Commands.Fine
{
    public class RemoveFine : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, string[] command)
        {
            throw new System.NotImplementedException();
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "removefine";
        public string Help => "";
        public string Syntax => "";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>();
    }
}