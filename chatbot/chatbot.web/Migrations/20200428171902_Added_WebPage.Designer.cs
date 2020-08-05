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
    [Migration("20200428171902_Added_WebPage")]
    partial class Added_WebPage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("Relational:Sequence:.LeadGenerationAction_Id_Sequence", "'LeadGenerationAction_Id_Sequence', '', '5', '1', '', '', 'Int64', 'False'");

            modelBuilder.Entity("IndianBank_ChatBOT.Models.ChatLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActivityId");

                    b.Property<string>("ActivityType");

                    b.Property<string>("ConversationId");

                    b.Property<string>("ConversationName");

                    b.Property<string>("ConversationType");

                    b.Property<string>("FromId");

                    b.Property<string>("FromName");

                    b.Property<bool?>("IsOnBoardingMessage");

                    b.Property<string>("MainTitle");

                    b.Property<string>("RasaEntities");

                    b.Property<string>("RasaIntent");

                    b.Property<double?>("RasaScore");

                    b.Property<string>("RecipientId");

                    b.Property<string>("RecipientName");

                    b.Property<string>("ReplyToActivityId");

                    b.Property<int?>("ResonseFeedback");

                    b.Property<string>("ResponseJsonText");

                    b.Property<int?>("ResponseSource");

                    b.Property<string>("SubTitle");

                    b.Property<string>("Text");

                    b.Property<DateTime?>("TimeStamp");

                    b.HasKey("Id");

                    b.ToTable("ChatLogs");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.LeadGenerationAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("nextval('\"LeadGenerationAction_Id_Sequence\"')");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

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
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConversationId");

                    b.Property<string>("DomainName");

                    b.Property<int?>("LeadGenerationActionId");

                    b.Property<string>("PhoneNumber");

                    b.Property<DateTime>("QueriedOn");

                    b.Property<int>("UserInfoId");

                    b.Property<string>("Visitor");

                    b.HasKey("Id");

                    b.ToTable("LeadGenerationInfos");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.StaticPage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EncodedPageUrl");

                    b.Property<string>("FileName");

                    b.Property<string>("PageConfig");

                    b.Property<string>("PageUrl");

                    b.HasKey("Id");

                    b.ToTable("StaticPages");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.Synonym", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Word");

                    b.HasKey("Id");

                    b.ToTable("Synonyms");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.SynonymWord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("SynonymId");

                    b.HasKey("Id");

                    b.HasIndex("SynonymId");

                    b.ToTable("SynonymWords");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.UserInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConversationId");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Name");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("Id");

                    b.ToTable("UserInfos");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.WebPage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("PageName")
                        .IsRequired();

                    b.Property<string>("Url")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("WebPages");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.WebPageScrapeRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CompletedDate");

                    b.Property<DateTime>("RequestedDate");

                    b.Property<int>("ScrapeStatus");

                    b.Property<int>("WebPageId");

                    b.HasKey("Id");

                    b.HasIndex("WebPageId");

                    b.ToTable("WebPageScrapeRequests");
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.SynonymWord", b =>
                {
                    b.HasOne("IndianBank_ChatBOT.Models.Synonym", "Synonym")
                        .WithMany("SynonymWords")
                        .HasForeignKey("SynonymId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("IndianBank_ChatBOT.Models.WebPageScrapeRequest", b =>
                {
                    b.HasOne("IndianBank_ChatBOT.Models.WebPage", "WebPage")
                        .WithMany()
                        .HasForeignKey("WebPageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
