/// <summary>
/// Configuration class for specifying styling options in Mermaid diagrams.
/// Allows customization of background color, node color, font, and font size.
/// </summary>
public class MermaidStyleConfig
{
    /// <summary>
    /// The background color of the Mermaid diagram.
    /// </summary>
    public string BackgroundColor { get; set; } = "white";

    /// <summary>
    /// The primary color used for nodes within the diagram.
    /// </summary>
    public string NodeColor { get; set; } = "#1f78b4";

    /// <summary>
    /// The font family used for all text within the diagram.
    /// </summary>
    public string FontFamily { get; set; } = "Arial, sans-serif";

    /// <summary>
    /// The font size for all text within the diagram, in pixels.
    /// </summary>
    public int FontSize { get; set; } = 14;

    /// <summary>
    /// Converts the style configuration to a JSON-compatible string for Mermaid's themeVariables.
    /// </summary>
    /// <returns>A string with Mermaid-compatible theme settings for use in diagram rendering.</returns>
    public override string ToString()
    {
        return $@"
            {{
                themeVariables: {{
                    background: '{BackgroundColor}',
                    primaryColor: '{NodeColor}',
                    fontFamily: '{FontFamily}',
                    fontSize: '{FontSize}px'
                }}
            }}
        ";
    }
}
