using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Exceptions;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands;

public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;

public record UpdateOrderResult(bool IsSuccess);

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Order.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("CustomerId is required");
    }
}

public class UpdateOrderCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders.FindAsync([OrderId.Of(request.Order.Id)], cancellationToken);

        if (order == null) throw new OrderNotFoundException(request.Order.Id);

        UpdateOrderWithNewValues(order, request.Order);


        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateOrderResult(true);
    }

    private static void UpdateOrderWithNewValues(Order order, OrderDto orderDto)
    {
        var updatedShippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName,
            orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine,
            orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
        var updatedBillingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName,
            orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country,
            orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);
        var updatedPayment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber,
            orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);

        order.Update(
            OrderName.Of(orderDto.OrderName),
            updatedShippingAddress,
            updatedBillingAddress,
            updatedPayment,
            orderDto.Status);
    }
}