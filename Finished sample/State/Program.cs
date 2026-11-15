using State;

// starts in regular, with a balance of 200
BankAccount bankAccount = new();
// regular, balance 300
bankAccount.Deposit(100);
// balance -200: regular -> overdrawn
bankAccount.Withdraw(500);
// cannot withdraw while overdrawn
bankAccount.Withdraw(100);
// balance 1800: overdrawn -> regular (an overdrawn account never jumps straight to gold)
bankAccount.Deposit(2000);
// balance 1900: regular -> gold
bankAccount.Deposit(100);
// balance -1100: gold -> overdrawn
bankAccount.Withdraw(3000);
// balance 1900: overdrawn -> regular
bankAccount.Deposit(3000);
// balance 2000: regular -> gold
bankAccount.Deposit(100);
// already gold, so this deposit gets the 10% bonus: balance 2110
bankAccount.Deposit(100);
