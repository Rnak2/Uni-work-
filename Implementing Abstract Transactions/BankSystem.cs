using System;
using System.Transactions;

namespace Validating_Accounts
{
    class BankSystem
    {
        static void Main(string[] args)
        {
            Bank bank = new Bank();

            Console.WriteLine("Welcome to the Bank System");

            bool quit = false;
            while (!quit)
            {
                //prompt user's choice using readuseroption static method 
                menuOption userChoice = ReadUserOption();

                switch (userChoice)
                {
                    case menuOption.Withdraw:
                        DoWithdraw(bank);
                        break;

                    case menuOption.Deposit:
                        DoDeposit(bank);
                        break;

                    case menuOption.Transfer:
                        DoTransfer(bank);
                        break;

                    case menuOption.RollBack:
                        DoRollback(bank);
                        break;


                    case menuOption.Print:
                        DoPrint(bank);
                        break;

                    case menuOption.AddNewAccount:
                        AddNewAccount(bank);
                        break;

                    case menuOption.Quit:
                        quit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

            }
        }

        static menuOption ReadUserOption()
        {
            int choice;
            do
            {
                //menu
                Console.WriteLine("|-------------------------------|");
                Console.WriteLine("| 1. Withdraw                   |");
                Console.WriteLine("| 2. Deposit                    |");
                Console.WriteLine("| 3. Transfer                   |");
                Console.WriteLine("| 4. Rollback                   |");
                Console.WriteLine("| 5. Print Account Details      |");
                Console.WriteLine("| 6. Add New Account            |");
                Console.WriteLine("| 7. Quit                       |");
                Console.WriteLine("|-------------------------------|");
                Console.Write("Enter your choice (1-7): ");

                //choice = Convert.ToInt32(Console.ReadLine());
                string input = Console.ReadLine();
                int.TryParse(input, out choice);



                // Check if the choice is valid
                if (choice < 1 || choice > 7)
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 7.");
                }
            } while (choice < 1 || choice > 7);
            
            // return and convert the choice integer to menuOption 
            return (menuOption)choice;
        }

        static void DoWithdraw(Bank bank)
        {
            Account account = FindAccount(bank);
            if (account != null)
            {
                Console.Write("Enter amount to withdraw: ");
                string withdrawInput = Console.ReadLine();
                decimal withdrawAmount;
                if (decimal.TryParse(withdrawInput, out withdrawAmount))
                {
                    // Create a new WithdrawTransaction object
                    WithdrawTransaction transaction = new WithdrawTransaction(account, withdrawAmount);
                    try
                    {
                        bank.Execute(transaction);
                        Console.WriteLine("Transaction executed successfully.");
                    }
                    catch (InvalidOperationException m)
                    {
                        Console.WriteLine($"Transaction failed: {m.Message}");
                    }

                    transaction.Print();
                }
                else
                {
                    Console.WriteLine("Invalid amount. Please enter a valid numeric value.");
                }
            }

        }

        static void DoDeposit(Bank bank)
        {
            Account account = FindAccount(bank);
            if (account != null)
            {
                Console.Write("Enter amount to deposit: ");
                string depositInput = Console.ReadLine();
                decimal depositAmount;
                if (decimal.TryParse(depositInput, out depositAmount))
                {
                    DepositTransaction transaction = new DepositTransaction(account, depositAmount);

                    try
                    {
                        bank.Execute(transaction);
                        Console.WriteLine("Deposit executed successfully.");
                    }
                    catch (InvalidOperationException m)
                    {
                        Console.WriteLine($"Deposit failed: {m.Message}");
                    }

                    transaction.Print();
                }

                else
                {
                    Console.WriteLine("Invalid amount.");
                }
            }     
           
        }

        static void DoTransfer(Bank bank)
        {
            Console.WriteLine("Source account");
            Account fromAccount = FindAccount(bank);
            if (fromAccount == null)
            {
                throw new InvalidOperationException("Source account not found.");
            }

            Console.WriteLine("Destination account");
            Account toAccount = FindAccount(bank);
            if (toAccount == null)
            {
                throw new InvalidOperationException("Destination account not found.");
            }

            Console.Write("Enter the amount to transfer: ");
            string amountInput = Console.ReadLine();
            decimal amount;

            if (decimal.TryParse(amountInput, out amount))
            {
                TransferTransaction transaction = new TransferTransaction(fromAccount, toAccount, amount);
                try
                {
                    bank.Execute(transaction);
                    Console.WriteLine();
                    Console.WriteLine("Transfer executed successfully.");
                }
                catch (InvalidOperationException m)
                {
                    Console.WriteLine($"Transfer failed: {m.Message}");
                }
                transaction.Print();
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
        }

        static void DoPrint(Bank bank)
        {
            Account account = FindAccount(bank);
            if (account != null)
            {
                account.Print();
            }
        }

        //private static Bank bank = new Bank();
        static void AddNewAccount(Bank bank)
        {
            Console.WriteLine("Add New Account");
            Console.Write("Enter the name of the account: ");
            string name = Console.ReadLine();
            var existingAccount = bank.GetAccount(name);
            if (existingAccount != null)
            {
                Console.WriteLine("Account already exists. Please enter a different account name.");
                return;  
            }
            Console.Write("Enter the starting balance: ");
            string input = Console.ReadLine();
            decimal balance;
            while (!decimal.TryParse(input, out balance) || balance < 0)
            {
                Console.WriteLine("Please enter a valid starting balance.");
            }

            Account newAccount = new Account(name, balance);
            bank.AddAccount(newAccount);
            Console.WriteLine($"New account for {name} with balance {balance:C} added successfully.");
        }

        private static Account FindAccount(Bank bank)
        {
            Console.Write("Please enter the account name: ");
            string accountName = Console.ReadLine();
            Account account = bank.GetAccount(accountName);

            if (account == null)
            {
                Console.WriteLine($"No account found with the name: {accountName}");
            }
            return account;
        }

        static void DoRollback(Bank bank)
        {
            bank.PrintTransactionHistory();
            Console.WriteLine("Enter the index of the transaction to rollback and 0 for no rollback:");
            int result = Convert.ToInt32(Console.ReadLine());
            try
            {
                if (result == 0)
                    return;
                bank.Rollback(bank._transactions[result-1]);
                //Console.WriteLine("Transaction rolled back successfully.");
            }
            catch (Exception m)
            {
                Console.WriteLine($"Failed to rollback transaction: {m.Message}");
            }
        }


    }

        public enum menuOption
        {
            Withdraw = 1,
            Deposit,
            Transfer,
            RollBack,
            Print,
            AddNewAccount,
            Quit
        }

}
