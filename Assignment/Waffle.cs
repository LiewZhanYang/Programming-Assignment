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
using static System.Formats.Asn1.AsnWriter;

namespace Assignment
{
    class Waffle : IceCream
    {
        public string WaffleFlavour { get; set; }

        double price = 3;


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
            string key = $"Waffle-{Scoops}--{WaffleFlavour}"; // Construct the key based on waffle properties

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
