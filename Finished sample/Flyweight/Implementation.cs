// Flyweight: save memory by sharing one object between everyone who needs it.
// What is shared lives inside the object (intrinsic state); what differs per
// use is passed in by the caller (extrinsic state).
namespace Flyweight;

/// <summary>Flyweight</summary>
public interface ICharacter
{
    // font family and size are the extrinsic state
    void Draw(string fontFamily, int fontSize);
}

/// <summary>ConcreteFlyweight</summary>
public class CharacterA : ICharacter
{
    // the intrinsic state
    private const char ActualCharacter = 'a';

    public void Draw(string fontFamily, int fontSize) =>
        Console.WriteLine($"Drawing {ActualCharacter}, {fontFamily} {fontSize}");
}

/// <summary>ConcreteFlyweight</summary>
public class CharacterB : ICharacter
{
    // the intrinsic state
    private const char ActualCharacter = 'b';

    public void Draw(string fontFamily, int fontSize) =>
        Console.WriteLine($"Drawing {ActualCharacter}, {fontFamily} {fontSize}");
}

/// <summary>FlyweightFactory: hands out shared characters</summary>
public class CharacterFactory
{
    private readonly Dictionary<char, ICharacter> _characters = [];

    public ICharacter? GetCharacter(char characterIdentifier)
    {
        // Was this character created before? Then reuse it.
        if (_characters.TryGetValue(characterIdentifier, out var existingCharacter))
        {
            Console.WriteLine("Character reuse");
            return existingCharacter;
        }

        // The character isn't in the dictionary: create it, store it, return it.
        ICharacter? newCharacter = characterIdentifier switch
        {
            'a' => new CharacterA(),
            'b' => new CharacterB(),
            // and so on...
            _ => null
        };

        if (newCharacter is not null)
        {
            Console.WriteLine("Character construction");
            _characters[characterIdentifier] = newCharacter;
        }

        return newCharacter;
    }

    public ICharacter CreateParagraph(List<ICharacter> characters, int location) =>
        new Paragraph(characters, location);
}

/// <summary>
/// UnsharedConcreteFlyweight: has the same interface, but each one is its own object.
/// </summary>
public class Paragraph(List<ICharacter> characters, int location) : ICharacter
{
    public void Draw(string fontFamily, int fontSize)
    {
        Console.WriteLine($"Drawing in paragraph at location {location}");
        foreach (var character in characters)
        {
            character.Draw(fontFamily, fontSize);
        }
    }
}
