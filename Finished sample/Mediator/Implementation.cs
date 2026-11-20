// Mediator: objects don't talk to each other directly. They all talk to one
// central object (the mediator), which decides who receives what.
namespace Mediator;

/// <summary>Mediator</summary>
public interface IChatRoom
{
    void Register(TeamMember teamMember);
    // to everyone
    void Send(string from, string message);
    // to one person
    void Send(string from, string to, string message);
    // to everyone of a certain type, e.g. all lawyers
    void SendTo<T>(string from, string message) where T : TeamMember;
}

/// <summary>
/// Colleague: only knows the chat room, not the other team members.
/// </summary>
public abstract class TeamMember(string name)
{
    public string Name { get; set; } = name;

    // set when the team member is registered in a chat room
    private IChatRoom? _chatroom;

    internal void SetChatroom(IChatRoom chatRoom) => _chatroom = chatRoom;

    public void Send(string message) => _chatroom?.Send(Name, message);

    public void Send(string to, string message) => _chatroom?.Send(Name, to, message);

    public void SendTo<T>(string message) where T : TeamMember => _chatroom?.SendTo<T>(Name, message);

    public virtual void Receive(string from, string message)
    {
        Console.WriteLine($"message from {from} to {Name}: {message}");
    }
}

/// <summary>ConcreteColleague</summary>
public class Lawyer(string name) : TeamMember(name)
{
    public override void Receive(string from, string message)
    {
        Console.Write($"{nameof(Lawyer)} {Name} received: ");
        base.Receive(from, message);
    }
}

/// <summary>ConcreteColleague</summary>
public class AccountManager(string name) : TeamMember(name)
{
    public override void Receive(string from, string message)
    {
        Console.Write($"{nameof(AccountManager)} {Name} received: ");
        base.Receive(from, message);
    }
}

/// <summary>ConcreteMediator</summary>
public class TeamChatRoom : IChatRoom
{
    // the registered team members, by name
    private readonly Dictionary<string, TeamMember> _teamMembers = [];

    public void Register(TeamMember teamMember)
    {
        teamMember.SetChatroom(this);
        // TryAdd does nothing when the name is already registered
        _teamMembers.TryAdd(teamMember.Name, teamMember);
    }

    public void Send(string from, string message)
    {
        foreach (var teamMember in _teamMembers.Values)
        {
            teamMember.Receive(from, message);
        }
    }

    public void Send(string from, string to, string message)
    {
        if (_teamMembers.TryGetValue(to, out var teamMember))
        {
            teamMember.Receive(from, message);
        }
        else
        {
            Console.WriteLine($"No team member named '{to}' is registered in the chat room.");
        }
    }

    public void SendTo<T>(string from, string message) where T : TeamMember
    {
        foreach (var teamMember in _teamMembers.Values.OfType<T>())
        {
            teamMember.Receive(from, message);
        }
    }
}
