using Application.Abstract;
using Application.Commands;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly IElasticsearchService _elasticsearchService;

    public PostController(
        IPostRepository postRepository,
        IElasticsearchService elasticsearchService
    )
    {
        _postRepository = postRepository;
        _elasticsearchService = elasticsearchService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var posts = await _postRepository.GetAllAsync();
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
            return NotFound();
        return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostCommand command)
    {
        var post = new Post
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            Content = command.Content,
            CreatedAt = DateTime.UtcNow,
        };

        await _postRepository.AddAsync(post);
        await _elasticsearchService.IndexDocumentAsync(post);

        return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePostCommand command)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
            return NotFound();

        post.Title = command.Title;
        post.Content = command.Content;

        await _postRepository.UpdateAsync(post);
        await _elasticsearchService.UpdateDocumentAsync(post);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
            return NotFound();

        await _postRepository.DeleteAsync(id);
        await _elasticsearchService.DeleteDocumentAsync(id);

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        var results = await _elasticsearchService.SearchAsync(query);
        return Ok(results);
    }
}
