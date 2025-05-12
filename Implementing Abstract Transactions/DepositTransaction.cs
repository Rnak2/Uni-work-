using System;
namespace Validating_Accounts
{
    public class DepositTransaction : Transaction
    {
        private Account _account;
        //private decimal _amount;
        //private bool _executed = false;
        //private bool _success = false;
        //private bool _reversed = false;

        // Read only property 
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

        public DepositTransaction(Account account, decimal amount) : base(amount)
        {
            _account = account;
            //_amount = amount;
        }

        public override void Print()
        {
            Console.WriteLine("Deposit Transaction Details:");
            Console.WriteLine($"Amount Deposited From Account {_account.Name}");
            base.Print();
        }


        public override void Execute()
        {
            base.Execute();

            if (_amount <= 0)
            {
                _success = false;
                throw new InvalidOperationException("Invalid deposit amount. Amount must be greater than zero.");
            }
            
            //_account.Deposit(_amount);
            _success = _account.Deposit(_amount);
        }

        public override void Rollback()
        {
            base.Rollback();
            bool withdrawSuccess = _account.Withdraw(_amount);
            if (!withdrawSuccess)
            {
                throw new InvalidOperationException("Unable to reverse the deposit due to insufficient funds in the account.");
            }
            _reversed = true;

         
        }
  
    }

}

