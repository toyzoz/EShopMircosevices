namespace Catalog.API.Products.CreateProduct;

public class CreateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Category = request.Category,
            ImageFile = request.ImageFile,
            Price = request.Price
        };

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        return new CreateProductResult(product.Id);
    }
}

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(p => p.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(p => p.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    string ImageFile,
    List<string> Category) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);