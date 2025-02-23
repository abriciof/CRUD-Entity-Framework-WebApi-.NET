using FafaAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FafaAPI.Infrastructure.EF;

public class PersonContext : DbContext
{
    public DbSet<PersonModel> People { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=person.sqlite");
        base.OnConfiguring(optionsBuilder);
        
    }
}