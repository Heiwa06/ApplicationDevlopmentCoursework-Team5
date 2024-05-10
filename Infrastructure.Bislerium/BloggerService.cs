
using Application.Bislerium;
using Domain.Bislerium;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Coursework
{
    public class BloggerService : IBloggerService
    {
        private readonly ApplicationDBContext _dbContext;

        public BloggerService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Blogger> AddBlogger(Blogger blogger)
        {
            var result = await _dbContext.Bloggers.AddAsync(blogger);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteBlogger(string id)
        {
            // Convert the provided string ID to a Guid
            Guid bloggerId = Guid.Parse(id);

            // Find the blogger by id
            var existingBlogger = await _dbContext.Bloggers.FindAsync(bloggerId);

            if (existingBlogger != null)
            {
                // Remove the blogger from the DbSet
                _dbContext.Bloggers.Remove(existingBlogger);

                // Save changes to the database
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // If the blogger with given Id is not found, you might want to handle this case accordingly

                throw new InvalidOperationException("Blogger not found.");
            }
        }

        public async Task<IEnumerable<Blogger>> GetAllBloggers()
        {
            return await _dbContext.Bloggers.ToListAsync();
        }

        public async Task<IEnumerable<Blogger>> GetBloggerById(string id)
        {
            var result = await _dbContext.Bloggers.Where(s => s.Id.ToString() == id).ToListAsync();
            return result;
        }


        public async Task<Blogger?> UpdateBlogger(Blogger blogger)
        {
            var existingBlogger = await _dbContext.Bloggers.FindAsync(blogger.Id);

            if (existingBlogger != null)
            {
                // Update existing blogger entity with new values
                existingBlogger.Name = blogger.Name;
                existingBlogger.Email = blogger.Email;
                existingBlogger.Gender = blogger.Gender;
                existingBlogger.Phone = blogger.Phone;

                // Update other properties as needed

                await _dbContext.SaveChangesAsync();
                return existingBlogger;
            }
            else
            {
                // If the blogger with given Id is not found, you might want to handle this case accordingly
                return null;
            }
        }
    }
}
