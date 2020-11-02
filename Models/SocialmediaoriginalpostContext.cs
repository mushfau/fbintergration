using Microsoft.EntityFrameworkCore;

namespace FBIntergrationApi.Models
{
    public class SocialmediaoriginalpostContext : DbContext
    {
        public SocialmediaoriginalpostContext(DbContextOptions<SocialmediaoriginalpostContext> options)
            : base(options)
        {
        }

        public DbSet<Socialmediaoriginalpost> Socialmediaoriginalposts { get; set; }
    }
}