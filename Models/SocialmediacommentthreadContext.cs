using Microsoft.EntityFrameworkCore;

namespace FBIntergrationApi.Models
{
    public class SocialmediacommentthreadContext : DbContext
    {
        public SocialmediacommentthreadContext(DbContextOptions<SocialmediacommentthreadContext> options)
            : base(options)
        {
        }

        public DbSet<Socialmediacommentthread> Socialmediacommentthreads { get; set; }
    }
}