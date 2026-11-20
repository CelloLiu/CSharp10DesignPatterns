using Prototype;

// shallow clone
var manager = new Manager("Cindy");
var managerClone = (Manager)manager.Clone();
Console.WriteLine($"Manager was cloned: {managerClone.Name}");

// deep clone: the employee's manager is copied as well
var employee = new Employee("Kevin", manager);
var employeeClone = (Employee)employee.Clone(true);
Console.WriteLine($"Employee was cloned: {employeeClone.Name}, with manager {employeeClone.Manager.Name}");

// change the manager name: the deep clone has its own manager, so it still says Cindy
manager.Name = "Karen";
Console.WriteLine($"Employee was cloned: {employeeClone.Name}, with manager {employeeClone.Manager.Name}");
