using GitHubPortfolio.Api.Models;
using GitHubPortfolio.Api.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Octokit;

namespace GitHubPortfolio.Api.Services
{
    public class CachedGitHubService : IGitHubService
    {
        private readonly IGitHubService _innerService;
        private readonly IMemoryCache _cache;
        private readonly GitHubClient _checkClient;
        private readonly GitHubOptions _options;

        private const string PortfolioKey = "portfolio_data";
        private const string LastActivityKey = "last_activity_date";

        public CachedGitHubService(IGitHubService innerService, IMemoryCache cache, IOptions<GitHubOptions> options)
        {
            _innerService = innerService;
            _cache = cache;
            _options = options.Value;

            // יצירת קליינט קטן לצורך בדיקת פעילות בלבד
            _checkClient = new GitHubClient(new ProductHeaderValue("CV-Check"))
            {
                Credentials = new Credentials(_options.Token)
            };
        }

        public async Task<List<RepositoryDto>> GetPortfolioAsync()
        {
            // 1. שלוף את תאריך הפעילות האחרונה ב-GitHub (האתגר)
            var events = await _checkClient.Activity.Events.GetAllUserPerformed(_options.Username);
            var latestEventDate = events.FirstOrDefault()?.CreatedAt;

            // 2. בדוק אם המידע ב-Cache קיים ותואם לתאריך הפעילות האחרון
            if (_cache.TryGetValue(PortfolioKey, out List<RepositoryDto> cachedData) &&
                _cache.TryGetValue(LastActivityKey, out DateTimeOffset? lastSavedActivity))
            {
                // אם לא הייתה פעילות חדשה מאז השמירה האחרונה - החזר מה-Cache
                if (latestEventDate == lastSavedActivity)
                {
                    return cachedData;
                }
            }

            // 3. אם יש פעילות חדשה או שה-Cache ריק - שלוף נתונים חדשים מהשירות האמיתי
            var freshData = await _innerService.GetPortfolioAsync();

            // שמירה ב-Cache עם תאריך הפעילות העדכני
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1)); // הגנת גיבוי לשעה

            _cache.Set(PortfolioKey, freshData, cacheOptions);
            _cache.Set(LastActivityKey, latestEventDate, cacheOptions);

            return freshData;
        }

        // פונקציית החיפוש - עוברת ישירות לשירות האמיתי (בדרך כלל לא שומרים חיפוש משתנה ב-Cache)
        public async Task<IEnumerable<Repository>> SearchRepositoriesAsync(string? repoName, string? language, string? user)
        {
            return await _innerService.SearchRepositoriesAsync(repoName, language, user);
        }
    }
}