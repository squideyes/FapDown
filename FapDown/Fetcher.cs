using HtmlAgilityPack;
using System.Text.Json;

namespace FapDown;

public class Fetcher
{
    private class ImageData
    {
        public required string? Author { get; init; }
        public required Uri? ContentUrl { get; init; }
        public required string DatePublished { get; init; }
        public required string? Name { get; init; }
        public required string? Width { get; init; }
        public required string? Height { get; init; }
    }

    private static readonly HttpClient client = new();

    private readonly Gallery gallery;

    public Fetcher(Gallery gallery)
    {
        this.gallery = gallery;
    }

    public async Task FetchAsync()
    {
        var uri = new FluentUriBuilder()
            .AddPathSegment("pictures")
            .AddPathSegment(gallery.Id)
            .AddPathSegment(gallery.Name)
            .SetQueryParam("gid", gallery.Id)
            .SetQueryParam("view", 2)
            .GetUri();

        var images = await GetImagesAsync(uri);
    }

    private static async Task<HtmlDocument> GetDocumentAsync(Uri uri)
    {
        var doc = new HtmlDocument();

        var html = await client.GetStringAsync(uri);

        doc.LoadHtml(html);

        return doc;
    }

    private async Task<List<Image>> GetImagesAsync(Uri galleryUri)
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        var galleryDoc = await GetDocumentAsync(galleryUri);

        foreach (var node in galleryDoc.DocumentNode.SelectNodes(
            "//td[@id and @align='center' and @valign='top']"))
        {
            var id = node.Attributes["id"].Value;

            var photoUri = new FluentUriBuilder()
                .AddPathSegment("photo")
                .AddPathSegment(id)
                .GetUri();

            var photoDoc = await GetDocumentAsync(photoUri);

            var script = photoDoc.DocumentNode.SelectSingleNode(
                "//div[@class='image_info']/script");

            if (script == null)
            {
                // Log????????
                continue;
            }

            var data = JsonSerializer.Deserialize<ImageData>(
                script.InnerText, options)!;

            var image = new Image()
            {
                Gallery = gallery,
                Uri = data.ContentUrl!,
                Author = data.Author!,
                Date = DateOnly.Parse(data.DatePublished),
                Title = data.Name!,
                Height = int.Parse(data.Height!),
                Width = int.Parse(data.Width!),
            };
        }

        return null!;
    }
}
