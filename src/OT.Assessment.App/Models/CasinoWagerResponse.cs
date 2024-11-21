using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OT.Assessment.App.Models
{
    public class CasinoWagerResponse
    {
        public Guid WagerId {get; set;}
        public string Game {get; set;}
        public string Provider {get; set;}
        public decimal Amount {get; set;}
        public DateTime CreatedDate {get; set;}
    }
}