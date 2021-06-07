using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.DataAccess;
using WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamsController : ControllerBase
    {
        private TeamDbContext teamDbContext;

        public TeamsController(TeamDbContext teamDbContext)
        {
            this.teamDbContext = teamDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Team>>> GetAllTeamsAsync([FromQuery] int? ranking, [FromQuery]string? teamName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                if (teamName != null && ranking != null)
                {
                    return await teamDbContext.Teams
                        .Where(t => t.Ranking >= ranking)
                        .Where(t => t.TeamName.Contains(teamName))
                        .Include(p => p.Players)
                        .ToListAsync();
                    
                }

                return await teamDbContext.Teams.Include(t => t.Players).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<Team>> AddTeamAsync([FromBody] Team team)
        {
            try
            {
                teamDbContext.Teams.Add(team);
                await teamDbContext.SaveChangesAsync();
                
                return Created($"/{team.TeamName}", team);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}