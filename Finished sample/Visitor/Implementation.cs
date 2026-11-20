// Visitor: add a new operation (here: calculating discounts) to a set of classes
// without changing those classes. The operation lives in a separate visitor
// object that the classes "accept".
namespace Visitor;

/// <summary>Visitor</summary>
public interface IVisitor
{
    void Visit(IElement element);
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
    public decimal Discount { get; set; }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
        Console.WriteLine($"Visited {nameof(Customer)} {Name}, discount given: {Discount}");
    }
}

/// <summary>ConcreteElement</summary>
public class Employee(string name, int yearsEmployed) : IElement
{
    public string Name { get; } = name;
    public int YearsEmployed { get; } = yearsEmployed;
    public decimal Discount { get; set; }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
        Console.WriteLine($"Visited {nameof(Employee)} {Name}, discount given: {Discount}");
    }
}

/// <summary>ConcreteVisitor</summary>
public class DiscountVisitor : IVisitor
{
    public decimal TotalDiscountGiven { get; private set; }

    // pick the right method for the kind of element being visited
    public void Visit(IElement element)
    {
        switch (element)
        {
            case Customer customer:
                VisitCustomer(customer);
                break;
            case Employee employee:
                VisitEmployee(employee);
                break;
        }
    }

    private void VisitCustomer(Customer customer)
    {
        // percentage of total amount
        var discount = customer.AmountOrdered / 10;
        // set it on the customer
        customer.Discount = discount;
        // add it to the total amount
        TotalDiscountGiven += discount;
    }

    private void VisitEmployee(Employee employee)
    {
        // fixed value depending on the amount of years employed
        var discount = employee.YearsEmployed < 10 ? 100 : 200;
        // set it on the employee
        employee.Discount = discount;
        // add it to the total amount
        TotalDiscountGiven += discount;
    }
}

/// <summary>ObjectStructure: the collection of elements a visitor is sent through</summary>
public class Container
{
    public List<Employee> Employees { get; } = [];
    public List<Customer> Customers { get; } = [];

    public void Accept(IVisitor visitor)
    {
        foreach (var employee in Employees)
        {
            employee.Accept(visitor);
        }
        foreach (var customer in Customers)
        {
            customer.Accept(visitor);
        }
    }
}
