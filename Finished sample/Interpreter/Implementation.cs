// Interpreter: represent a small language (here, simple math like "(x + 5) - y")
// as a tree of expression objects, and evaluate it by calling Interpret() on the root.
// Note: Interpreter evaluates an already-built tree; parsing raw text into that tree
// is a separate step.
namespace Interpreter;

/// <summary>Context: holds variable values used while interpreting an expression</summary>
public class Context(IReadOnlyDictionary<string, int> variables)
{
    public int GetValue(string name) =>
        variables.TryGetValue(name, out var value)
            ? value
            : throw new KeyNotFoundException($"Variable '{name}' is not defined.");
}

/// <summary>AbstractExpression</summary>
public interface IExpression
{
    int Interpret(Context context);
}

/// <summary>TerminalExpression: a literal number (e.g., 5)</summary>
public class NumberExpression(int value) : IExpression
{
    public int Interpret(Context context) => value;
}

/// <summary>TerminalExpression: a variable looked up in the context (e.g., x)</summary>
public class VariableExpression(string name) : IExpression
{
    public int Interpret(Context context) => context.GetValue(name);
}

/// <summary>NonTerminalExpression: adds two child expressions (left + right)</summary>
public class AddExpression(IExpression left, IExpression right) : IExpression
{
    // A non-terminal asks its children to interpret themselves, then combines the results
    public int Interpret(Context context) =>
        left.Interpret(context) + right.Interpret(context);
}

/// <summary>NonTerminalExpression: subtracts two child expressions (left - right)</summary>
public class SubtractExpression(IExpression left, IExpression right) : IExpression
{
    public int Interpret(Context context) =>
        left.Interpret(context) - right.Interpret(context);
}
