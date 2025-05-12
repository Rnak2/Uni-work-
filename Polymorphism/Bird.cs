using System;
namespace Polymorphism
{
	public class Bird
	{
		public String name { get; set; }

		public Bird()
		{
		}

		public virtual void fly()
		{
			Console.WriteLine("Flap, Flap, Flap");
		}

		public override String ToString()
		{
			return "A bird called " + name;
		}

	
	}
}
	
