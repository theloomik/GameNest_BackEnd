using GameNest_BackEnd.Data;
using GameNest_BackEnd.DTOs;
using GameNest_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameNest_BackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly GameNestDbContext _context;

    public GamesController(GameNestDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetGames()
    {
        var games = await _context.Games.ToListAsync();

        return Ok(games);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGame(CreateGameDto dto)
    {
        var game = new Game
        {
            Title = dto.Title,
            Description = dto.Description,
            CoverUrl = dto.CoverUrl,
            TrailerUrl = dto.TrailerUrl,
            Developer = dto.Developer,
            Publisher = dto.Publisher,
            ReleaseDate = dto.ReleaseDate
        };

        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGames), new { id = game.Id }, game);
    }
}