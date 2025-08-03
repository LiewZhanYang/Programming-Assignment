//==========================================================
// Student Number : S10259432
// Student Name : Liew Zhan Yang
// Partner Number : S10257777
// Partner Name : Amicus Lee Ming Ge
//==========================================================

using System;
using System.Collections.Generic;

namespace Assignment
{
    public class IceCreamShopApp
    {
        private readonly DataManager dataManager;
        private readonly CustomerService customerService;
        private readonly OrderService orderService;
        private readonly MenuDisplay menuDisplay;

        public IceCreamShopApp()
        {
            // Initialize data manager and load all data
            dataManager = new DataManager();

            // Initialize services with loaded data
            customerService = new CustomerService(dataManager);
            orderService = new OrderService(dataManager);
            menuDisplay = new MenuDisplay();
        }

        public void Run()
        {
            Console.WriteLine("Welcome to Ice Cream Shop Management System!");
            Console.WriteLine("============================================");

            while (true)
            {
                try
                {
                    menuDisplay.DisplayMainMenu();
                    Console.Write("What is your option: ");

                    if (!int.TryParse(Console.ReadLine(), out int option))
                    {
                        Console.WriteLine("Please enter a valid number.");
                        continue;
                    }

                    if (option == 0)
                    {
                        Console.WriteLine("Thank you for using Ice Cream Shop Management System!");
                        break;
                    }

                    ExecuteOption(option);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine("Please try again.");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void ExecuteOption(int option)
        {
            switch (option)
            {
                case 1:
                    customerService.ListAllCustomers();
                    break;
                case 2:
                    orderService.ListAllCurrentOrders();
                    break;
                case 3:
                    customerService.RegisterNewCustomer();
                    break;
                case 4:
                    orderService.CreateOrder();
                    break;
                case 5:
                    customerService.DisplayCustomerOrderHistory();
                    break;
                case 6:
                    orderService.ModifyOrderDetails();
                    break;
                case 7:
                    orderService.ProcessOrderAndCheckout();
                    break;
                case 8:
                    orderService.DisplayMonthlyCharges();
                    break;
                default:
                    Console.WriteLine("Invalid option. Please select a number between 0-8.");
                    break;
            }
        }
    }
}