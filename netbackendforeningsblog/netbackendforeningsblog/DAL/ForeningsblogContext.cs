using Microsoft.EntityFrameworkCore;
using netbackendforeningsblog.Models;

namespace netbackendforeningsblog.DAL
{
    public class ForeningsblogContext : DbContext
    {
        public ForeningsblogContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<SignedupUsers> SignedupUsers { get; set; }

    }
}
