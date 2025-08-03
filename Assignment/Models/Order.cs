using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment
{
    public class Order  // Added 'public'
    {
        public int Id { get; set; }
        public DateTime TimeReceived { get; set; }
        public DateTime? TimeFulfilled { get; set; }
        public List<IceCream> IceCreamList { get; set; } = new List<IceCream>();

        public Order()
        {

        }

        public Order(int id, DateTime timeReceived)
        {
            Id = id;
            TimeReceived = timeReceived;
        }

        public void ModifyIceCream(int id)
        {
            // Implementation can be added later if needed
        }

        public void AddiceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);
        }

        public void DeleteIceCream(int index)
        {
            if (index >= 0 && index < IceCreamList.Count)
            {
                IceCreamList.RemoveAt(index);
            }
            else
            {
                throw new Exception("The ice cream index is out of range.");
            }
        }

        public double CalculateTotal(Dictionary<string, double> optionsDict, List<Flavour> allFlavours, List<Topping> allToppings, PointCard pointCard, bool redeemPoints = false)
        {
            double total = 0;

            foreach (var iceCream in IceCreamList)
            {
                total += iceCream.CalculatePrice(optionsDict, allFlavours, allToppings);
            }

            if (pointCard != null && redeemPoints && pointCard.Tier != "Ordinary")
            {
                int pointsToRedeem = (int)(total / 0.02);
                if (pointCard.Points >= pointsToRedeem)
                {
                    bool redeemed = pointCard.RedeemPoints(pointsToRedeem);
                    if (redeemed)
                    {
                        total -= pointsToRedeem * 0.02;
                    }
                }
            }

            return total;
        }

        public override string ToString()
        {
            return ("Id: " + Id + " TimeReceived: " + TimeReceived);
        }
    }
}