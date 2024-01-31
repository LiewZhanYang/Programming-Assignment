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
    abstract class IceCream
    {
        public string Option { get; set; }
        public int Scoops { get; set; }
        public List<Flavour> Flavours { get; set; }
        public List<Topping> Toppings { get; set; }

        public IceCream()
        {
            
        }

        public IceCream(string option,int scoops, 
            List<Flavour> flavours,List<Topping> toppings)
        {
           Option = option;
            Scoops = scoops;
            Flavours = flavours ?? new List<Flavour>();
            Toppings = toppings ?? new List<Topping>();

        }

        public abstract double CalculatePrice(Dictionary<string, double> optionsDict,
                                      List<Flavour> allFlavours,
                                      List<Topping> allToppings);

        public double CalculateFlavoursPrice(List<Flavour> allFlavours)
        {
            double price = 0;
            foreach (var flavour in Flavours)
            {
                var matchingFlavour = allFlavours.FirstOrDefault(f => f.Type == flavour.Type);
                if (matchingFlavour != null && matchingFlavour.Premium)
                {
                    price += 2.0 * flavour.Quantity; // Assuming premium flavours add $2 per quantity
                }
            }
            return price;
        }

        public double CalculateToppingsPrice(List<Topping> allToppings)
        {
            // Assuming each topping adds a fixed cost (e.g., $1)
            return Toppings.Count * 1.0;
        }





        public override string ToString()
        {
            return ("Option: " + Option + "\tScoops: " + Scoops);
        
        }
    }
}
