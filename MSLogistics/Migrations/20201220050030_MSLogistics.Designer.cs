﻿// <auto-generated />
using System;
using MSLogistics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MSLogistics.Migrations
{
    [DbContext(typeof(MSLogisticsContext))]
    [Migration("20201220050030_MSLogistics")]
    partial class MSLogistics
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Database.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PersonId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PrivateKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Wallet")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Database.ClientPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClientId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DeliveryPointId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("DeliveryPointId");

                    b.ToTable("ClientPoints");
                });

            modelBuilder.Entity("Database.Delivery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("Database.DeliveryPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DeliveryPoints");
                });

            modelBuilder.Entity("Database.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrganizationAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PersonId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationAccountId");

                    b.HasIndex("PersonId");

                    b.HasIndex("RoleId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Database.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClientId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("From")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("IsStatusDelivery")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsStatusPaid")
                        .HasColumnType("INTEGER");

                    b.Property<int>("To")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TypeDeliveryId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("From");

                    b.HasIndex("To");

                    b.HasIndex("TypeDeliveryId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Database.OrganizationAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("PrivateKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Wallet")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("OrganizationAccounts");
                });

            modelBuilder.Entity("Database.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MiddleName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("Database.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Database.TypeDelivery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TypeDeliveries");
                });

            modelBuilder.Entity("Database.Client", b =>
                {
                    b.HasOne("Database.Person", "Person")
                        .WithMany("Clients")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Database.ClientPoint", b =>
                {
                    b.HasOne("Database.Client", "Client")
                        .WithMany("ClientPoints")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.DeliveryPoint", "DeliveryPoint")
                        .WithMany("ClientPoints")
                        .HasForeignKey("DeliveryPointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("DeliveryPoint");
                });

            modelBuilder.Entity("Database.Delivery", b =>
                {
                    b.HasOne("Database.Employee", "Employee")
                        .WithMany("Deliveries")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Database.Employee", b =>
                {
                    b.HasOne("Database.OrganizationAccount", "OrganizationAccount")
                        .WithMany("Employees")
                        .HasForeignKey("OrganizationAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Person", "Person")
                        .WithMany("Employees")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Role", "Role")
                        .WithMany("Employees")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrganizationAccount");

                    b.Navigation("Person");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Database.Order", b =>
                {
                    b.HasOne("Database.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.DeliveryPoint", "FromDeliveryPoint")
                        .WithMany()
                        .HasForeignKey("From")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.DeliveryPoint", "ToDeliveryPoint")
                        .WithMany()
                        .HasForeignKey("To")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.TypeDelivery", "TypeDelivery")
                        .WithMany("Orders")
                        .HasForeignKey("TypeDeliveryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("FromDeliveryPoint");

                    b.Navigation("ToDeliveryPoint");

                    b.Navigation("TypeDelivery");
                });

            modelBuilder.Entity("Database.Client", b =>
                {
                    b.Navigation("ClientPoints");
                });

            modelBuilder.Entity("Database.DeliveryPoint", b =>
                {
                    b.Navigation("ClientPoints");
                });

            modelBuilder.Entity("Database.Employee", b =>
                {
                    b.Navigation("Deliveries");
                });

            modelBuilder.Entity("Database.OrganizationAccount", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Database.Person", b =>
                {
                    b.Navigation("Clients");

                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Database.Role", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Database.TypeDelivery", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
