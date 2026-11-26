// State: an object changes its behavior when its internal state changes.
// Each state is its own class that handles actions and decides the next state.
namespace State;

// State types and TransitionTo are internal: hidden from other assemblies,
// and signaling in this single-project demo that callers should only use Order.
/// <summary>State</summary>
internal interface IOrderState
{
    string Name { get; }
    void Pay(Order order);
    void Ship(Order order);
}

/// <summary>
/// Context: delegates every action to its current state object.
/// </summary>
public class Order
{
    private IOrderState _state = new PendingState();

    public string StateName => _state.Name;

    // The behavior changes because the state object changes, not because of an if/switch
    public void Pay() => _state.Pay(this);

    public void Ship() => _state.Ship(this);

    // Intended for the state classes only (marked internal for this single-project demo)
    internal void TransitionTo(IOrderState nextState)
    {
        Console.WriteLine($"Order state: {_state.Name} -> {nextState.Name}");
        _state = nextState;
    }
}

/// <summary>ConcreteState</summary>
internal class PendingState : IOrderState
{
    public string Name => "Pending";

    public void Pay(Order order)
    {
        Console.WriteLine("Payment received.");
        order.TransitionTo(new PaidState());
    }

    public void Ship(Order order) =>
        Console.WriteLine("Cannot ship: order has not been paid yet.");
}

/// <summary>ConcreteState</summary>
internal class PaidState : IOrderState
{
    public string Name => "Paid";

    public void Pay(Order order) =>
        Console.WriteLine("Order is already paid.");

    public void Ship(Order order)
    {
        Console.WriteLine("Order shipped.");
        order.TransitionTo(new ShippedState());
    }
}

/// <summary>ConcreteState</summary>
internal class ShippedState : IOrderState
{
    public string Name => "Shipped";

    public void Pay(Order order) =>
        Console.WriteLine("Order is already paid.");

    public void Ship(Order order) =>
        Console.WriteLine("Order is already shipped.");
}
