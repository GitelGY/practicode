using GitHubPortfolio.Api.Models;
using GitHubPortfolio.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitHubPortfolio.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        public PortfolioController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        // GET: api/Portfolio
        [HttpGet]
        public async Task<IActionResult> GetPortfolio()
        {
            var result = await _gitHubService.GetPortfolioAsync();
            return Ok(result);
        }

        // GET: api/Portfolio/search?user=Sara
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? repoName, [FromQuery] string? language, [FromQuery] string? user)
        {
            try
            {
                var repositories = await _gitHubService.SearchRepositoriesAsync(repoName, language, user);

                // מיפוי ל-Dto שמתאים בדיוק לשדות ב-index.html
                var result = repositories.Select(r => new RepositoryDto
                {
                    Name = r.Name,
                    Url = r.HtmlUrl,
                    StarsCount = r.StargazersCount, // JS: repo.starsCount
                    LastCommit = r.UpdatedAt.DateTime, // JS: repo.lastCommit
                    // בחיפוש כללי GitHub מחזיר שפה אחת עיקרית
                    Languages = !string.IsNullOrEmpty(r.Language) ? new List<string> { r.Language } : new List<string>()
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "שגיאה בתקשורת מול GitHub", details = ex.Message });
            }
        }
    }
}