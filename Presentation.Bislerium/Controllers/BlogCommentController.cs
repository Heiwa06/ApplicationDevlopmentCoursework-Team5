using Application.Bislerium;
using Domain.Bislerium;
using Infrastructure.Bislerium;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using System;
using System.ComponentModel.Design;
using System.Security.Claims;
using System.Threading.Tasks;
using static Domain.Bislerium.RequestedViewModel.FormViewModel;

namespace Presentation.Bislerium.Controllers
{
    public class BlogCommentController : Controller
    {
        private readonly ICommentService commentService;

        public BlogCommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpPost, Route("blog/addComment")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> AddComment([FromBody] CommentViewModel model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id claim not found in token.");
                }
                var comment = await commentService.AddComment(userId, model.PostId, model.TextComment);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet, Route("blog/getAllComment")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllComments()
        {
            var blogComments = await commentService.GetAllComments();
            return Ok(blogComments);
        }

        [HttpGet, Route("blog/getCommentById")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCommentbyId(string id)
        {
            var comment = await commentService.GetCommentbyId(id);
            if (comment == null)
            {
                // If the result is null, it means the comment was not found
                return NotFound("Comment not found");
            }

            // If the comment was successfully updated, return Ok response
            return Ok(comment);
        }

        [HttpDelete, Route("blog/deleteComment")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> DeleteComment(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            await commentService.DeleteComment(id);

            return Ok();
        }

        [HttpDelete, Route("blog/updateComment")]
        [Authorize(Roles = "Blogger")]
        public async Task<BlogComment> UpdateComment(CommentUpdateModel comment)
        {
            var result = await commentService.UpdateComment(comment);

            if (result == null)
            {
                // If the result is null, it means the post was not found
                return null;
            }

            // If the post was successfully updated, return Ok response
            return result;
        }


    }
}
