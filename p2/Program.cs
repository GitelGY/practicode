// See https://aka.ms/new-console-template for more information
using p2;

try
{
    // 1. בניית העץ
    HtmlLoader loader = new HtmlLoader();
    string url = "https://forum.netfree.link/category/1/%D7%94%D7%9B%D7%A8%D7%92%D7%95%D7%AA";

    Console.WriteLine($"Attempting to load: {url}...");
    string html = await loader.Load(url);

    HtmlParser parser = new HtmlParser();
    HtmlElement root = parser.Parse(html);

    Console.WriteLine("================================================================================");

    // 2. יצירת סלקטור לחיפוש
    string query = "div"; 
    var selector = Selector.Parse(query);
    Console.WriteLine($"Searching for selector: '{query}'");
    Console.WriteLine("================================================================================");

    // 3. חיפוש בעץ
    var results = root.Find(selector);

    // 4. הצגת התוצאות עם ה-InnerHtml
    Console.WriteLine($"Found {results.Count} matches:");
    Console.WriteLine("================================================================================");

    foreach (var result in results)
    {
        Console.WriteLine($"Element: <{result.Name}>");
        if (!string.IsNullOrEmpty(result.Id))
            Console.WriteLine($"   Id: {result.Id}");

        // הדפסת הטקסט שבתוך האלמנט
        string text = string.IsNullOrWhiteSpace(result.InnerHtml) ? "(Empty)" : result.InnerHtml.Trim();
        Console.WriteLine($"   Text: {text}");
        Console.WriteLine("--------------------------------------------------------------------------------");
    }
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Network error: The requested address cannot be reached. Make sure there is an internet connection: ({ex.Message})");
}
catch (Exception ex)
{
    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
}

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();