using System;
using System.Transactions;

namespace Validating_Accounts
{
    public class Bank
    {
        private List<Account> _accounts;
        public List<Transaction> _transactions { get; private set; }


        public Bank()
        {
            _accounts = new List<Account>();
            _transactions = new List<Transaction>();

        }

        public void AddAccount(Account account)
        {
            _accounts.Add(account);
        }

        public Account GetAccount(string name)
        {
            Account account = null;
            for (int i = 0; i < _accounts.Count; i++)
            {
                if (_accounts[i].Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    account = _accounts[i];
                    break; //break the loop 
                }
            }
            return account; //returns account or null if no match found
        }


        public void Execute(Transaction transaction)
        {
            _transactions.Add(transaction);
            try
            {
                transaction.Execute();
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine("An error occurred in executing the transaction");
                Console.WriteLine("The error was: " + exception.Message);
            }
        }

        public void Rollback(Transaction transaction)
        {
            try
            {
                transaction.Rollback();
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine("An error occurred in rolling the transaction back");
                Console.WriteLine("The error was: " + exception.Message);
            }
        }


        public void PrintTransactionHistory()
        {
            string transactionType;
            string transactionStatus;

            Console.WriteLine("\nTransaction History");
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("No. | Date       | Type       | Amount   | Status");
            Console.WriteLine("---------------------------------------------------------");

            for (int i = 0; i < _transactions.Count; i++)
            {
                transactionType = TransactionType(_transactions[i]);
                transactionStatus = TransactionStatus(_transactions[i]);

                Console.WriteLine($"{i + 1,-4} | {_transactions[i].DateStamp.ToShortDateString(),-10} | {transactionType,-10} | {_transactions[i].Amount.ToString("C"),-8} | {transactionStatus}");
            }
        }


        public string TransactionType(Transaction transaction)
        {
            switch (transaction.GetType().ToString())
            {
                case "Validating_Accounts.DepositTransaction":
                    return "Deposit";
                case "Validating_Accounts.WithdrawTransaction":
                    return "Withdraw";
                case "Validating_Accounts.TransferTransaction":
                    return "Transfer";
            }
            return "Null";
        }

        public string TransactionStatus(Transaction transaction)
        {
            if (!transaction.Executed)
            {
                return "Executed";
            }
            else if (transaction.Reversed)
            {
                return "Reversed";
            }
            else if (!transaction.Success)
            {
                return "Transaction Failed";
            }
            else
            {
                return "Complete";
            }
        }


    }
}

