using System;
using System.Linq;
using System.Linq.Expressions;
using IndianBank_ChatBOT.Migrations;
using IndianBank_ChatBOT.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Newtonsoft.Json;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ForNpgsqlUseIdentityColumns();

            modelBuilder.Entity<LeadGenerationAction>().SeedWithSequence(
                modelBuilder, r => r.Id,
                new LeadGenerationAction { Id = 1, Name = "Seen", Description = "Seen" },
                new LeadGenerationAction { Id = 2, Name = "Ignore", Description = "Ignore" },
                new LeadGenerationAction { Id = 3, Name = "Qualified", Description = "Qualified" },
                new LeadGenerationAction { Id = 4, Name = "ActedUpon", Description = "ActedUpon" }
            );


            modelBuilder.Entity<WebPageLanguage>().SeedWithSequence(
                modelBuilder, r => r.LanguageId,
                new WebPageLanguage { LanguageId = 1, LanguageName = "English" },
                new WebPageLanguage { LanguageId = 2, LanguageName = "Hindi" }
            );

            modelBuilder.Entity<User>().SeedWithSequence(
                modelBuilder, r => r.Id,
                new User { Id = 1, Name = "Admin", Username = "admin", Password = HashingUtils.Hash("admin@123$%^", "1") }
            );


            modelBuilder.Entity<StaticPage>()
                   .Property(e => e.CreatedOn)
                   .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<WebScrapeConfig>()
                 .Property(e => e.CreatedOn)
                 .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<StaticPage>()
                .Property(s=>s.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<WebScrapeConfig>()
                   .Property(s => s.IsActive)
                   .HasDefaultValue(true);
        }

        public DbSet<ChatLog> ChatLogs { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Synonym> Synonyms { get; set; }
        public DbSet<StaticPage> StaticPages { get; set; }
        public DbSet<SynonymWord> SynonymWords { get; set; }
        public DbSet<LeadGenerationInfo> LeadGenerationInfos { get; set; }
        public DbSet<LeadGenerationAction> LeadGenerationActions { get; set; }
        //public DbSet<WebPageScrapeRequest> WebPageScrapeRequests { get; set; }
        public DbSet<WebScrapeConfig> WebScrapeConfig { get; set; }

        public DbSet<WebPageLanguage> WebPageLanguages { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public DbQuery<FrequentlyAskedQueries> FrequentlyAskedQueries { get; set; }

        public DbQuery<UnAnsweredQueries> UnAnsweredQueries { get; set; }

        public DbQuery<Top10DomainsVisitedViewModel> Top10DomainsVisitedViewModels { get; set; }

        public DbQuery<ChatBotVisitorDetail> ChatBotVisitorDetails { get; set; }

        public DbQuery<VisitorsByYearMonthViewModel> VisitorsByYearMonthViewModels { get; set; }

        public DbQuery<VisitorsByMonthViewModel> VisitorsByMonthViewModels { get; set; }
    }

    internal static class MyExtensions
    {
        public static void SeedWithSequence<TEntity>(
            this EntityTypeBuilder<TEntity> entity,
            ModelBuilder modelBuilder,
            Expression<Func<TEntity, int>> idSelector,
            params TEntity[] seedData) where TEntity : class
        {
            var seqName = $"{typeof(TEntity).Name}_Id_Sequence";

            var seqStartValue = seedData.Max(idSelector.Compile()) + 1;

            modelBuilder.HasSequence(seqName)
                .StartsAt(seqStartValue)
                .IncrementsBy(1);

            entity.Property(idSelector)
                 .HasDefaultValueSql($"nextval('\"{seqName}\"')");

            entity.HasData(seedData);
        }

        public static void SeedWithSequence<TEntity>(
            this EntityTypeBuilder<TEntity> entity,
            ModelBuilder modelBuilder,
            Expression<Func<TEntity, long>> idSelector,
            params TEntity[] seedData) where TEntity : class
        {
            var seqName = $"{typeof(TEntity).Name}_Id_Sequence";

            var seqStartValue = seedData.Max(idSelector.Compile()) + 1;

            modelBuilder.HasSequence(seqName)
                .StartsAt(seqStartValue)
                .IncrementsBy(1);

            entity.Property(idSelector)
                 .HasDefaultValueSql($"nextval('\"{seqName}\"')");

            entity.HasData(seedData);
        }

        public static void HasEnumToStringConversion<TEnum>(this PropertyBuilder<TEnum> property) where TEnum : struct
        {
            property.HasConversion(
                v => v.ToString(),
                v => Enum.Parse<TEnum>(v));
        }

        public static void UseJsonSerialization<TEnum>(this PropertyBuilder<TEnum> property)
        {
            property
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<TEnum>(v)
                );
        }
    }
}

