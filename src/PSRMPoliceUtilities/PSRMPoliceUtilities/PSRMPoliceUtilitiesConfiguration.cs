using System.Collections.Generic;
using PSRMPoliceUtilities.Models;
using Rocket.API;
using UnityEngine;

namespace PSRMPoliceUtilities
{
    public class PSRMPoliceUtilitiesConfiguration : IRocketPluginConfiguration
    {
        public float JailRadius { get; set; }
        public Vector3 RelaseLocation { get; set; }
        public double CheckInterval { get; set; }
        public List<Jail> Jails { get; set; }

        public void LoadDefaults()
        {
            JailRadius = 5;
            RelaseLocation = new Vector3(0, 0 ,0);
            CheckInterval = 15;
            Jails = new List<Jail>()
            {
                new Jail()
                {
                    Name = "Default",
                    X = 0,
                    Y = 0,
                    Z = 0
                }
            };
        }
    }
}