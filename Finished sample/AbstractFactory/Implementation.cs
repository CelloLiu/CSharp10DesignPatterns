// Abstract Factory: create a family of related objects (here: the discount and
// shipping rules of one country) without the client naming their concrete classes.
namespace AbstractFactory;

/// <summary>AbstractFactory</summary>
public interface IShoppingCartPurchaseFactory
{
    IDiscountService CreateDiscountService();
    IShippingCostsService CreateShippingCostsService();
}

/// <summary>AbstractProduct</summary>
public interface IDiscountService
{
    int DiscountPercentage { get; }
}

/// <summary>ConcreteProduct</summary>
public class BelgiumDiscountService : IDiscountService
{
    public int DiscountPercentage => 20;
}

/// <summary>ConcreteProduct</summary>
public class FranceDiscountService : IDiscountService
{
    public int DiscountPercentage => 10;
}

/// <summary>AbstractProduct</summary>
public interface IShippingCostsService
{
    decimal ShippingCosts { get; }
}

/// <summary>ConcreteProduct</summary>
public class BelgiumShippingCostsService : IShippingCostsService
{
    public decimal ShippingCosts => 20;
}

/// <summary>ConcreteProduct</summary>
public class FranceShippingCostsService : IShippingCostsService
{
    public decimal ShippingCosts => 25;
}

/// <summary>ConcreteFactory: everything needed for a purchase in Belgium</summary>
public class BelgiumShoppingCartPurchaseFactory : IShoppingCartPurchaseFactory
{
    public IDiscountService CreateDiscountService() => new BelgiumDiscountService();
    public IShippingCostsService CreateShippingCostsService() => new BelgiumShippingCostsService();
}

/// <summary>ConcreteFactory: everything needed for a purchase in France</summary>
public class FranceShoppingCartPurchaseFactory : IShoppingCartPurchaseFactory
{
    public IDiscountService CreateDiscountService() => new FranceDiscountService();
    public IShippingCostsService CreateShippingCostsService() => new FranceShippingCostsService();
}

/// <summary>
/// Client: only knows the factory and product interfaces, never a concrete country.
/// </summary>
public class ShoppingCart(IShoppingCartPurchaseFactory factory)
{
    // assume that the total cost of all the items we ordered = 200 euro
    private const decimal OrderCosts = 200m;

    private readonly IDiscountService _discountService = factory.CreateDiscountService();
    private readonly IShippingCostsService _shippingCostsService = factory.CreateShippingCostsService();

    public void CalculateCosts()
    {
        var discount = OrderCosts * _discountService.DiscountPercentage / 100m;
        var totalCosts = OrderCosts - discount + _shippingCostsService.ShippingCosts;

        Console.WriteLine($"Total costs = {totalCosts}");
    }
}
