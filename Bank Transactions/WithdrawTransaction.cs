using System;
namespace Validating_Accounts
{
    public class WithdrawTransaction
    {
        private Account _account;
        private decimal _amount;
        private bool _executed = false;
        private bool _success = false;
        private bool _reversed = false;

        // Read only property 
        public bool Executed
        {
            get { return _executed; }
        }

        public bool Success
        {
            get { return _success; }
        }

        public bool Reversed
        {
            get { return _reversed; }
        }

        public WithdrawTransaction(Account account, decimal amount)
        {
            _account = account;
            _amount = amount;
        }


        // Method to print transaction details
        public void Print()
        {
            Console.WriteLine("Withdraw Transaction Details:");
            Console.WriteLine($"Amount Attempted to Withdraw From Account {_account.Name}: {_amount:C}");
            Console.WriteLine($"Transaction Executed: {(_executed ? "Yes" : "No")}");
            Console.WriteLine($"Transaction Success: {(_success ? "Yes" : "No")}");
            Console.WriteLine($"Transaction Reversed: {(_reversed ? "Yes" : "No")}");
        }


        // Method to execute the withdrawal
        public void Execute()
        {
            if (_executed)
            {
                throw new InvalidOperationException("Transaction has already been executed.");
            }

            _executed = true;  

            if (_account.Withdraw(_amount))
            {
                _success = true;  
                Console.WriteLine("Withdrawal successful: " + _amount.ToString("c"));
            }
            else
            {
                _success = false;  
                                   
            }
        }

    public void Rollback()
        {
            if (!_executed)
            {
                throw new InvalidOperationException("Transaction has not been executed yet.");
            }
            if (_reversed)
            {
                throw new InvalidOperationException("Transaction has already been reversed.");
            }
            if (_success)
            {
                _account.Deposit(_amount);
                _reversed = true;
            }
            else
            {
                throw new InvalidOperationException("Transaction failed, cannot be reversed.");
            }
        }

        

    }
}

