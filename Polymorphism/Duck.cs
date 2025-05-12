using System;
namespace Polymorphism
{
	public class Duck : Bird
	{
		public double size { get; set; }
		public String kind { get; set; }

		public Duck()
		{
		}

        public override string ToString()
        {
			return "A duck named " + base.name + " is a " + size + " inch " + kind;
        }


    }
}

