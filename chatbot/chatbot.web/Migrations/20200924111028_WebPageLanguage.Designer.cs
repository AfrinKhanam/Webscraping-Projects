﻿// <auto-generated />
using System;
using System.Collections.Generic;
using IndianBank_ChatBOT.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IndianBank_ChatBOT.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200924111028_WebPageLanguage")]
    partial class WebPageLanguage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("Relational:Sequence:.LeadGenerationAction_Id_Sequence", "'LeadGenerationAction_Id_Sequence', '', '5', '1', '', '', 'Int64', 'False'")
                .HasAnnotation("Relational:Sequence:.User_Id_Sequence", "'User_Id_Sequence', '', '2', '1', '', '', 'Int64', 'False'");

            modelBuilder.Entity("IndianBank_ChatBOT.Models.ChatBotVisitorDetail", b =>
                {
                    b.Property<DateTime>("LastVisited")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("NumberOfQueries")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfVisits")
                        .HasColumnType("integer");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("Visitor")
                        .HasColumnType("text");

                    b.ToTable("ChatBotVisitorDetails");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.ChatLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ActivityId")
                        .HasColumnType("text");

                    b.Property<string>("ActivityType")
                        .HasColumnType("text");

                    b.Property<string>("ConversationId")
                        .HasColumnType("text");

                    b.Property<string>("ConversationName")
                        .HasColumnType("text");

                    b.Property<string>("ConversationType")
                        .HasColumnType("text");

                    b.Property<string>("FromId")
                        .HasColumnType("text");

                    b.Property<string>("FromName")
                        .HasColumnType("text");

                    b.Property<bool?>("IsOnBoardingMessage")
                        .HasColumnType("boolean");

                    b.Property<string>("MainTitle")
                        .HasColumnType("text");

                    b.Property<string>("RasaEntities")
                        .HasColumnType("text");

                    b.Property<string>("RasaIntent")
                        .HasColumnType("text");

                    b.Property<double?>("RasaScore")
                        .HasColumnType("double precision");

                    b.Property<string>("RecipientId")
                        .HasColumnType("text");

                    b.Property<string>("RecipientName")
                        .HasColumnType("text");

                    b.Property<string>("ReplyToActivityId")
                        .HasColumnType("text");

                    b.Property<int?>("ResonseFeedback")
                        .HasColumnType("integer");

                    b.Property<string>("ResponseJsonText")
                        .HasColumnType("text");

                    b.Property<int?>("ResponseSource")
                        .HasColumnType("integer");

                    b.Property<string>("SubTitle")
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.Property<DateTime?>("TimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("ChatLogs");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.FrequentlyAskedQueries", b =>
                {
                    b.Property<List<string>>("ActivityIds")
                        .HasColumnType("text[]");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<int>("NegetiveFeedback")
                        .HasColumnType("integer");

                    b.Property<int>("PositiveFeedback")
                        .HasColumnType("integer");

                    b.Property<string>("Query")
                        .HasColumnType("text");

                    b.ToTable("FrequentlyAskedQueries");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.LeadGenerationAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValueSql("nextval('\"LeadGenerationAction_Id_Sequence\"')");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("LeadGenerationActions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Seen",
                            Name = "Seen"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Ignore",
                            Name = "Ignore"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Qualified",
                            Name = "Qualified"
                        },
                        new
                        {
                            Id = 4,
                            Description = "ActedUpon",
                            Name = "ActedUpon"
                        });
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.LeadGenerationInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ConversationId")
                        .HasColumnType("text");

                    b.Property<string>("DomainName")
                        .HasColumnType("text");

                    b.Property<int?>("LeadGenerationActionId")
                        .HasColumnType("integer");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<DateTime>("QueriedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("UserInfoId")
                        .HasColumnType("integer");

                    b.Property<string>("Visitor")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("LeadGenerationInfos");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.StaticPage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValue(new DateTime(2020, 9, 24, 16, 40, 28, 496, DateTimeKind.Local).AddTicks(7861));

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("text");

                    b.Property<string>("FileData")
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<string>("FileType")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("LanguageId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("LastScrapedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("PageConfig")
                        .HasColumnType("text");

                    b.Property<string>("PageUrl")
                        .HasColumnType("text");

                    b.Property<int>("ScrapeStatus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.ToTable("StaticPages");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.Synonym", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("LanguageId")
                        .HasColumnType("integer");

                    b.Property<string>("Word")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.ToTable("Synonyms");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.SynonymWord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("SynonymId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SynonymId");

                    b.ToTable("SynonymWords");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.Top10DomainsVisitedViewModel", b =>
                {
                    b.Property<string>("DomainName")
                        .HasColumnType("text");

                    b.Property<float>("HitPercentage")
                        .HasColumnType("real");

                    b.Property<int>("TotalHits")
                        .HasColumnType("integer");

                    b.ToTable("Top10DomainsVisitedViewModels");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.UnAnsweredQueries", b =>
                {
                    b.Property<string>("BotResponse")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("Query")
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.ToTable("UnAnsweredQueries");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValueSql("nextval('\"User_Id_Sequence\"')");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Admin",
                            Password = "1agNZaFS+rqXmkl8Ewth1UQJq4le7RcFftwdBbrYf4FykftFJhXV11hPrX5d9YTtlv+9gXiJ6lB1Q44qpuGsow==",
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.UserInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ConversationId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserInfos");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.VisitorsByMonthViewModel", b =>
                {
                    b.Property<int>("DayNumber")
                        .HasColumnType("integer");

                    b.Property<string>("MonthText")
                        .HasColumnType("text");

                    b.Property<int>("Visits")
                        .HasColumnType("integer");

                    b.ToTable("VisitorsByMonthViewModels");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.VisitorsByYearMonthViewModel", b =>
                {
                    b.Property<int>("MonthNumber")
                        .HasColumnType("integer");

                    b.Property<string>("MonthText")
                        .HasColumnType("text");

                    b.Property<int>("Visits")
                        .HasColumnType("integer");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.ToTable("VisitorsByYearMonthViewModels");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.WebPageLanguage", b =>
                {
                    b.Property<int>("LanguageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("LanguageName")
                        .HasColumnType("text");

                    b.HasKey("LanguageId");

                    b.ToTable("WebPageLanguage");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.WebScrapeConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValue(new DateTime(2020, 9, 24, 16, 40, 28, 498, DateTimeKind.Local).AddTicks(2856));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int?>("LanguageId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("LastScrapedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("PageConfig")
                        .HasColumnType("jsonb");

                    b.Property<string>("PageName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ScrapeStatus")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.ToTable("WebScrapeConfig");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.StaticPage", b =>
                {
                    b.HasOne("IndianBank_ChatBOT.Models.WebPageLanguage", "WebPageLanguage")
                        .WithMany()
                        .HasForeignKey("LanguageId");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.Synonym", b =>
                {
                    b.HasOne("IndianBank_ChatBOT.Models.WebPageLanguage", "WebPageLanguage")
                        .WithMany()
                        .HasForeignKey("LanguageId");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.SynonymWord", b =>
                {
                    b.HasOne("IndianBank_ChatBOT.Models.Synonym", "Synonym")
                        .WithMany("SynonymWords")
                        .HasForeignKey("SynonymId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.WebScrapeConfig", b =>
                {
                    b.HasOne("IndianBank_ChatBOT.Models.WebPageLanguage", "WebPageLanguage")
                        .WithMany()
                        .HasForeignKey("LanguageId");
                });
#pragma warning restore 612, 618
        }
    }
}
