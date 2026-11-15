// Builder: separate HOW a complex object is put together (the director's steps)
// from WHAT each step produces (the concrete builders).
namespace BuilderPattern;

/// <summary>
/// Director: knows the order of the build steps, not what they produce.
/// </summary>
public class Garage
{
    private CarBuilder? _builder;

    public void Construct(CarBuilder builder)
    {
        _builder = builder;

        _builder.BuildEngine();
        _builder.BuildFrame();
    }

    // variation: the show method on the director instead of on the product.
    public void Show()
    {
        Console.WriteLine(_builder?.Car.ToString());
    }
}

/// <summary>Builder</summary>
public abstract class CarBuilder(string carType)
{
    public Car Car { get; } = new(carType);

    public abstract void BuildEngine();
    public abstract void BuildFrame();
}

/// <summary>ConcreteBuilder</summary>
public class MiniBuilder : CarBuilder
{
    public MiniBuilder() : base("Mini")
    {
    }

    public override void BuildEngine() => Car.AddPart("'not a V8'");
    public override void BuildFrame() => Car.AddPart("'3-door with stripes'");
}

/// <summary>ConcreteBuilder</summary>
public class BMWBuilder : CarBuilder
{
    public BMWBuilder() : base("BMW")
    {
    }

    public override void BuildEngine() => Car.AddPart("'a fancy V8 engine'");
    public override void BuildFrame() => Car.AddPart("'5-door with metallic finish'");
}

/// <summary>Product</summary>
public class Car(string carType)
{
    private readonly List<string> _parts = [];

    public void AddPart(string part) => _parts.Add(part);

    // one sentence per part, all on a single line
    public override string ToString() =>
        string.Concat(_parts.Select(part => $"Car of type {carType} has part {part}. "));
}
