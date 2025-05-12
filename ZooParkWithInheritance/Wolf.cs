using System;
namespace ZooParkWithInheritance
{
    class Wolf : Animal
    {

        public Wolf(String name, String diet, String location, double weight,
             int age, String colour)
            : base(name, diet, location, weight, age, colour)
        {

        }

        public override void makeNoise()
        {
            Console.WriteLine("WOOOOOOOOORRRR");
        }

        public override void eat()
        {
            Console.WriteLine("A Wolf eat 10lbs of meat");
        }

        public override void interactWithVisitors()
        {
            Console.WriteLine("The wolf howls, catching the attention of visitors");
        }


    }

}

