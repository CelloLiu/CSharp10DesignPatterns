using Memento;

var player = new Player();
var saveManager = new SaveManager();

// Save checkpoint 1 at the start
saveManager.Save(player.Save());
Console.WriteLine($"Start:       {player}");

// Reach Level 2 and save checkpoint 2
player.Play(nextLevel: 2, healthLost: 10, coinsFound: 50);
saveManager.Save(player.Save());
Console.WriteLine($"Level 2:     {player}");

// Enter the boss fight and lose all health
player.Play(nextLevel: 3, healthLost: 90, coinsFound: 10);
Console.WriteLine($"Boss fight:  {player}");

// Load the last checkpoint (Level 2)
player.Restore(saveManager.LoadLast());
Console.WriteLine($"Restored 1:  {player}");

// Load the earlier checkpoint (Start)
player.Restore(saveManager.LoadLast());
Console.WriteLine($"Restored 2:  {player}");

// Try loading again when no checkpoints are left
player.Restore(saveManager.LoadLast());
