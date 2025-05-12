using System;
namespace Validating_Accounts
{
    public class Account
    {
        // Instance Variables
        private decimal _balance;
        private string _name;

        // Constructor
        public Account(string name, decimal balance)
        {
            this._name = name;
            this._balance = balance;
        }

        //Mutator Methods
        public bool Deposit(decimal amount)
        {
            if (amount > 0)
            {
                this._balance += amount;
                return true; 
            }
            else
            {
                Console.WriteLine("Invalid deposit amount.");
                return false; 
            }
        }

        public bool Withdraw(decimal amount)
        {
            if (amount > 0)
            {
                if (amount <= this._balance)
                {
                    this._balance -= amount;
                    return true;
                }
                else
                {
                    Console.WriteLine("Insufficient funds.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Invalid withdrawal amount.");
                return false;
            }

            
        }

        public void Print()
        {
            Console.WriteLine("Account Name: " + this._name);
            Console.WriteLine("Account Balance: " + this._balance.ToString("c"));
        }


        //Property  
        public string Name
        {
            get { return this._name; }
        }

       
    }
}

