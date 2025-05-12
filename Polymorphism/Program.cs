namespace Polymorphism;

class Program
{
    static void Main(string[] args)
    {

        //bird object
        Bird bird1 = new Bird();
        bird1.name = "Feathers";
        Bird bird2 = new Bird();
        bird2.name = "Polly";

        //test bird
        Console.WriteLine(bird1.ToString());
        bird1.fly();
        Console.WriteLine(bird2.ToString());
        bird2.fly();
        Console.ReadLine();

        //penquin object
        Penquin penquin1 = new Penquin();
        penquin1.name = "Happy Feet";
        Penquin penquin2 = new Penquin();
        penquin2.name = "Gloria";

        //test penquin 
        Console.WriteLine(penquin1.ToString());
        penquin1.fly();
        Console.WriteLine(penquin2.ToString());
        penquin2.fly();
        Console.ReadLine();

        //duck object
        Duck duck1 = new Duck();
        duck1.name = "Daffy";
        duck1.size = 15;
        duck1.kind = "Mallard";
        Duck duck2 = new Duck();
        duck2.name = "Donald";
        duck2.size = 20;
        duck2.kind = "Decoy";

        //test duck 
        Console.WriteLine(duck1.ToString());
        Console.WriteLine(duck2.ToString());

        //create list of duck
        List<Duck> ducksToAdd = new List<Duck>()
        {
            duck1,
            duck2,
        };
        IEnumerable<Bird> upcastDucks = ducksToAdd;

        // create birds list 
        List<Bird> birds = new List<Bird>();

        //add to list 
        birds.Add(bird1);
        birds.Add(bird2);
        birds.Add(penquin1);
        birds.Add(penquin2);
        //birds.Add(duck1);
        //birds.Add(duck2);
        birds.Add(new Bird { name = "Birdy" });
        birds.Add(new Bird() { name = "Feather" });
        //add range 
        birds.AddRange(upcastDucks);
        Console.WriteLine();

        //print list 
        foreach (Bird bird in birds)
        {
            Console.WriteLine(bird);
        }
        Console.ReadLine();

 
    }
}

