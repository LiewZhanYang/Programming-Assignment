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
    class Customer
    {
        public string Name { get; set; }
        public int MemberId { get; set; }
        public DateTime Dob { get; set; }
        public Order CurrentOrder { get; set; }
        public List<Order> OrderHistory { get; set; }
        public PointCard Rewards { get; set; }

        public Customer()
        {
            
        }

        public Customer(string name,int memberID,DateTime dob)
        {
            Name = name;
            MemberId = memberID;
            Dob = dob;
        }

        public Order MakeOrder()
        {
            CurrentOrder = new Order(MemberId,DateTime.Now);
            OrderHistory.Add(CurrentOrder);
            return CurrentOrder;
        }

        public bool IsBirthday()
        {
            return DateTime.Today.Month == Dob.Month && DateTime.Today.Day == Dob.Day;
        }


        public override string ToString()
        {
            return ("Name: " + Name + "MemberID: " + MemberId
                + "DOB: " + Dob + "Points: " + Rewards.Points);
        }
    }
}
