using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket;

public class StoreBasketCommandHandler(
    IBasketRepository repository,
    DiscountProtoService.DiscountProtoServiceClient discountServiceClient)
    : IRequestHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
    {
        await DeductDiscount(request.Cart, cancellationToken);

        await repository.StoreBasketAsync(request.Cart, cancellationToken);

        return new StoreBasketResult(request.Cart.UserName);
    }

    private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        foreach (var item in cart.Items)
        {
            var coupon = await discountServiceClient.GetDiscountAsync(
                new GetDiscountRequest() { ProductName = item.ProductName },
                cancellationToken: cancellationToken
            );
            item.Price -= coupon.Amount;
        }
    }
}

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string Username);

public class StoreBasketValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
    }
}