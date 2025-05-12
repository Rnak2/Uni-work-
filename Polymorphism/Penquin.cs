using System;
namespace Polymorphism
{
	public class Penquin : Bird 
	{
		public Penquin()
		{
		}

        public override void fly()
        {
            Console.WriteLine("Penquins cannot fly");
        }

        public override string ToString()
        {
            return "A penquin named " + base.name;
        }
    }
}

