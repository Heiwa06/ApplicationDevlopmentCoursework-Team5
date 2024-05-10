using System.ComponentModel.DataAnnotations;

namespace Domain.Bislerium
{
    public class Blogger
    {
        [Key]

        public Guid Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Gender { get; set; }
        [Required]
        public string? Phone { get; set; }
    }
}
