using System;
namespace ZooParkWithInheritance
{
	class Tiger : Feline
	{
		private string colourStripes;

		public Tiger(String name, String diet, String location, double weight,
			 int age, String colour, String species, String colourStripes)
			: base(name, diet, location, weight, age, colour, species)
		{
			this.colourStripes = colourStripes; 
		}

        public override void makeNoise()
        {
			Console.WriteLine("ROARRRRRRRR");
        }

		public override void eat()
		{
			Console.WriteLine("A Tiger eat 20lbs of meat");
		}

        public override void interactWithVisitors()
        {
            Console.WriteLine("The tiger growls, catching the attention of visitors");
        }


    }
}

