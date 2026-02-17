using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Web.Books;

namespace TaskManagementSystem.Web.Controllers;

[ApiController]
[Route("api/books")]
public sealed class BooksController : ControllerBase
{
    private readonly IBookStore _bookStore;
    private readonly ILogger<BooksController> _logger;

    public BooksController(IBookStore bookStore, ILogger<BooksController> logger)
    {
        _bookStore = bookStore;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<BookItem>), StatusCodes.Status200OK)]
    public IActionResult GetAll([FromQuery] int? userId)
    {
        var items = _bookStore.GetAll(userId);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BookItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var item = _bookStore.GetById(id);
        if (item is null)
        {
            return NotFound(new { message = $"Book with id '{id}' was not found." });
        }

        return Ok(item);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BookItem), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CreateBookRequest request)
    {
        var created = _bookStore.Create(request);
        _logger.LogInformation("Book {BookId} created for user {UserId}.", created.Id, created.UserId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BookItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Update(int id, [FromBody] UpdateBookRequest request)
    {
        var updated = _bookStore.Update(id, request);
        if (updated is null)
        {
            return NotFound(new { message = $"Book with id '{id}' was not found." });
        }

        _logger.LogInformation("Book {BookId} updated.", updated.Id);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var deleted = _bookStore.Delete(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Book with id '{id}' was not found." });
        }

        _logger.LogInformation("Book {BookId} deleted.", id);
        return NoContent();
    }
}
