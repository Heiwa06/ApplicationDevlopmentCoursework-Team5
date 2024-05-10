using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Bislerium;

namespace Application.Bislerium
{
    public interface IBloggerService
    {
        Task<Blogger> AddBlogger(Blogger blogger);
        Task<IEnumerable<Blogger>> GetAllBloggers();
        Task<Blogger?> UpdateBlogger(Blogger blogger);
        Task DeleteBlogger(string id);
        Task<IEnumerable<Blogger>> GetBloggerById(string id);

    }
}
