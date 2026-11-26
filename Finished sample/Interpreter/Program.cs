using Interpreter;

// Build the expression tree for: (x + 5) - y
IExpression expression = new SubtractExpression(
    new AddExpression(
        new VariableExpression("x"),
        new NumberExpression(5)),
    new VariableExpression("y"));

// Evaluate the same expression tree with different variable values
var context1 = new Context(new Dictionary<string, int> { ["x"] = 10, ["y"] = 3 });
Console.WriteLine($"(x + 5) - y with x=10, y=3 => {expression.Interpret(context1)}");

var context2 = new Context(new Dictionary<string, int> { ["x"] = 20, ["y"] = 8 });
Console.WriteLine($"(x + 5) - y with x=20, y=8 => {expression.Interpret(context2)}");
