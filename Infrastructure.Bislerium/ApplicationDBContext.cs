using Domaim.Bislerium;
using Domain.Bislerium;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Coursework
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=MUNA; Database=Bislerium; Trusted_Connection=True; TrustServerCertificate=True; MultipleActiveResultSets=True");
        }

        public DbSet<Blogger> Bloggers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<BlogVote> BlogVotes { get; set; }


        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<CommentVote> CommentVotes { get; set; }
        /*public DbSet<Notification> Notifications { get; set; }
        public DbSet<History> Historys { get; set; }*/

    }

}
