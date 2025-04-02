﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Patients.Infrastructure.Data;

#nullable disable

namespace Patients.Infrastructure.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Patients.Domain.Models.Patient", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Diagnosis")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Info")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<Guid>("TherapistId")
                        .HasColumnType("uuid");

                    b.ComplexProperty<Dictionary<string, object>>("PatientAddress", "Patients.Domain.Models.Patient.PatientAddress#Address", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("AddressLine")
                                .IsRequired()
                                .HasMaxLength(180)
                                .HasColumnType("character varying(180)");

                            b1.Property<string>("District")
                                .IsRequired()
                                .HasMaxLength(25)
                                .HasColumnType("character varying(25)");

                            b1.Property<string>("Location")
                                .IsRequired()
                                .HasMaxLength(25)
                                .HasColumnType("character varying(25)");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasMaxLength(8)
                                .HasColumnType("character varying(8)");
                        });

                    b.HasKey("Id");

                    b.ToTable("Patients");
                });
#pragma warning restore 612, 618
        }
    }
}
