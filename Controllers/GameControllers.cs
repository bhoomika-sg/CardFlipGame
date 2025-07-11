using Microsoft.AspNetCore.Mvc;
using MemoryCardGame.GameLogic;

namespace MemoryCardGame.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private static MemoryGame game = new MemoryGame();

        [HttpPost("start")]
        public IActionResult StartGame()
        {
            game = new MemoryGame();
            return Ok();
        }

        [HttpPost("flip")]
        public IActionResult FlipCard([FromBody] FlipRequest request)
        {
            game.FlipCard(request.Row, request.Col);
            return Ok();
        }

        [HttpPost("hide")]
        public IActionResult HideCards()
        {
            game.HideUnmatched();
            return Ok();
        }

        [HttpGet("board")]
        public IActionResult GetBoard()
        {
            return Ok(game.GetBoard());
        }

        public class FlipRequest
        {
            public int Row { get; set; }
            public int Col { get; set; }
        }
    }
}
