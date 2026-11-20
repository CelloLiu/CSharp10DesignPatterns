// State: an object (the bank account) changes its behavior when its internal
// state changes. Each state is its own class, and decides which state comes next.
namespace State;

/// <summary>State</summary>
public abstract class BankAccountState(decimal balance, BankAccount bankAccount)
{
    public BankAccount BankAccount { get; } = bankAccount;
    public decimal Balance { get; protected set; } = balance;

    public abstract void Deposit(decimal amount);
    public abstract void Withdraw(decimal amount);
}

/// <summary>ConcreteState: the balance is below 0</summary>
public class OverdrawnState(decimal balance, BankAccount bankAccount)
    : BankAccountState(balance, bankAccount)
{
    public override void Deposit(decimal amount)
    {
        Console.WriteLine($"In {GetType().Name}, depositing {amount}");
        Balance += amount;
        if (Balance >= 0)
        {
            // change state to regular
            BankAccount.BankAccountState = new RegularState(Balance, BankAccount);
        }
    }

    public override void Withdraw(decimal amount)
    {
        // cannot withdraw
        Console.WriteLine($"In {GetType().Name}, cannot withdraw, balance {Balance}");
    }
}

/// <summary>ConcreteState: the balance is between 0 and 1000</summary>
public class RegularState(decimal balance, BankAccount bankAccount)
    : BankAccountState(balance, bankAccount)
{
    public override void Deposit(decimal amount)
    {
        Console.WriteLine($"In {GetType().Name}, depositing {amount}");
        Balance += amount;
        if (Balance >= 1000)
        {
            // change state to gold
            BankAccount.BankAccountState = new GoldState(Balance, BankAccount);
        }
    }

    public override void Withdraw(decimal amount)
    {
        Console.WriteLine($"In {GetType().Name}, withdrawing {amount} from {Balance}");
        Balance -= amount;
        if (Balance < 0)
        {
            // change state to overdrawn
            BankAccount.BankAccountState = new OverdrawnState(Balance, BankAccount);
        }
    }
}

/// <summary>ConcreteState: the balance is 1000 or more, deposits get a bonus</summary>
public class GoldState(decimal balance, BankAccount bankAccount)
    : BankAccountState(balance, bankAccount)
{
    public override void Deposit(decimal amount)
    {
        Console.WriteLine($"In {GetType().Name}, depositing {amount} + 10% bonus: {amount / 10}");
        Balance += amount + (amount / 10);
    }

    public override void Withdraw(decimal amount)
    {
        Console.WriteLine($"In {GetType().Name}, withdrawing {amount} from {Balance}");
        Balance -= amount;
        if (Balance < 0)
        {
            // change state to overdrawn
            BankAccount.BankAccountState = new OverdrawnState(Balance, BankAccount);
        }
        else if (Balance < 1000)
        {
            // change state to regular
            BankAccount.BankAccountState = new RegularState(Balance, BankAccount);
        }
    }
}

/// <summary>
/// Context: hands every request to its current state object.
/// </summary>
public class BankAccount
{
    public BankAccountState BankAccountState { get; set; }
    public decimal Balance => BankAccountState.Balance;

    public BankAccount()
    {
        // a new account starts as a regular account with a balance of 200
        BankAccountState = new RegularState(200, this);
    }

    // let the current state handle the deposit
    public void Deposit(decimal amount) => BankAccountState.Deposit(amount);

    // let the current state handle the withdrawal
    public void Withdraw(decimal amount) => BankAccountState.Withdraw(amount);
}
