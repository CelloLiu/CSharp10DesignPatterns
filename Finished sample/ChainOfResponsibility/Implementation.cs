// Chain of Responsibility: pass a request along a chain of handlers. Each handler
// does its own check and then hands the request to the next handler in the chain.
using System.ComponentModel.DataAnnotations;

namespace ChainOfResponsibility;

// the request that travels through the chain
public class Document(
    string title,
    DateTimeOffset lastModified,
    bool approvedByLitigation,
    bool approvedByManagement)
{
    public string Title { get; set; } = title;
    public DateTimeOffset LastModified { get; set; } = lastModified;
    public bool ApprovedByLitigation { get; set; } = approvedByLitigation;
    public bool ApprovedByManagement { get; set; } = approvedByManagement;
}

/// <summary>Handler</summary>
public interface IHandler<T> where T : class
{
    IHandler<T> SetSuccessor(IHandler<T> successor);
    void Handle(T request);
}

/// <summary>
/// Base class for the document handlers. It holds the part every handler shares:
/// remembering the next handler and passing the document on to it.
/// </summary>
public abstract class DocumentHandler : IHandler<Document>
{
    private IHandler<Document>? _successor;

    // returns the successor, so calls can be chained: a.SetSuccessor(b).SetSuccessor(c)
    public IHandler<Document> SetSuccessor(IHandler<Document> successor)
    {
        _successor = successor;
        return successor;
    }

    public void Handle(Document document)
    {
        // do this handler's own check (throws when the document is not valid)
        Validate(document);

        // go to the next handler, if there is one
        _successor?.Handle(document);
    }

    protected abstract void Validate(Document document);

    // helper to create the exception that is thrown when validation doesn't check out
    protected static ValidationException ValidationError(string message, string propertyName) =>
        new(new ValidationResult(message, [propertyName]), null, null);
}

/// <summary>ConcreteHandler</summary>
public class DocumentTitleHandler : DocumentHandler
{
    protected override void Validate(Document document)
    {
        if (document.Title == string.Empty)
        {
            throw ValidationError("Title must be filled out", nameof(Document.Title));
        }
    }
}

/// <summary>ConcreteHandler</summary>
public class DocumentLastModifiedHandler : DocumentHandler
{
    protected override void Validate(Document document)
    {
        if (document.LastModified < DateTimeOffset.UtcNow.AddDays(-30))
        {
            throw ValidationError(
                "Document must be modified in the last 30 days", nameof(Document.LastModified));
        }
    }
}

/// <summary>ConcreteHandler</summary>
public class DocumentApprovedByLitigationHandler : DocumentHandler
{
    protected override void Validate(Document document)
    {
        if (!document.ApprovedByLitigation)
        {
            throw ValidationError(
                "Document must be approved by litigation", nameof(Document.ApprovedByLitigation));
        }
    }
}

/// <summary>ConcreteHandler</summary>
public class DocumentApprovedByManagementHandler : DocumentHandler
{
    protected override void Validate(Document document)
    {
        if (!document.ApprovedByManagement)
        {
            throw ValidationError(
                "Document must be approved by management", nameof(Document.ApprovedByManagement));
        }
    }
}
