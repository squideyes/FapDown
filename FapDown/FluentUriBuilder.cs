using System.Text;

namespace FapDown;

internal class FluentUriBuilder
{
    private readonly Dictionary<string, object> keyValues = new();
    private readonly List<string> pathSegments = new();

    private readonly Uri baseUri;

    public FluentUriBuilder()
    {
        baseUri = new Uri("https://www.imagefap.com");
    }

    public FluentUriBuilder AddPathSegment(object segment)
    {
        pathSegments.Add(segment.ToString()!);

        return this;
    }

    public FluentUriBuilder SetQueryParam(string key, object value)
    {
        keyValues.Add(key, value);

        return this;
    }

    public Uri GetUri()
    {
        var sb = new StringBuilder();

        foreach (var segment in pathSegments)
        {
            sb.Append('/');
            sb.Append(segment);
        }

        int count = 0;

        foreach (var kv in keyValues)
        {
            sb.Append(count++ == 0 ? "?" : "&");
            sb.Append($"{kv.Key}={kv.Value}");
        }    

        return new Uri(baseUri, sb.ToString());
    }
}