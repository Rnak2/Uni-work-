using System;
namespace Validating_Accounts
{
    public class TransferTransaction : Transaction
    {
        private Account _fromAccount;
        private Account _toAccount;
        //private decimal _amount;
        private WithdrawTransaction _withdraw;
        private DepositTransaction _deposit;
        //private bool _executed = false;
        //private bool _reversed = false;

        // Read only property 
        //public bool Executed
        //{
        //    get { return _executed; }
        //}

        public new bool Success
        {
           get { return _withdraw.Success && _deposit.Success; }
        }

        //public bool Reversed
        //{
        //    get { return _reversed; }
        //}


        public TransferTransaction(Account fromAccount, Account toAccount, decimal amount) : base(amount)
        {
            _fromAccount = fromAccount;
            _toAccount = toAccount;
            _amount = amount;
            // Create the withdraw and deposit transactions (local variables)
            _withdraw = new WithdrawTransaction(_fromAccount, _amount);
            _deposit = new DepositTransaction(_toAccount, _amount);
        }

        public override void Print()
        {
            Console.WriteLine($"Transferred {_amount:c} from {_fromAccount.Name}'s account to {_toAccount.Name}'s account.");
            _withdraw.Print();
            _deposit.Print();
            base.Print();
        }

        public override void Execute()
        {
            base.Execute();

            try
            {
                _withdraw.Execute();
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine("Transfer failed: " + exception.Message);
                _withdraw.Print();
            }

            if (_withdraw.Success)
            {
                try
                {
                    _deposit.Execute();
                }
                catch (InvalidOperationException exception)
                {
                    Console.WriteLine("Transfer failed: " + exception.Message);
                    _deposit.Print();
                    try
                    {
                        _withdraw.Rollback();
                    }
                    catch (InvalidOperationException m)
                    {
                        Console.WriteLine("Withdraw cannot be reversed: " + m.Message);
                        _withdraw.Print();
                        return;
                    }
                }
            }
            _success = true; 
        }

        public override void Rollback()
        {
            base.Rollback();

            if (Success)
            {
                try
                {
                    _deposit.Rollback();
                }
                catch (InvalidOperationException exception)
                {
                    Console.WriteLine("Rollback failed" + exception.Message);
                    return;
                }
                try
                {
                    _withdraw.Rollback();
                }
                catch (InvalidOperationException exception)
                {
                    Console.WriteLine("Failed to rollback withdraw: "+ exception.Message);
                    return;
                }
            }
        }


    }

}