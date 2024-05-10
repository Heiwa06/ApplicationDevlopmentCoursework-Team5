using Application.Bislerium;
using Domain.Bislerium;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using static Domain.Bislerium.RequestedViewModel.FormViewModel;

namespace Presentation.Bisleium.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }


        [HttpPost, Route("AddPost")]
        [Authorize(Roles = "Blogger")]
        public async Task<IActionResult> AddPost([FromForm] PostViewModel model, IFormFile blogimageFile)
        {
            try
            {
                // Retrieve the user ID of the currently logged-in user
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound("User ID not found."); // Return a "Not Found" error message
                }

                // Assuming model contains Title, ImagePath, and Description
                var addPost = await postService.AddPost(userId, model.Title, model.ImagePath, blogimageFile, model.Description);
                return Ok(addPost);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet, Route("GetAllPosts")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await postService.GetAllPosts();
            return Ok(posts);
        }

        [HttpGet, Route("GetPostById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostById(String id)
        {
            var post = await postService.GetPostById(id);
            if (post == null)
            {
                // If the result is null, it means the post was not found
                return NotFound("Post not found");
            }

            // If the post was successfully updated, return Ok response
            return Ok(post);
        }

        [HttpPut, Route("UpdatePost")]
        [Authorize(Roles = "Blogger")]

        public async Task<IActionResult> UpdatePost(Post post)
        {
            var result = await postService.UpdatePost(post);

            if (result == null)
            {
                // If the result is null, it means the post was not found
                return NotFound("Post not found");
            }

            // If the post was successfully updated, return Ok response
            return Ok(result);
        }



        [HttpDelete, Route("DeletePost")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> DeletePost(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            await postService.DeletePost(id);

            return Ok();
        }
    }
}