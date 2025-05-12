using static ZooParkWithInheritance.Penquin;

namespace ZooParkWithInheritance;

class ZooPark
{
    static void Main(string[] args)
    {
        //Animal williamWolf = new Animal("William the Wolf", "Meat", "Dog Village", 50.6, 9, "Grey");
        //Animal tonyTiger = new Animal("Tony the Tiger", "Meat", "Cat Land", 110, 6, "Orange and White");
        //Animal edgarEagle = new Animal("Edgar the Eagle", "Fish", "Bird Mania", 20, 15, "Black");
        Tiger tonyTiger = new Tiger("Tony the Tiger", "Meat", "Cat Land", 110, 6, "Orange and White", "Siberian", "White");
        Wolf williamWolf = new Wolf("William the Wolf", "Meat", "Dog Village", 50.6, 9, "Grey");
        Eagle edgarEagle = new Eagle("Edgar the Eagle", "Fish", "Bird Mania", 20, 15, "Black","Harpy", 98.5);

        tonyTiger.makeNoise();

        Animal baseAnimal = new Animal(" Animal name", "Animal diet", "Animal Location", 0.0, 0, "Animal Colour");
        baseAnimal.makeNoise();

        Console.ReadLine();

        //eat
        tonyTiger.eat();
        williamWolf.eat();
        edgarEagle.eat();
        Console.ReadLine();

        //interact with visitors
        tonyTiger.interactWithVisitors();
        williamWolf.interactWithVisitors();
        edgarEagle.interactWithVisitors();
        Console.ReadLine();

        //eagle fly and lay egg
        edgarEagle.fly();
        edgarEagle.layEgg();
        Console.ReadLine();

       
        baseAnimal.sleep();
        tonyTiger.sleep();
        williamWolf.sleep();
        edgarEagle.sleep();
        tonyTiger.eat();
        Console.ReadLine();

        //test new animal
        Lion simbaLion = new Lion("Simba", "Meat", "Savannah Zone", 192, 5, "Golden", "African Lion");
        simbaLion.eat();
        simbaLion.makeNoise();
        simbaLion.interactWithVisitors();

        Penguin hardyPenquin = new Penguin("Happy Feet", "Fish", "Arctic Zone", 25, 10, "Black and White", "Snow", 2.3);
        hardyPenquin.fly();
        hardyPenquin.swim();
        hardyPenquin.interactWithVisitors();
        hardyPenquin.makeNoise();
        Console.ReadLine();



    }

}






