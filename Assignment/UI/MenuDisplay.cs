//==========================================================
// Student Number : S10259432
// Student Name : Liew Zhan Yang
// Partner Number : S10257777
// Partner Name : Amicus Lee Ming Ge
//==========================================================

using System;

namespace Assignment
{
    public class MenuDisplay
    {
        public void DisplayMainMenu()
        {
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine("        ICE CREAM SHOP MANAGEMENT SYSTEM");
            Console.WriteLine(new string('=', 50));
            Console.WriteLine();
            Console.WriteLine("1. List All Customers");
            Console.WriteLine("2. List All Current Orders");
            Console.WriteLine("3. Register A New Customer");
            Console.WriteLine("4. Create A Customer's Order");
            Console.WriteLine("5. Display Order Details Of A Customer");
            Console.WriteLine("6. Modify Order Details");
            Console.WriteLine("7. Process Order And Checkout");
            Console.WriteLine("8. Display Monthly Charged Amounts Breakdown");
            Console.WriteLine("0. EXIT");
            Console.WriteLine();
            Console.WriteLine(new string('-', 50));
        }

        public void DisplayWelcomeMessage()
        {
            Console.WriteLine(new string('*', 60));
            Console.WriteLine("*" + new string(' ', 58) + "*");
            Console.WriteLine("*        WELCOME TO ICE CREAM SHOP MANAGEMENT        *");
            Console.WriteLine("*" + new string(' ', 58) + "*");
            Console.WriteLine(new string('*', 60));
            Console.WriteLine();
        }

        public void DisplayGoodbyeMessage()
        {
            Console.WriteLine();
            Console.WriteLine(new string('*', 60));
            Console.WriteLine("*" + new string(' ', 58) + "*");
            Console.WriteLine("*          THANK YOU FOR USING OUR SYSTEM!          *");
            Console.WriteLine("*               HAVE A GREAT DAY!                   *");
            Console.WriteLine("*" + new string(' ', 58) + "*");
            Console.WriteLine(new string('*', 60));
        }

        public void DisplayErrorMessage(string message)
        {
            Console.WriteLine();
            Console.WriteLine("❌ ERROR: " + message);
            Console.WriteLine();
        }

        public void DisplaySuccessMessage(string message)
        {
            Console.WriteLine();
            Console.WriteLine("✅ SUCCESS: " + message);
            Console.WriteLine();
        }

        public void DisplayInfoMessage(string message)
        {
            Console.WriteLine();
            Console.WriteLine("ℹ️  INFO: " + message);
            Console.WriteLine();
        }
    }
}