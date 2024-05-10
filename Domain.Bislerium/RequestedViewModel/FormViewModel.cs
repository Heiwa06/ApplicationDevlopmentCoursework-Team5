using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Bislerium.RequestedViewModel
{
    public class FormViewModel
    {
        public class PostViewModel
        {
            public string? Title { get; set; }
            public string? Description { get; set; }
            public string? ImagePath { get; set; }
        }

        public class VoteViewModel
        {
            public Guid? PostId { get; set; }
            public BlogVoteType BlogVoteType { get; set; }
        }

        public class CommentViewModel
        {
            [Required]
            public Guid PostId { get; set; }
            public string? TextComment { get; set; }
        }

        public class CommentUpdateModel
        {
            [Required]
            public Guid CommentId { get; set; }
            [Required]
            public Guid PostId { get; set; }
            public string? TextComment { get; set; }

        }


        public class VoteCommentViewModel
        {
            public Guid PostId { get; set;}
            public Guid CommentId { get; set;}
            public BlogVoteType BlogVoteType { get; set; }

        }


    }
}
