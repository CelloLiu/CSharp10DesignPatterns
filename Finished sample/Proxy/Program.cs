using Proxy;

// without proxy: the expensive loading happens right away
Console.WriteLine("Constructing document.");
var myDocument = new Document("MyDocument.pdf");
Console.WriteLine("Document constructed.");
myDocument.DisplayDocument();

Console.WriteLine();

// with proxy: the loading only happens when the document is displayed
Console.WriteLine("Constructing document proxy.");
var myDocumentProxy = new DocumentProxy("MyDocument.pdf");
Console.WriteLine("Document proxy constructed.");
myDocumentProxy.DisplayDocument();

Console.WriteLine();

// with chained proxy
Console.WriteLine("Constructing protected document proxy.");
var myProtectedDocumentProxy = new ProtectedDocumentProxy("MyDocument.pdf", "Viewer");
Console.WriteLine("Protected document proxy constructed.");
myProtectedDocumentProxy.DisplayDocument();

Console.WriteLine();

// with chained proxy, no access: the protection proxy blocks access
Console.WriteLine("Constructing protected document proxy.");
myProtectedDocumentProxy = new ProtectedDocumentProxy("MyDocument.pdf", "AnotherRole");
Console.WriteLine("Protected document proxy constructed.");
try
{
    myProtectedDocumentProxy.DisplayDocument();
}
catch (UnauthorizedAccessException ex)
{
    Console.WriteLine($"Access denied: {ex.Message}");
}
