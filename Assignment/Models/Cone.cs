using System;
using System.Collections.Generic;

namespace Assignment
{
    public class Cone : IceCream  // Added 'public'
    {
        public bool Dipped { get; set; }

        public Cone()
        {

        }

        public Cone(int scoops, List<Flavour> flavours, List<Topping> toppings, bool dipped) : base("Cone", scoops, flavours, toppings)
        {
            Dipped = dipped;
        }

        public override double CalculatePrice(Dictionary<string, double> optionsDict,
                                          List<Flavour> allFlavours,
                                          List<Topping> allToppings)
        {
            double price = 0;
            string dippedKeyPart = Dipped ? "TRUE" : "FALSE";
            string key = $"Cone-{Scoops}-{dippedKeyPart}--";

            if (optionsDict.TryGetValue(key, out double basePrice))
            {
                price += basePrice;
            }

            price += CalculateFlavoursPrice(allFlavours) + CalculateToppingsPrice(allToppings);

            return price;
        }

        public override string ToString()
        {
            return base.ToString() + ("\tDipped: " + Dipped);
        }
    }
}