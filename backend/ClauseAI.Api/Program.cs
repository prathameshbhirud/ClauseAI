using ClauseAI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using ClauseAI.Application.Interfaces;
using ClauseAI.Infrastructure.Pdf;
using ClauseAI.Infrastructure.AI;
using Pgvector.EntityFrameworkCore;
using ClauseAI.Infrastructure.VectorStore;
using Hangfire;
using Hangfire.MemoryStorage;
using ClauseAI.Infrastructure.OCR;



var builder = WebApplication.CreateBuilder(args);

// Add built-in services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Custom Services
builder.Services.AddScoped<IPdfTextExtractor, PdfTextExtractor>();
builder.Services.AddScoped<ITextChunker, TextChunker>();
builder.Services.AddScoped<IVectorSearchService, VectorSearchService>();
builder.Services.AddScoped<IRagService, RagService>();
builder.Services.AddHttpClient<
    IChatCompletionService,
    OllamaChatCompletionService>(
    client =>
    {
        client.BaseAddress =
            new Uri("http://localhost:11434");
    });
builder.Services.AddHttpClient<IEmbeddingService, OllamaEmbeddingService>(
    client =>
    {
        client.BaseAddress = new Uri("http://localhost:11434");
    });
builder.Services.AddScoped<IDocumentIngestionService, DocumentIngestionService>();
builder.Services.AddScoped<IOcrService, TesseractOcrService>();


// DB Context for Postgres
builder.Services.AddDbContext<ClauseAIDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        o => o.UseVector());
});
// Hangfire
builder.Services.AddHangfire(config =>
{
    config.UseMemoryStorage();
});

builder.Services.AddHangfireServer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("http://localhost:4200");
        });
});

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseCors("AllowAngular");

app.UseHangfireDashboard();

app.Run();