using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OT.Assessment.Consumer.Model;

namespace OT.Assessment.Consumer.Db
{
    public class ConsumerDb : DbContext
    {
       public ConsumerDb(DbContextOptions<ConsumerDb> options)
            :base(options)
        {
        }

          public DbSet<CasinoWager> casinoWagers {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CasinoWager>(entity =>{
               entity.HasKey(e => e.Id);
            });
        }
    }
}