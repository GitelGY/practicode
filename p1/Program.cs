using System.CommandLine;
using System.Text;
using System.Text.RegularExpressions;


var languageOption = new Option<List<string>>(
    name: "--language",
    description: "List of programming languages (extensions) to include. Use 'all' for all code files.")
{
    IsRequired = true,
    AllowMultipleArgumentsPerToken = true
};
languageOption.AddAlias("-l");

var bundleOption = new Option<FileInfo>("--output", "File path and name of the bundle file.");
bundleOption.AddAlias("-o");

var noteOption = new Option<bool>(
    name: "--note",
    description: "Indicates whether to include the source file path as a comment.")
{
    Arity = ArgumentArity.ZeroOrOne
};
noteOption.AddAlias("-n");

var sortOption = new Option<string>(
    name: "--sort",
    description: "Order of code files: 'name' (default) or 'type'.",
    getDefaultValue: () => "name"
);
sortOption.AddAlias("-s");
sortOption.AddValidator(result =>
{
    var sortValue = result.GetValueOrDefault<string>()?.ToLower();
    if (sortValue != "name" && sortValue != "type")
    {
        result.ErrorMessage = "Error: --sort must be 'name' or 'type'. Defaulting to 'name'.";
    }
});

var removeEmptyLinesOption = new Option<bool>(
    name: "--remove-empty-lines",
    description: "Indicates whether to remove empty lines from source code.")
{
    Arity = ArgumentArity.ZeroOrOne
};
removeEmptyLinesOption.AddAlias("-r");

var authorOption = new Option<string>(
    name: "--author",
    description: "Name of the file author to be included as a comment at the beginning of the bundle file.");
authorOption.AddAlias("-a");


var bundleCommand = new Command("bundle", "Bundles code files into a single output file.");

bundleCommand.AddOption(languageOption);
bundleCommand.AddOption(bundleOption);
bundleCommand.AddOption(noteOption);
bundleCommand.AddOption(sortOption);
bundleCommand.AddOption(removeEmptyLinesOption);
bundleCommand.AddOption(authorOption);

// קורא מטא-דאטה, ומבצע את איסוף הקבצים, המיון והשמירה.
bundleCommand.SetHandler((language, output, note, sort, removeEmptyLines, author) =>
{
    try
    {
        // קבלת נתיב התיקייה הנוכחית
        var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

        // הגדרת קבצים לא רצויים בתיקיות
        string[] ignoredDirectories = { "bin", "obj", ".vs", "debug", "release" };

        // חיפוש רקורסיבי של כל הקבצים, תוך התעלמות מתיקיות לא רצויות
        var files = currentDirectory.GetFiles("*", SearchOption.AllDirectories)
            .Where(f => !ignoredDirectories.Any(dir => f.FullName.Contains($"\\{dir}\\")));

        // סינון לפי שפות
        if (!language.Contains("all", StringComparer.OrdinalIgnoreCase))
        {
            var extensions = language.Select(l => $".{l.TrimStart('.')}");
            files = files.Where(f => extensions.Contains(f.Extension, StringComparer.OrdinalIgnoreCase));
        }

        // מיון הקבצים
        if (sort.Equals("type", StringComparison.OrdinalIgnoreCase))
        {
            files = files.OrderBy(f => f.Extension).ThenBy(f => f.Name);
        }
        else // ברירת מחדל: name
        {
            files = files.OrderBy(f => f.Name);
        }

        // יצירת תוכן הקובץ המאוחד
        var bundleContent = new StringBuilder();

        // הוספת הערת מחבר במידת הצורך
        if (!string.IsNullOrWhiteSpace(author))
        {
            bundleContent.AppendLine($"// Author: {author}");
            bundleContent.AppendLine("// --------------------------------------------------------------------------------");
        }

        foreach (var file in files)
        {
            // הוספת הערה עם נתיב הקובץ אם נדרש
            if (note)
            {
                var relativePath = Path.GetRelativePath(currentDirectory.FullName, file.FullName);
                bundleContent.AppendLine($"// Source: {relativePath}");
            }

            var fileContent = File.ReadAllLines(file.FullName);

            // ניקוי שורות ריקות במידת הצורך
            if (removeEmptyLines)
            {
                fileContent = fileContent.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            }

            bundleContent.AppendLine(string.Join(Environment.NewLine, fileContent));
            bundleContent.AppendLine(Environment.NewLine); // הפרדה ברורה בין קבצים
        }

        // שמירת הקובץ
        File.WriteAllText(output.FullName, bundleContent.ToString());
        Console.WriteLine($"✅ Successfully bundled {files.Count()} files to: {output.FullName}");
    }
    catch (DirectoryNotFoundException)
    {
        Console.WriteLine("❌ Error: The specified output path is invalid or the directory does not exist.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ An unexpected error occurred: {ex.Message}");
    }

}, languageOption, bundleOption, noteOption, sortOption, removeEmptyLinesOption, authorOption);

// --------------------------------------------------------------------------------
// הגדרת פקודת create-rsp
// --------------------------------------------------------------------------------

var createRspCommand = new Command("create-rsp", "Generates a response file (.rsp) for the bundle command.");

// המטפל של create-rsp
createRspCommand.SetHandler(() =>
{
    // רשימה של כל האפשרויות בפקודת bundle
    var options = new List<(Option option, string prompt)>
    {
        (languageOption, "Enter a comma-separated list of languages (e.g., cs,js) or 'all':"),
        (bundleOption, "Enter the output file path and name (e.g., ./bundle.txt):"),
        (noteOption, "Include source file path as comment? (Y/N):"),
        (sortOption, "Sort files by 'name' or 'type' (default is name):"),
        (removeEmptyLinesOption, "Remove empty lines? (Y/N):"),
        (authorOption, "Enter the author's name (optional):")
    };

    var rspCommandArgs = new List<string> { "bundle" };

    foreach (var (option, prompt) in options)
    {
        Console.Write($"❓ {prompt} ");
        var input = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(input) && option != authorOption)
        {
            // בדיקה וטיפול באי-קלט לאפשרויות חובה
            if (option.IsRequired)
            {
                Console.WriteLine("⚠️ This option is required. Please try again.");
                return;
            }
            continue; // דלג אם הקלט ריק עבור אפשרות לא חובה
        }

        // טיפול מיוחד לאפשרויות בוליאניות והגדרת פרמטרים
        if (option == noteOption || option == removeEmptyLinesOption)
        {
            if (input.Equals("Y", StringComparison.OrdinalIgnoreCase) || input.Equals("Yes", StringComparison.OrdinalIgnoreCase))
            {
                rspCommandArgs.Add(option.Name); // דגל בוליאני מופיע ללא ערך
            }
        }
        else if (option == languageOption)
        {
            // טיפול בקלט מרובה (שפות)
            var languages = input.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(l => l.StartsWith('.') ? l.Substring(1) : l)
                                 .ToArray();

            if (languages.Length > 0)
            {
                rspCommandArgs.Add(option.Name);
                rspCommandArgs.AddRange(languages);
            }
        }
        else
        {
            // טיפול כללי: עוטף ערכים שיש בהם רווחים במירכאות
            var value = input.Contains(' ') ? $"\"{input}\"" : input;
            rspCommandArgs.Add(option.Name);
            rspCommandArgs.Add(value);
        }
    }

    var finalCommand = string.Join(" ", rspCommandArgs);
    var rspFileName = "bundle.rsp";

    // כתיבת הפקודה לקובץ תגובה
    try
    {
        File.WriteAllText(rspFileName, finalCommand);
        Console.WriteLine($"\n⭐ Response file '{rspFileName}' created successfully.");
        Console.WriteLine($"\nTo run the bundle command, use: dotnet @{rspFileName}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error writing response file: {ex.Message}");
    }
});


var rootCommand = new RootCommand("A CLI tool to bundle code files.");
rootCommand.AddCommand(bundleCommand);
rootCommand.AddCommand(createRspCommand);

await rootCommand.InvokeAsync(args);