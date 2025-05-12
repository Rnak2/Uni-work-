using System;
namespace ZooParkWithInheritance
{
	public class Penquin
	{
        public class Penguin : Bird
        {
            public Penguin(String name, String diet, String location, double weight,
                int age, String colour, String species, double wingSpan)
                : base(name, diet, location, weight, age, colour, species, wingSpan)
            {
            }

            public override void fly()
            {
                Console.WriteLine("Penquins cannot fly");
            }

            public void swim()
            {
                Console.WriteLine("The penguin swims in the water");
            }

            public override void interactWithVisitors()
            {
                Console.WriteLine("The penquin flaps its wings when at the visitors");
            }
        }
    }
}

