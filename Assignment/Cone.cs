//==========================================================
// Student Number : S10259432
// Student Name : Liew Zhan Yang
// Partner Number : S10257777
// Partner Name : Amicus Lee Ming Ge
//==========================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Assignment
{
    class Cone:IceCream
    {
        public bool Dipped { get; set; }

        double price = 0;
        private const double ChocolateDippedPrice = 2.0;

        public Cone()
        {
            
        }

        public Cone(int scoops, List<Flavour> flavours, List<Topping> toppings,bool dipped) : base("Cone", scoops, flavours, toppings)
        {
            Dipped = dipped;
        }

        public override double CalculatePrice(Dictionary<string, double> optionsDict,
                                          List<Flavour> allFlavours,
                                          List<Topping> allToppings)
        {
            double price = 0;
            string dippedKeyPart = Dipped ? "TRUE" : "FALSE";
            string key = $"Cone-{Scoops}-{dippedKeyPart}--"; // Construct the key based on cone properties

            if (optionsDict.TryGetValue(key, out double basePrice))
            {
                price += basePrice;
            }

            price += CalculateFlavoursPrice(allFlavours) + CalculateToppingsPrice(allToppings);

            return price;


        }

        public override string ToString()
        {
            return base.ToString() + ("\tDipped: " + Dipped );
        }
    }
}
