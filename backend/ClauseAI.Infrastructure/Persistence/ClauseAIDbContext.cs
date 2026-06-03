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
    public DbSet<Conversation> Conversations => Set<Conversation>();

    public DbSet<ConversationMessage> ConversationMessages => Set<ConversationMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("vector");

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

            entity.Property(x => x.SearchVector);

            entity.Property(x => x.Embedding)
                .HasColumnType("vector(768)");

            entity.HasIndex(x => x.DocumentId);

            entity.HasOne<Document>()
                .WithMany()
                .HasForeignKey(x => x.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                .HasMaxLength(500);

            entity.HasIndex(x => x.DocumentId);

            entity.HasMany(x => x.Messages)
                .WithOne(x => x.Conversation)
                .HasForeignKey(x => x.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Document>()
                .WithMany()
                .HasForeignKey(x => x.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ConversationMessage>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Role)
                .HasMaxLength(50);

            entity.Property(x => x.Content);

            entity.HasIndex(x => x.ConversationId);
        });
    }
}