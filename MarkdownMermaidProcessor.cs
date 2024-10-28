using System;
using System.IO;
using System.Threading.Tasks;

/// <summary>
/// The primary class to control Markdown processing and image generation. 
/// Provides a single public method, UpdateMarkdownImagesAsync, which processes a Markdown file,
/// detects Mermaid code blocks, renders each block as an image, and replaces the code blocks with image tags.
/// </summary>
public static class MarkdownMermaidProcessor
{
    /// <summary>
    /// Processes a Markdown file, renders all Mermaid code blocks as images, 
    /// and replaces the code blocks with image references in the modified Markdown file.
    /// </summary>
    /// <param name="markdownFilePath">Path to the input Markdown file.</param>
    /// <param name="outputMarkdownFilePath">Path where the modified Markdown file will be saved.</param>
    /// <param name="outputImageDirectory">Directory where the generated images will be stored.</param>
    /// <param name="styleConfig">Optional MermaidStyleConfig for customizing diagram appearance.</param>
    public static async Task UpdateMarkdownImagesAsync(
        string markdownFilePath,
        string outputMarkdownFilePath,
        string outputImageDirectory,
        MermaidStyleConfig? styleConfig = null)
    {
        // Verify file path availability
        if (!File.Exists(markdownFilePath))
            throw new FileNotFoundException($"Markdown file not found: {markdownFilePath}");

        if (!Directory.Exists(outputImageDirectory))
            Directory.CreateDirectory(outputImageDirectory);

        // Initialize core components
        var renderer = new MermaidRenderer(outputImageDirectory, styleConfig ?? new MermaidStyleConfig());
        var pathManager = new ImagePathManager(outputImageDirectory);
        var parser = new MarkdownParser(renderer, pathManager);

        try
        {
            // Read the Markdown content, process Mermaid blocks, and write modified content to file
            string markdownContent = await File.ReadAllTextAsync(markdownFilePath);
            string modifiedMarkdown = await parser.ParseAndReplaceMermaidBlocksAsync(markdownContent);
            await File.WriteAllTextAsync(outputMarkdownFilePath, modifiedMarkdown);

            Console.WriteLine("Markdown file has been processed, and images have been rendered and embedded.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during processing: {ex.Message}");
            throw;
        }
    }
}
