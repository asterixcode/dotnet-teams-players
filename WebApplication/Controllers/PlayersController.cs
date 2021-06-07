using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase
    {
        private TeamDbContext teamDbContext;

        public PlayersController(TeamDbContext teamDbContext)
        {
            this.teamDbContext = teamDbContext;
        }
        
        [HttpPost]
        [Route("{teamName}")]
        public async Task<ActionResult<Player>> AddPlayerAsync([FromBody] Player player, [FromRoute] string teamName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                await teamDbContext.Players.AddAsync(player);
                
                Team TeamToAddNewPlayer = 
                    await teamDbContext
                        .Teams
                        .Include(p => p.Players)
                        .FirstAsync(t => t.TeamName.Equals(teamName));
                
                TeamToAddNewPlayer.Players.Add(player);
                
                teamDbContext.Update(TeamToAddNewPlayer);
                await teamDbContext.SaveChangesAsync();
                
                return Ok(player);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
        
        
        [HttpDelete]
        [Route("{shirtNumber:int}")]
        public async Task<ActionResult<Player>> DeletePlayer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                Player playerToDelete = await teamDbContext.Players.FindAsync(id);

                if (playerToDelete == null)
                {
                    return NotFound($"Player with Id = {id} not found");
                }
                teamDbContext.Players.Remove(playerToDelete);
                await teamDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
            
            return NoContent();
        }
        
    }
}