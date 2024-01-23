using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    class Waffle
    {
        public string waffleFlavour { get; set; }

        public Waffle()
        {

        }

        public Waffle(string waffleFlavour,string option, int scoops, List<Flavour> flavours, List<Topping> toppings) : base(option, scoops, flavours, toppings)
        {
            this.waffleFlavour = waffleFlavour;
        }

        public double CalculatePrice()
        {
            return
        }

        public override string ToString()
        {
            return base.ToString() + "waffleFlavour: {waffleFlavour}";
        }




    }
}
