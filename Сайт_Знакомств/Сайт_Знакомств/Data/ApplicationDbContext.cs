using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Сайт_Знакомств.Models;

namespace Сайт_Знакомств.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Reciprocity> Reciprocity { get; set; }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<User>().Property(x => x.FirstName).IsRequired();
        //    builder.Entity<User>().Property(x => x.LastName).IsRequired();
        //    builder.Entity<User>().Property(x => x.Description).IsRequired();
        //    builder.Entity<User>().Property(x => x.Sex).IsRequired();
        //}

    }
}
