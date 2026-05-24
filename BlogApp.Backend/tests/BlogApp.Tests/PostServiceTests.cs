using BlogApp.Persistence;
using BlogApp.Persistence.Models;
using BlogApp.Services;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Tests;

public class PostServiceTests
{
    private readonly BlogAppDbContext _dbContext;

    public PostServiceTests()
    {
        var options = new DbContextOptionsBuilder<BlogAppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new BlogAppDbContext(options);
        _dbContext.AddRange(PostSeed());
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task GetAllPostsAsync_ReturnsAll3Posts()
    {
        // Arrange
        var service = new PostService(_dbContext);

        // Act
        var posts = await service.GetAllPostsAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(posts);
        Assert.Equal(3, posts.Count());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetPostByIdAsync_ReturnsPostWithMatchingId(int id)
    {
        // Arrange
        var service = new PostService(_dbContext);

        // Act
        var post = await service.GetPostByIdAsync(id, CancellationToken.None);

        // Assert
        Assert.NotNull(post);
        Assert.Equal(id, post.Id);
    }

    private static List<Post> PostSeed()
    {
        return
        [
            new Post { Id = 1, Title = "First Post", Content = "This is the first post.", AuthorId = 1 },
            new Post { Id = 2, Title = "Second Post", Content = "This is the second post.", AuthorId = 1 },
            new Post { Id = 3, Title = "Third Post", Content = "This is the third post.", AuthorId = 1 }
        ];
    }
}
