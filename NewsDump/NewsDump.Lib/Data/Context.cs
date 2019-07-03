using Microsoft.EntityFrameworkCore;
using NewsDump.Lib.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsDump.Lib.Data
{
    public class Context : DbContext
    {
        public Context() : base() { }
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=dumperdb.sqlite");
        }

        public DbSet<News> News { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<News>()
                .Ignore(x => x.PublishDateInFarsi);


        }

    }
}
