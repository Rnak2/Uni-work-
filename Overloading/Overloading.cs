namespace Overloading;

class Program
{
    static void Main(string[] args)
    {
        methodToBeOverloaded("Bob");
        methodToBeOverloaded("Jack", 30);

    }
    public static void methodToBeOverloaded(String name)
    {
        Console.WriteLine("Name: " + name);
    }

    public static void methodToBeOverloaded(String name, int age)
    {
        Console.WriteLine("Name: " + name + "\nAge:" + age);
    }

}

