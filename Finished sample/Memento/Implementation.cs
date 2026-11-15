// Memento: capture an object's state in a separate object (the memento), so the
// state can be restored later without exposing the object's internals.
// This sample builds on the Command sample: instead of keeping every command
// around for undo, the manager keeps small mementos of the command's state.
namespace Memento;

/// <summary>Command</summary>
public interface ICommand
{
    void Execute();
    bool CanExecute();
    void Undo();
}

/// <summary>Memento: a snapshot of the command's state</summary>
public record AddEmployeeToManagerListMemento(int ManagerId, Employee? Employee);

/// <summary>Originator (and ConcreteCommand from the Command pattern)</summary>
public class AddEmployeeToManagerList(
    IEmployeeManagerRepository employeeManagerRepository,
    int managerId,
    Employee? employee) : ICommand
{
    // the state that is saved to, and restored from, a memento
    private int _managerId = managerId;
    private Employee? _employee = employee;

    public AddEmployeeToManagerListMemento CreateMemento() => new(_managerId, _employee);

    public void RestoreMemento(AddEmployeeToManagerListMemento memento)
    {
        _managerId = memento.ManagerId;
        _employee = memento.Employee;
    }

    // Potentially check against business/technical rules that might block execution.
    // Here: the employee shouldn't be null, and shouldn't be on the manager's list already.
    // Other potential rule(s): ensure that an employee can only be added to
    // one manager's list at the same time, etc.
    public bool CanExecute() =>
        _employee is not null
        && !employeeManagerRepository.HasEmployee(_managerId, _employee.Id);

    public void Execute()
    {
        if (_employee is null)
        {
            return;
        }

        employeeManagerRepository.AddEmployee(_managerId, _employee);
    }

    public void Undo()
    {
        if (_employee is null)
        {
            return;
        }

        employeeManagerRepository.RemoveEmployee(_managerId, _employee);
    }
}

/// <summary>
/// Caretaker (and Invoker from the Command pattern): keeps the mementos,
/// but never looks inside them.
/// </summary>
public class CommandManager
{
    // a stack gives back the most recently added memento first
    private readonly Stack<AddEmployeeToManagerListMemento> _mementos = new();
    private AddEmployeeToManagerList? _command;

    public void Invoke(AddEmployeeToManagerList command)
    {
        // if no command has been stored yet, store this one - we will
        // reuse it for every undo instead of storing different instances
        _command ??= command;

        if (command.CanExecute())
        {
            command.Execute();
            _mementos.Push(command.CreateMemento());
        }
    }

    public void Undo()
    {
        if (_mementos.TryPop(out var memento))
        {
            // put the stored command back in the remembered state, then undo it
            _command?.RestoreMemento(memento);
            _command?.Undo();
        }
    }

    public void UndoAll()
    {
        while (_mementos.TryPop(out var memento))
        {
            _command?.RestoreMemento(memento);
            _command?.Undo();
        }
    }
}

public class Employee(int id, string name)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
}

public class Manager(int id, string name) : Employee(id, name)
{
    public List<Employee> Employees { get; } = [];
}

/// <summary>Receiver (interface)</summary>
public interface IEmployeeManagerRepository
{
    void AddEmployee(int managerId, Employee employee);
    void RemoveEmployee(int managerId, Employee employee);
    bool HasEmployee(int managerId, int employeeId);
    void WriteDataStore();
}

/// <summary>
/// Receiver (implementation): does the actual work when a command is executed.
/// </summary>
public class EmployeeManagerRepository : IEmployeeManagerRepository
{
    // for demo purposes, use an in-memory datastore as a fake "manager list"
    private readonly List<Manager> _managers = [new(1, "Katie"), new(2, "Geoff")];

    // in real-life, add additional input & error checks to the methods below

    public void AddEmployee(int managerId, Employee employee) =>
        FindManager(managerId).Employees.Add(employee);

    public void RemoveEmployee(int managerId, Employee employee) =>
        FindManager(managerId).Employees.Remove(employee);

    public bool HasEmployee(int managerId, int employeeId) =>
        FindManager(managerId).Employees.Any(e => e.Id == employeeId);

    /// <summary>
    /// For demo purposes, write out the data store to the console window
    /// </summary>
    public void WriteDataStore()
    {
        foreach (var manager in _managers)
        {
            Console.WriteLine($"Manager {manager.Id}, {manager.Name}");

            if (manager.Employees.Count == 0)
            {
                Console.WriteLine("\t No employees.");
            }

            foreach (var employee in manager.Employees)
            {
                Console.WriteLine($"\t Employee {employee.Id}, {employee.Name}");
            }
        }
        Console.WriteLine();
    }

    private Manager FindManager(int managerId) => _managers.First(m => m.Id == managerId);
}
