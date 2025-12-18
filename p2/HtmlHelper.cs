using System;
using System.Collections.Generic;
using System.Text;

using System.Text.Json;

namespace p2
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;

        public string[] AllTags { get; set; }
        public string[] SelfClosingTags { get; set; }

        private HtmlHelper()
        {

            var allTagsContent = File.ReadAllText("C:/Users/tzipi/Documents/פרקטיקוד/p2/p2/JSON Files/HtmlTags.json");
            var selfClosingContent = File.ReadAllText("C:/Users/tzipi/Documents/פרקטיקוד/p2/p2/JSON Files/HtmlVoidTags.json");

            AllTags = JsonSerializer.Deserialize<string[]>(allTagsContent);
            SelfClosingTags = JsonSerializer.Deserialize<string[]>(selfClosingContent);
        }
    }
}
