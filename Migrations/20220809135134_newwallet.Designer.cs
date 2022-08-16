﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RealtyWebApp.Context;

#nullable disable

namespace RealtyWebApp.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20220809135134_newwallet")]
    partial class newwallet
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("RealtyWebApp.Entities.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<string>("RegId")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Admins");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "Apata,Ibadan",
                            RegId = "Ad0001",
                            UserId = 1
                        });
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Buyer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<string>("RegId")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Buyers");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.File.PropertyDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<byte[]>("Data")
                        .HasColumnType("longblob");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("DocumentName")
                        .HasColumnType("longtext");

                    b.Property<string>("Extension")
                        .HasColumnType("longtext");

                    b.Property<string>("FileType")
                        .HasColumnType("longtext");

                    b.Property<int?>("PropertyId")
                        .HasColumnType("int");

                    b.Property<string>("PropertyRegNo")
                        .HasColumnType("longtext");

                    b.Property<int>("UploadedBy")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PropertyId");

                    b.ToTable("PropertyDocuments");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.File.PropertyImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("DocumentName")
                        .HasColumnType("longtext");

                    b.Property<string>("DocumentPath")
                        .HasColumnType("longtext");

                    b.Property<string>("Extension")
                        .HasColumnType("longtext");

                    b.Property<string>("FileType")
                        .HasColumnType("longtext");

                    b.Property<int?>("PropertyId")
                        .HasColumnType("int");

                    b.Property<string>("PropertyRegNo")
                        .HasColumnType("longtext");

                    b.Property<int>("UploadedBy")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PropertyId");

                    b.ToTable("PropertyImages");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Identity.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("RoleName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            RoleName = "Administrator"
                        },
                        new
                        {
                            Id = 2,
                            RoleName = "Realtor"
                        },
                        new
                        {
                            Id = 3,
                            RoleName = "Buyer"
                        });
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Identity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "oladejimujib@yahoo.com",
                            FirstName = "Mujib",
                            LastName = "Oladeji",
                            Password = "$2a$11$HRRee8yLSwrGIo8Whu9NC.s4oQ7M.tQqfKTiOPHjXSS8PeGM60Pc6",
                            PhoneNumber = "08136794915"
                        });
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Identity.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            RoleId = 1,
                            UserId = 1
                        });
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("Amount")
                        .HasColumnType("double");

                    b.Property<string>("BuyerEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("BuyerName")
                        .HasColumnType("longtext");

                    b.Property<string>("BuyerTelephone")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("PropertyId")
                        .HasColumnType("int");

                    b.Property<string>("PropertyType")
                        .HasColumnType("longtext");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("double");

                    b.Property<string>("TransactionId")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("PropertyId")
                        .IsUnique();

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Property", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Action")
                        .HasColumnType("longtext");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<int>("Bedroom")
                        .HasColumnType("int");

                    b.Property<string>("BuildingType")
                        .HasColumnType("longtext");

                    b.Property<int?>("BuyerId")
                        .HasColumnType("int");

                    b.Property<int>("BuyerIdentity")
                        .HasColumnType("int");

                    b.Property<string>("Features")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsSold")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LGA")
                        .HasColumnType("longtext");

                    b.Property<double>("Latitude")
                        .HasColumnType("double");

                    b.Property<double>("Longitude")
                        .HasColumnType("double");

                    b.Property<double>("PlotArea")
                        .HasColumnType("double");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.Property<string>("PropertyRegNo")
                        .HasColumnType("longtext");

                    b.Property<string>("PropertyType")
                        .HasColumnType("longtext");

                    b.Property<int>("RealtorId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("RegisteredDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("State")
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .HasColumnType("longtext");

                    b.Property<int>("Toilet")
                        .HasColumnType("int");

                    b.Property<bool>("VerificationStatus")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("RealtorId");

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Realtor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<string>("AgentId")
                        .HasColumnType("longtext");

                    b.Property<string>("BusinessName")
                        .HasColumnType("longtext");

                    b.Property<string>("CacRegistrationNumber")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Realtors");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.VisitationRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<string>("BuyerEmail")
                        .HasColumnType("longtext");

                    b.Property<int>("BuyerId")
                        .HasColumnType("int");

                    b.Property<string>("BuyerName")
                        .HasColumnType("longtext");

                    b.Property<string>("BuyerTelephone")
                        .HasColumnType("longtext");

                    b.Property<int>("PropertyId")
                        .HasColumnType("int");

                    b.Property<string>("PropertyRegNo")
                        .HasColumnType("longtext");

                    b.Property<string>("PropertyType")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("PropertyId");

                    b.ToTable("VisitationRequests");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Wallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("AccountBalance")
                        .HasColumnType("double");

                    b.Property<string>("AccountName")
                        .HasColumnType("longtext");

                    b.Property<string>("AccountNo")
                        .HasColumnType("longtext");

                    b.Property<string>("BankName")
                        .HasColumnType("longtext");

                    b.Property<int>("RealtorId")
                        .HasColumnType("int");

                    b.Property<string>("ReceipientCode")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("RealtorId")
                        .IsUnique();

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Admin", b =>
                {
                    b.HasOne("RealtyWebApp.Entities.Identity.User", "User")
                        .WithOne("Admin")
                        .HasForeignKey("RealtyWebApp.Entities.Admin", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Buyer", b =>
                {
                    b.HasOne("RealtyWebApp.Entities.Identity.User", "User")
                        .WithOne("Buyer")
                        .HasForeignKey("RealtyWebApp.Entities.Buyer", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.File.PropertyDocument", b =>
                {
                    b.HasOne("RealtyWebApp.Entities.Property", "Property")
                        .WithMany("PropertyDocuments")
                        .HasForeignKey("PropertyId");

                    b.Navigation("Property");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.File.PropertyImage", b =>
                {
                    b.HasOne("RealtyWebApp.Entities.Property", "Property")
                        .WithMany("PropertyImages")
                        .HasForeignKey("PropertyId");

                    b.Navigation("Property");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Identity.UserRole", b =>
                {
                    b.HasOne("RealtyWebApp.Entities.Identity.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealtyWebApp.Entities.Identity.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Payment", b =>
                {
                    b.HasOne("RealtyWebApp.Entities.Property", "Property")
                        .WithOne("Payment")
                        .HasForeignKey("RealtyWebApp.Entities.Payment", "PropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Property");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Property", b =>
                {
                    b.HasOne("RealtyWebApp.Entities.Buyer", null)
                        .WithMany("Properties")
                        .HasForeignKey("BuyerId");

                    b.HasOne("RealtyWebApp.Entities.Realtor", "Realtor")
                        .WithMany("Properties")
                        .HasForeignKey("RealtorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Realtor");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Realtor", b =>
                {
                    b.HasOne("RealtyWebApp.Entities.Identity.User", "User")
                        .WithOne("Realtor")
                        .HasForeignKey("RealtyWebApp.Entities.Realtor", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.VisitationRequest", b =>
                {
                    b.HasOne("RealtyWebApp.Entities.Buyer", "Buyer")
                        .WithMany("VisitationRequests")
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealtyWebApp.Entities.Property", "Property")
                        .WithMany("VisitationRequests")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Buyer");

                    b.Navigation("Property");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Wallet", b =>
                {
                    b.HasOne("RealtyWebApp.Entities.Realtor", "Realtor")
                        .WithOne("Wallet")
                        .HasForeignKey("RealtyWebApp.Entities.Wallet", "RealtorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Realtor");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Buyer", b =>
                {
                    b.Navigation("Properties");

                    b.Navigation("VisitationRequests");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Identity.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Identity.User", b =>
                {
                    b.Navigation("Admin");

                    b.Navigation("Buyer");

                    b.Navigation("Realtor");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Property", b =>
                {
                    b.Navigation("Payment");

                    b.Navigation("PropertyDocuments");

                    b.Navigation("PropertyImages");

                    b.Navigation("VisitationRequests");
                });

            modelBuilder.Entity("RealtyWebApp.Entities.Realtor", b =>
                {
                    b.Navigation("Properties");

                    b.Navigation("Wallet");
                });
#pragma warning restore 612, 618
        }
    }
}
