// Visitor: add new operations to a set of element classes without modifying them.
// Each element "accepts" a visitor and calls the Visit overload matching its own
// concrete type (double dispatch).
// Trade-off: adding a new visitor (operation) is easy, but adding a new element
// type means updating IVisitor and every concrete visitor.
namespace Visitor;

/// <summary>Visitor: declares a Visit method for each concrete element type</summary>
public interface IVisitor
{
    void Visit(Customer customer);
    void Visit(Employee employee);
}

/// <summary>Element</summary>
public interface IElement
{
    void Accept(IVisitor visitor);
}

/// <summary>ConcreteElement</summary>
public class Customer(string name, decimal amountOrdered) : IElement
{
    public string Name { get; } = name;
    public decimal AmountOrdered { get; } = amountOrdered;

    // Double dispatch: inside Customer, 'this' has its real type, so the right Visit overload is chosen
    public void Accept(IVisitor visitor) => visitor.Visit(this);
}

/// <summary>ConcreteElement</summary>
public class Employee(string name, int yearsEmployed) : IElement
{
    public string Name { get; } = name;
    public int YearsEmployed { get; } = yearsEmployed;

    // Double dispatch: inside Employee, 'this' has its real type, so the right Visit overload is chosen
    public void Accept(IVisitor visitor) => visitor.Visit(this);
}

/// <summary>ConcreteVisitor: calculates discounts without changing Customer or Employee</summary>
public class DiscountVisitor : IVisitor
{
    // Visitors can hold state across Visit calls (here, a running total)
    public decimal TotalDiscount { get; private set; }

    public void Visit(Customer customer)
    {
        // Customer rule: 10% of the order amount
        var discount = customer.AmountOrdered / 10;
        TotalDiscount += discount;
        Console.WriteLine($"Customer {customer.Name}: discount = {discount}");
    }

    public void Visit(Employee employee)
    {
        // Employee rule: 100 for under 10 years employed, 200 for 10+ years
        decimal discount = employee.YearsEmployed < 10 ? 100m : 200m;
        TotalDiscount += discount;
        Console.WriteLine($"Employee {employee.Name}: discount = {discount}");
    }
}

/// <summary>ConcreteVisitor: a second operation added without touching the element classes</summary>
public class SummaryVisitor : IVisitor
{
    public void Visit(Customer customer) =>
        Console.WriteLine($"Customer: {customer.Name} (ordered {customer.AmountOrdered})");

    public void Visit(Employee employee) =>
        Console.WriteLine($"Employee: {employee.Name} ({employee.YearsEmployed} years)");
}
