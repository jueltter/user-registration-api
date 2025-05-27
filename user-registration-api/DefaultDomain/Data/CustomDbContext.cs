using Microsoft.EntityFrameworkCore;
using user_registration_api.DefaultDomain.Models;

namespace user_registration_api.DefaultDomain.Data;

public class CustomDbContext : DbContext
{
    public CustomDbContext(DbContextOptions<CustomDbContext> options): base(options)
    {
    
    }
    
    public DbSet<User> Users { get; set; }
    
}