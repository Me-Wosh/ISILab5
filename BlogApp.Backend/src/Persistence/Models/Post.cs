using System.ComponentModel.DataAnnotations;

namespace BlogApp.Persistence.Models;

public class Post
{
    public int Id { get; set; }
    [MaxLength(200)]
    public required string Title { get; set; }
    public required string Content { get; set; }
    public Author Author { get; set; } = null!;
    public int AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime PublishedAt { get; set; } = DateTime.Now;
}
