//==========================================================
// Student Number : S10259432
// Student Name : Liew Zhan Yang
// Partner Number : S10257777
// Partner Name : Amicus Lee Ming Ge
//==========================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Assignment
{
    public class CustomerService
    {
        private readonly DataManager dataManager;

        public CustomerService(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public void ListAllCustomers()
        {
            Console.WriteLine("\n=== ALL CUSTOMERS ===");
            Console.WriteLine();

            if (!dataManager.Customers.Any())
            {
                Console.WriteLine("No customers found.");
                return;
            }

            // Display column headers
            Console.WriteLine("{0,-20}{1,-15}{2,-15}{3,-20}{4,-15}{5,-15}",
                "Name", "Member ID", "DOB", "Membership Status", "Points", "PunchCard");
            Console.WriteLine(new string('-', 100));

            // Display each customer's information
            foreach (Customer customer in dataManager.Customers)
            {
                Console.WriteLine("{0,-20}{1,-15}{2,-15}{3,-20}{4,-15}{5,-15}",
                    customer.Name,
                    customer.MemberId,
                    customer.Dob.ToString("dd/MM/yyyy"),
                    customer.Rewards.Tier,
                    customer.Rewards.Points,
                    customer.Rewards.PunchCard);
            }
        }

        public void RegisterNewCustomer()
        {
            Console.WriteLine("\n=== REGISTER NEW CUSTOMER ===");

            try
            {
                // Get customer name
                string name = GetValidCustomerName();
                if (string.IsNullOrEmpty(name)) return;

                // Get unique member ID
                int? memberId = GetUniqueMemberId();
                if (!memberId.HasValue) return;

                // Get date of birth
                DateTime? dob = GetValidDateOfBirth();
                if (!dob.HasValue) return;

                // Create new customer
                Customer newCustomer = new Customer(name, memberId.Value, dob.Value)
                {
                    Rewards = new PointCard(),
                    OrderHistory = new List<Order>()
                };

                // Add to customers list
                dataManager.Customers.Add(newCustomer);

                // Save to CSV
                dataManager.SaveCustomerToCsv(newCustomer);

                Console.WriteLine($"\nCustomer '{name}' registered successfully with Member ID: {memberId.Value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering customer: {ex.Message}");
            }
        }

        public void DisplayCustomerOrderHistory()
        {
            Console.WriteLine("\n=== CUSTOMER ORDER HISTORY ===");

            if (!dataManager.Customers.Any())
            {
                Console.WriteLine("No customers found.");
                return;
            }

            ListAllCustomers();
            Console.WriteLine();

            Customer selectedCustomer = SelectCustomerById();
            if (selectedCustomer == null) return;

            DisplayOrderHistoryForCustomer(selectedCustomer);
        }

        private string GetValidCustomerName()
        {
            const int maxAttempts = 3;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    Console.Write("Enter customer's name: ");
                    string name = Console.ReadLine()?.Trim();

                    if (!string.IsNullOrWhiteSpace(name) && name.Length >= 2 && name.Length <= 50)
                    {
                        return name;
                    }

                    var remainingAttempts = maxAttempts - attempt;
                    if (remainingAttempts > 0)
                    {
                        Console.WriteLine($"Invalid name. Name must be 2-50 characters long. {remainingAttempts} attempts remaining.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Input error: {ex.Message}");
                }
            }

            Console.WriteLine("Failed to get valid customer name after maximum attempts.");
            return null;
        }

        private int? GetUniqueMemberId()
        {
            const int maxAttempts = 3;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    Console.Write("Enter customer's ID number: ");
                    string memberIdInput = Console.ReadLine();

                    if (!int.TryParse(memberIdInput, out int memberId))
                    {
                        throw new FormatException("Member ID must be a valid number.");
                    }

                    if (memberId <= 0)
                    {
                        throw new ArgumentException("Member ID must be a positive number.");
                    }

                    if (dataManager.Customers.Any(c => c.MemberId == memberId))
                    {
                        throw new InvalidOperationException($"Member ID {memberId} already exists.");
                    }

                    return memberId;
                }
                catch (Exception ex)
                {
                    var remainingAttempts = maxAttempts - attempt;
                    if (remainingAttempts > 0)
                    {
                        Console.WriteLine($"{ex.Message} {remainingAttempts} attempts remaining.");
                    }
                }
            }

            Console.WriteLine("Failed to get valid member ID after maximum attempts.");
            return null;
        }

        private DateTime? GetValidDateOfBirth()
        {
            const int maxAttempts = 3;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    Console.Write("Enter customer's date of birth (dd/MM/yyyy): ");
                    string dobInput = Console.ReadLine();

                    DateTime dob = DateTime.ParseExact(dobInput, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    // Validate that the date is not in the future
                    if (dob > DateTime.Today)
                    {
                        throw new ArgumentException("Date of birth cannot be in the future.");
                    }

                    // Validate reasonable age range
                    if (dob < DateTime.Today.AddYears(-150))
                    {
                        throw new ArgumentException("Please enter a valid date of birth.");
                    }

                    return dob;
                }
                catch (Exception ex)
                {
                    var remainingAttempts = maxAttempts - attempt;
                    if (remainingAttempts > 0)
                    {
                        Console.WriteLine($"{ex.Message} {remainingAttempts} attempts remaining.");
                    }
                }
            }

            Console.WriteLine("Failed to get valid date of birth after maximum attempts.");
            return null;
        }

        public Customer SelectCustomerById()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter Customer's Member ID: ");
                    int memberID = Convert.ToInt32(Console.ReadLine());

                    Customer customer = dataManager.Customers.FirstOrDefault(c => c.MemberId == memberID);
                    if (customer != null)
                    {
                        return customer;
                    }

                    Console.WriteLine("Customer not found. Please try again.");
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a valid Member ID.");
                }
            }
        }

        public Customer SelectCustomerFromList()
        {
            if (!dataManager.Customers.Any())
            {
                Console.WriteLine("No customers available.");
                return null;
            }

            Console.WriteLine("\nSelect a customer:");
            for (int i = 0; i < dataManager.Customers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {dataManager.Customers[i].Name} (ID: {dataManager.Customers[i].MemberId})");
            }

            while (true)
            {
                try
                {
                    Console.Write("Enter customer number: ");
                    int selection = Convert.ToInt32(Console.ReadLine()) - 1;

                    if (selection >= 0 && selection < dataManager.Customers.Count)
                    {
                        return dataManager.Customers[selection];
                    }

                    Console.WriteLine("Invalid selection. Please try again.");
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a valid number.");
                }
            }
        }

        private void DisplayOrderHistoryForCustomer(Customer customer)
        {
            Console.WriteLine($"\n=== ORDER HISTORY FOR {customer.Name.ToUpper()} ===");

            if (!customer.OrderHistory.Any())
            {
                Console.WriteLine("No order history found for this customer.");
                return;
            }

            foreach (Order order in customer.OrderHistory)
            {
                Console.WriteLine($"\nOrder ID: {order.Id}");
                Console.WriteLine($"Order Time: {order.TimeReceived}");
                if (order.TimeFulfilled.HasValue)
                {
                    Console.WriteLine($"Fulfilled Time: {order.TimeFulfilled}");
                }

                Console.WriteLine("Ice Creams:");
                foreach (IceCream iceCream in order.IceCreamList)
                {
                    Console.WriteLine($"  - {iceCream.Option}, {iceCream.Scoops} scoop(s)");

                    if (iceCream.Flavours.Any())
                    {
                        Console.WriteLine("    Flavours:");
                        foreach (var flavour in iceCream.Flavours)
                        {
                            Console.WriteLine($"      * {flavour.Type} {(flavour.Premium ? "(Premium)" : "")}");
                        }
                    }

                    if (iceCream.Toppings.Any())
                    {
                        Console.WriteLine("    Toppings:");
                        foreach (var topping in iceCream.Toppings)
                        {
                            Console.WriteLine($"      * {topping.Type}");
                        }
                    }

                    // Display special properties
                    if (iceCream is Cone cone)
                    {
                        Console.WriteLine($"    Dipped: {cone.Dipped}");
                    }
                    else if (iceCream is Waffle waffle)
                    {
                        Console.WriteLine($"    Waffle Flavour: {waffle.WaffleFlavour}");
                    }
                }
                Console.WriteLine(new string('-', 50));
            }
        }
    }
}