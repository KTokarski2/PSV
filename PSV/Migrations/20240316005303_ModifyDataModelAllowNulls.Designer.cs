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
    [Migration("20240316005303_ModifyDataModelAllowNulls")]
    partial class ModifyDataModelAllowNulls
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

                    b.Property<int>("IdOrder")
                        .HasColumnType("int");

                    b.Property<bool>("IsPresent")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("To")
                        .HasColumnType("datetime2");

                    b.HasKey("Id")
                        .HasName("Cut_pk");

                    b.HasIndex("IdOrder")
                        .IsUnique();

                    b.ToTable("Cut", (string)null);
                });

            modelBuilder.Entity("PSV.Models.Milling", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("From")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdOrder")
                        .HasColumnType("int");

                    b.Property<bool>("IsPresent")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("To")
                        .HasColumnType("datetime2");

                    b.HasKey("Id")
                        .HasName("Milling_pk");

                    b.HasIndex("IdOrder")
                        .IsUnique();

                    b.ToTable("Milling", (string)null);
                });

            modelBuilder.Entity("PSV.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Format")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<string>("Photos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QrCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("Order_pk");

                    b.HasIndex("IdClient");

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

                    b.Property<int>("IdOrder")
                        .HasColumnType("int");

                    b.Property<bool>("IsPresent")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("To")
                        .HasColumnType("datetime2");

                    b.HasKey("Id")
                        .HasName("Wrapping_pk");

                    b.HasIndex("IdOrder")
                        .IsUnique();

                    b.ToTable("Wrapping", (string)null);
                });

            modelBuilder.Entity("PSV.Models.Cut", b =>
                {
                    b.HasOne("PSV.Models.Order", "Order")
                        .WithOne("Cut")
                        .HasForeignKey("PSV.Models.Cut", "IdOrder")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("Order_Cut");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("PSV.Models.Milling", b =>
                {
                    b.HasOne("PSV.Models.Order", "Order")
                        .WithOne("Milling")
                        .HasForeignKey("PSV.Models.Milling", "IdOrder")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("Order_Milling");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("PSV.Models.Order", b =>
                {
                    b.HasOne("PSV.Models.Client", "Client")
                        .WithMany("Orders")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("Client_Orders");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("PSV.Models.Wrapping", b =>
                {
                    b.HasOne("PSV.Models.Order", "Order")
                        .WithOne("Wrapping")
                        .HasForeignKey("PSV.Models.Wrapping", "IdOrder")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("Order_Wrapping");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("PSV.Models.Client", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("PSV.Models.Order", b =>
                {
                    b.Navigation("Cut")
                        .IsRequired();

                    b.Navigation("Milling")
                        .IsRequired();

                    b.Navigation("Wrapping")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
