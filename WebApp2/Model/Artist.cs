namespace WebApp2.Model
{
    public class Artist : AuditableEntity
    {
        public int ArtistId { get; set; }

        public string? ArtistName { get; set; }
    }
}
