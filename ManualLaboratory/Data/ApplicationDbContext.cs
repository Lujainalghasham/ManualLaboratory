using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ManualLaboratory.Models;

namespace ManualLaboratory.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ManualLaboratory.Models.Request>? Request { get; set; }
        public DbSet<ManualLaboratory.Models.Manage>? Manage { get; set; }
        public DbSet<ManualLaboratory.Models.College>? College { get; set; }
    }
}