using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace p2
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public Selector() { }

        public static Selector Parse(string query)
        {
            var levels = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Selector root = null;
            Selector current = null;

            foreach (var level in levels)
            {
                Selector newSelector = new Selector();


                var parts = Regex.Matches(level, @"(?=[#.])|([^#.]+)");

                foreach (Match part in parts)
                {
                    string fragment = part.Value;
                    if (string.IsNullOrEmpty(fragment)) continue;

                    if (fragment.StartsWith("#"))
                    {
                        newSelector.Id = fragment.Substring(1);
                    }
                    else if (fragment.StartsWith("."))
                    {
                        newSelector.Classes.Add(fragment.Substring(1));
                    }
                    else
                    {
                        string potentialTagName = fragment.ToLower();

                        if (HtmlHelper.Instance.AllTags.Contains(potentialTagName))
                        {
                            newSelector.TagName = potentialTagName;
                        }
                    }
                }

                if (root == null)
                {
                    root = newSelector;
                    current = root;
                }
                else
                {
                    newSelector.Parent = current;
                    current.Child = newSelector;
                    current = newSelector;
                }
            }

            return root;
        }
    }
}