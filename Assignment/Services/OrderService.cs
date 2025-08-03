//==========================================================
// Student Number : S10259432
// Student Name : Liew Zhan Yang
// Partner Number : S10257777
// Partner Name : Amicus Lee Ming Ge
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment
{
    public class OrderService
    {
        private readonly DataManager dataManager;
        private readonly CustomerService customerService;

        public OrderService(DataManager dataManager)
        {
            this.dataManager = dataManager;
            this.customerService = new CustomerService(dataManager);
        }

        public void ListAllCurrentOrders()
        {
            Console.WriteLine("\n=== CURRENT ORDERS ===");

            Console.WriteLine("\n--- GOLD MEMBER ORDERS ---");
            DisplayOrderQueue(dataManager.GoldQueue);

            Console.WriteLine("\n--- REGULAR MEMBER ORDERS ---");
            DisplayOrderQueue(dataManager.OrdinaryQueue);
        }

        public void CreateOrder()
        {
            Console.WriteLine("\n=== CREATE NEW ORDER ===");

            if (!dataManager.Customers.Any())
            {
                Console.WriteLine("No customers found. Please register a customer first.");
                return;
            }

            customerService.ListAllCustomers();
            Console.WriteLine();

            Customer customer = customerService.SelectCustomerById();
            if (customer == null) return;

            // Check if customer already has an ongoing order
            if (customer.CurrentOrder != null)
            {
                Console.WriteLine("Customer already has an ongoing order. Please process the current order first.");
                return;
            }

            // Create new order
            customer.MakeOrder();

            // Add ice creams to order
            AddIceCreamsToOrder(customer.CurrentOrder);

            // Add order to appropriate queue
            if (customer.Rewards.Tier.Equals("Gold", StringComparison.OrdinalIgnoreCase))
            {
                dataManager.GoldQueue.Enqueue(customer.CurrentOrder);
            }
            else
            {
                dataManager.OrdinaryQueue.Enqueue(customer.CurrentOrder);
            }

            Console.WriteLine("\nOrder created successfully!");
        }

        public void ModifyOrderDetails()
        {
            Console.WriteLine("\n=== MODIFY ORDER DETAILS ===");

            var customersWithOrders = dataManager.Customers.Where(c => c.CurrentOrder != null).ToList();

            if (!customersWithOrders.Any())
            {
                Console.WriteLine("No customers with current orders found.");
                return;
            }

            Console.WriteLine("Customers with current orders:");
            for (int i = 0; i < customersWithOrders.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {customersWithOrders[i].Name} (ID: {customersWithOrders[i].MemberId})");
            }

            Customer selectedCustomer = SelectCustomerFromList(customersWithOrders);
            if (selectedCustomer?.CurrentOrder == null) return;

            ModifyOrder(selectedCustomer.CurrentOrder);
        }

        public void ProcessOrderAndCheckout()
        {
            Console.WriteLine("\n=== PROCESS ORDER AND CHECKOUT ===");

            // Process Gold queue first
            if (dataManager.GoldQueue.Any())
            {
                Order order = dataManager.GoldQueue.Dequeue();
                Customer customer = dataManager.Customers.FirstOrDefault(c => c.CurrentOrder?.Id == order.Id);
                if (customer != null)
                {
                    ProcessCustomerOrder(customer, order);
                    return;
                }
            }

            // Then process ordinary queue
            if (dataManager.OrdinaryQueue.Any())
            {
                Order order = dataManager.OrdinaryQueue.Dequeue();
                Customer customer = dataManager.Customers.FirstOrDefault(c => c.CurrentOrder?.Id == order.Id);
                if (customer != null)
                {
                    ProcessCustomerOrder(customer, order);
                    return;
                }
            }

            Console.WriteLine("No orders in queue to process.");
        }

        public void DisplayMonthlyCharges()
        {
            Console.WriteLine("\n=== MONTHLY CHARGES BREAKDOWN ===");

            int year = GetValidYear();

            var allOrders = dataManager.Customers
                .SelectMany(c => c.OrderHistory)
                .Where(o => o.TimeReceived.Year == year && o.TimeFulfilled.HasValue)
                .ToList();

            if (!allOrders.Any())
            {
                Console.WriteLine($"No completed orders found for year {year}.");
                return;
            }

            DisplayYearlyBreakdown(allOrders, year);
        }

        private void DisplayOrderQueue(Queue<Order> queue)
        {
            if (!queue.Any())
            {
                Console.WriteLine("No orders in queue.");
                return;
            }

            foreach (Order order in queue)
            {
                Console.WriteLine($"\nOrder ID: {order.Id}");
                Console.WriteLine($"Order Time: {order.TimeReceived}");

                foreach (IceCream iceCream in order.IceCreamList)
                {
                    Console.WriteLine($"  - {iceCream.Option}, {iceCream.Scoops} scoop(s)");

                    foreach (var flavour in iceCream.Flavours)
                    {
                        Console.WriteLine($"    Flavour: {flavour.Type} {(flavour.Premium ? "(Premium)" : "")}");
                    }

                    foreach (var topping in iceCream.Toppings)
                    {
                        Console.WriteLine($"    Topping: {topping.Type}");
                    }
                }
                Console.WriteLine(new string('-', 40));
            }
        }

        private void AddIceCreamsToOrder(Order order)
        {
            bool addMore;
            do
            {
                IceCream iceCream = CreateIceCream();
                if (iceCream != null)
                {
                    order.AddiceCream(iceCream);

                    Console.Write("Add another ice cream to the order? (Y/N): ");
                    addMore = Console.ReadLine()?.Trim().ToUpper() == "Y";
                }
                else
                {
                    addMore = false;
                }
            } while (addMore);
        }

        private IceCream CreateIceCream()
        {
            // Get ice cream option
            string option = GetValidIceCreamOption();

            // Get number of scoops
            int scoops = GetValidScoopCount();

            // Select flavours
            List<Flavour> selectedFlavours = SelectFlavours(scoops);

            // Select toppings
            List<Topping> selectedToppings = SelectToppings();

            // Create specific ice cream type
            return CreateSpecificIceCream(option, scoops, selectedFlavours, selectedToppings);
        }

        private string GetValidIceCreamOption()
        {
            string[] validOptions = { "Cup", "Cone", "Waffle" };

            while (true)
            {
                Console.Write("Enter Ice Cream Option (Cup/Cone/Waffle): ");
                string option = Console.ReadLine()?.Trim();

                if (validOptions.Any(v => v.Equals(option, StringComparison.OrdinalIgnoreCase)))
                {
                    return option;
                }

                Console.WriteLine("Please enter a valid option: Cup, Cone, or Waffle");
            }
        }

        private int GetValidScoopCount()
        {
            while (true)
            {
                Console.Write("Number of scoops (1-3): ");
                if (int.TryParse(Console.ReadLine(), out int scoops) && scoops >= 1 && scoops <= 3)
                {
                    return scoops;
                }
                Console.WriteLine("Please enter a number between 1 and 3.");
            }
        }

        private List<Flavour> SelectFlavours(int scoopCount)
        {
            Console.WriteLine("\nAvailable Flavours:");
            Console.WriteLine("Regular Flavours:");
            var regularFlavours = dataManager.AllFlavours.Where(f => !f.Premium).ToList();
            foreach (var flavour in regularFlavours)
            {
                Console.WriteLine($"  - {flavour.Type}");
            }

            Console.WriteLine("Premium Flavours:");
            var premiumFlavours = dataManager.AllFlavours.Where(f => f.Premium).ToList();
            foreach (var flavour in premiumFlavours)
            {
                Console.WriteLine($"  - {flavour.Type}");
            }

            List<Flavour> selectedFlavours = new List<Flavour>();

            for (int i = 0; i < scoopCount; i++)
            {
                while (true)
                {
                    Console.Write($"Enter flavour {i + 1}: ");
                    string flavourName = Console.ReadLine()?.Trim();

                    var selectedFlavour = dataManager.AllFlavours.FirstOrDefault(f =>
                        f.Type.Equals(flavourName, StringComparison.OrdinalIgnoreCase));

                    if (selectedFlavour != null)
                    {
                        selectedFlavours.Add(new Flavour(selectedFlavour.Type, selectedFlavour.Premium, 1));
                        break;
                    }

                    Console.WriteLine("Invalid flavour. Please try again.");
                }
            }

            return selectedFlavours;
        }

        private List<Topping> SelectToppings()
        {
            Console.WriteLine("\nAvailable Toppings:");
            foreach (var topping in dataManager.AllToppings)
            {
                Console.WriteLine($"  - {topping.Type}");
            }

            List<Topping> selectedToppings = new List<Topping>();

            while (true)
            {
                Console.Write("Enter topping (or 'X' to finish): ");
                string toppingInput = Console.ReadLine()?.Trim();

                if (toppingInput?.ToUpper() == "X")
                {
                    break;
                }

                var selectedTopping = dataManager.AllToppings.FirstOrDefault(t =>
                    t.Type.Equals(toppingInput, StringComparison.OrdinalIgnoreCase));

                if (selectedTopping != null)
                {
                    selectedToppings.Add(new Topping(selectedTopping.Type));
                    Console.WriteLine($"Added {selectedTopping.Type}");
                }
                else
                {
                    Console.WriteLine("Invalid topping. Please try again.");
                }
            }

            return selectedToppings;
        }

        private IceCream CreateSpecificIceCream(string option, int scoops, List<Flavour> flavours, List<Topping> toppings)
        {
            switch (option.ToLower())
            {
                case "cup":
                    return new Cup(scoops, flavours, toppings);

                case "cone":
                    bool isDipped = GetYesNoAnswer("Is the cone dipped in chocolate? (Y/N): ");
                    return new Cone(scoops, flavours, toppings, isDipped);

                case "waffle":
                    string waffleFlavour = GetValidWaffleFlavour();
                    return new Waffle(scoops, flavours, toppings, waffleFlavour);

                default:
                    Console.WriteLine("Invalid ice cream option.");
                    return null;
            }
        }

        private bool GetYesNoAnswer(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string response = Console.ReadLine()?.Trim().ToUpper();

                if (response == "Y" || response == "YES")
                    return true;
                if (response == "N" || response == "NO")
                    return false;

                Console.WriteLine("Please enter Y for Yes or N for No.");
            }
        }

        private string GetValidWaffleFlavour()
        {
            string[] validFlavours = { "Original", "Red Velvet", "Charcoal", "Pandan" };

            while (true)
            {
                Console.Write("Enter waffle flavour (Original/Red Velvet/Charcoal/Pandan): ");
                string flavour = Console.ReadLine()?.Trim();

                if (validFlavours.Any(v => v.Equals(flavour, StringComparison.OrdinalIgnoreCase)))
                {
                    return flavour;
                }

                Console.WriteLine("Please enter a valid waffle flavour.");
            }
        }

        private Customer SelectCustomerFromList(List<Customer> customers)
        {
            while (true)
            {
                try
                {
                    Console.Write("Select customer number: ");
                    int selection = Convert.ToInt32(Console.ReadLine()) - 1;

                    if (selection >= 0 && selection < customers.Count)
                    {
                        return customers[selection];
                    }

                    Console.WriteLine("Invalid selection. Please try again.");
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a valid number.");
                }
            }
        }

        private void ModifyOrder(Order order)
        {
            Console.WriteLine("\nCurrent Ice Creams in Order:");
            for (int i = 0; i < order.IceCreamList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {order.IceCreamList[i]}");
            }

            Console.WriteLine("\nChoose an action:");
            Console.WriteLine("1. Add a New Ice Cream");
            Console.WriteLine("2. Delete an Ice Cream");

            while (true)
            {
                try
                {
                    Console.Write("Enter your choice (1-2): ");
                    int action = Convert.ToInt32(Console.ReadLine());

                    switch (action)
                    {
                        case 1:
                            AddNewIceCreamToOrder(order);
                            return;
                        case 2:
                            DeleteIceCreamFromOrder(order);
                            return;
                        default:
                            Console.WriteLine("Please enter 1 or 2.");
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a valid number.");
                }
            }
        }

        private void AddNewIceCreamToOrder(Order order)
        {
            IceCream newIceCream = CreateIceCream();
            if (newIceCream != null)
            {
                order.AddiceCream(newIceCream);
                Console.WriteLine("New ice cream added successfully!");
            }
        }

        private void DeleteIceCreamFromOrder(Order order)
        {
            if (!order.IceCreamList.Any())
            {
                Console.WriteLine("No ice creams to delete.");
                return;
            }

            if (order.IceCreamList.Count == 1)
            {
                Console.WriteLine("Cannot delete the only ice cream in the order.");
                return;
            }

            int index = GetValidIceCreamIndex(order, "delete");
            if (index == -1) return;

            try
            {
                order.DeleteIceCream(index);
                Console.WriteLine("Ice cream deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting ice cream: {ex.Message}");
            }
        }

        private int GetValidIceCreamIndex(Order order, string action)
        {
            while (true)
            {
                try
                {
                    Console.Write($"Enter the number of the ice cream to {action}: ");
                    int index = Convert.ToInt32(Console.ReadLine()) - 1;

                    if (index >= 0 && index < order.IceCreamList.Count)
                    {
                        return index;
                    }

                    Console.WriteLine("Invalid selection. Please try again.");
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a valid number.");
                }
            }
        }

        private void ProcessCustomerOrder(Customer customer, Order order)
        {
            Console.WriteLine($"Processing order for: {customer.Name}");
            Console.WriteLine($"Membership Status: {customer.Rewards.Tier}");
            Console.WriteLine($"Available Points: {customer.Rewards.Points}");
            Console.WriteLine($"Punch Card: {customer.Rewards.PunchCard}/10");

            // Display order details
            DisplayOrderDetails(order);

            // Calculate total
            double total = CalculateOrderTotal(customer, order);

            Console.WriteLine($"\nFinal Total: ${total:F2}");
            Console.WriteLine("Press Enter to complete payment...");
            Console.ReadKey();

            // Complete the order
            CompleteOrder(customer, order, total);

            Console.WriteLine("Order completed successfully!");
        }

        private void DisplayOrderDetails(Order order)
        {
            Console.WriteLine("\nOrder Details:");
            foreach (var iceCream in order.IceCreamList)
            {
                Console.WriteLine($"- {iceCream}");
                double iceCreamPrice = iceCream.CalculatePrice(dataManager.OptionsDict,
                    dataManager.AllFlavours, dataManager.AllToppings);
                Console.WriteLine($"  Price: ${iceCreamPrice:F2}");
            }
        }

        private double CalculateOrderTotal(Customer customer, Order order)
        {
            double total = order.CalculateTotal(dataManager.OptionsDict,
                dataManager.AllFlavours, dataManager.AllToppings, customer.Rewards, false);

            // Apply birthday discount
            if (customer.IsBirthday())
            {
                var mostExpensive = order.IceCreamList
                    .OrderByDescending(ic => ic.CalculatePrice(dataManager.OptionsDict,
                        dataManager.AllFlavours, dataManager.AllToppings))
                    .First();

                double discount = mostExpensive.CalculatePrice(dataManager.OptionsDict,
                    dataManager.AllFlavours, dataManager.AllToppings);

                total -= discount;
                Console.WriteLine($"🎂 Happy Birthday! Free most expensive ice cream: -${discount:F2}");
            }

            // Apply punch card discount
            if (customer.Rewards.PunchCard >= 10)
            {
                var firstIceCream = order.IceCreamList.First();
                double discount = firstIceCream.CalculatePrice(dataManager.OptionsDict,
                    dataManager.AllFlavours, dataManager.AllToppings);

                total -= discount;
                customer.Rewards.PunchCard = 0;
                Console.WriteLine($"🎫 Punch card reward! Free ice cream: -${discount:F2}");
            }

            // Apply points redemption
            if (customer.Rewards.Tier != "Ordinary" && customer.Rewards.Points > 0)
            {
                if (GetYesNoAnswer($"Redeem points? You have {customer.Rewards.Points} points (1 point = $0.02): "))
                {
                    int maxPoints = Math.Min(customer.Rewards.Points, (int)(total / 0.02));
                    Console.Write($"How many points to redeem (max {maxPoints}): ");

                    if (int.TryParse(Console.ReadLine(), out int pointsToRedeem) &&
                        pointsToRedeem > 0 && pointsToRedeem <= maxPoints)
                    {
                        double pointsValue = pointsToRedeem * 0.02;
                        total -= pointsValue;
                        customer.Rewards.Points -= pointsToRedeem;
                        Console.WriteLine($"Redeemed {pointsToRedeem} points: -${pointsValue:F2}");
                    }
                }
            }

            return Math.Max(0, total);
        }

        private void CompleteOrder(Customer customer, Order order, double total)
        {
            // Mark order as fulfilled
            order.TimeFulfilled = DateTime.Now;

            // Add to order history
            customer.OrderHistory.Add(order);

            // Clear current order
            customer.CurrentOrder = null;

            // Update rewards
            foreach (var iceCream in order.IceCreamList)
            {
                customer.Rewards.Punch();
            }

            // Add points (10 points per dollar)
            int pointsEarned = (int)(total * 10);
            customer.Rewards.AddPoints(pointsEarned);

            Console.WriteLine($"Points earned: {pointsEarned}");
            Console.WriteLine($"Total points: {customer.Rewards.Points}");
            Console.WriteLine($"Punch card: {customer.Rewards.PunchCard}/10");
        }

        private int GetValidYear()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter year (e.g., 2023): ");
                    int year = Convert.ToInt32(Console.ReadLine());

                    if (year >= 2000 && year <= DateTime.Now.Year)
                    {
                        return year;
                    }

                    Console.WriteLine($"Please enter a year between 2000 and {DateTime.Now.Year}.");
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a valid year.");
                }
            }
        }

        private void DisplayYearlyBreakdown(List<Order> orders, int year)
        {
            double[] monthlyTotals = new double[12];
            string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                              "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            foreach (var order in orders)
            {
                int monthIndex = order.TimeReceived.Month - 1;
                double orderTotal = order.CalculateTotal(dataManager.OptionsDict,
                    dataManager.AllFlavours, dataManager.AllToppings, new PointCard(), false);
                monthlyTotals[monthIndex] += orderTotal;
            }

            Console.WriteLine($"\n--- {year} Monthly Breakdown ---");
            double yearTotal = 0;

            for (int i = 0; i < 12; i++)
            {
                Console.WriteLine($"{months[i]} {year}: ${monthlyTotals[i]:F2}");
                yearTotal += monthlyTotals[i];
            }

            Console.WriteLine(new string('-', 30));
            Console.WriteLine($"Total for {year}: ${yearTotal:F2}");
        }
    }
}