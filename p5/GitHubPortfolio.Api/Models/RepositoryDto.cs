namespace GitHubPortfolio.Api.Models
{
    public class RepositoryDto
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Languages { get; set; } = new();
        public DateTimeOffset LastCommit { get; set; }
        public int StarsCount { get; set; } // זה השם שחייב להופיע כאן
        public int PullRequestsCount { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}