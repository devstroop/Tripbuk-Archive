using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TripBUK.Server.Models.Postgres;

namespace TripBUK.Server.Data
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

            builder.Entity<TripBUK.Server.Models.Postgres.AccountGroup>()
              .HasOne(i => i.AccountGroup1)
              .WithMany(i => i.AccountGroups1)
              .HasForeignKey(i => i.Parent)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<TripBUK.Server.Models.Postgres.Account>()
              .HasOne(i => i.AccountGroup)
              .WithMany(i => i.Accounts)
              .HasForeignKey(i => i.Group)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<TripBUK.Server.Models.Postgres.ItemGroup>()
              .HasOne(i => i.ItemGroup1)
              .WithMany(i => i.ItemGroups1)
              .HasForeignKey(i => i.Parent)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<TripBUK.Server.Models.Postgres.Item>()
              .HasOne(i => i.ItemGroup)
              .WithMany(i => i.Items)
              .HasForeignKey(i => i.Group)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<TripBUK.Server.Models.Postgres.UnitConversion>()
              .HasOne(i => i.Unit)
              .WithMany(i => i.UnitConversions)
              .HasForeignKey(i => i.MainUnit)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<TripBUK.Server.Models.Postgres.UnitConversion>()
              .HasOne(i => i.Unit1)
              .WithMany(i => i.UnitConversions1)
              .HasForeignKey(i => i.SubUnit)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<TripBUK.Server.Models.Postgres.UnitConversion>()
              .Property(p => p.ConversionFactor)
              .HasDefaultValueSql(@"1");

            builder.Entity<TripBUK.Server.Models.Postgres.Account>()
              .Property(p => p.CreditLimit)
              .HasPrecision(18,2);

            builder.Entity<TripBUK.Server.Models.Postgres.Account>()
              .Property(p => p.OpeningBalance)
              .HasPrecision(18,2);
            this.OnModelBuilding(builder);
        }

        public DbSet<TripBUK.Server.Models.Postgres.AccountGroup> AccountGroups { get; set; }

        public DbSet<TripBUK.Server.Models.Postgres.Account> Accounts { get; set; }

        public DbSet<TripBUK.Server.Models.Postgres.ItemGroup> ItemGroups { get; set; }

        public DbSet<TripBUK.Server.Models.Postgres.Item> Items { get; set; }

        public DbSet<TripBUK.Server.Models.Postgres.StandardNarration> StandardNarrations { get; set; }

        public DbSet<TripBUK.Server.Models.Postgres.UnitConversion> UnitConversions { get; set; }

        public DbSet<TripBUK.Server.Models.Postgres.Unit> Units { get; set; }

        public DbSet<TripBUK.Server.Models.Postgres.SmtpConfig> SmtpConfigs { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}