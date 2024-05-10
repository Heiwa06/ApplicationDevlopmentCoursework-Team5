using Domain.Bislerium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Bislerium.RequestedViewModel.FormViewModel;
using static Domain.Bislerium.RequestedViewModel.ResponseModel;

namespace Application.Bislerium
{
    public interface ICommentService
    {
        Task<BlogComment> AddComment(string userId, Guid postId, string textComment);
        Task<IEnumerable<CommentModel>> GetAllComments();
        Task<IEnumerable<BlogComment>> GetCommentbyId(string commentId);
        Task DeleteComment(string id);
        Task<BlogComment?> UpdateComment(CommentUpdateModel comment);

    }
}
