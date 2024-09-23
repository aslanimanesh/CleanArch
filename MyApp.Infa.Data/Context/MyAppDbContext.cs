using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Models;

namespace MyApp.Infa.Data.Context
{
    public class MyAppDbContext : DbContext
    {

        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

    }
}
