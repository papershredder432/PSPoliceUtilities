using System;
using UnityEngine;

namespace PSPoliceUtilities.Models
{
    public class Jail
    {
        public int JailID { get; set; }
        
        public string Name { get; set; }

        public int MaxCapacity { get; set; }
        
        public float JailRadius { get; set; }
        
        public string JailLocation { get; set; }

        public string ReleaseLocation { get; set; }
    }
}