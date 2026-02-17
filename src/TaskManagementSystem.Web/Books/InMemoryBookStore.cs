using System.Collections.Concurrent;
using System.Threading;

namespace TaskManagementSystem.Web.Books;

public sealed class InMemoryBookStore : IBookStore
{
    private readonly ConcurrentDictionary<int, BookItem> _books = new();
    private int _nextId = 0;

    public InMemoryBookStore()
    {
        var seed = new[]
        {
            new CreateBookRequest
            {
                Title = "Seed Book: API Testing",
                Author = "QA Team",
                UserId = 1001,
            },
            new CreateBookRequest
            {
                Title = "Seed Book: Integration Basics",
                Author = "QA Team",
                UserId = 1002,
            },
        };

        foreach (var item in seed)
        {
            Create(item);
        }
    }

    public IReadOnlyCollection<BookItem> GetAll(int? userId)
    {
        IEnumerable<BookItem> query = _books.Values.OrderBy(x => x.Id);
        if (userId is not null)
        {
            query = query.Where(x => x.UserId == userId.Value);
        }

        return query.ToList();
    }

    public BookItem? GetById(int id)
    {
        _books.TryGetValue(id, out var item);
        return item;
    }

    public BookItem Create(CreateBookRequest request)
    {
        var now = DateTime.UtcNow;
        var id = Interlocked.Increment(ref _nextId);
        var item = new BookItem
        {
            Id = id,
            Title = request.Title.Trim(),
            Author = request.Author.Trim(),
            UserId = request.UserId,
            CreatedAt = now,
            UpdatedAt = now,
        };

        _books[id] = item;
        return item;
    }

    public BookItem? Update(int id, UpdateBookRequest request)
    {
        if (!_books.TryGetValue(id, out var existing))
        {
            return null;
        }

        existing.Title = request.Title.Trim();
        existing.Author = request.Author.Trim();
        existing.UserId = request.UserId;
        existing.UpdatedAt = DateTime.UtcNow;
        return existing;
    }

    public bool Delete(int id)
    {
        return _books.TryRemove(id, out _);
    }
}
