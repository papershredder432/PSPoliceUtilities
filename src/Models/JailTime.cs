using System;

namespace PSPoliceUtilities.Models
{
    public class JailTime
    {
        public int CaseID { get; set; }
        
        public bool Active { get; set; }
        
        public long PlayerID { get; set; }

        public int JailID { get; set; }
        
        public DateTime ExpireDate { get; set; }
        
        public string Reason { get; set; }
    }
}