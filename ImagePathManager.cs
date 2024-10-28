using System.IO;

/// <summary>
/// Manages paths and generates Markdown-compatible image tags for rendered Mermaid diagrams.
/// </summary>
public class ImagePathManager
{
    private readonly string _baseImagePath;

    /// <summary>
    /// Initializes a new instance of ImagePathManager with a base path for storing images.
    /// </summary>
    /// <param name="baseImagePath">The directory where images will be stored.</param>
    public ImagePathManager(string baseImagePath)
    {
        _baseImagePath = baseImagePath;
    }

    /// <summary>
    /// Generates an HTML-compatible image tag for the Markdown file, centered and with an ID.
    /// </summary>
    /// <param name="imagePath">The path to the generated image file.</param>
    /// <param name="label">The label used as the image ID and caption reference.</param>
    /// <returns>A string containing an HTML image tag for Markdown embedding.</returns>
    public string GenerateImageTag(string imagePath, string label)
    {
        string imageName = Path.GetFileName(imagePath);
        return $"<p align=\"center\">\n<img src=\"{_baseImagePath}/{imageName}\" border=\"0\" id=\"_FIG{{{label}}}\"/>\n</p>";
    }

    /// <summary>
    /// Generates a caption for the image in Markdown with a central alignment.
    /// </summary>
    /// <param name="label">The label used for the caption.</param>
    /// <returns>A string containing the caption for the diagram.</returns>
    public string GenerateFigureCaption(string label)
    {
        return $"<p style=\"text-align: center; margin-bottom:1\">Figure _FIG{{{label}}} : {label}</p>";
    }
}
