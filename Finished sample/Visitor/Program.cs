using Visitor;

// ObjectStructure: mixed elements stored through the shared IElement interface
List<IElement> elements =
[
    new Customer("Sophie", 500),
    new Customer("Karen", 1000),
    new Employee("Kevin", 18),
    new Employee("Tom", 5)
];

// Operation 1: print a summary
// Because each item is typed as IElement, Accept(visitor) dispatches to the right Visit overload
Console.WriteLine("--- Summary ---");
var summaryVisitor = new SummaryVisitor();
foreach (var element in elements)
{
    element.Accept(summaryVisitor);
}

// Operation 2: calculate discounts (added without changing Customer or Employee)
Console.WriteLine();
Console.WriteLine("--- Discounts ---");
var discountVisitor = new DiscountVisitor();
foreach (var element in elements)
{
    element.Accept(discountVisitor);
}

Console.WriteLine($"Total discount: {discountVisitor.TotalDiscount}");
