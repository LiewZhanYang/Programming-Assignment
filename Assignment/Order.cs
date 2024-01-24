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
        public List<IceCream> IceCreamList { get; set; }

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

        public double CalculateTotal(bool redeemPoints = false)
        {
            double total = 0;
            foreach(var icecream in IceCreamList)
            {
                total += icecream.CalculatePrice();
            }

            if(redeemPoints && (PointCard.Tier != "Ordinary")
            {
                int pointsToRedeem = (int)(total / 0.02);
                if(PointCard.Points >= pointsToRedeem) 
                {
                    bool redeemed = 
                
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
