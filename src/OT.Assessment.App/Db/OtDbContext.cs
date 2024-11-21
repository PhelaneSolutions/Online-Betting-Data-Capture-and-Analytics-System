using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OT.Assessment.App.Models;

namespace OT.Assessment.App.OtDbContext
{
    public class OtDbContext : DbContext
    {
        public OtDbContext(DbContextOptions<OtDbContext> options)
            :base(options)
        {
        }

          public DbSet<CasinoWager> casinoWagers {get; set;}
    }
}