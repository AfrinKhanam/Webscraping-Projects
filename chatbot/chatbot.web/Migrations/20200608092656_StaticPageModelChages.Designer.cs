﻿// <auto-generated />
using System;
using System.Collections.Generic;
using UjjivanBank_ChatBOT.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace UjjivanBank_ChatBOT.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200608092656_StaticPageModelChages")]
    partial class StaticPageModelChages
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("Relational:Sequence:.LeadGenerationAction_Id_Sequence", "'LeadGenerationAction_Id_Sequence', '', '5', '1', '', '', 'Int64', 'False'");

            modelBuilder.Entity("UjjivanBank_ChatBOT.Models.ChatBotVisitorDetail", b =>
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

            modelBuilder.Entity("UjjivanBank_ChatBOT.Models.ChatLog", b =>
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

            modelBuilder.Entity("UjjivanBank_ChatBOT.Models.FrequentlyAskedQueries", b =>
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

            modelBuilder.Entity("UjjivanBank_ChatBOT.Models.LeadGenerationAction", b =>
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

            modelBuilder.Entity("UjjivanBank_ChatBOT.Models.LeadGenerationInfo", b =>
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

            modelBuilder.Entity("UjjivanBank_ChatBOT.Models.StaticPage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<string>("Html")
                        .HasColumnType("text")
                        .HasMaxLength(2147483647);

                    b.Property<string>("PageConfig")
                        .HasColumnType("text");

                    b.Property<int>("ScrapeStatus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("StaticPages");
                });

            modelBuilder.Entity("UjjivanBank_ChatBOT.Models.Synonym", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Word")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Synonyms");
                });

            modelBuilder.Entity("UjjivanBank_ChatBOT.Models.SynonymWord", b =>
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

            modelBuilder.Entity("UjjivanBank_ChatBOT.Models.Top10DomainsVisitedViewModel", b =>
                {
                    b.Property<string>("DomainName")
                        .HasColumnType("text");

                    b.Property<float>("HitPercentage")
                        .HasColumnType("real");

                    b.Property<int>("TotalHits")
                        .HasColumnType("integer");

                    b.ToTable("Top10DomainsVisitedViewModels");
                });

            modelBuilder.Entity("UjjivanBank_ChatBOT.Models.UnAnsweredQueries", b =>
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

            modelBuilder.Entity("UjjivanBank_ChatBOT.Models.UserInfo", b =>
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

            modelBuilder.Entity("UjjivanBank_ChatBOT.Models.VisitorsByMonthViewModel", b =>
                {
                    b.Property<int>("DayNumber")
                        .HasColumnType("integer");

                    b.Property<string>("MonthText")
                        .HasColumnType("text");

                    b.Property<int>("Visits")
                        .HasColumnType("integer");

                    b.ToTable("VisitorsByMonthViewModels");
                });

            modelBuilder.Entity("UjjivanBank_ChatBOT.Models.VisitorsByYearMonthViewModel", b =>
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

            modelBuilder.Entity("UjjivanBank_ChatBOT.Models.WebPage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("PageName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("WebPages");
                });

            modelBuilder.Entity("UjjivanBank_ChatBOT.Models.WebPageScrapeRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("CompletedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("RequestedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("ScrapeStatus")
                        .HasColumnType("integer");

                    b.Property<int>("WebPageId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("WebPageId");

                    b.ToTable("WebPageScrapeRequests");
                });

            modelBuilder.Entity("UjjivanBank_ChatBOT.Models.SynonymWord", b =>
                {
                    b.HasOne("UjjivanBank_ChatBOT.Models.Synonym", "Synonym")
                        .WithMany("SynonymWords")
                        .HasForeignKey("SynonymId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UjjivanBank_ChatBOT.Models.WebPageScrapeRequest", b =>
                {
                    b.HasOne("UjjivanBank_ChatBOT.Models.WebPage", "WebPage")
                        .WithMany()
                        .HasForeignKey("WebPageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
