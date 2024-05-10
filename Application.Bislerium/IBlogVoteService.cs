using Domain.Bislerium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Bislerium
{
    public interface IBlogVoteService
    {
        Task<BlogVote> UpVote(string userId, Guid? postId, /*Guid? commentId, Guid? replyId,*/ BlogVoteType blogVoteType);
        Task UpdateBlogVoteTyoe(string userId, Guid BlogVoteId, BlogVoteType newVoteType);
        Task DownVote(string userId, Guid BlogVoteId);
    }

}