using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorApplication.Models
{
    public class Team
    {
        public string TeamName { get; set; }
        
        [Required, MaxLength(50)]
        public string NameOfCoach { get; set; }
        
        public int Ranking { get; set; }
        
        public IList<Player> Players { get; set; }
    }
}