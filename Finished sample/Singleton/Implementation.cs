// Singleton: make sure a class has only one instance, and give everyone
// a single place to get it from.
namespace Singleton;

/// <summary>Singleton</summary>
public class Logger
{
    // Lazy<T> creates the instance the first time it is asked for, and is thread-safe
    private static readonly Lazy<Logger> _lazyLogger = new(() => new Logger());

    /// <summary>The one and only instance</summary>
    public static Logger Instance => _lazyLogger.Value;

    // a private constructor stops other code from creating a second instance
    private Logger()
    {
    }

    /// <summary>SingletonOperation</summary>
    public void Log(string message) => Console.WriteLine($"Message to log: {message}");
}
