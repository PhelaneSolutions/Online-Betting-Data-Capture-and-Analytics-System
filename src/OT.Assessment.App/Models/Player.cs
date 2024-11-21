using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OT.Assessment.App.Models
{
    public class Player
    {
        public Guid AccountId {get; set;}
        public string Username {get; set;}
        public decimal TotalAmountSpend {get; set;}
    }
}