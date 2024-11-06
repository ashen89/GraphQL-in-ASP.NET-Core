using GraphQL.API.Models;
using GraphQL.API.Quaries;
using GraphQL.API.Types;
using HotChocolate.Subscriptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddSubscriptionType<SubscriptionsType>()
    .AddInMemorySubscriptions();

builder.Services.AddSingleton<DBContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => Results.Ok("Hello from graphql api")).WithOpenApi();

app.MapPost("/books/add", async ([Service] DBContext DBContext, [Service] ITopicEventSender eventSender) =>
{
    var newBook = new Book()
    {
        Title = "nodejs in depth.",
        Author = new Author
        {
            Name = "Ashen"
        }
    };

    DBContext.Books.Add(newBook);

    await eventSender.SendAsync("OnBookAdded", newBook);

    return newBook;
}).WithOpenApi();

app.UseWebSockets();

app.MapGraphQL();

app.Run();

public class DBContext
{
    public DBContext()
    {
        Books = [new()
        {
            Title = "C# in depth.",
            Author = new Author
            {
                Name = "Jon Skeet"
            }
        }];
    }

    public List<Book> Books { get; set; }
}
