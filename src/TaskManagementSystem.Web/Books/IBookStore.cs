namespace TaskManagementSystem.Web.Books;

public interface IBookStore
{
    IReadOnlyCollection<BookItem> GetAll(int? userId);

    BookItem? GetById(int id);

    BookItem Create(CreateBookRequest request);

    BookItem? Update(int id, UpdateBookRequest request);

    bool Delete(int id);
}
