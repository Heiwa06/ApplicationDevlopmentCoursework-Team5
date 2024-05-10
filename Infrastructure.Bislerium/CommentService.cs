using Application.Bislerium;
using Domain.Bislerium;
using Infrastructure.Coursework;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Bislerium.RequestedViewModel.FormViewModel;
using static Domain.Bislerium.RequestedViewModel.ResponseModel;

namespace Infrastructure.Bislerium
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDBContext _dbContext;

        public CommentService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BlogComment> AddComment(string userId, Guid postId, string textComment)
        {
            var postComment = new BlogComment
            {
                userId = userId,
                postId = (Guid)postId,
                TextComment = textComment
            };

            try
            {
                _dbContext.BlogComments.Add(postComment);
                await _dbContext.SaveChangesAsync();
                return postComment;
            }
            catch (DbUpdateException ex)
            {
                // Handle unique constraint violation
                if (ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlException && sqlException.Number == 2601)
                {
                    // Unique constraint violation
                    Console.WriteLine(ex);
                    throw new InvalidOperationException("Already liked this post.");
                }
                throw;
            }
        }

        public async Task DeleteComment(string id)
        {
            var comment = await _dbContext.BlogComments.FindAsync(Guid.Parse(id));
            if (comment != null)
            {
                _dbContext.BlogComments.Remove(comment);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CommentModel>> GetAllComments()
        {
            var data = await _dbContext.BlogComments
            .Include(blog => blog.Votes) // Include related reactions
                    .AsNoTracking()
            .ToListAsync();
            // Adapt each blog entity to BlogModel
            var responseDTOs = data.Select(blog =>
            {
                var blogResponseDTO = blog.Adapt<CommentModel>();

                // Calculate upvote count
                blogResponseDTO.UpVoteCount = blog.Votes?.Count(r => r.IsUpvote) ?? 0;

                // Calculate downvote count
                blogResponseDTO.DownVoteCount = blog.Votes?.Count(r => !r.IsUpvote) ?? 0;
                return blogResponseDTO;
            });

            return responseDTOs;
        }

        public async Task<IEnumerable<BlogComment>> GetCommentbyId(string id)
        {
            var results = await _dbContext.BlogComments.Where(c => c.commentId.ToString() == id).ToListAsync();
            return results;
        }


        public async Task<BlogComment?> UpdateComment(CommentUpdateModel comment)
        {
            var existingComment = await _dbContext.BlogComments.FindAsync(comment.CommentId);
            if (existingComment != null)
            {
                existingComment.TextComment = comment.TextComment;
                _dbContext.Entry(existingComment).State = EntityState.Modified;

                await _dbContext.SaveChangesAsync();
                return existingComment;
            }
            else
            {
                return null;
            }
        }
    }
}
