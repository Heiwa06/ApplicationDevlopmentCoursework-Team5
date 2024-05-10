using Application.Bislerium;
using Domain.Bislerium;
using Infrastructure.Coursework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Bislerium
{
    public class BlogVoteService : IBlogVoteService
    {
        private readonly ApplicationDBContext _dbContext;

        public BlogVoteService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BlogVote> UpVote(string userId, Guid? postId, BlogVoteType blogVoteType)
        {
            var blogVote = new BlogVote
            {
                BlogVoteId = Guid.NewGuid(),
                userId = userId,
                postId = (Guid)postId,
                BlogVoteType = blogVoteType
            };

            try
            {
                _dbContext.BlogVotes.Add(blogVote);
                await _dbContext.SaveChangesAsync();
                return blogVote;
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

        public async Task DownVote(string userId, Guid BlogVoteId)
        {
            var updatedvote = await _dbContext.BlogVotes.FirstOrDefaultAsync(v => v.BlogVoteId == BlogVoteId && v.userId == userId);

            if (updatedvote != null)
            {
                _dbContext.BlogVotes.Remove(updatedvote);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("You haven't vote yet.");
            }
        }

        public async Task UpdateBlogVoteTyoe(string userId, Guid BlogVoteId, BlogVoteType newVoteType)
        {
            var vote = await _dbContext.BlogVotes.FirstOrDefaultAsync(v => v.BlogVoteId == BlogVoteId && v.userId == userId);

            if (vote != null)
            {
                vote.BlogVoteType = newVoteType;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("You haven't vote yet.");
            }
        }
    }
}
