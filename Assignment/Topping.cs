using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    class Topping
    {
        public string type { get; set; }

        public Topping()
        {
            
        }

        public Topping(string type)
        {
            this.type = type;
        }

        public override string ToString() 
        {
            return $"Type : {type}";


        }



    }
}
