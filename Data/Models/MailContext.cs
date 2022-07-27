using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MailTesting1.Data.Models
{
    public class MailContext : IdentityDbContext
    {
        IConfiguration _configuration {get;set;}
        public MailContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("MailContext"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                string TName = entity.GetTableName()!;
                if(!string.IsNullOrEmpty(TName) && TName.StartsWith("AspNet"))
                {
                    entity.SetTableName(TName.Substring(6));
                }
            }
        }

    }
}