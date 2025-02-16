namespace Basket.API.Data;

public class BasketNotFoundException(string username) : NotFoundException("ShoppingCart", username);