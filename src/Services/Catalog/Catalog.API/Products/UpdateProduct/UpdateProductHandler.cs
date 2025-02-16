namespace Catalog.API.Products.UpdateProduct;

public class UpdateProductHandler(IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(request.Id, cancellationToken);
        if (product == null) throw new ProductNotFoundException(request.Id);

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

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage("Id is required");

        RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required")
            .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");

        RuleFor(p => p.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(p => p.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
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