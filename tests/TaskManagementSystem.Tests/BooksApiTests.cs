using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TaskManagementSystem.Tests;

public sealed class BooksApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BooksApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CrudFlow_ShouldCreateReadUpdateListAndDeleteBook()
    {
        using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });

        var randomTitle = $"Book {Guid.NewGuid():N}".Substring(0, 15);
        var createRequest = new
        {
            title = randomTitle,
            author = "Postman Student",
            userId = 1001,
        };

        var createResponse = await client.PostAsJsonAsync("/api/books", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createResponse.Content.ReadFromJsonAsync<BookApiModel>(JsonOptions);
        created.Should().NotBeNull();
        created!.Id.Should().BeGreaterThan(0);
        created.Title.Should().Be(randomTitle);
        created.UserId.Should().Be(1001);

        var getByIdResponse = await client.GetAsync($"/api/books/{created.Id}");
        getByIdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var loaded = await getByIdResponse.Content.ReadFromJsonAsync<BookApiModel>(JsonOptions);
        loaded.Should().NotBeNull();
        loaded!.Id.Should().Be(created.Id);

        var updatedTitle = $"Updated {randomTitle}";
        var updateRequest = new
        {
            title = updatedTitle,
            author = "Postman Student",
            userId = 1001,
        };

        var updateResponse = await client.PutAsJsonAsync($"/api/books/{created.Id}", updateRequest);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await updateResponse.Content.ReadFromJsonAsync<BookApiModel>(JsonOptions);
        updated.Should().NotBeNull();
        updated!.Title.Should().Be(updatedTitle);

        var listResponse = await client.GetAsync("/api/books?userId=1001");
        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await listResponse.Content.ReadFromJsonAsync<List<BookApiModel>>(JsonOptions);
        list.Should().NotBeNull();
        list!.Any(x => x.Id == created.Id).Should().BeTrue();

        var deleteResponse = await client.DeleteAsync($"/api/books/{created.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getAfterDelete = await client.GetAsync($"/api/books/{created.Id}");
        getAfterDelete.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private sealed class BookApiModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public int UserId { get; set; }
    }
}
