// Mediator: objects don't talk to each other directly. They all talk to one
// central object (the mediator), which decides who receives what.
namespace Mediator;

/// <summary>Mediator</summary>
public interface IChatRoom
{
    void Join(User user);
    void Broadcast(User sender, string message);
    void SendTo(User sender, string recipientName, string message);
}

/// <summary>
/// Colleague: only knows the chat room and never holds references to other users.
/// </summary>
public class User
{
    private readonly IChatRoom _chatRoom;

    public string Name { get; }

    public User(string name, IChatRoom chatRoom)
    {
        Name = name;
        _chatRoom = chatRoom;

        // Kept in the constructor for simplicity here; in larger classes,
        // prefer registering after construction so 'this' isn't exposed early.
        _chatRoom.Join(this);
    }

    public void Broadcast(string message) => _chatRoom.Broadcast(this, message);

    public void SendTo(string recipientName, string message) =>
        _chatRoom.SendTo(this, recipientName, message);

    public void Receive(string from, string message) =>
        Console.WriteLine($"[{Name}] received from {from}: {message}");
}

/// <summary>
/// ConcreteMediator: the only object that knows who is in the chat room.
/// </summary>
public class ChatRoom : IChatRoom
{
    private readonly Dictionary<string, User> _users = [];

    public void Join(User user)
    {
        if (!_users.TryAdd(user.Name, user))
        {
            throw new InvalidOperationException($"A user named '{user.Name}' is already in the chat room.");
        }
    }

    public void Broadcast(User sender, string message)
    {
        // Deliver to everyone in the room except the sender
        foreach (var user in _users.Values)
        {
            if (user != sender)
            {
                user.Receive(sender.Name, message);
            }
        }
    }

    public void SendTo(User sender, string recipientName, string message)
    {
        if (_users.TryGetValue(recipientName, out var recipient))
        {
            recipient.Receive(sender.Name, message);
        }
        else
        {
            Console.WriteLine($"No user named '{recipientName}' is in the chat room.");
        }
    }
}
