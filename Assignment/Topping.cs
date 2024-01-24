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
    class Topping
    {
        public string Type { get; set; }

        public Topping()
        {
            
        }

        public Topping(string type)
        {
            Type = type;
        }

        public override string ToString() 
        {
            return ("Type: " +  Type);


        }



    }
}
