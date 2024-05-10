using Domaim.Bislerium;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Domain.Bislerium
{
    public class BlogVote
    {
        [Key]
        public Guid BlogVoteId { get; set; }
        public Guid postId { get; set; }
        [Required]
        public string userId { get; set; }
        [Required]
        public BlogVoteType BlogVoteType { get; set; }
        [NotMapped] // Not mapped to the database
        public bool IsUpvote => BlogVoteType == BlogVoteType.Upvote;

        [NotMapped] // Not mapped to the database
        public bool IsDownvote => BlogVoteType == BlogVoteType.Downvote ;
        /*        public Guid BlogCommentId { get; set; }
                public Guid BlogReplyId {  get; set; }*/
        public DateTime? Created { get; set; }
        = DateTime.Now;
        public virtual AppUser? User { get; set; }
        public virtual Post? Post { get; set; }
        /*        public virtual BlogComment? BlogComment { get; set; }
                public virtual BlogReply? BlogReply { get; set; }*/
    }

    public enum BlogVoteType
    {
        Downvote = 0,
        Upvote = 1
    }
}