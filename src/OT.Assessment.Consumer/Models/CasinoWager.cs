using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace OT.Assessment.Consumer.Model
{
    public class CasinoWager
    {
        public int Id {get;set;}
        public Guid WagerId {get; set;}
        public string Theme {get; set;}
        public string Provider {get; set;}
        public string GameName {get; set;}
        public Guid TransactionId {get; set;}
        public Guid BrandId {get; set;}
        public Guid AccountId {get; set;}
        public string Username {get; set;}
        public Guid ExternalReferenceId {get; set;}
        public Guid TransactionTypeId {get; set;}
        public decimal Amount {get; set;}
        public DateTime CreatedDateTime {get; set;}
        public int NumberOfBets {get; set;}
        public string CountryCode {get; set;}
        public string SessionData {get; set;} 
        public int Duration {get; set;}       
    }
}