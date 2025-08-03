using System;

namespace Assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
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