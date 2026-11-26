// Command: turn a request into an object so an invoker can store it, queue it,
// run it, and undo it later — without knowing the details.
namespace Command;

/// <summary>Command: common interface for all executable actions</summary>
public interface ICommand
{
    void Execute();
    void Undo();
}

/// <summary>Receiver: the object that holds the data and performs the actual work</summary>
public class TextEditor
{
    public string Content { get; private set; } = string.Empty;

    public void Append(string text) => Content += text;

    public void RemoveLast(int length) =>
        Content = length >= Content.Length ? string.Empty : Content[..^length];

    public void SetContent(string text) => Content = text;
}

/// <summary>ConcreteCommand: appends text (undo removes what was appended)</summary>
public class AppendTextCommand(TextEditor editor, string text) : ICommand
{
    private bool _wasExecuted;

    public void Execute()
    {
        if (_wasExecuted)
        {
            throw new InvalidOperationException("Command has already been executed.");
        }

        editor.Append(text);
        _wasExecuted = true;
    }

    public void Undo()
    {
        if (!_wasExecuted)
        {
            return;
        }

        editor.RemoveLast(text.Length);
        _wasExecuted = false;
    }
}

/// <summary>ConcreteCommand: clears all text (undo restores the previous text)</summary>
public class ClearTextCommand(TextEditor editor) : ICommand
{
    // The command keeps its own undo data so it can restore the editor later
    private string? _previousContent;

    public void Execute()
    {
        if (_previousContent is not null)
        {
            throw new InvalidOperationException("Command has already been executed.");
        }

        _previousContent = editor.Content;
        editor.SetContent(string.Empty);
    }

    public void Undo()
    {
        if (_previousContent is null)
        {
            return;
        }

        editor.SetContent(_previousContent);
        _previousContent = null;
    }
}

/// <summary>
/// Invoker: runs commands and keeps a stack of them so they can be undone in reverse order.
/// It only knows ICommand — never the concrete commands or the TextEditor.
/// </summary>
public class CommandHistory
{
    private readonly Stack<ICommand> _history = new();

    public void Run(ICommand command)
    {
        command.Execute();
        _history.Push(command);
    }

    public void Undo()
    {
        // TryPop safely does nothing when the history is empty
        if (_history.TryPop(out var command))
        {
            command.Undo();
        }
    }
}
