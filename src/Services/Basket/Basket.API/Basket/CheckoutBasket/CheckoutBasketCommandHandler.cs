namespace Basket.API.Basket.CheckoutBasket;

public class CheckoutBasketCommandHandler(IBasketRepository basketRepository)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand request,
        CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetBasketAsync(request.BasketCheckoutDto.UserName, cancellationToken);

        await basketRepository.DeleteBasketAsync(request.BasketCheckoutDto.UserName, cancellationToken);
        return new CheckoutBasketResult(true);
    }
}

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess);