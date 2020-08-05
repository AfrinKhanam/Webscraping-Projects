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
    [Migration("20200329172053_LeadGenerationAction")]
    partial class LeadGenerationAction
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

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
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("LeadGenerationActions");
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

            modelBuilder.Entity("IndianBank_ChatBOT.Models.SynonymWord", b =>
                {
                    b.HasOne("IndianBank_ChatBOT.Models.Synonym", "Synonym")
                        .WithMany("SynonymWords")
                        .HasForeignKey("SynonymId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
