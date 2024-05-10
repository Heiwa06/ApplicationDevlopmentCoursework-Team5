using Application.Bislerium;
using Domain.Bislerium;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Domain.Bislerium.RequestedViewModel.FormViewModel;

namespace Presentation.Bislerium.Controllers
{
    public class BlogVoteController : Controller
    {
        private readonly IBlogVoteService _blogVoteService;
        public BlogVoteController(IBlogVoteService blogVoteService)
        {
            _blogVoteService = blogVoteService;
        }

        [HttpPost, Route("post/upVote")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> UpVote([FromBody] VoteViewModel model)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id claim not found in token.");
                }
                var blogVote = await _blogVoteService.UpVote(userId, model.PostId, model.BlogVoteType);
                return Ok(blogVote);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete, Route("post/undoVote")]
        [AllowAnonymous]
        public async Task<IActionResult> UndoVote(Guid BlogVoteId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id claim not found in token.");
                }

                await _blogVoteService.DownVote(userId, BlogVoteId);

                return Ok("Vote removed successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut, Route("update/Vote")]
        [Authorize(Roles = "Admin, Blogger")]
        public async Task<IActionResult> UpdateBlogVoteType(Guid voteId, BlogVoteType newVoteType)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User id claim not found in token.");
                }

                await _blogVoteService.UpdateBlogVoteTyoe(userId, voteId, newVoteType);
                return Ok("Vote type updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}