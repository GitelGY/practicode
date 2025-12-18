using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace p2
{
    public class HtmlElement
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public List<string> Attributes { get; set; } = new();
        public List<string> Classes { get; set; } = new(); public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new();


        public HtmlElement() { }


        public void Print(string indent = "")
        {
            Console.WriteLine($"{indent}<{Name}> id: {Id} classes: {string.Join(", ", Classes)}");
            foreach (var child in Children)
            {
                child.Print(indent + "  ");
            }
        }

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();

            foreach (var child in Children)
            {
                queue.Enqueue(child);
            }

            while (queue.Count > 0)
            {
                HtmlElement current = queue.Dequeue();
                yield return current;
                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = this.Parent;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

        public HashSet<HtmlElement> Find(Selector selector)
        {
            HashSet<HtmlElement> results = new HashSet<HtmlElement>();

            var descendants = this.Descendants();

            var matches = descendants.Where(el => IsMatch(el, selector));
            if (selector.Child == null)
            {
                foreach (var match in matches) results.Add(match);
            }
            else
            {
                foreach (var match in matches)
                {
                    var childResults = match.Find(selector.Child);
                    foreach (var res in childResults) results.Add(res);
                }
            }

            return results;
        }

        private bool IsMatch(HtmlElement el, Selector selector)
        {
            if (!string.IsNullOrEmpty(selector.TagName) && el.Name != selector.TagName)
                return false;
            if (!string.IsNullOrEmpty(selector.Id) && el.Id != selector.Id)
                return false;
            if (selector.Classes.Any() && !selector.Classes.All(c => el.Classes.Contains(c)))
                return false;

            return true;
        }
    }
}
