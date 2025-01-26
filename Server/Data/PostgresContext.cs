using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tripbuk.Server.Models.Postgres;

namespace Tripbuk.Server.Data
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

            builder.Entity<Tripbuk.Server.Models.Postgres.ParentTag>().HasNoKey();

            builder.Entity<Tripbuk.Server.Models.Postgres.PlaceTag>().HasNoKey();

            builder.Entity<Tripbuk.Server.Models.Postgres.Destination>()
              .HasOne(i => i.LocationCenter)
              .WithMany(i => i.Destinations)
              .HasForeignKey(i => i.LocationCenterId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Tripbuk.Server.Models.Postgres.Place>()
              .HasOne(i => i.Destination)
              .WithMany(i => i.Places)
              .HasForeignKey(i => i.DestinationId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Tripbuk.Server.Models.Postgres.LocationCenter>()
              .Property(p => p.CreatedAt)
              .HasDefaultValueSql(@"now()");

            builder.Entity<Tripbuk.Server.Models.Postgres.Place>()
              .Property(p => p.CreatedAt)
              .HasDefaultValueSql(@"CURRENT_TIMESTAMP");
            this.OnModelBuilding(builder);
        }

        public DbSet<Tripbuk.Server.Models.Postgres.Destination> Destinations { get; set; }

        public DbSet<Tripbuk.Server.Models.Postgres.LocationCenter> LocationCenters { get; set; }

        public DbSet<Tripbuk.Server.Models.Postgres.ParentTag> ParentTags { get; set; }

        public DbSet<Tripbuk.Server.Models.Postgres.Place> Places { get; set; }

        public DbSet<Tripbuk.Server.Models.Postgres.PlaceTag> PlaceTags { get; set; }

        public DbSet<Tripbuk.Server.Models.Postgres.Tag> Tags { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}