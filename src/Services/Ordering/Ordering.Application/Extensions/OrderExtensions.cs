using Ordering.Application.Dtos;
using Ordering.Domain.Models;

namespace Ordering.Application.Extensions;

public static class OrderExtensions
{
    /// <summary>
    ///     entity to dto
    /// </summary>
    /// <param name="orders"></param>
    /// <returns></returns>
    public static IEnumerable<OrderDto> ToOrderDtoList(this IEnumerable<Order> orders)
    {
        return orders.Select(ToOrderDto);
    }

    /// <summary>
    ///     entity to dto
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public static OrderDto ToOrderDto(this Order order)
    {
        return DtoFromOrder(order);
    }

    /// <summary>
    ///     entity to dto
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    private static OrderDto DtoFromOrder(Order order)
    {
        return new OrderDto(order.Id.Value,
            order.CustomerId.Value,
            order.OrderName.Value,
            new AddressDto(
                order.ShippingAddress.FirstName,
                order.ShippingAddress.LastName,
                order.ShippingAddress.EmailAddress,
                order.ShippingAddress.AddressLine,
                order.ShippingAddress.Country,
                order.ShippingAddress.State,
                order.ShippingAddress.ZipCode),
            new AddressDto(
                order.BillingAddress.FirstName,
                order.BillingAddress.LastName,
                order.BillingAddress.EmailAddress,
                order.BillingAddress.AddressLine,
                order.BillingAddress.Country,
                order.BillingAddress.State,
                order.BillingAddress.ZipCode
            ),
            new PaymentDto(order.Payment.CardName,
                order.Payment.CardNumber,
                order.Payment.Expiration,
                order.Payment.CVV,
                order.Payment.PaymentMethod),
            order.Status,
            order.OrderItems.Select(x => new OrderItemDto(
                x.OrderId.Value,
                x.ProductId.Value,
                x.Quantity,
                x.Price
            )).ToList()
        );
    }
}