using System;

namespace Assignment
{
    public class PointCard  // Added 'public'
    {
        public int Points { get; set; }
        public int PunchCard { get; set; }
        public string Tier { get; set; }

        public PointCard()
        {
            Tier = "Ordinary";
        }

        public PointCard(int points, int punchCard)
        {
            Points = points;
            PunchCard = punchCard;
            Tier = "Ordinary";
        }

        public void AddPoints(int pointsToAdd)
        {
            Points += pointsToAdd;

            if (Points >= 100 && Tier != "Gold")
            {
                Tier = "Gold";
            }
            else if (Points >= 50 && Tier != "Silver")
            {
                Tier = "Silver";
            }
        }

        public bool RedeemPoints(int pointsToRedeem)
        {
            if (Tier != "Ordinary" && Points >= pointsToRedeem)
            {
                Points -= pointsToRedeem;
                return true;
            }
            return false;
        }

        public void Punch()
        {
            PunchCard++;
            if (PunchCard >= 10)
            {
                PunchCard = 0;
            }
        }

        public override string ToString()
        {
            return ("Points: " + Points + "\tPunchCard: " + PunchCard + "\tTier: " + Tier);
        }
    }
}