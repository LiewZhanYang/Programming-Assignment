using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    class Flavour
    {
        public string Type{ get; set; }
        public bool Premium { get; set; }
        public int Quantity { get; set; }


        public Flavour()
        {
            
        }

        public Flavour(string type,bool premium,int quantity)
        {
            Type = type;
            Premium = premium;
            Quantity = quantity;
        }

        public double Price()
        {
            double basePrice = 2.0;
        }


        public override string ToString() 
        {
            return $"Type : {type}  Premium : {premium}  Quantity : {quantity}"
        
        }

    }
}
