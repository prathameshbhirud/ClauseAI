namespace ClauseAI.Application.Interfaces;

public interface IOcrService
{
    Task<string> ExtractTextAsync(
        string pdfPath,
        int pageNumber);
}