// Command: wrap a request ("add this employee to that manager's list") in an
// object, so it can be passed around, executed later, and undone.
namespace Command;

/// <summary>Command</summary>
public interface ICommand
{
    void Execute();
    bool CanExecute();
    void Undo();
}

/// <summary>ConcreteCommand</summary>
public class AddEmployeeToManagerList(
    IEmployeeManagerRepository employeeManagerRepository,
    int managerId,
    Employee? employee) : ICommand
{
    // Potentially check against business/technical rules that might block execution.
    // Here: the employee shouldn't be null, and shouldn't be on the manager's list already.
    // Other potential rule(s): ensure that an employee can only be added to
    // one manager's list at the same time, etc.
    public bool CanExecute() =>
        employee is not null
        && !employeeManagerRepository.HasEmployee(managerId, employee.Id);

    public void Execute()
    {
        if (employee is null)
        {
            return;
        }

        employeeManagerRepository.AddEmployee(managerId, employee);
    }

    public void Undo()
    {
        if (employee is null)
        {
            return;
        }

        employeeManagerRepository.RemoveEmployee(managerId, employee);
    }
}

/// <summary>
/// Invoker: runs commands and remembers them, so they can be undone later.
/// </summary>
public class CommandManager
{
    // a stack gives back the most recently added command first
    private readonly Stack<ICommand> _commands = new();

    public void Invoke(ICommand command)
    {
        if (command.CanExecute())
        {
            command.Execute();
            _commands.Push(command);
        }
    }

    public void Undo()
    {
        if (_commands.TryPop(out var command))
        {
            command.Undo();
        }
    }

    public void UndoAll()
    {
        while (_commands.TryPop(out var command))
        {
            command.Undo();
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
