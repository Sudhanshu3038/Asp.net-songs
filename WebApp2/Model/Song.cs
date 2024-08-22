using System.ComponentModel.DataAnnotations;

namespace WebApp2.Model
{
    public class Song : AuditableEntity
    {
        public int Id { get; set; }
        [Required]
        public string SongName { get; set; }

        public string? FilePath { get; set; }

        public int ArtistId { get; set; }
        [Required]
        public Artist? Artist { get; set; }

        public int CategoryId { get; set; }
        [Required]
        public Category?  Category { get; set; }

        
    }
}
