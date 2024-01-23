﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    class Flavour
    {
        public string type{ get; set; }
        public bool premium { get; set; }
        public int quantity { get; set; }


        public Flavour()
        {
            
        }

        public Flavour(string type,bool premium,int quantity)
        {
            this.type = type;
            this.premium = premium;
            this.quantity = quantity;
            
        }

        public override string ToString() 
        {
            return $"Type : {type}  Premium : {premium}  Quantity : {quantity}"
        
        }

    }
}