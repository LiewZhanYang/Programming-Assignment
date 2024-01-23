using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    class Cone:IceCream
    {
        public bool dipped { get; set; }

        public Cone()
        {
            
        }

        public Cone(bool dipped,string option, int scoops, List<Flavour> flavours, List<Topping> toppings) : base(option, scoops, flavours, toppings)
        {
            this.dipped = dipped;
        }

        public double CalculatePrice()
        {
            return
        }

        public override string ToString()
        {
            return base.ToString() + $"\tDipped: {dipped}";
        }
    }
}
