namespace Catalog.API.Products.UpdateProduct;

public class UpdateProductHandler(IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(request.Id, cancellationToken);
        if (product == null) throw new ProductNotFoundException();

        product.Name = request.Name;
        product.Description = request.Description;
        product.ImageFile = request.ImageFile;
        product.Category = request.Category;
        product.Price = request.Price;
        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }
}

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    string ImageFile,
    decimal Price,
    List<string> Category)
    : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);