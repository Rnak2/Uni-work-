using System;
namespace Validating_Accounts
{
	public abstract class Transaction
	{
        protected decimal _amount;
        protected bool _success;
        protected bool _executed;
        protected bool _reversed;
        protected DateTime _dateStamp;

        //read only property 
        public virtual bool Success
        {
            get { return _success; }
            protected set { _success = value; }
        }
        public bool Executed
        {
            get { return _executed; }
        }
        public bool Reversed
        {
            get { return _reversed; }
        }
        public DateTime DateStamp
        {
            get { return _dateStamp; }
        }
        public decimal Amount
        {
            get { return _amount; }
        }

        //constructor
        public Transaction(decimal amount)
		{
            this._amount = amount;
            this._executed = false;
            this._reversed = false;
            this._success = false;
            this._dateStamp = DateTime.Now;
        }

        

        public virtual void Print()
        {
            Console.WriteLine($"Transaction executed on: {this._dateStamp}");
            Console.WriteLine($"Amount: {this._amount:c}");
            Console.WriteLine($"Executed: {this._executed}, Success: {this._success}, Reversed: {this._reversed}");
        }

        public virtual void Execute()
        {
            if (_executed && _success)
            {
                throw new InvalidOperationException("Transaction has already been executed.");
            }
            _dateStamp = DateTime.Now;
            _executed = true;
        }

        public virtual void Rollback()
        {
            if (_reversed)
            {
                throw new InvalidOperationException("Transaction has already been reversed.");
            }
            else if (!_success)
            {
                throw new InvalidOperationException("Transaction has not been executed yet.");
            }
            _dateStamp = DateTime.Now;
            _reversed = true;
        }


    }
}

