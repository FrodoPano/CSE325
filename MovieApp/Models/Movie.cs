using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;
        
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        
        [Required]
        [StringLength(30)]
        public string Genre { get; set; } = string.Empty;
        
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        
        [Required]
        [StringLength(5)]
        public string Rating { get; set; } = string.Empty;
    }
}