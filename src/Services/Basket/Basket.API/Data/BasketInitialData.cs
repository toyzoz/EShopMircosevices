using Marten.Schema;

public class BasketInitialData : IInitialData
{
    public Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }
}