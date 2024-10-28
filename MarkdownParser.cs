using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/// <summary>
/// Parses a Markdown document, identifies Mermaid code blocks, and replaces each block 
/// with an image tag referencing the generated image of the diagram.
/// </summary>
public class MarkdownParser
{
    private readonly MermaidRenderer _renderer;
    private readonly ImagePathManager _pathManager;

    /// <summary>
    /// Initializes a new instance of MarkdownParser with a renderer for Mermaid blocks and a path manager.
    /// </summary>
    public MarkdownParser(MermaidRenderer renderer, ImagePathManager pathManager)
    {
        _renderer = renderer;
        _pathManager = pathManager;
    }

    /// <summary>
    /// Processes a Markdown document, identifying and replacing Mermaid code blocks with image references.
    /// </summary>
    /// <param name="markdownContent">The Markdown content as a string.</param>
    /// <returns>A string containing the modified Markdown with image references.</returns>
    public async Task<string> ParseAndReplaceMermaidBlocksAsync(string markdownContent)
    {
        var result = new StringBuilder();
        bool inMermaidBlock = false;
        StringBuilder mermaidCode = new();
        string blockLabel = "";

        foreach (var line in markdownContent.Split('\n'))
        {
            if (line.Trim().StartsWith("```mermaid"))
            {
                inMermaidBlock = true;
                blockLabel = ExtractLabel(line);
                continue;
            }
            else if (line.Trim() == "```" && inMermaidBlock)
            {
                try
                {
                    // Attempt to render Mermaid block and replace with image
                    string renderedImagePath = await _renderer.RenderMermaidToImageAsync(mermaidCode.ToString(), blockLabel);
                    result.AppendLine(_pathManager.GenerateImageTag(renderedImagePath, blockLabel));
                    result.AppendLine(_pathManager.GenerateFigureCaption(blockLabel));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error rendering Mermaid block '{blockLabel}': {ex.Message}");
                }

                inMermaidBlock = false;
                mermaidCode.Clear();
                blockLabel = "";
                continue;
            }

            if (inMermaidBlock)
                mermaidCode.AppendLine(line);
            else
                result.AppendLine(line);
        }

        return result.ToString();
    }

    /// <summary>
    /// Extracts the label from a Mermaid block's opening line for use in file naming and captions.
    /// </summary>
    private string ExtractLabel(string line)
    {
        var match = Regex.Match(line, @"```mermaid\s*-\s*(.*)");
        return match.Success ? match.Groups[1].Value.Trim() : "UnnamedFlowchart";
    }
}
