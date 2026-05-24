using BlogApp.Persistence;
using BlogApp.Persistence.Models;
using BlogApp.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlogAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlogAppDb"));
});

builder.Services.AddScoped<PostService>();

var app = builder.Build();

var group = app.MapGroup("/api")
    .DisableAntiforgery();

group.MapPost("/posts", async Task<Created<int>> (
    [FromForm] CreatePostRequest request,
    PostService postService,
    CancellationToken cancellationToken) =>
{
    var id = await postService.CreatePostAsync(request, cancellationToken);
    return TypedResults.Created($"/api/posts/{id}", id);
});

group.MapGet("/posts", async Task<Ok<IEnumerable<Post>>> (PostService postService, CancellationToken cancellationToken) =>
{
    var posts = await postService.GetAllPostsAsync(cancellationToken);
    return TypedResults.Ok(posts);
});

group.MapGet("/posts/{id:int}", async Task<Results<Ok<Post>, NotFound>> (
    [FromRoute] int id,
    PostService postService,
    CancellationToken cancellationToken) =>
{
    var post = await postService.GetPostByIdAsync(id, cancellationToken);
    return post is not null ? TypedResults.Ok(post) : TypedResults.NotFound();
});

app.Run();
