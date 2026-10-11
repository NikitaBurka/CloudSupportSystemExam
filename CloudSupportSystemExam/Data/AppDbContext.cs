using CloudSupportSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudSupportSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SupportRequest> SupportRequests => Set<SupportRequest>();
    }
}