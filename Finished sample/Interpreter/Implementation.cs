// Interpreter: describe a simple language as a set of small expression classes,
// and let each one interpret its own piece of the input.
// Here the "language" is Roman numerals: each expression translates one digit
// position (hundreds, tens or ones) of a number.
namespace Interpreter;

/// <summary>
/// Context: the number still to translate (Input) and the result so far (Output).
/// </summary>
public class RomanContext(int input)
{
    public int Input { get; set; } = input;
    public string Output { get; set; } = string.Empty;
}

/// <summary>AbstractExpression</summary>
public abstract class RomanExpression
{
    // The numerals this expression knows, from the largest value to the smallest.
    // Four of them are enough for one digit position, e.g. for the ones:
    // 9 = IX, 5 = V, 4 = IV and 1 = I. Everything else is a combination of those
    // (8 = V + I + I + I = VIII, 6 = V + I = VI, 3 = I + I + I = III, ...).
    protected abstract (int Value, string Numeral)[] Numerals { get; }

    public void Interpret(RomanContext context)
    {
        foreach (var (value, numeral) in Numerals)
        {
            // as long as the value still fits in the input: write the numeral
            // and take the value off the input
            while (context.Input >= value)
            {
                context.Output += numeral;
                context.Input -= value;
            }
        }
    }
}

/// <summary>TerminalExpression: translates 1 to 9</summary>
public class RomanOneExpression : RomanExpression
{
    protected override (int Value, string Numeral)[] Numerals { get; } =
        [(9, "IX"), (5, "V"), (4, "IV"), (1, "I")];
}

/// <summary>TerminalExpression: translates 10 to 90</summary>
public class RomanTenExpression : RomanExpression
{
    protected override (int Value, string Numeral)[] Numerals { get; } =
        [(90, "XC"), (50, "L"), (40, "XL"), (10, "X")];
}

/// <summary>TerminalExpression: translates 100 to 900</summary>
public class RomanHundredExpression : RomanExpression
{
    protected override (int Value, string Numeral)[] Numerals { get; } =
        [(900, "CM"), (500, "D"), (400, "CD"), (100, "C")];
}
