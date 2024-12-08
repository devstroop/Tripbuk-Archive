using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ERP.Server.Models.Postgres;

namespace ERP.Server.Data
{
    public partial class PostgresContext : DbContext
    {
        public PostgresContext()
        {
        }

        public PostgresContext(DbContextOptions<PostgresContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ERP.Server.Models.Postgres.StdNarrationMaster>()
              .HasOne(i => i.Master)
              .WithMany(i => i.StdNarrationMasters)
              .HasForeignKey(i => i.MasterId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<ERP.Server.Models.Postgres.ItemMaster>()
              .HasOne(i => i.ItemGroupMaster)
              .WithMany(i => i.ItemMasters)
              .HasForeignKey(i => i.Group)
              .HasPrincipalKey(i => i.MasterId);

            builder.Entity<ERP.Server.Models.Postgres.ItemMaster>()
              .HasOne(i => i.Master)
              .WithMany(i => i.ItemMasters)
              .HasForeignKey(i => i.MasterId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<ERP.Server.Models.Postgres.ItemGroupMaster>()
              .HasOne(i => i.Master)
              .WithMany(i => i.ItemGroupMasters)
              .HasForeignKey(i => i.MasterId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<ERP.Server.Models.Postgres.ItemGroupMaster>()
              .HasOne(i => i.ItemGroupMaster1)
              .WithMany(i => i.ItemGroupMasters1)
              .HasForeignKey(i => i.Parent)
              .HasPrincipalKey(i => i.MasterId);

            builder.Entity<ERP.Server.Models.Postgres.AccountMaster>()
              .HasOne(i => i.AccountGroupMaster)
              .WithMany(i => i.AccountMasters)
              .HasForeignKey(i => i.Group)
              .HasPrincipalKey(i => i.MasterId);

            builder.Entity<ERP.Server.Models.Postgres.AccountMaster>()
              .HasOne(i => i.Master)
              .WithMany(i => i.AccountMasters)
              .HasForeignKey(i => i.MasterId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<ERP.Server.Models.Postgres.AccountGroupMaster>()
              .HasOne(i => i.Master)
              .WithMany(i => i.AccountGroupMasters)
              .HasForeignKey(i => i.MasterId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<ERP.Server.Models.Postgres.AccountGroupMaster>()
              .HasOne(i => i.AccountGroupMaster1)
              .WithMany(i => i.AccountGroupMasters1)
              .HasForeignKey(i => i.Parent)
              .HasPrincipalKey(i => i.MasterId);

            builder.Entity<ERP.Server.Models.Postgres.Master>()
              .Property(p => p.CreatedAt)
              .HasDefaultValueSql(@"CURRENT_TIMESTAMP");

            builder.Entity<ERP.Server.Models.Postgres.AccountMaster>()
              .Property(p => p.CreditLimit)
              .HasPrecision(18,2);

            builder.Entity<ERP.Server.Models.Postgres.AccountMaster>()
              .Property(p => p.OpeningBalance)
              .HasPrecision(18,2);
            this.OnModelBuilding(builder);
        }

        public DbSet<ERP.Server.Models.Postgres.Master> Masters { get; set; }

        public DbSet<ERP.Server.Models.Postgres.StdNarrationMaster> StdNarrationMasters { get; set; }

        public DbSet<ERP.Server.Models.Postgres.ItemMaster> ItemMasters { get; set; }

        public DbSet<ERP.Server.Models.Postgres.ItemGroupMaster> ItemGroupMasters { get; set; }

        public DbSet<ERP.Server.Models.Postgres.AccountMaster> AccountMasters { get; set; }

        public DbSet<ERP.Server.Models.Postgres.AccountGroupMaster> AccountGroupMasters { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}