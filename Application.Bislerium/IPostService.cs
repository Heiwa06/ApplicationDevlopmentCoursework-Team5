using Domain.Bislerium;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Bislerium.RequestedViewModel.ResponseModel;

namespace Application.Bislerium
{
    public interface IPostService
    {
        Task<Post> AddPost (string userId, string Title, string ImageFilePath, IFormFile blogimageFile, string description);
        Task<Post?> UpdatePost (Post post);  
        Task DeletePost (string id);

        Task<IEnumerable<BlogModel>> GetAllPosts ();
        Task<IEnumerable<Post>> GetPostById (string id);
        
    }
}
