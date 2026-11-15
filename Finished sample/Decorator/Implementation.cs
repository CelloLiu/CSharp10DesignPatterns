// Decorator: add behavior to an object by wrapping it in another object
// with the same interface, instead of changing or subclassing it.
namespace Decorator;

/// <summary>Component</summary>
public interface IMailService
{
    bool SendMail(string message);
}

/// <summary>ConcreteComponent</summary>
public class CloudMailService : IMailService
{
    public bool SendMail(string message)
    {
        Console.WriteLine($"Message \"{message}\" sent via {nameof(CloudMailService)}.");
        return true;
    }
}

/// <summary>ConcreteComponent</summary>
public class OnPremiseMailService : IMailService
{
    public bool SendMail(string message)
    {
        Console.WriteLine($"Message \"{message}\" sent via {nameof(OnPremiseMailService)}.");
        return true;
    }
}

/// <summary>
/// Decorator: wraps a mail service and by default just passes the call on to it.
/// </summary>
public abstract class MailServiceDecoratorBase(IMailService mailService) : IMailService
{
    public virtual bool SendMail(string message) => mailService.SendMail(message);
}

/// <summary>ConcreteDecorator: adds statistics BEFORE sending</summary>
public class StatisticsDecorator(IMailService mailService) : MailServiceDecoratorBase(mailService)
{
    public override bool SendMail(string message)
    {
        // Fake collecting statistics
        Console.WriteLine($"Collecting statistics in {nameof(StatisticsDecorator)}.");
        return base.SendMail(message);
    }
}

/// <summary>ConcreteDecorator: stores the message AFTER sending</summary>
public class MessageDatabaseDecorator(IMailService mailService) : MailServiceDecoratorBase(mailService)
{
    /// <summary>A list of sent messages - a "fake" database</summary>
    public List<string> SentMessages { get; } = [];

    public override bool SendMail(string message)
    {
        var wasSent = base.SendMail(message);
        if (wasSent)
        {
            // store sent message
            SentMessages.Add(message);
        }

        return wasSent;
    }
}
