﻿// <auto-generated />
using System;
using EmpTracker.DgiService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EmpTracker.DgiService.Infrastructure.Migrations.Saga
{
    [DbContext(typeof(SagaContext))]
    [Migration("20250203180739_sagaTables")]
    partial class sagaTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("saga")
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EmpTracker.DgiService.Core.Domain.SagaState.DesignationDeletionState", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CurrentState")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DesignationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("EmployeeRetryCount")
                        .HasColumnType("int");

                    b.HasKey("CorrelationId");

                    b.ToTable("DesignationDeletionStates", "saga");
                });
#pragma warning restore 612, 618
        }
    }
}
