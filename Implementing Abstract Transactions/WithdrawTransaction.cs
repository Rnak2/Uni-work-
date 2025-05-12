using System;
namespace Validating_Accounts
{
    public class WithdrawTransaction : Transaction
    {
        private Account _account;
        //private decimal _amount;
        //private bool _executed = false;
        //private bool _success = false;
        //private bool _reversed = false;

        //// Read only property 
        //public bool Executed
        //{
        //    get { return _executed; }
        //}

        //public bool Success
        //{
        //    get { return _success; }
        //}

        //public bool Reversed
        //{
        //    get { return _reversed; }
        //}

        public WithdrawTransaction(Account account, decimal amount) : base(amount)
        {
            _account = account;
            _amount = amount;
        }


        // Method to print transaction details
        public override void Print()
        {
            Console.WriteLine("Withdraw Transaction Details:");
            Console.WriteLine($"Attempted to Withdraw From Account {_account.Name}");
            base.Print();

        }


        // Method to execute the withdrawal
        public override void Execute()
        {
            base.Execute();
            _success = _account.Withdraw(_amount);
            if (_success)
            {
                Console.WriteLine("Withdrawal successful: " + _amount.ToString("c"));
            }
            else
            {
                _success = false;  
                                   
            }
        }

        public override void Rollback()
        {
            base.Rollback();
            if (_account.Deposit(_amount))
            {
                _reversed = true;
            }
            else
            {
                throw new InvalidOperationException("Invalid Amount.");
            }
        }

        

    }
}

