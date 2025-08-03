using System;

namespace Assignment
{
    public class Topping  // Added 'public'
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
            return ("Type: " + Type);
        }
    }
}