using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OT.Assessment.Model.Request;
using OT.Assessment.Model.Response;
using OT.Assessment.Services.BusinessLogic.Interfaces;

namespace OT.Assessment.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        public GameController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpPost("game")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> CreateCasionWager([FromBody] GameCreateRequest request)
        {
            try
            {
                var result = await _playerService.CreateGameAsync(request);
                if (result.IsSuccessful)
                {
                    return Ok(result);
                }
                return BadRequest(new BaseResponse { IsSuccessful = true, Message = result.Message });
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                new { error = "Service temporarily unavailable" });
            }

        }
    }
}
