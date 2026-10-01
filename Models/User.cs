namespace GameNest_BackEnd.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "User";

    public ICollection<UserGame> UserGames { get; set; } = new List<UserGame>();

    public ICollection<GameSubmission> GameSubmissions { get; set; } = new List<GameSubmission>();
}