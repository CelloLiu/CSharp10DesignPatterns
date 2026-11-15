using Interpreter;

// the order matters: hundreds first, then tens, then ones
List<RomanExpression> expressions =
[
    new RomanHundredExpression(),
    new RomanTenExpression(),
    new RomanOneExpression()
];

Translate(5);
Translate(81);
Translate(733);

// let every expression interpret its part of the number
void Translate(int number)
{
    var context = new RomanContext(number);
    foreach (var expression in expressions)
    {
        expression.Interpret(context);
    }

    Console.WriteLine($"Translating Arabic numerals to Roman numerals: {number} = {context.Output}");
}
