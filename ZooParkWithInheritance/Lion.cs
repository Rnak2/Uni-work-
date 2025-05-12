using System;
namespace ZooParkWithInheritance
{
    public class Lion : Feline
    {
        public Lion(String name, String diet, String location, double weight, int age,
            string colour, string species)
            : base(name, diet, location, weight, age, colour, species)
        {
        }

        public override void makeNoise()
        {
            Console.WriteLine("The lion Grooaarrrrrr");
        }

        public override void interactWithVisitors()
        {
            Console.WriteLine("The lion stares and roar at the visitors");
        }

        public override void eat()
        {
            Console.WriteLine("A Lion eat 25lbs of meat");
        }

    }
}

