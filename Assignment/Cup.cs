//==========================================================
// Student Number : S10259432
// Student Name : Liew Zhan Yang
// Partner Number : S10257777
// Partner Name : Amicus Lee Ming Ge
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    class Cup : IceCream
    {

        
        double price = 0;

        public Cup() 
        {
            
        }

        public Cup(int scoops,List<Flavour>flavours,List<Topping>toppings) : base("Cup",scoops,flavours,toppings)
        {
            
        }

        public override double CalculatePrice(Dictionary<string, double> optionsDict,
                                      List<Flavour> allFlavours,
                                      List<Topping> allToppings)
        {
            double price = 0;
            string key = $"Cup-{Scoops}---"; // Construct the key based on cup properties

            // Fetch the base price for the cup from the options dictionary
            if (optionsDict.TryGetValue(key, out double basePrice))
            {
                price += basePrice;
            }

            // Add the price for premium flavours
            price += CalculateFlavoursPrice(allFlavours);

            // Add price for toppings
            price += CalculateToppingsPrice(allToppings);

            return price;



        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
