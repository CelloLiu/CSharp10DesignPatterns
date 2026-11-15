// Factory Method: a creator class declares a method that returns a product,
// and each subclass decides which concrete product to create.
namespace FactoryMethod;

/// <summary>Product</summary>
public abstract class DiscountService
{
    public abstract int DiscountPercentage { get; }

    public override string ToString() => GetType().Name;
}

/// <summary>ConcreteProduct</summary>
public class CountryDiscountService(string countryIdentifier) : DiscountService
{
    // if you're from Belgium, you get a better discount :)
    public override int DiscountPercentage => countryIdentifier == "BE" ? 20 : 10;
}

/// <summary>ConcreteProduct</summary>
public class CodeDiscountService(Guid code) : DiscountService
{
    public Guid Code { get; } = code;

    // each code returns the same fixed percentage, but a code is only valid
    // once - a real implementation would check whether it has been used before
    public override int DiscountPercentage => 15;
}

/// <summary>Creator</summary>
public abstract class DiscountFactory
{
    // the factory method
    public abstract DiscountService CreateDiscountService();
}

/// <summary>ConcreteCreator</summary>
public class CountryDiscountFactory(string countryIdentifier) : DiscountFactory
{
    public override DiscountService CreateDiscountService() =>
        new CountryDiscountService(countryIdentifier);
}

/// <summary>ConcreteCreator</summary>
public class CodeDiscountFactory(Guid code) : DiscountFactory
{
    public override DiscountService CreateDiscountService() =>
        new CodeDiscountService(code);
}
