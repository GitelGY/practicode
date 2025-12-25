using GitHubPortfolio.Api.Options;
using GitHubPortfolio.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// --- 1. הוספת בקרים ו-Swagger ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- 2. הגדרת CORS (חובה בשביל דף ה-HTML שיצרנו) ---
// מאפשר לדפדפן לגשת ל-API ממקורות אחרים (כמו קובץ מקומי)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --- 3. הגדרת ה-Options (חיבור ל-Secrets.json) ---
builder.Services.Configure<GitHubOptions>(builder.Configuration.GetSection("GitHubOptions"));

// --- 4. הגדרת Memory Cache ---
builder.Services.AddMemoryCache();

// --- 5. רישום שירותים עם תבנית Decorator (באמצעות Scrutor) ---
// קודם נרשום את השירות הבסיסי
builder.Services.AddScoped<IGitHubService, GitHubService>();

// לאחר מכן "נלביש" עליו את שכבת ה-Cache
builder.Services.Decorate<IGitHubService, CachedGitHubService>();

var app = builder.Build();

// --- 6. הגדרת ה-Pipeline של הבקשות ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// הפעלת ה-CORS שרשמנו למעלה
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();