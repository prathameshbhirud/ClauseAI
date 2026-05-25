namespace ClauseAI.Application.Interfaces;

public interface IDocumentIngestionService
{
    Task ProcessAsync(Guid documentId);
}