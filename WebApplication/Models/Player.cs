using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Range(1, 99, ErrorMessage = "Please insert a shirt number between 1 and 99")]
        public int ShirtNumber { get; set; }
        
        public decimal Salary { get; set; }
        public int GoalsThisSeason { get; set; }
        
        [Required]
        public string Position { get; set; }
    }
}   