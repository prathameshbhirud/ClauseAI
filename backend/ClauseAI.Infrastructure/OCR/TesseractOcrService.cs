using ClauseAI.Application.Interfaces;
using PdfiumViewer;
using Tesseract;

namespace ClauseAI.Infrastructure.OCR;

public class TesseractOcrService : IOcrService
{
    public async Task<string> ExtractTextAsync(
        string pdfPath,
        int pageNumber)
    {
        using var document =
            PdfDocument.Load(pdfPath);

        using var image =
            document.Render(
                pageNumber - 1,
                300,
                300,
                true);

        var tempImage =
            Path.GetTempFileName() + ".png";

        image.Save(tempImage);

        using var engine =
            new TesseractEngine(
                @"./tessdata",
                "eng",
                EngineMode.Default);

        using var img = Pix.LoadFromFile(tempImage);

        using var page = engine.Process(img);

        var text = page.GetText();

        File.Delete(tempImage);

        return await Task.FromResult(text);
    }
}