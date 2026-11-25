using ChainOfResponsibility;

// Build the chain: Title -> LastModified -> Litigation -> Management
var chain = new TitleHandler();
chain
    .SetNext(new LastModifiedHandler())
    .SetNext(new LitigationApprovalHandler())
    .SetNext(new ManagementApprovalHandler());

var validDocument = new Document(
    Title: "How to Avoid Java Development",
    LastModified: DateTimeOffset.UtcNow,
    ApprovedByLitigation: true,
    ApprovedByManagement: true);

var invalidDocument = validDocument with { ApprovedByLitigation = false };

Check(validDocument);
Check(invalidDocument);

void Check(Document document)
{
    var error = chain.Handle(document);
    Console.WriteLine(error is null
        ? $"Approved: '{document.Title}' passed all checks."
        : $"Rejected: {error}");
}
