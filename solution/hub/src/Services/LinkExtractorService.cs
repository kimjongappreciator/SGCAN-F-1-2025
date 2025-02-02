using System.Text.RegularExpressions;

namespace hub.Services;

public class LinkExtractorService
{
    private static readonly Regex UrlRegex = new Regex(
        @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)",
        RegexOptions.Compiled);

    public List<string> ExtractLinksFromText(string text) {
        
        var matches = UrlRegex.Matches(text);
        return matches.Select(m => m.Value).ToList();
    }
}