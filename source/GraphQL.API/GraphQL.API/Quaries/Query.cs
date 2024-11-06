using GraphQL.API.Models;

namespace GraphQL.API.Quaries;

public class Query
{
    public Book GetBook([Service] DBContext dBContext) =>
        new()
        {
            Title = "C# in depth.",
            Author = new Author
            {
                Name = "Jon Skeet"
            }
        };

    public List<Book> GetBooks([Service] DBContext dBContext) =>
        dBContext.Books;
}
