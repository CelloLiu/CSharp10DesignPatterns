using Command;

var editor = new TextEditor();
var history = new CommandHistory();

history.Run(new AppendTextCommand(editor, "Hello "));
Console.WriteLine($"After append 1: \"{editor.Content}\"");

history.Run(new AppendTextCommand(editor, "World!"));
Console.WriteLine($"After append 2: \"{editor.Content}\"");

history.Run(new ClearTextCommand(editor));
Console.WriteLine($"After clear:    \"{editor.Content}\"");

// Undo commands in reverse order (LIFO)
history.Undo();
Console.WriteLine($"After undo 1:   \"{editor.Content}\"");

history.Undo();
Console.WriteLine($"After undo 2:   \"{editor.Content}\"");

history.Undo();
Console.WriteLine($"After undo 3:   \"{editor.Content}\"");
