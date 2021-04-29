using System;

namespace PSPoliceUtilities.Models
{
    public class Fine
    {
        public int CaseID { get; set; }
        
        public bool Active { get; set; }
        
        public long PlayerID { get; set; }
        
        public DateTime FineDate { get; set; }
        
        public decimal FinedAmount { get; set; }
        
        public string Reason { get; set; }
    }
}