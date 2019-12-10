
using System;
using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;
using System.Linq;
//using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IndianBank_ChatBOT.Models
{
    public class LogDataContext : DbContext
    {
           
        public LogDataContext(DbContextOptions<LogDataContext> options)
            : base(options)
        {
        }
        public LogDataContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          
        }
        public DbSet<LogData> ChatLog { get; set; }
        public DbSet<Faq> Faq { get; set; }
       

    }
}
