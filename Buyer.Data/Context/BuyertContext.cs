﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Context.Models;

#nullable disable

namespace Context.Context
{
    public partial class BuyertContext : DbContext
    {
        public BuyertContext()
        {
        }

        public BuyertContext(DbContextOptions<BuyertContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BuyerRole> BuyerRoles { get; set; }
        public virtual DbSet<BuyerUser> BuyerUsers { get; set; }
        public virtual DbSet<BuyerUserRole> BuyerUserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=ZP\\SQLEXPRESS;Initial Catalog=Buyer_Ent;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BuyerRole>(entity =>
            {
                entity.ToTable("BuyerRole");

                entity.HasIndex(e => e.RoleName, "BuyerRole_LK")
                    .IsUnique();

                entity.Property(e => e.CreatedUser)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedUser)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BuyerUser>(entity =>
            {
                entity.ToTable("BuyerUser");

                entity.HasIndex(e => e.UserName, "LK")
                    .IsUnique();

                entity.Property(e => e.CreatedUser)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedUser)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BuyerUserRole>(entity =>
            {
                entity.ToTable("BuyerUserRole");

                entity.Property(e => e.CreatedUser)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedUser)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.BuyerUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BuyerUserRole_Role_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BuyerUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BuyerUserRole_User_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}