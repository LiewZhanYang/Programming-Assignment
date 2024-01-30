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
    class Order
    {
        public int Id { get; set; }
        public DateTime TimeReceived { get; set; }
        public DateTime? TimeFulfilled { get; set; }
        public List<IceCream> IceCreamList { get; set; } = new List<IceCream>();

        public Order()
        {
            
        }

        public Order(int id,DateTime timeReceived)
        {
            Id = id;
            TimeReceived = timeReceived;
        }

        public void ModifyIceCream(int id)
        {

        }

        public void AddiceCream(IceCream iceCream) 
        {
            IceCreamList.Add(iceCream);
        
        }

        public void DeleteIceCream(int index)
        {
            if(index >= 0 && index < IceCreamList.Count) 
            {
                IceCreamList.RemoveAt(index);
            
            }
            else
            {
                throw new Exception("The ice cream index is out of range.");


            }

        }

        public double CalculateTotal(PointCard pointCard, bool redeemPoints = false)
        {
            double total = IceCreamList.Sum(IceCream => IceCream.CalculatePrice());

            if(redeemPoints && (pointCard.Tier != "Ordinary"))
            {
                int pointsToRedeem = (int)(total / 0.02);
                if(pointCard.Points >= pointsToRedeem) 
                {
                    bool redeemed = pointCard.RedeemPoints(pointsToRedeem);
                    
                    if(redeemed) 
                    {
                        total -= pointsToRedeem * 0.02;



                    }
                
                }

            }
            
            return total;
        }


        private int CalculatePointsEarned(double total)
        {
            return (int)Math.Floor(total * 0.72);
        }



        public override string ToString() 
        {
            return ("Id: " + Id + "TimeReceived: " + TimeReceived);
              
        }
    }
}
