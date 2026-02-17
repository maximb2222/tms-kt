# Postman Assignment

Файлы задания:

- `postman/Book-CRUD.postman_collection.json`
- `postman/Book-CRUD-Local.postman_environment.json`

## Что реализовано

- 5 CRUD-запросов в коллекции:
  1. `POST /api/books`
  2. `GET /api/books/{{bookId}}`
  3. `PUT /api/books/{{bookId}}`
  4. `GET /api/books?userId={{userId}}`
  5. `DELETE /api/books/{{bookId}}`
- `baseUrl` задан на уровне коллекции.
- В окружении создана переменная `userId` и используется в запросах.
- В `POST` есть Pre-request Script для генерации случайного названия книги.
- В каждом запросе есть проверка времени ответа (`responseTime`) и дополнительные проверки.
- `bookId` и `bookTitle` сохраняются и переиспользуются.

## Как запустить

1. Запустить API:

```bash
dotnet run --project src/TaskManagementSystem.Web --urls http://127.0.0.1:5180
```

2. В Postman импортировать:
- коллекцию `postman/Book-CRUD.postman_collection.json`
- окружение `postman/Book-CRUD-Local.postman_environment.json`

3. Выбрать окружение `Book CRUD Local` и запустить Runner на всю коллекцию.
