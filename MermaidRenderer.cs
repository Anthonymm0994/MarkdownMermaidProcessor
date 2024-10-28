using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PuppeteerSharp;

/// <summary>
/// Renders Mermaid diagrams as images using PuppeteerSharp and applies style configurations.
/// Handles rendering exceptions and generates HTML for Mermaid rendering.
/// </summary>
public class MermaidRenderer
{
    private readonly string _outputDirectory;
    private readonly MermaidStyleConfig _styleConfig;

    public MermaidRenderer(string outputDirectory, MermaidStyleConfig styleConfig)
    {
        _outputDirectory = outputDirectory;
        _styleConfig = styleConfig;
        Directory.CreateDirectory(_outputDirectory);
    }

    /// <summary>
    /// Renders a Mermaid diagram as an image, applying style configurations and managing errors.
    /// </summary>
    public async Task<string> RenderMermaidToImageAsync(string mermaidCode, string label)
    {
        string sanitizedLabel = Regex.Replace(label, @"\W+", "_");
        string imagePath = Path.Combine(_outputDirectory, $"image_{sanitizedLabel}.png");

        string htmlContent = GenerateMermaidHtml(mermaidCode);
        string htmlFilePath = Path.Combine(_outputDirectory, "temp.html");
        await File.WriteAllTextAsync(htmlFilePath, htmlContent);

        try
        {
            await RenderWithPuppeteerAsync(htmlFilePath, imagePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to render Mermaid image '{label}': {ex.Message}");
            throw;
        }
        finally
        {
            File.Delete(htmlFilePath); // Clean up temp file
        }

        return imagePath;
    }

    /// <summary>
    /// Generates an HTML string to load Mermaid code and apply custom styles.
    /// </summary>
    private string GenerateMermaidHtml(string mermaidCode)
    {
        return $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <title>Mermaid Diagram</title>
    <style>
        body, html {{
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
            background-color: {_styleConfig.BackgroundColor};
        }}
        .mermaid {{
            max-width: 80%;
            max-height: 80%;
            width: auto;
            height: auto;
            font-family: {_styleConfig.FontFamily};
            font-size: {_styleConfig.FontSize}px;
        }}
    </style>
    <script type=""module"">
        import mermaid from 'https://cdn.jsdelivr.net/npm/mermaid@10.1.0/dist/mermaid.esm.min.mjs';
        mermaid.initialize({{ startOnLoad: true, {_styleConfig} }});
    </script>
</head>
<body>
    <div class=""mermaid"">
        {mermaidCode}
    </div>
</body>
</html>";
    }

    /// <summary>
    /// Renders the generated HTML to an image using Puppeteer and captures any rendering issues.
    /// </summary>
    private async Task RenderWithPuppeteerAsync(string htmlFilePath, string outputPath)
    {
        await new BrowserFetcher().DownloadAsync();
        using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true,
            Args = new[] { "--no-sandbox", "--disable-setuid-sandbox", "--disable-gpu", "--disable-software-rasterizer" }
        });

        using var page = await browser.NewPageAsync();
        await page.GoToAsync($"file:///{htmlFilePath}");
        await page.WaitForSelectorAsync(".mermaid");
        await Task.Delay(1000);

        await page.ScreenshotAsync(outputPath, new ScreenshotOptions { FullPage = true });
    }
}
