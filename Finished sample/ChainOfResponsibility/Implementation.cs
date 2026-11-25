// Chain of Responsibility: pass a request along a chain of handlers. Each handler
// does its own check and either stops the chain or passes the request to the next handler.
namespace ChainOfResponsibility;

// The request that travels through the chain
public record Document(
    string Title,
    DateTimeOffset LastModified,
    bool ApprovedByLitigation,
    bool ApprovedByManagement);

/// <summary>
/// Handler: holds a reference to the next handler and runs the shared chain flow.
/// </summary>
public abstract class DocumentHandler
{
    private DocumentHandler? _next;

    // Returns the next handler so calls can be chained: a.SetNext(b).SetNext(c)
    public DocumentHandler SetNext(DocumentHandler next)
    {
        _next = next;
        return next;
    }

    public string? Handle(Document document)
    {
        // Run this handler's check; if it fails, stop the chain and return the error.
        // Note: in the classic pattern each handler decides whether to pass the request
        // along; here the base class does that so each subclass only writes its rule.
        var error = Validate(document);
        if (error is not null)
        {
            return $"{GetType().Name}: {error}";
        }

        // Otherwise pass to the next handler (returns null once the end of the chain passes)
        return _next?.Handle(document);
    }

    // Return an error message if invalid, or null if valid
    protected abstract string? Validate(Document document);
}

/// <summary>ConcreteHandler</summary>
public class TitleHandler : DocumentHandler
{
    protected override string? Validate(Document document) =>
        string.IsNullOrWhiteSpace(document.Title)
            ? "Title must be filled out."
            : null;
}

/// <summary>ConcreteHandler</summary>
public class LastModifiedHandler : DocumentHandler
{
    protected override string? Validate(Document document) =>
        document.LastModified < DateTimeOffset.UtcNow.AddDays(-30)
            ? "Document must be modified in the last 30 days."
            : null;
}

/// <summary>ConcreteHandler</summary>
public class LitigationApprovalHandler : DocumentHandler
{
    protected override string? Validate(Document document) =>
        !document.ApprovedByLitigation
            ? "Document must be approved by litigation."
            : null;
}

/// <summary>ConcreteHandler</summary>
public class ManagementApprovalHandler : DocumentHandler
{
    protected override string? Validate(Document document) =>
        !document.ApprovedByManagement
            ? "Document must be approved by management."
            : null;
}
