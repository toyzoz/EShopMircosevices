using Marten.Schema;

namespace Basket.API.Data;

public class BasketInitialData : IInitialData
{
    public Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }
}