using System;

namespace Validating_Accounts
{
    class BankSystem
    {
        static void Main(string[] args)
        {

            // create object
            Account Nak = new Account("Rathanak Sambo", 10000.0m);
            Account bob = new Account("bob", 5000.0m);

            //test transferTransaction rollback when insufficient funds 
            //TransferTransaction one = new TransferTransaction(Nak, bob, 5000);
            //one.Execute();
            //WithdrawTransaction test = new WithdrawTransaction(bob, 10000.0m);
            //test.Execute();
            //one.Rollback();


            Console.WriteLine("Welcome to the Bank System");

            bool quit = false;
            while (!quit)
            {
                //prompt user's choice using readuseroption static method 
                menuOption userChoice = ReadUserOption();

                switch (userChoice)
                {
                    case menuOption.Withdraw:
                        DoWithdraw(Nak);
                        break;

                    case menuOption.Deposit:
                        DoDeposit(Nak);
                        break;

                    case menuOption.Transfer:
                        DoTransfer(Nak, bob);
                        break;

                    case menuOption.Print:
                        DoPrint(Nak);
                        DoPrint(bob);
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
                Console.WriteLine();
                Console.WriteLine("1. Withdraw");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Transfer");
                Console.WriteLine("4. Print Account Details");
                Console.WriteLine("5. Quit");
                Console.Write("Select an option: ");


                Console.Write("Enter your choice (1-5): ");

                //choice = Convert.ToInt32(Console.ReadLine());
                string input = Console.ReadLine();
                int.TryParse(input, out choice);



                // Check if the choice is valid
                if (choice < 1 || choice > 5)
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                }
            } while (choice < 1 || choice > 5);

            // return and convert the choice integer to menuOption 
            return (menuOption)choice;
        }

        static void DoWithdraw(Account account)
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
                    transaction.Execute();
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

        static void DoDeposit(Account account)
        {
            Console.Write("Enter amount to deposit: ");
            string depositInput = Console.ReadLine();
            decimal depositAmount;
            if (decimal.TryParse(depositInput, out depositAmount))
            {
                DepositTransaction transaction = new DepositTransaction(account, depositAmount);

                try
                {
                    transaction.Execute();
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

        static void DoTransfer(Account fromAccount, Account toAccount)
        {
            Console.Write("Enter the amount to transfer: ");
            string amountInput = Console.ReadLine();
            decimal amount;

            if (decimal.TryParse(amountInput, out amount))
            {
                TransferTransaction transaction = new TransferTransaction(fromAccount, toAccount, amount);
                try
                {
                    transaction.Execute();
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

        static void DoPrint(Account account)
        {
            account.Print();
        }

    }

        public enum menuOption
        {
            Withdraw = 1,
            Deposit,
            Transfer,
            Print,
            Quit
        }

}

