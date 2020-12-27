using System;

namespace PSRMPoliceUtilities.Models
{
    public class Fine
    {
        public string PlayerId { get; set; }
        
        public DateTime FinedDate { get; set; }
        
        public int FinedAmount { get; set; }
        
        public string Reason { get; set; }
    }
}