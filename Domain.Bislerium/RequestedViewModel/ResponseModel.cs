using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Bislerium.RequestedViewModel
{
    public class ResponseModel
    {
        public class BlogModel
        {
            public Guid postId { get; set; }
            public string? userId { get; set; }
            public string? Title { get; set; }
            public string? ImagePath{ get; set; }
            public string? Description { get; set; }
            public string? Created { get; set; }
            public string? Updated { get; set; }
            public int UpVoteCount {get; set; }
            public int DownVoteCount { get; set; }
            public int TotalComment { get; set; }

            public Post First()
            {
                throw new NotImplementedException();
            }
        }

        public class CommentModel:BlogComment
        {
            public int UpVoteCount { get; set; }
            public int DownVoteCount { get; set; }
        }
    }
}
