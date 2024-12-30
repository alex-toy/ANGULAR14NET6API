using Microsoft.AspNetCore.Mvc;
using SoccerPlayerApi.Entities;
using SoccerPlayerApi.Services.Players;

namespace SoccerPlayerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {

        private readonly ILogger<PlayerController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPlayerService _playerService;

        public PlayerController(ILogger<PlayerController> logger, IConfiguration configuration, IPlayerService playerService)
        {
            _logger = logger;
            _configuration = configuration;
            _playerService = playerService;
        }

        [HttpGet("player/{playerId}")]
        public async Task<Player?> GetById(int playerId)
        {
            Player? articles = await _playerService.GetByIdAsync(playerId);
            return articles;
        }

        [HttpGet("players")]
        public async Task<IEnumerable<Player>> GetAll()
        {
            IEnumerable<Player> articles = await _playerService.GetAllAsync();
            return articles;
        }

        [HttpPost("player")]
        public async Task<int> Create(Player player)
        {
            int playerId = await _playerService.CreateAsync(player);
            return playerId;
        }

        [HttpPut("player")]
        public async Task<int> Update(Player player)
        {
            int playerId = await _playerService.UpdateAsync(player);
            return playerId;
        }

        [HttpDelete("player/{playerId}")]
        public async Task<bool> Delete(int playerId)
        {
            await _playerService.DeleteAsync(playerId);
            return true;
        }
    }
}
