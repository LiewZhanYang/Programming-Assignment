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

        public Waffle(string waffleFlavour,string option, int scoops, List<Flavour> flavours, List<Topping> toppings) : base(option, scoops, flavours, toppings)
        {
            WaffleFlavour = waffleFlavour;
        }

        public override double CalculatePrice()
        {
            if (Scoops == 1)
            {
                price += 4.0;

            }
            else if (Scoops == 2)
            {
                price += 5.5;
            }
            else if (Scoops == 3)
            {
                price += 6.5;
            }


            if (!string.IsNullOrEmpty(WaffleFlavour)) 
            {
                price += 3.0;
            
            }

            foreach (var flavour in Flavours)
            {
                if (flavour.Premium)
                {
                    price += 2.0 * flavour.Quantity;
                }
            }


            price += Toppings.Count * 1.0;

            return price;

        }

        public override string ToString()
        {
            return base.ToString() + ("\tWaffleFlavour: " + WaffleFlavour);
        }




    }
}
