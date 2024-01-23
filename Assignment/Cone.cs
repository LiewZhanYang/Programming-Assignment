using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    class Cone:IceCream
    {
        public bool Dipped { get; set; }

        public Cone()
        {
            
        }

        public Cone(bool dipped,string option, int scoops, List<Flavour> flavours, List<Topping> toppings) : base(option, scoops, flavours, toppings)
        {
            Dipped = dipped;
        }

        public override double CalculatePrice()
        {
            double price = 4.0;
            if (Dipped) 
            {
                price += 2.0;
            
            }
            foreach(var flavour in Flavour)
            {
                price += flavour.Price();
            }
            foreach(var topping in Topping)
            {
                price += topping.Price();
            }
            return price;





        }

        public override string ToString()
        {
            return base.ToString() + ("\tDipped: " + Dipped );
        }
    }
}
