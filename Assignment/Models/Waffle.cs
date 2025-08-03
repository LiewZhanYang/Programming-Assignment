using System;
using System.Collections.Generic;

namespace Assignment
{
    public class Waffle : IceCream  // Added 'public'
    {
        public string WaffleFlavour { get; set; }

        public Waffle()
        {

        }

        public Waffle(int scoops, List<Flavour> flavours, List<Topping> toppings, string waffleFlavour) : base("Waffle", scoops, flavours, toppings)
        {
            WaffleFlavour = waffleFlavour;
        }

        public override double CalculatePrice(Dictionary<string, double> optionsDict,
                                          List<Flavour> allFlavours,
                                          List<Topping> allToppings)
        {
            double price = 0;
            string key = $"Waffle-{Scoops}--{WaffleFlavour}";

            if (optionsDict.TryGetValue(key, out double basePrice))
            {
                price += basePrice;
            }

            price += CalculateFlavoursPrice(allFlavours) + CalculateToppingsPrice(allToppings);

            return price;
        }

        public override string ToString()
        {
            return base.ToString() + ("\tWaffleFlavour: " + WaffleFlavour);
        }
    }
}