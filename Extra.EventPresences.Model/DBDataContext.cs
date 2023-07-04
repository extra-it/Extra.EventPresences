using Azure;
using Extra.EventPresences.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Model
{
    public class DBDataContext : DbContext
    {
        private readonly string connectionString;

        //public DBDataContext(string connectionString)
        //{
        //    this.connectionString = connectionString;
        //}
        public DBDataContext(DbContextOptions<DBDataContext> options)
           : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connectionString);
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<WebApiLog> WebApiLogs { get; set; }
        public DbSet<FkEnums> FkEnums { get; set; }
        public DbSet<FkGroups> FkGroups { get; set; }



    }
}
