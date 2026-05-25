using ClauseAI.Application.Interfaces;
using ClauseAI.Application.Models;
using UglyToad.PdfPig;

namespace ClauseAI.Infrastructure.Pdf;

public class PdfTextExtractor : IPdfTextExtractor
{
    private readonly IOcrService _ocrService;

    public PdfTextExtractor(IOcrService ocrService)
    {
        _ocrService = ocrService;
    }
    
    public async Task<ExtractedDocument> ExtractAsync(
        Guid documentId,
        string filePath)
    {
        var result = new ExtractedDocument
        {
            DocumentId = documentId
        };

        using var document = PdfDocument.Open(filePath);

        foreach (var page in document.GetPages())
        {
            var text = page.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine($"OCR fallback triggered for page {page.Number}");

                text = await _ocrService.ExtractTextAsync(filePath, page.Number);

                Console.WriteLine($"OCR extracted {text.Length} chars from page {page.Number}");
            }

            result.Pages.Add(new ExtractedPage
            {
                PageNumber = page.Number,
                Text = text
            });
        }

        return await Task.FromResult(result);
    }
}