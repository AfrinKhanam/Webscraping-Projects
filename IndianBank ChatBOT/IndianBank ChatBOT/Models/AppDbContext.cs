using IndianBank_ChatBOT.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(string connectionString)
          : base(new DbContextOptionsBuilder<AppDbContext>().UseNpgsql(connectionString).Options)
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        public DbSet<ChatLog> ChatLogs { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbQuery<FrequentlyAskedQueries> FrequentlyAskedQueries { get; set; }
        public DbQuery<UnAnsweredQueries> UnAnsweredQueries { get; set; }
        
    }
}
