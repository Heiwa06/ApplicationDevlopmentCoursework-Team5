using Domaim.Bislerium;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Bislerium
{
    public class BlogComment
    {
        [Key]
        public Guid commentId { get; set; }
        [Required]
        public string? userId { get; set; }
        [Required]
        public Guid postId { get; set; }
        [Required]
        public string? TextComment { get; set; }
        public DateTime? CreatedAt { get; set; }
        = DateTime.Now;
        public AppUser? User { get; set; }
        [ForeignKey("postId")]
        public Post? Post { get; set; }
        // Navigation properties (optional)

        public ICollection<BlogVote> Votes { get; set; }
        /*        public ICollection<Reply>? Replys { get; set; }*/
        /*        public ICollection<History>? Historys { get; set; }*/
    }
}
