using GitHubPortfolio.Api.Models;
using GitHubPortfolio.Api.Options;
using Microsoft.Extensions.Options;
using Octokit;

namespace GitHubPortfolio.Api.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly GitHubOptions _options;

        public GitHubService(IOptions<GitHubOptions> options)
        {
            _options = options.Value;
            _client = new GitHubClient(new ProductHeaderValue("GitHubPortfolioApp"))
            {
                Credentials = new Credentials(_options.Token)
            };
        }

        // שליפת הפורטפוליו האישי שלך
        public async Task<List<RepositoryDto>> GetPortfolioAsync()
        {
            var repos = await _client.Repository.GetAllForUser(_options.Username);
            var portfolio = new List<RepositoryDto>();

            foreach (var repo in repos)
            {
                var languages = await _client.Repository.GetAllLanguages(repo.Owner.Login, repo.Name);

                portfolio.Add(new RepositoryDto
                {
                    Name = repo.Name,
                    Url = repo.HtmlUrl,
                    StarsCount = repo.StargazersCount, // תואם ל-HTML שלך
                    LastCommit = repo.UpdatedAt.DateTime,
                    Languages = languages.Select(l => l.Name).ToList()
                });
            }

            return portfolio;
        }

        // חיפוש משתמשים/פרויקטים אחרים
        public async Task<IEnumerable<Repository>> SearchRepositoriesAsync(string? repoName, string? language, string? user)
        {
            // אם לא הוזן שם פרויקט, נחפש באופן כללי לפי המשתמש
            var request = new SearchRepositoriesRequest(repoName ?? $"user:{user}");

            if (!string.IsNullOrEmpty(language))
                request.Language = Enum.Parse<Language>(language, true);

            if (!string.IsNullOrEmpty(user) && string.IsNullOrEmpty(repoName))
                request.User = user;

            var result = await _client.Search.SearchRepo(request);
            return result.Items;
        }
    }
}