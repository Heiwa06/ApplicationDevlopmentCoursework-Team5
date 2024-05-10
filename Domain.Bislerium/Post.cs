using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Bislerium
{
    public class Post
    {
        [Key]
        public Guid postId { get; set; }
        [Required]
        public string? userId { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? ImagePath { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string? Description { get; set; }
        public DateTime? Created { get; set; }
        = DateTime.Now;
        public DateTime? Updated { get; set; }
        = DateTime.Now;
        public ICollection<BlogVote> Votes { get; set; }
        public ICollection<BlogComment> Comments { get; set;}

    }
}
