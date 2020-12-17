using System;

namespace PSRMPoliceUtilities.Models
{
    public class JailTime
    {
        public string PlayerId { get; set; }
        public string JailName { get; set; }
        public DateTime ExpireDate { get; set; }
        
        public string Reason { get; set; }
    }
}