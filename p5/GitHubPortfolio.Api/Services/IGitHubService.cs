using GitHubPortfolio.Api.Models;
using Octokit;

namespace GitHubPortfolio.Api.Services
{
    public interface IGitHubService
    {
        Task<List<RepositoryDto>> GetPortfolioAsync();
        Task<IEnumerable<Repository>> SearchRepositoriesAsync(string? repoName, string? language, string? user);
    }
}