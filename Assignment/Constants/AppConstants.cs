//==========================================================
// Student Number : S10259432
// Student Name : Liew Zhan Yang
// Partner Number : S10257777
// Partner Name : Amicus Lee Ming Ge
//==========================================================

namespace Assignment
{
    public static class AppConstants
    {
        // File paths
        public const string CUSTOMERS_FILE = "customers.csv";
        public const string FLAVOURS_FILE = "flavours.csv";
        public const string OPTIONS_FILE = "options.csv";
        public const string TOPPINGS_FILE = "toppings.csv";
        public const string ORDERS_FILE = "orders.csv";

        // Pricing constants
        public const double PREMIUM_FLAVOUR_COST = 2.0;
        public const double TOPPING_COST = 1.0;
        public const double POINTS_TO_DOLLAR_RATIO = 0.02;
        public const int POINTS_PER_DOLLAR = 10;

        // Membership thresholds
        public const int SILVER_POINTS_THRESHOLD = 50;
        public const int GOLD_POINTS_THRESHOLD = 100;
        public const int PUNCH_CARD_LIMIT = 10;

        // Validation limits
        public const int MIN_SCOOPS = 1;
        public const int MAX_SCOOPS = 3;
        public const int MIN_MEMBER_ID = 1;
        public const int MAX_AGE_YEARS = 150;

        // Display formatting
        public const int DISPLAY_WIDTH = 50;
        public const string DATE_FORMAT = "dd/MM/yyyy";

        // Membership tiers
        public const string TIER_ORDINARY = "Ordinary";
        public const string TIER_SILVER = "Silver";
        public const string TIER_GOLD = "Gold";

        // Ice cream options
        public static readonly string[] VALID_ICE_CREAM_OPTIONS = { "Cup", "Cone", "Waffle" };
        public static readonly string[] VALID_WAFFLE_FLAVOURS = { "Original", "Red Velvet", "Charcoal", "Pandan" };
    }
}