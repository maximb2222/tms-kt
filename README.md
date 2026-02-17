# Task Management System

Практическая работа по .NET: автономное CRUD-приложение с БД, тестами и документацией.

## Что в репозитории

- `src/TaskManagementSystem.Web` - веб-приложение ASP.NET Core MVC
- `tests/TaskManagementSystem.Tests` - автотесты
- `docs/` - материалы КТ1-КТ4

## Функции приложения

- CRUD для сущности `TaskItem`
- Поиск и фильтрация по статусу
- Пагинация списка
- Логирование и хранение данных в SQLite

## Как запустить

```bash
dotnet restore
dotnet run --project src/TaskManagementSystem.Web
```

## Как проверить тесты

```bash
dotnet test
```

## Документы для сдачи

- `docs/KT1-Plan.pdf`
- `docs/KT3-FinalDocumentation.pdf`
- `docs/KT4-Presentation-Material.pptx`

## Postman (новое задание)

- Коллекция: `postman/Book-CRUD.postman_collection.json`
- Окружение: `postman/Book-CRUD-Local.postman_environment.json`
- Инструкция: `docs/Postman-Assignment.md`
