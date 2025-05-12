using System;
using ZooParkWithInheritance;

namespace ZooParkWithInheritance
{
    public class Bird : Animal
    {
        public double WingSpan;
        public String species;

        public Bird(String name, String diet, String location, double weight, int age,
            String colour, String species, double wingSpan)
            : base(name, diet, location, weight, age, colour)
        {
            this.WingSpan = wingSpan;
            this.species = species;
        }

        public virtual void fly()
        {
            Console.WriteLine("This bird flies high");
        }

        public override void makeNoise()
        {
            Console.WriteLine("The bird make noise");
        }
    }
}

