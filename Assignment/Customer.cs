using System;
using System.Collections.Generic;

namespace Assignment
{
    public class Customer  // Added 'public'
    {
        public string Name { get; set; }
        public int MemberId { get; set; }
        public DateTime Dob { get; set; }
        public Order CurrentOrder { get; set; }
        public List<Order> OrderHistory { get; set; }
        public PointCard Rewards { get; set; }

        public Customer()
        {
            OrderHistory = new List<Order>();
        }

        public Customer(string name, int memberID, DateTime dob)
        {
            Name = name;
            MemberId = memberID;
            Dob = dob;
            OrderHistory = new List<Order>();
        }

        public Order MakeOrder()
        {
            CurrentOrder = new Order(MemberId, DateTime.Now);
            return CurrentOrder;
        }

        public bool IsBirthday()
        {
            return DateTime.Today.Month == Dob.Month && DateTime.Today.Day == Dob.Day;
        }

        public override string ToString()
        {
            return $"Name: {Name}, MemberID: {MemberId}, DOB: {Dob:dd/MM/yyyy}, Points: {Rewards?.Points ?? 0}";
        }
    }
}