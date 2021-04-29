using System;
using LiteDB;

namespace PSRMPoliceUtilities.Models
{
    public class Fine
    {
        public ObjectId FineID { get; set; } //Only required by LiteDB. Yes, LiteDB is a dumb bruh.
        
        public string PlayerId { get; set; }
        
        public DateTime FinedDate { get; set; }
        
        public Decimal FinedAmount { get; set; }
        
        public string Reason { get; set; }

        public bool Active { get; set; }
        
        public int CaseID { get; set; }
    }
}