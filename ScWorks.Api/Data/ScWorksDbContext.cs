using Microsoft.EntityFrameworkCore;
using ScWorks.Api.Data.Models;

namespace ScWorks.Api.Data;

public class ScWorksDbContext(DbContextOptions<ScWorksDbContext> opt) : DbContext(opt)
{
    public DbSet<ScWork> ScWorks { get; set; }
}
