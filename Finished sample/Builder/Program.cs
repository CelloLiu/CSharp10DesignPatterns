using BuilderPattern;

var garage = new Garage();

var miniBuilder = new MiniBuilder();
var bmwBuilder = new BMWBuilder();

// the garage runs the same steps; the builder decides what they produce
garage.Construct(miniBuilder);
Console.WriteLine(miniBuilder.Car.ToString());
// or:
garage.Show();

garage.Construct(bmwBuilder);
Console.WriteLine(bmwBuilder.Car.ToString());
// or:
garage.Show();
