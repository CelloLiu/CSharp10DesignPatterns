// Iterator: walk through the items of a collection one by one, without the
// caller needing to know how the collection stores or orders them.
namespace Iterator;

public class Person(string name, string country)
{
    public string Name { get; set; } = name;
    public string Country { get; set; } = country;
}

/// <summary>Iterator</summary>
public interface IPeopleIterator
{
    Person First();
    // returns null when there is no next person
    Person? Next();
    bool IsDone { get; }
    Person CurrentItem { get; }
}

/// <summary>ConcreteIterator: goes through the people in alphabetical order</summary>
public class PeopleIterator(PeopleCollection collection) : IPeopleIterator
{
    // the position of the current person in the sorted collection
    private int _current = 0;

    public Person First()
    {
        _current = 0;
        return CurrentItem;
    }

    public Person? Next()
    {
        _current++;
        return IsDone ? null : CurrentItem;
    }

    public bool IsDone => _current >= collection.Count;

    public Person CurrentItem => collection.OrderBy(p => p.Name).ElementAt(_current);
}

/// <summary>Aggregate</summary>
public interface IPeopleCollection
{
    IPeopleIterator CreateIterator();
}

/// <summary>ConcreteAggregate</summary>
public class PeopleCollection : List<Person>, IPeopleCollection
{
    public IPeopleIterator CreateIterator() => new PeopleIterator(this);
}
