namespace WebApp2.Model
{
    public class Category : AuditableEntity
    {
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }
    }
}
