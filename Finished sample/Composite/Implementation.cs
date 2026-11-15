// Composite: treat a single object (a file) and a group of objects (a directory)
// in the same way, so they can be combined into a tree.
namespace Composite;

/// <summary>Component</summary>
public abstract class FileSystemItem(string name)
{
    public string Name { get; set; } = name;

    public abstract long GetSize();
}

/// <summary>Leaf: has no children</summary>
public class File(string name, long size) : FileSystemItem(name)
{
    public override long GetSize() => size;
}

/// <summary>Composite: can contain leaves and other composites</summary>
public class Directory(string name, long size) : FileSystemItem(name)
{
    private readonly List<FileSystemItem> _fileSystemItems = [];

    public void Add(FileSystemItem itemToAdd) => _fileSystemItems.Add(itemToAdd);

    public void Remove(FileSystemItem itemToRemove) => _fileSystemItems.Remove(itemToRemove);

    // its own size plus the size of everything inside it
    public override long GetSize()
    {
        var treeSize = size;
        foreach (var fileSystemItem in _fileSystemItems)
        {
            // a child can be a file or another directory: both know their own size
            treeSize += fileSystemItem.GetSize();
        }
        return treeSize;
    }
}
