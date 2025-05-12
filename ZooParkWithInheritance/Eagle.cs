using System;
namespace ZooParkWithInheritance
{
    class Eagle : Bird
    {
       

        public Eagle(String name, String diet, String location, double weight,
             int age, String colour, String species, double wingSpan)
            : base(name, diet, location, weight, age, colour, species, wingSpan)
        {
        }

        public void layEgg()
        {
            Console.WriteLine("The eagle lays an egg");
        }
        public override void fly()
        {
            Console.WriteLine("The eagle fly high");
        }

        public override void makeNoise()
        {
            Console.WriteLine("EIKKKKKKKKK");
        }

        public override void eat()
        {
            Console.WriteLine("An Eagle eat 1lbs of fish");
        }

        public override void interactWithVisitors()
        {
            Console.WriteLine("The eagle spreads its wings as visitors approach");
        }

    }
}

