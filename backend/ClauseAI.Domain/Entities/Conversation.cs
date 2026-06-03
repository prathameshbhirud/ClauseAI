namespace ClauseAI.Domain.Entities;

public class Conversation
{
    public Guid Id { get; set; }

    public Guid DocumentId { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public List<ConversationMessage> Messages { get; set; } = [];
}