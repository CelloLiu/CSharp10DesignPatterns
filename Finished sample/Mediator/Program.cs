using Mediator;

var chatRoom = new ChatRoom();

var alice = new User("Alice", chatRoom);
var bob = new User("Bob", chatRoom);
var carol = new User("Carol", chatRoom);

// Users only talk through the chat room — never directly to each other
alice.Broadcast("Hi everyone!");
bob.SendTo("Alice", "Hey Alice, welcome!");
carol.SendTo("Dave", "Are you there?");
