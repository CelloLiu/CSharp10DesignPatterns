// change this to Adapter.ObjectAdapter to try the other variation:
// the code below stays exactly the same
using Adapter.ClassAdapter;

ICityAdapter adapter = new CityAdapter();
var city = adapter.GetCity();

Console.WriteLine($"{city.FullName}, {city.Inhabitants}");
Console.ReadKey();
