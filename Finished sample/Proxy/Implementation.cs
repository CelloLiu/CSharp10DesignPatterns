// Proxy: put a stand-in object in front of the real one to control access to it,
// for example to delay expensive work or to check permissions first.
namespace Proxy;

/// <summary>Subject</summary>
public interface IDocument
{
    void DisplayDocument();
}

/// <summary>RealSubject: expensive to create</summary>
public class Document : IDocument
{
    public string Title { get; }
    public string Content { get; }
    public int AuthorId { get; }
    public DateTimeOffset LastAccessed { get; }

    public Document(string fileName)
    {
        Console.WriteLine("Executing expensive action: loading a file from disk");
        // fake loading the file...
        Thread.Sleep(1000);

        Title = "An expensive document";
        Content = "Lots and lots of content";
        AuthorId = 1;
        LastAccessed = DateTimeOffset.UtcNow;
    }

    public void DisplayDocument()
    {
        Console.WriteLine($"Title: {Title}, Content: {Content}");
    }
}

/// <summary>
/// Proxy (virtual proxy): avoids creating the document until it is really needed.
/// </summary>
public class DocumentProxy : IDocument
{
    // Lazy<T> runs the code that creates the document the first time Value is used
    private readonly Lazy<Document> _document;

    public DocumentProxy(string fileName)
    {
        _document = new Lazy<Document>(() => new Document(fileName));
    }

    public void DisplayDocument() => _document.Value.DisplayDocument();
}

/// <summary>
/// Proxy (protection proxy): only lets users with the "Viewer" role through.
/// It wraps the other proxy, which shows that proxies can be chained.
/// </summary>
public class ProtectedDocumentProxy(string fileName, string userRole) : IDocument
{
    private readonly DocumentProxy _documentProxy = new(fileName);

    public void DisplayDocument()
    {
        Console.WriteLine($"Entering DisplayDocument in {nameof(ProtectedDocumentProxy)}.");

        if (userRole != "Viewer")
        {
            throw new UnauthorizedAccessException();
        }

        _documentProxy.DisplayDocument();

        Console.WriteLine($"Exiting DisplayDocument in {nameof(ProtectedDocumentProxy)}.");
    }
}
