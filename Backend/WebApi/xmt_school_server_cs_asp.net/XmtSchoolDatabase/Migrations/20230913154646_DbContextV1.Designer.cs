﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

using XmtSchoolDatabase;

#nullable disable

namespace TestsManagerDatabase.Migrations
{
    [DbContext(typeof(XmtSchoolDbContext))]
    [Migration("20230913154646_DbContextV1")]
    partial class DbContextV1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("TestsManagerTypes.Login.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUsed")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("TokenString")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Tokens", (string)null);
                });

            modelBuilder.Entity("TestsManagerTypes.Tests.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AnswerText")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsValidAnswer")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers", (string)null);
                });

            modelBuilder.Entity("TestsManagerTypes.Tests.Mark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Points")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.HasIndex("UserId");

                    b.ToTable("Marks", (string)null);
                });

            modelBuilder.Entity("TestsManagerTypes.Tests.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("Questions", (string)null);
                });

            modelBuilder.Entity("TestsManagerTypes.Tests.SelectedAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AnswerId")
                        .HasColumnType("int");

                    b.Property<int?>("MarkId")
                        .HasColumnType("int");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("MarkId");

                    b.HasIndex("QuestionId");

                    b.ToTable("SelectedAnswers", (string)null);
                });

            modelBuilder.Entity("TestsManagerTypes.Tests.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AuthorUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("RandomiseAnswersOrder")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("RandomiseQuestionsOrder")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorUserId");

                    b.ToTable("Tests", (string)null);
                });

            modelBuilder.Entity("TestsManagerTypes.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AvatarUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint unsigned");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("TestsManagerTypes.Login.Token", b =>
                {
                    b.HasOne("TestsManagerTypes.Users.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TestsManagerTypes.Tests.Answer", b =>
                {
                    b.HasOne("TestsManagerTypes.Tests.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("TestsManagerTypes.Tests.Mark", b =>
                {
                    b.HasOne("TestsManagerTypes.Tests.Test", "Test")
                        .WithMany()
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestsManagerTypes.Users.User", "User")
                        .WithMany("Marks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Test");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TestsManagerTypes.Tests.Question", b =>
                {
                    b.HasOne("TestsManagerTypes.Tests.Test", "Test")
                        .WithMany("Questions")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Test");
                });

            modelBuilder.Entity("TestsManagerTypes.Tests.SelectedAnswer", b =>
                {
                    b.HasOne("TestsManagerTypes.Tests.Answer", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TestsManagerTypes.Tests.Mark", null)
                        .WithMany("SelectedAnswers")
                        .HasForeignKey("MarkId");

                    b.HasOne("TestsManagerTypes.Tests.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("TestsManagerTypes.Tests.Test", b =>
                {
                    b.HasOne("TestsManagerTypes.Users.User", "Author")
                        .WithMany("Tests")
                        .HasForeignKey("AuthorUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("TestsManagerTypes.Tests.Mark", b =>
                {
                    b.Navigation("SelectedAnswers");
                });

            modelBuilder.Entity("TestsManagerTypes.Tests.Question", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("TestsManagerTypes.Tests.Test", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("TestsManagerTypes.Users.User", b =>
                {
                    b.Navigation("Marks");

                    b.Navigation("Tests");

                    b.Navigation("Tokens");
                });
#pragma warning restore 612, 618
        }
    }
}
