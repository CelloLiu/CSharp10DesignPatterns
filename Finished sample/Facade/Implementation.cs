// Facade: offer one simple entry point that hides a group of more complicated
// classes (the subsystem) from the caller.
namespace Facade;

/// <summary>Facade</summary>
public class DiscountFacade
{
    private readonly OrderService _orderService = new();
    private readonly CustomerDiscountBaseService _customerDiscountBaseService = new();
    private readonly DayOfTheWeekFactorService _dayOfTheWeekFactorService = new();

    public double CalculateDiscountPercentage(int customerId)
    {
        if (!_orderService.HasEnoughOrders(customerId))
        {
            return 0;
        }

        return _customerDiscountBaseService.CalculateDiscountBase(customerId)
            * _dayOfTheWeekFactorService.CalculateDayOfTheWeekFactor();
    }
}

// The subsystem classes below all use fake calculations for demo purposes.

/// <summary>Subsystem class</summary>
public class OrderService
{
    // does the customer have enough orders?
    public bool HasEnoughOrders(int customerId) => customerId > 5;
}

/// <summary>Subsystem class</summary>
public class CustomerDiscountBaseService
{
    public double CalculateDiscountBase(int customerId) => customerId > 8 ? 10 : 20;
}

/// <summary>Subsystem class</summary>
public class DayOfTheWeekFactorService
{
    // a different factor in the weekend than on weekdays
    public double CalculateDayOfTheWeekFactor() =>
        (DateTime.UtcNow.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) ? 0.8 : 1.2;
}
