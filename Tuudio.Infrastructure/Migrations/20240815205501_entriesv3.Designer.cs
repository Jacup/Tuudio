﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tuudio.Infrastructure.Data;

#nullable disable

namespace Tuudio.Infrastructure.Migrations
{
    [DbContext(typeof(TuudioDbContext))]
    [Migration("20240815205501_entriesv3")]
    partial class entriesv3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("ActivityPassTemplate", b =>
                {
                    b.Property<Guid>("ActivitiesId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("PassTemplatesId")
                        .HasColumnType("char(36)");

                    b.HasKey("ActivitiesId", "PassTemplatesId");

                    b.HasIndex("PassTemplatesId");

                    b.ToTable("ActivityPassTemplate");

                    b.HasData(
                        new
                        {
                            ActivitiesId = new Guid("00000000-0000-0000-0001-000000000001"),
                            PassTemplatesId = new Guid("00000000-0000-0000-0002-000000000001")
                        });
                });

            modelBuilder.Entity("Tuudio.Domain.Entities.Activities.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Activities", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0001-000000000001"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Yoga group class",
                            Name = "Yoga",
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0001-000000000002"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Individual pool session",
                            Name = "Pool",
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0001-000000000003"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "EMS training",
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Tuudio.Domain.Entities.Entries.Entry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ActivityId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Note")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("PassId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.HasIndex("ClientId");

                    b.HasIndex("PassId");

                    b.ToTable("Entries", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0004-000000000001"),
                            ActivityId = new Guid("00000000-0000-0000-0001-000000000001"),
                            ClientId = new Guid("00000000-0000-0000-0000-000000000001"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EntryDate = new DateTime(2024, 8, 15, 22, 55, 1, 228, DateTimeKind.Local).AddTicks(2242),
                            Note = "Client first entry without pass",
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0004-000000000002"),
                            ActivityId = new Guid("00000000-0000-0000-0001-000000000001"),
                            ClientId = new Guid("00000000-0000-0000-0000-000000000001"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EntryDate = new DateTime(2024, 8, 15, 22, 55, 1, 228, DateTimeKind.Local).AddTicks(2290),
                            Note = "Client second entry with pass",
                            PassId = new Guid("00000000-0000-0000-0003-000000000001"),
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Tuudio.Domain.Entities.Passes.Pass", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateOnly>("FromDate")
                        .HasColumnType("date");

                    b.Property<Guid>("PassTemplateId")
                        .HasColumnType("char(36)");

                    b.Property<DateOnly>("ToDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("PassTemplateId");

                    b.ToTable("Passes", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0003-000000000001"),
                            ClientId = new Guid("00000000-0000-0000-0000-000000000001"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FromDate = new DateOnly(2024, 8, 15),
                            PassTemplateId = new Guid("00000000-0000-0000-0002-000000000001"),
                            ToDate = new DateOnly(2024, 11, 15),
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Tuudio.Domain.Entities.Passes.PassTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<int>("EntriesAmount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("PassTemplate");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0002-000000000001"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Pass for yoga classes, unlimited entries, paid monthly, 3 months",
                            EntriesAmount = 0,
                            Name = "Monthly yoga pass",
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Tuudio.Domain.Entities.People.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Clients", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "example@site.com",
                            FirstName = "John",
                            LastName = "Doe",
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000002"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Jane",
                            LastName = "Doe",
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("ActivityPassTemplate", b =>
                {
                    b.HasOne("Tuudio.Domain.Entities.Activities.Activity", null)
                        .WithMany()
                        .HasForeignKey("ActivitiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tuudio.Domain.Entities.Passes.PassTemplate", null)
                        .WithMany()
                        .HasForeignKey("PassTemplatesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Tuudio.Domain.Entities.Entries.Entry", b =>
                {
                    b.HasOne("Tuudio.Domain.Entities.Activities.Activity", "Activity")
                        .WithMany("Entries")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tuudio.Domain.Entities.People.Client", "Client")
                        .WithMany("Entries")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tuudio.Domain.Entities.Passes.Pass", "Pass")
                        .WithMany("Entries")
                        .HasForeignKey("PassId");

                    b.Navigation("Activity");

                    b.Navigation("Client");

                    b.Navigation("Pass");
                });

            modelBuilder.Entity("Tuudio.Domain.Entities.Passes.Pass", b =>
                {
                    b.HasOne("Tuudio.Domain.Entities.People.Client", "Client")
                        .WithMany("Passes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tuudio.Domain.Entities.Passes.PassTemplate", "PassTemplate")
                        .WithMany("Passes")
                        .HasForeignKey("PassTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("PassTemplate");
                });

            modelBuilder.Entity("Tuudio.Domain.Entities.Passes.PassTemplate", b =>
                {
                    b.OwnsOne("Tuudio.Domain.Entities.Passes.Duration", "Duration", b1 =>
                        {
                            b1.Property<Guid>("PassTemplateId")
                                .HasColumnType("char(36)");

                            b1.Property<int>("Amount")
                                .HasColumnType("int");

                            b1.Property<int>("Period")
                                .HasColumnType("int");

                            b1.HasKey("PassTemplateId");

                            b1.ToTable("PassTemplate");

                            b1.WithOwner()
                                .HasForeignKey("PassTemplateId");

                            b1.HasData(
                                new
                                {
                                    PassTemplateId = new Guid("00000000-0000-0000-0002-000000000001"),
                                    Amount = 3,
                                    Period = 3
                                });
                        });

                    b.OwnsOne("Tuudio.Domain.Entities.Passes.Price", "Price", b1 =>
                        {
                            b1.Property<Guid>("PassTemplateId")
                                .HasColumnType("char(36)");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(65,30)");

                            b1.Property<int>("Period")
                                .HasColumnType("int");

                            b1.HasKey("PassTemplateId");

                            b1.ToTable("PassTemplate");

                            b1.WithOwner()
                                .HasForeignKey("PassTemplateId");

                            b1.HasData(
                                new
                                {
                                    PassTemplateId = new Guid("00000000-0000-0000-0002-000000000001"),
                                    Amount = 100.0m,
                                    Period = 3
                                });
                        });

                    b.Navigation("Duration")
                        .IsRequired();

                    b.Navigation("Price")
                        .IsRequired();
                });

            modelBuilder.Entity("Tuudio.Domain.Entities.Activities.Activity", b =>
                {
                    b.Navigation("Entries");
                });

            modelBuilder.Entity("Tuudio.Domain.Entities.Passes.Pass", b =>
                {
                    b.Navigation("Entries");
                });

            modelBuilder.Entity("Tuudio.Domain.Entities.Passes.PassTemplate", b =>
                {
                    b.Navigation("Passes");
                });

            modelBuilder.Entity("Tuudio.Domain.Entities.People.Client", b =>
                {
                    b.Navigation("Entries");

                    b.Navigation("Passes");
                });
#pragma warning restore 612, 618
        }
    }
}
