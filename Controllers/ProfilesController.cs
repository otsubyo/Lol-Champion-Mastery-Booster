using System.Security.Claims;
using Lol_Champion_Mastery_Booster.Data;
using Lol_Champion_Mastery_Booster.DTOs;
using Lol_Champion_Mastery_Booster.DTOs.Lol_Champion_Mastery_Booster.DTOs;
using Lol_Champion_Mastery_Booster.Models;
using Lol_Champion_Mastery_Booster.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lol_Champion_Mastery_Booster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // toutes les routes ici nécessitent un token JWT valide
    public class ProfilesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly RiotApiService _riotApi;

        public ProfilesController(AppDbContext db, RiotApiService riotApi)
        {
            _db = db;
            _riotApi = riotApi;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] CreateProfileDto dto)
        {
            // Vérifie si l'utilisateur est authentifié
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            // Crée un nouveau profil lié à l'utilisateur connecté
            var profile = new Profile
            {
                RiotName = dto.RiotName,
                Region = dto.Region,
                UserId = Guid.Parse(userId)
            };

            _db.Profiles.Add(profile);
            await _db.SaveChangesAsync();

            return Ok(profile);
        }

        [HttpGet("{id}/mastery")]
        public async Task<IActionResult> GetMastery(Guid id)
        {
            // Vérifie si l'utilisateur est authentifié
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            // Recherche le profil appartenant à l'utilisateur
            var profile = await _db.Profiles.FirstOrDefaultAsync(p => p.Id == id && p.UserId == Guid.Parse(userId));
            if (profile == null)
                return NotFound("Profile not found.");

            // Sépare le RiotName en gameName et tagLine
            var parts = profile.RiotName.Split('#');
            if (parts.Length != 2)
                return BadRequest("Invalid RiotName format. Use GameName#TAG.");

            var gameName = parts[0];
            var tagLine = parts[1];

            // Appelle l’API Riot pour récupérer le puuid
            var puuid = await _riotApi.GetPuuidAsync(gameName, tagLine, profile.Region);
            if (puuid == null)
                return NotFound("Could not fetch PUUID from Riot.");

            // Appelle l’API Riot pour récupérer les masteries
            var masteries = await _riotApi.GetMasteryAsync(puuid, profile.Region);

            // Trie par championPoints décroissant
            var sorted = masteries.OrderByDescending(m => m.championPoints).ToList();

            return Ok(sorted);
        }
    }
}
