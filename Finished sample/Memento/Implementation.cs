// Memento: capture an object's state in a snapshot (the memento) so it can be
// restored later. Player saves and restores its own state; SaveManager simply
// holds the snapshots without looking inside them.
namespace Memento;

/// <summary>
/// Memento: an immutable snapshot of a player's state.
/// Its constructor and properties are internal (hidden from other assemblies,
/// and signaling here that only Player should create or read it).
/// </summary>
public sealed class GameSave
{
    internal int Level { get; }
    internal int Health { get; }
    internal int Coins { get; }

    internal GameSave(int level, int health, int coins) =>
        (Level, Health, Coins) = (level, health, coins);
}

/// <summary>Originator: the object whose state we want to save and restore</summary>
public class Player
{
    public int Level { get; private set; } = 1;
    public int Health { get; private set; } = 100;
    public int Coins { get; private set; } = 0;

    public void Play(int nextLevel, int healthLost, int coinsFound)
    {
        Level = nextLevel;
        Health = Math.Max(0, Health - healthLost);
        Coins += coinsFound;
    }

    public GameSave Save() => new(Level, Health, Coins);

    public void Restore(GameSave? save)
    {
        if (save is null)
        {
            Console.WriteLine("No save to load.");
            return;
        }

        Level = save.Level;
        Health = save.Health;
        Coins = save.Coins;
    }

    public override string ToString() =>
        $"Level {Level} | Health {Health} | Coins {Coins}";
}

/// <summary>
/// Caretaker: stores saved checkpoints in a stack without ever looking inside them.
/// </summary>
public class SaveManager
{
    private readonly Stack<GameSave> _saves = new();

    public void Save(GameSave save) => _saves.Push(save);

    // Returns null when there are no saved checkpoints left
    public GameSave? LoadLast() => _saves.TryPop(out var save) ? save : null;
}
