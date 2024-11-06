using GraphQL.API.Models;

namespace GraphQL.API.Types;

public class SubscriptionsType
{
    [Subscribe]
    [Topic("OnBookAdded")]
    public Book OnBookAdded([EventMessage] Book book) => book;
}
