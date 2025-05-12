using System;
namespace Validating_Accounts
{
    public class TransferTransaction
    {
        private Account _fromAccount;
        private Account _toAccount;
        private decimal _amount;
        private WithdrawTransaction _withdraw;
        private DepositTransaction _deposit;
        private bool _executed = false;
        private bool _reversed = false;

        // Read only property 
        public bool Executed
        {
            get { return _executed; }
        }

        public bool Success
        {
            get { return _withdraw.Success && _deposit.Success; }
        }

        public bool Reversed
        {
            get { return _reversed; }
        }


        public TransferTransaction(Account fromAccount, Account toAccount, decimal amount)
        {
            _fromAccount = fromAccount;
            _toAccount = toAccount;
            _amount = amount;
            // Create the withdraw and deposit transactions (local variables)
            _withdraw = new WithdrawTransaction(_fromAccount, _amount);
            _deposit = new DepositTransaction(_toAccount, _amount);
        }

        public void Print()
        {
            Console.WriteLine($"Transferred {_amount:c} from {_fromAccount.Name}'s account to {_toAccount.Name}'s account.");
            _withdraw.Print();
            _deposit.Print();
        }

        public void Execute()
        {
            if (_executed)
            {
                throw new InvalidOperationException("Transfer has already been executed.");
            }

            try
            {
                _withdraw.Execute();
                if (!_withdraw.Success)
                {
                    throw new InvalidOperationException("Withdrawal process of the transfer failed. Transaction cannot be executed.");
                }

                _deposit.Execute();
                if (!_deposit.Success)
                {
                    _withdraw.Rollback();
                    throw new InvalidOperationException("Deposit process of the transfer failed. Withdrawal has been reversed.");
                }

                _executed = true;
            }
            catch
            {
               
                throw; //Throw any exception caught by the catch block.
            }
        }

        public void Rollback()
        {
            if (!_executed)
            {
                throw new InvalidOperationException("Transfer has not been executed and cannot be rolled back.");
            }

            if (_reversed)
            {
                throw new InvalidOperationException("Transfer has already been reversed.");
            }
            if (!Success)
            {
                throw new InvalidOperationException("Transfer transaction was not successful and cannot be rolled back.");
            }

            try
            {
                _deposit.Rollback();
            }
            catch (InvalidOperationException m)
            {
                throw new InvalidOperationException(m.Message);
            }

            try
            {
                _withdraw.Rollback();
            }
            catch (InvalidOperationException m)
            {
                
                throw new InvalidOperationException(m.Message);
            }
            _reversed = true;
        }



    }

}