using Application.Bislerium;
using Application.Bislerium.Utils;
using Domain.Bislerium;
using Infrastructure.Bislerium.Migrations;
using Infrastructure.Coursework;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Domain.Bislerium.RequestedViewModel.ResponseModel;

namespace Infrastructure.Bislerium
{
    public class PostService : IPostService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IImageService _imageService;

        public PostService(ApplicationDBContext dbContext, IImageService fs)
        {
            _dbContext = dbContext;
            this._imageService = fs;
        }

        public async Task<Post?> AddPost(string userId, string Title, string ImageFilePath, IFormFile blogimageFile, string description)
        {
            if (blogimageFile != null)
            {
                var Result = _imageService.SaveImageFile(blogimageFile);
                if (Result.Item1 == 1)
                {
                    var post = new Post
                    {
                        postId = Guid.NewGuid(),
                        userId = userId,
                        Title = Title,
                        ImagePath = Result.Item2,
                        Description = description
                    };
                    _dbContext.Posts.Add(post);
                    await _dbContext.SaveChangesAsync();

                    return post;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        public async Task DeletePost(string id)
        {
            Guid postId = Guid.Parse(id);

            var existingPost = await _dbContext.Posts.FindAsync(postId);
            if (existingPost != null)
            {
                _dbContext.Posts.Remove(existingPost);

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Post not found.");
            }
        }

        public async Task<IEnumerable<BlogModel>> GetAllPosts()
        {
            var data = await _dbContext.Posts
            .Include(blog => blog.Votes) // Include related reactions
            .Include(blog => blog.Comments) // Include related comments
                    .AsNoTracking()
            .ToListAsync();
            // Adapt each blog entity to BlogModel
            var responseDTOs = data.Select(blog =>
            {
                var blogResponseDTO = blog.Adapt<BlogModel>();

                // Calculate upvote count
                blogResponseDTO.UpVoteCount = blog.Votes?.Count(r => r.IsUpvote) ?? 0;

                // Calculate downvote count
                blogResponseDTO.DownVoteCount = blog.Votes?.Count(r => !r.IsUpvote) ?? 0;

                blogResponseDTO.TotalComment = blog.Comments?.Count() ?? 0;
                return blogResponseDTO;
            });

            return responseDTOs;
        }


        public async Task<IEnumerable<Post>> GetPostById(string id)
        {
            var result = await _dbContext.Posts.Where(p => p.postId.ToString() == id).ToListAsync();
            return result;
        }

        public async Task<Post?> UpdatePost(Post post)
        {
            var existingPost = await _dbContext.Posts.FindAsync(post.postId);
            if (existingPost != null)
            {
                existingPost.Title = post.Title;
                existingPost.Description = post.Description;
                existingPost.ImagePath = post.ImagePath;
                existingPost.Updated = DateTime.Now;

                _dbContext.Entry(existingPost).State = EntityState.Modified;    

                await _dbContext.SaveChangesAsync();
                return existingPost;
            }
            else
            {
                return null;
            }
        }

    }
}
