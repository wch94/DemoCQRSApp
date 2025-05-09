using DemoCQRSApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoCQRSApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
}