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

            builder.Entity<Tripbuk.Server.Models.Postgres.PlaceTag>().HasKey(table => new {
                table.PlaceId, table.TagId
            });

            builder.Entity<Tripbuk.Server.Models.Postgres.PlaceTag>()
              .HasOne(i => i.Place)
              .WithMany(i => i.PlaceTags)
              .HasForeignKey(i => i.PlaceId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Tripbuk.Server.Models.Postgres.PlaceTag>()
              .HasOne(i => i.Tag)
              .WithMany(i => i.PlaceTags)
              .HasForeignKey(i => i.TagId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Tripbuk.Server.Models.Postgres.Destination>()
              .HasOne(i => i.Destination1)
              .WithMany(i => i.Destinations1)
              .HasForeignKey(i => i.ParentDestinationId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Tripbuk.Server.Models.Postgres.Place>()
              .Property(p => p.CreatedAt)
              .HasDefaultValueSql(@"CURRENT_TIMESTAMP");
            this.OnModelBuilding(builder);
        }

        public DbSet<Tripbuk.Server.Models.Postgres.Tag> Tags { get; set; }

        public DbSet<Tripbuk.Server.Models.Postgres.PlaceTag> PlaceTags { get; set; }

        public DbSet<Tripbuk.Server.Models.Postgres.Place> Places { get; set; }

        public DbSet<Tripbuk.Server.Models.Postgres.Destination> Destinations { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}