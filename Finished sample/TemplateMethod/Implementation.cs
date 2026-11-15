// Template Method: a base class fixes the steps of an algorithm and their
// order, and subclasses fill in or replace individual steps.
namespace TemplateMethod;

/// <summary>AbstractClass</summary>
public abstract class MailParser
{
    /// <summary>
    /// Template method: the fixed order of the steps. Subclasses can't change it.
    /// </summary>
    public string ParseMailBody(string identifier)
    {
        Console.WriteLine("Parsing mail body (in template method)...");
        FindServer();
        AuthenticateToServer();
        return ParseHtmlMailBody(identifier);
    }

    // a step with a default implementation: subclasses MAY override it
    public virtual void FindServer() => Console.WriteLine("Finding server...");

    // a step without an implementation: subclasses MUST provide it
    public abstract void AuthenticateToServer();

    // a step with a default implementation: subclasses MAY override it
    public virtual string ParseHtmlMailBody(string identifier)
    {
        Console.WriteLine("Parsing HTML mail body...");
        return $"This is the body of mail with id {identifier}";
    }
}

/// <summary>ConcreteClass</summary>
public class ExchangeMailParser : MailParser
{
    public override void AuthenticateToServer() => Console.WriteLine("Connecting to Exchange");
}

/// <summary>ConcreteClass</summary>
public class ApacheMailParser : MailParser
{
    public override void AuthenticateToServer() => Console.WriteLine("Connecting to Apache");
}

/// <summary>ConcreteClass: also replaces the optional FindServer step</summary>
public class EudoraMailParser : MailParser
{
    public override void FindServer() =>
        Console.WriteLine("Finding Eudora server through a custom algorithm...");

    public override void AuthenticateToServer() => Console.WriteLine("Connecting to Eudora");
}
