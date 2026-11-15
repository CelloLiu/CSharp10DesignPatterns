using FactoryMethod;

List<DiscountFactory> factories =
[
    new CodeDiscountFactory(Guid.NewGuid()),
    new CountryDiscountFactory("BE")
];

// this loop never needs to know which concrete service it gets
foreach (var factory in factories)
{
    var discountService = factory.CreateDiscountService();
    Console.WriteLine($"Percentage {discountService.DiscountPercentage} coming from {discountService}");
}

Console.ReadKey();
