using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OT.Assessment.Model.Request;
using OT.Assessment.Model.Response;
using OT.Assessment.Services.BusinessLogic.Interfaces;

namespace OT.Assessment.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderService _providerService;
        public ProviderController(IProviderService providerService)
        {
            _providerService = providerService;
        }


        [HttpPost("provider")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> CreateProviderAsync([FromBody] ProviderCreateRequest request)
        {
            try
            {
                var result = await _providerService.PublishProviderCreationAsync(request);
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

        [HttpPost("game")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> CreateGameAsync([FromBody] GameCreateRequest request)
        {
            try
            {
                var result = await _providerService.CreateGameAsync(request);
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
