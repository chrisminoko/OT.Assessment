using Microsoft.AspNetCore.Mvc;
namespace OT.Assessment.App.Controllers
{
  
    [ApiController]
    public class PlayerController : ControllerBase
    {
        //POST api/player/casinowager

        //GET api/player/{playerId}/wagers

        //GET api/player/topSpenders?count=10
        //

        [HttpPost("VerifyPayent")]
        public async Task<IActionResult> Test(string reference)
        {
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../OT.Assessment.App"))
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                 .Build();
            
            var connectionString = configuration.GetConnectionString("DatabaseConnection")
                ?? throw new InvalidOperationException("Connection string 'DatabaseConnection' not found.");

            return Ok();

        }

      
    }
}
