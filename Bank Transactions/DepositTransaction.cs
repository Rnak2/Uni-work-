using System;
namespace Validating_Accounts
{
    public class DepositTransaction
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

        public DepositTransaction(Account account, decimal amount)
        {
            _account = account;
            _amount = amount;
        }

        public void Print()
        {
            Console.WriteLine("Deposit Transaction Details:");
            Console.WriteLine($"Amount Deposited From Account {_account.Name}: {_amount:c}");
            Console.WriteLine($"Transaction Executed: {(_executed ? "Yes" : "No")}");
            Console.WriteLine($"Transaction Success: {(_success ? "Yes" : "No")}");
            Console.WriteLine($"Transaction Reversed: {(_reversed ? "Yes" : "No")}");
        }


        public void Execute()
        {
            if (_executed)
            {
                throw new InvalidOperationException("Transaction has already been executed.");
            }
            _executed = true;

            if (_amount <= 0)
            {
                _success = false;
                throw new InvalidOperationException("Invalid deposit amount. Amount must be greater than zero.");
            }

            _account.Deposit(_amount);
            _success = true;
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
                bool withdrawSuccess = _account.Withdraw(_amount);
                if (!withdrawSuccess)
                {
                    throw new InvalidOperationException("Unable to reverse the deposit due to insufficient funds in the account.");
                }
                _reversed = true; 
            }
            else
            {
                throw new InvalidOperationException("Transaction failed, cannot be reversed.");
            }
        }
  
    }

}

