using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    class Cup : IceCream
    {
        public Cup() 
        {
            
        }

        public Cup(string option,int scoops,List<Flavour>flavours,List<Topping>toppings) : base(option,scoops,flavours,toppings)
        {
            
        }

        public override double CalculatePrice()
        {
            double price = 4.0;
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
            return base.ToString();
        }
    }
}
