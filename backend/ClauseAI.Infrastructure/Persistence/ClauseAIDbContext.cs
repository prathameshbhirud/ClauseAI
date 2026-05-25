using ClauseAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;

namespace ClauseAI.Infrastructure.Persistence;

public class ClauseAIDbContext : DbContext
{
    public ClauseAIDbContext(DbContextOptions<ClauseAIDbContext> options)
        : base(options)
    {
    }

    public DbSet<Document> Documents => Set<Document>();
    public DbSet<DocumentChunk> DocumentChunks => Set<DocumentChunk>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.FileName)
                .HasMaxLength(500);

            entity.Property(x => x.StoredFileName)
                .HasMaxLength(500);

            entity.Property(x => x.ContentType)
                .HasMaxLength(200);
        });

        modelBuilder.Entity<DocumentChunk>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Content);

            entity.HasIndex(x => x.DocumentId);
        });

        modelBuilder.Entity<DocumentChunk>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Content);

            entity.Property(x => x.Embedding)
                .HasColumnType("vector(768)");

            entity.HasIndex(x => x.DocumentId);
        });
    }
}