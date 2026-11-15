// Strategy: put each way of doing something (here: exporting an order) in its
// own class with a shared interface, so the caller can pick one at runtime.
namespace Strategy;

/// <summary>Strategy</summary>
public interface IExportService
{
    void Export(Order order);
}

/// <summary>ConcreteStrategy</summary>
public class JsonExportService : IExportService
{
    public void Export(Order order) => Console.WriteLine($"Exporting {order.Name} to Json.");
}

/// <summary>ConcreteStrategy</summary>
public class XMLExportService : IExportService
{
    public void Export(Order order) => Console.WriteLine($"Exporting {order.Name} to XML.");
}

/// <summary>ConcreteStrategy</summary>
public class CSVExportService : IExportService
{
    public void Export(Order order) => Console.WriteLine($"Exporting {order.Name} to CSV.");
}

/// <summary>Context</summary>
public class Order(string customer, int amount, string name)
{
    public string Customer { get; set; } = customer;
    public int Amount { get; set; } = amount;
    public string Name { get; set; } = name;
    public string? Description { get; set; }

    // the order doesn't know HOW it is exported: the strategy passed in decides
    public void Export(IExportService exportService)
    {
        ArgumentNullException.ThrowIfNull(exportService);

        exportService.Export(this);
    }
}
