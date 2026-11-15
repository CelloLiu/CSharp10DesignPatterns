// Adapter (object adapter): let two incompatible interfaces work together.
// This variation HOLDS an instance of the class it adapts.
namespace Adapter.ObjectAdapter;

// a city the way the external system describes it
public record CityFromExternalSystem(string Name, string NickName, int Inhabitants);

// a city the way our own application wants it
public record City(string FullName, long Inhabitants);

/// <summary>Adaptee: the existing class with the "wrong" interface</summary>
public class ExternalSystem
{
    public CityFromExternalSystem GetCity() => new("Antwerp", "'t Stad", 500000);
}

/// <summary>Target: the interface our application expects</summary>
public interface ICityAdapter
{
    City GetCity();
}

/// <summary>Adapter</summary>
public class CityAdapter : ICityAdapter
{
    private readonly ExternalSystem _externalSystem = new();

    public City GetCity()
    {
        // call into the external system
        var cityFromExternalSystem = _externalSystem.GetCity();

        // adapt the CityFromExternalSystem to a City
        return new City(
            $"{cityFromExternalSystem.Name} - {cityFromExternalSystem.NickName}",
            cityFromExternalSystem.Inhabitants);
    }
}
