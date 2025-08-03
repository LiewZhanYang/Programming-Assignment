//==========================================================
// Student Number : S10259432
// Student Name : Liew Zhan Yang
// Partner Number : S10257777
// Partner Name : Amicus Lee Ming Ge
//==========================================================

using System;

namespace Assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize the application
                var app = new IceCreamShopApp();
                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical error: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}