// Prototype: create new objects by copying an existing one (the prototype)
// instead of building them from scratch.
namespace Prototype;

/// <summary>Prototype</summary>
public abstract class Person(string name)
{
    public string Name { get; set; } = name;

    /// <summary>
    /// A shallow clone copies the object but shares the objects it refers to.
    /// A deep clone also copies everything the object refers to.
    /// </summary>
    public abstract Person Clone(bool deepClone = false);
}

/// <summary>ConcretePrototype</summary>
public class Employee(string name, Manager manager) : Person(name)
{
    public Manager Manager { get; set; } = manager;

    // MemberwiseClone preserves the runtime type and copies all fields shallowly.
    // For a deep clone, each mutable reference-type property must also be cloned by hand.
    public override Person Clone(bool deepClone = false)
    {
        var clone = (Employee)MemberwiseClone();
        if (deepClone)
        {
            clone.Manager = (Manager)Manager.Clone(deepClone: true);
        }

        return clone;
    }
}

/// <summary>ConcretePrototype</summary>
public class Manager(string name) : Person(name)
{
    // Manager only holds an immutable string (Name), so MemberwiseClone is already
    // a safe copy and preserves any derived type's fields. Subclasses that add
    // mutable reference-type properties should override Clone to deep-copy them.
    public override Person Clone(bool deepClone = false) =>
        (Person)MemberwiseClone();
}
