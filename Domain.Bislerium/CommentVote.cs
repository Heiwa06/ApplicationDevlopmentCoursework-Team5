using Domaim.Bislerium;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Bislerium
{
    public class CommentVote
    {
        [Key]
        public Guid BlogCommentId { get; set; }
        public Guid commentId { get; set; }
        [Required]
        public string userId { get; set; }
        public Guid postId {  get; set; }
        [Required]
        public BlogVoteType BlogVoteType { get; set; }
        [NotMapped] // Not mapped to the database
        public bool IsUpvote => BlogVoteType == BlogVoteType.Upvote;

        [NotMapped] // Not mapped to the database
        public bool IsDownvote => BlogVoteType == BlogVoteType.Downvote;
        public DateTime? Created { get; set; }
        = DateTime.Now;
        public virtual AppUser? User { get; set; }
        public virtual Post? Post { get; set; }
        /*        public virtual BlogComment? BlogComment { get; set; }*/
    }
}
