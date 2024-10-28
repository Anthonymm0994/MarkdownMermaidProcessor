
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string markdownFilePath = "C:\\Users\\antho\\source\\repos\\MarkdownThing\\NewFolder\\test.md";
        string outputMarkdownFilePath = "C:\\Users\\antho\\source\\repos\\MarkdownThing\\NewFolder\\test_temp.md";
        string outputImageDirectory = "C:\\Users\\antho\\source\\repos\\MarkdownThing\\NewFolder";

        var styleConfig = new MermaidStyleConfig
        {
            BackgroundColor = "lightgrey",
            NodeColor = "#ff6347",
            FontFamily = "Verdana, sans-serif",
            FontSize = 16
        };

        try
        {
            // Process the Markdown file, rendering diagrams and replacing code blocks with image references
            await MarkdownMermaidProcessor.UpdateMarkdownImagesAsync(
                markdownFilePath,
                outputMarkdownFilePath,
                outputImageDirectory,
                styleConfig
            );

            Console.WriteLine("Markdown processing complete. Images rendered and embedded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during processing: {ex.Message}");
        }
    }
}
