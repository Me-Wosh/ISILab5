using BlogApp.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Persistence;

public class BlogAppDbContext(DbContextOptions<BlogAppDbContext> options) : DbContext(options)
{
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Author> Authors => Set<Author>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimeStamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>()
            .HasData(new Author { Id = 1, Name = "Admin" });
    }

    private void UpdateTimeStamps()
    {
        var modifiedEntries = ChangeTracker.Entries<Post>().Where(e => e.State is EntityState.Added);

        foreach (var modifiedEntry in modifiedEntries)
        {
            modifiedEntry.Entity.CreatedAt = DateTime.Now;
        }
    }
}
