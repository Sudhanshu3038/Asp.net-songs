namespace WebApp2.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public abstract class AuditableEntity
    {
        
        public int BaseId { get; set; }
        [Required]
        public string? CreatedBy { get; set; }
       
        public DateTime CreatedOn { get; set; } 
        [Required]
        public string? ModifiedBy { get; set; }
        [Required]
        public DateTime ModifiedOn { get; set; }

        public bool IsRowDeleted { get; set; } = false;
    }

}
