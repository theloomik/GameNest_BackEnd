namespace GameNest_BackEnd.DTOs;

public class CreateGameDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string CoverUrl { get; set; } = string.Empty;

    public string TrailerUrl { get; set; } = string.Empty;

    public string Developer { get; set; } = string.Empty;

    public string Publisher { get; set; } = string.Empty;

    public DateTime? ReleaseDate { get; set; }
}