using ClauseAI.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClauseAI.Api.Controllers;

[ApiController]
[Route("api/conversations")]
public class ConversationsController
    : ControllerBase
{
    private readonly ClauseAIDbContext
        _dbContext;

    public ConversationsController(
        ClauseAIDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{documentId:guid}")]
    public async Task<IActionResult> Get(Guid documentId)
    {
        var conversations = await _dbContext.Conversations
                .Where(x => x.DocumentId == documentId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.CreatedAt
                })
                .ToListAsync();

        return Ok(conversations);
    }

    [HttpGet("messages/{conversationId:guid}")]
    public async Task<IActionResult> GetMessages(Guid conversationId)
    {
        var messages = await _dbContext.ConversationMessages
                .Where(x => x.ConversationId == conversationId)
                .OrderBy(x => x.CreatedAt)
                .Select(x => new
                {
                    x.Id,
                    x.Role,
                    x.Content,
                    x.CreatedAt
                })
                .ToListAsync();

        return Ok(messages);
    }
}