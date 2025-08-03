using System;
using System.Collections.Generic;

namespace Assignment
{
    public class Cup : IceCream  // Added 'public'
    {
        public Cup()
        {

        }

        public Cup(int scoops, List<Flavour> flavours, List<Topping> toppings) : base("Cup", scoops, flavours, toppings)
        {

        }

        public override double CalculatePrice(Dictionary<string, double> optionsDict,
                                      List<Flavour> allFlavours,
                                      List<Topping> allToppings)
        {
            double price = 0;
            string key = $"Cup-{Scoops}---";

            if (optionsDict.TryGetValue(key, out double basePrice))
            {
                price += basePrice;
            }

            price += CalculateFlavoursPrice(allFlavours);
            price += CalculateToppingsPrice(allToppings);

            return price;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
