using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace p2
{
    public class HtmlParser
    {
        public HtmlElement Parse(string html)
        {
            var tokens = Tokenize(html);

            HtmlElement root = new HtmlElement { Name = "root" };
            HtmlElement currentElement = root;

            foreach (var token in tokens)
            {
                if (token.StartsWith("</"))
                {
                    if (currentElement.Parent != null)
                        currentElement = currentElement.Parent;
                }
                else if (token.StartsWith("<"))
                {
                    var newElement = BuildElement(token);

                    newElement.Parent = currentElement;
                    currentElement.Children.Add(newElement);

                    bool isSelfClosing = HtmlHelper.Instance.SelfClosingTags.Contains(newElement.Name) || token.EndsWith("/>");

                    if (!isSelfClosing)
                    {
                        currentElement = newElement;
                    }
                }
                else
                {
                    currentElement.InnerHtml += token;
                }
            }

            return root.Children.FirstOrDefault();
        }

        private HtmlElement BuildElement(string token)
        {
            string tagContent = token.Trim('<', '>', '/');
            string[] parts = tagContent.Split(' ', 2);

            var newElement = new HtmlElement { Name = parts[0] };

            if (parts.Length > 1)
            {
                var attributes = Regex.Matches(parts[1], "([^\\s=]+)=\"([^\"]*)\"");
                foreach (Match attr in attributes)
                {
                    string name = attr.Groups[1].Value.ToLower();
                    string value = attr.Groups[2].Value;

                    if (name == "id")
                        newElement.Id = value;
                    else if (name == "class")
                        newElement.Classes.AddRange(value.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                    else
                        newElement.Attributes.Add($"{name}=\"{value}\"");
                }
            }
            return newElement;
        }

        private List<string> Tokenize(string html)
        {
            var tokens = new List<string>();
            var matches = Regex.Matches(html, "<[^>]+>|[^<]+");

            foreach (Match match in matches)
            {
                string value = match.Value.Trim();
                if (!string.IsNullOrWhiteSpace(value))
                    tokens.Add(value);
            }
            return tokens;
        }
    }
}