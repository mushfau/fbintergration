using Microsoft.EntityFrameworkCore;

namespace FBIntergrationApi.Models
{
    public class SocialmediasubcommentContext : DbContext
    {
        public SocialmediasubcommentContext(DbContextOptions<SocialmediasubcommentContext> options)
            : base(options)
        {
        }

        public DbSet<Socialmediasubcomment> Socialmediasubcomments { get; set; }
    }
}