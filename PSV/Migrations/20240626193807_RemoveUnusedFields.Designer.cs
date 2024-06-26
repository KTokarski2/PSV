﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PSV.Models;

#nullable disable

namespace PSV.Migrations
{
    [DbContext(typeof(Repository))]
    [Migration("20240626193807_RemoveUnusedFields")]
    partial class RemoveUnusedFields
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PSV.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NIP")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("Client_pk");

                    b.ToTable("Client", (string)null);
                });

            modelBuilder.Entity("PSV.Models.Cut", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("From")
                        .HasColumnType("datetime2");

                    b.Property<int?>("IdOperator")
                        .HasColumnType("int");

                    b.Property<bool>("IsPresent")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("To")
                        .HasColumnType("datetime2");

                    b.HasKey("Id")
                        .HasName("Cut_pk");

                    b.HasIndex("IdOperator");

                    b.ToTable("Cut", (string)null);
                });

            modelBuilder.Entity("PSV.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("Location_pk");

                    b.ToTable("Location", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Przasnysz"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Jednorożec"
                        });
                });

            modelBuilder.Entity("PSV.Models.Milling", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("From")
                        .HasColumnType("datetime2");

                    b.Property<int?>("IdOperator")
                        .HasColumnType("int");

                    b.Property<bool>("IsPresent")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("To")
                        .HasColumnType("datetime2");

                    b.HasKey("Id")
                        .HasName("Milling_pk");

                    b.HasIndex("IdOperator");

                    b.ToTable("Milling", (string)null);
                });

            modelBuilder.Entity("PSV.Models.Operator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("Operator_pk");

                    b.ToTable("Operator", (string)null);
                });

            modelBuilder.Entity("PSV.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BarCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("EdgeCodeProvided")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EdgeCodeUsed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<int>("IdCut")
                        .HasColumnType("int");

                    b.Property<int>("IdLocation")
                        .HasColumnType("int");

                    b.Property<int>("IdMilling")
                        .HasColumnType("int");

                    b.Property<int>("IdWrapping")
                        .HasColumnType("int");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photos")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QrCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("Order_pk");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdCut")
                        .IsUnique();

                    b.HasIndex("IdLocation");

                    b.HasIndex("IdMilling")
                        .IsUnique();

                    b.HasIndex("IdWrapping")
                        .IsUnique();

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("PSV.Models.Wrapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("From")
                        .HasColumnType("datetime2");

                    b.Property<int?>("IdOperator")
                        .HasColumnType("int");

                    b.Property<bool>("IsPresent")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("To")
                        .HasColumnType("datetime2");

                    b.HasKey("Id")
                        .HasName("Wrapping_pk");

                    b.HasIndex("IdOperator");

                    b.ToTable("Wrapping", (string)null);
                });

            modelBuilder.Entity("PSV.Models.Cut", b =>
                {
                    b.HasOne("PSV.Models.Operator", "Operator")
                        .WithMany("Cuts")
                        .HasForeignKey("IdOperator")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("Operator_Cut");

                    b.Navigation("Operator");
                });

            modelBuilder.Entity("PSV.Models.Milling", b =>
                {
                    b.HasOne("PSV.Models.Operator", "Operator")
                        .WithMany("Millings")
                        .HasForeignKey("IdOperator")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("Operator_Milling");

                    b.Navigation("Operator");
                });

            modelBuilder.Entity("PSV.Models.Order", b =>
                {
                    b.HasOne("PSV.Models.Client", "Client")
                        .WithMany("Orders")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("Client_Orders");

                    b.HasOne("PSV.Models.Cut", "Cut")
                        .WithOne("Order")
                        .HasForeignKey("PSV.Models.Order", "IdCut")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("Order_Cut");

                    b.HasOne("PSV.Models.Location", "Location")
                        .WithMany("Orders")
                        .HasForeignKey("IdLocation")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("Order_Location");

                    b.HasOne("PSV.Models.Milling", "Milling")
                        .WithOne("Order")
                        .HasForeignKey("PSV.Models.Order", "IdMilling")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("Order_Milling");

                    b.HasOne("PSV.Models.Wrapping", "Wrapping")
                        .WithOne("Order")
                        .HasForeignKey("PSV.Models.Order", "IdWrapping")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("Order_Wrapping");

                    b.Navigation("Client");

                    b.Navigation("Cut");

                    b.Navigation("Location");

                    b.Navigation("Milling");

                    b.Navigation("Wrapping");
                });

            modelBuilder.Entity("PSV.Models.Wrapping", b =>
                {
                    b.HasOne("PSV.Models.Operator", "Operator")
                        .WithMany("Wrappings")
                        .HasForeignKey("IdOperator")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("Operator_Wrapping");

                    b.Navigation("Operator");
                });

            modelBuilder.Entity("PSV.Models.Client", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("PSV.Models.Cut", b =>
                {
                    b.Navigation("Order")
                        .IsRequired();
                });

            modelBuilder.Entity("PSV.Models.Location", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("PSV.Models.Milling", b =>
                {
                    b.Navigation("Order")
                        .IsRequired();
                });

            modelBuilder.Entity("PSV.Models.Operator", b =>
                {
                    b.Navigation("Cuts");

                    b.Navigation("Millings");

                    b.Navigation("Wrappings");
                });

            modelBuilder.Entity("PSV.Models.Wrapping", b =>
                {
                    b.Navigation("Order")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
