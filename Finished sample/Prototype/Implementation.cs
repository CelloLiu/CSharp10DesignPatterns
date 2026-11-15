// Prototype: create new objects by copying an existing one (the prototype)
// instead of building them from scratch.
using Newtonsoft.Json;

namespace Prototype;

/// <summary>Prototype</summary>
public abstract class Person(string name)
{
    public string Name { get; set; } = name;

    /// <summary>
    /// A shallow clone copies the object but shares the objects it refers to.
    /// A deep clone also copies everything the object refers to.
    /// </summary>
    public abstract Person Clone(bool deepClone);

    // Deep clone by writing the whole object to JSON and reading it back,
    // which produces brand-new copies of everything it contains.
    protected Person DeepClone()
    {
        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        var objectAsJson = JsonConvert.SerializeObject(this, GetType(), settings);

        return JsonConvert.DeserializeObject<Person>(objectAsJson, settings)
            ?? throw new InvalidOperationException("The object could not be cloned.");
    }
}

/// <summary>ConcretePrototype</summary>
public class Employee(string name, Manager manager) : Person(name)
{
    public Manager Manager { get; set; } = manager;

    public override Person Clone(bool deepClone = false) =>
        deepClone ? DeepClone() : (Person)MemberwiseClone();
}

/// <summary>ConcretePrototype</summary>
public class Manager(string name) : Person(name)
{
    public override Person Clone(bool deepClone = false) =>
        deepClone ? DeepClone() : (Person)MemberwiseClone();
}
