using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    class Waffle
    {
        public string WaffleFlavour { get; set; }

        public Waffle()
        {
            d
        }

        public Waffle(string waffleFlavour,string option, int scoops, List<Flavour> flavours, List<Topping> toppings) : base(option, scoops, flavours, toppings)
        {
            WaffleFlavour = waffleFlavour;
        }

        public override double CalculatePrice()
        {
            double price = 7.0;
            if(!string.IsNullOrEmpty(WaffleFlavour)) 
            {
                price += 3.0;
            
            }
            foreach(var flavour in Flavour)
            {
                price += flavour.Price();
            }
            foreach(var topping in Topping)
            {
                price += topping.Price();
            }



        }

        public override string ToString()
        {
            return base.ToString() + ("\tWaffleFlavour: " + WaffleFlavour);
        }




    }
}
