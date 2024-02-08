using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace API_REST_avec_état.Models.EntityFramework
{
    public partial class SeriesDBContext : DbContext
    {
        public SeriesDBContext()
        {
        }

        public SeriesDBContext(DbContextOptions<SeriesDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Serie> Series { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Serie>(entity =>
            {
                entity.HasKey(e => e.Serieid).HasName("serie_pkey");
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
    