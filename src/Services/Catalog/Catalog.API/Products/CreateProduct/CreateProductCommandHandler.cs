using Catalog.API.Models;
using MediatR;

namespace Catalog.API.Products.CreateProduct;

public class CreateProductCommandHandler:IRequestHandler<CreateProductCommand,CreateProductResult>
{
    public Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public record CreateProductCommand(
    string Name,
    string? Description,
    decimal? Price,
    string? ImageUrl) : IRequest<CreateProductResult>;

public record CreateProductResult(Guid Id);