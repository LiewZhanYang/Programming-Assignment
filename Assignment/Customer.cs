using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    class Customer
    {
        public string name { get; set; }
        public int memberId { get; set; }
        public DateTime dob { get; set; }
        public Order currentOrder { get; set; }
        public List<Order> orderHistory { get; set; }
        public PointCard rewards { get; set; }

        public Customer()
        {
            
        }

        public Customer(string name,int memberID,DateTime dob)
        {
            this.name = name;
            this.memberId = memberID;
            this.dob = dob;
        }

        public Order MakeOrder()
        {
            return
        }

        public bool IsBirthday()
        {
            return
        }

        public override string ToString()
        {
            return 
        }
    }
}
