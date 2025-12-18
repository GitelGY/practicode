using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class HtmlTokenizer
{
    public static List<string> Tokenize(string html)
    {
        List<string> tokens = new List<string>();

        var matches = Regex.Matches(html, "<[^>]+>");
        int lastIndex = 0;

        foreach (Match match in matches)
        {
            if (match.Index > lastIndex)
            {
                string text = html.Substring(lastIndex, match.Index - lastIndex).Trim();
                if (!string.IsNullOrEmpty(text))
                    tokens.Add(text);
            }

            tokens.Add(match.Value);

            lastIndex = match.Index + match.Length;
        }

        if (lastIndex < html.Length)
        {
            string text = html.Substring(lastIndex).Trim();
            if (!string.IsNullOrEmpty(text))
                tokens.Add(text);
        }

        return tokens;
    }

    public static async Task Main()
    {
        string html = "<div class=\"my-class\">Hello World</div>";

        List<string> tokens = Tokenize(html);

        Console.WriteLine("Tokens found:");
        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }
}
