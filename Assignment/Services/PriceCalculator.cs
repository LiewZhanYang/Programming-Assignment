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
    /// <summary>
    /// Professional service class for managing ice cream orders
    /// Handles order creation, modification, processing, and queue management
    /// </summary>
    public class OrderService
    {
        #region Private Fields
        private readonly DataManager _dataManager;
        private readonly CustomerService _customerService;
        private readonly ConsoleHelper _consoleHelper;
        private readonly PriceCalculator _priceCalculator;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the OrderService class
        /// </summary>
        /// <param name="dataManager">Data manager for accessing application data</param>
        public OrderService(DataManager dataManager)
        {
            _dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
            _customerService = new CustomerService(dataManager);
            _consoleHelper = new ConsoleHelper();
            _priceCalculator = new PriceCalculator();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Displays all current orders organized by customer tier
        /// </summary>
        public void ListAllCurrentOrders()
        {
            try
            {
                _consoleHelper.DisplaySectionHeader("CURRENT ORDER QUEUES");

                DisplayOrderQueue("GOLD MEMBER QUEUE", _dataManager.GoldQueue, ConsoleColor.Yellow);
                DisplayOrderQueue("REGULAR MEMBER QUEUE", _dataManager.OrdinaryQueue, ConsoleColor.White);

                var totalOrders = _dataManager.GoldQueue.Count + _dataManager.OrdinaryQueue.Count;
                _consoleHelper.DisplayInfoMessage($"Total pending orders: {totalOrders}");
            }
            catch (Exception ex)
            {
                _consoleHelper.DisplayErrorMessage($"Error displaying orders: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new order for a selected customer with comprehensive validation
        /// </summary>
        public void CreateOrder()
        {
            try
            {
                _consoleHelper.DisplaySectionHeader("CREATE NEW ORDER");

                // Validate system state
                if (!_dataManager.Customers.Any())
                {
                    _consoleHelper.DisplayInfoMessage("No customers found. Please register a customer first.");
                    return;
                }

                // Display customers and get selection
                _customerService.ListAllCustomers();
                var customer = _customerService.SelectCustomerById();
                if (customer == null) return;

                // Validate customer can place order
                if (!CanCustomerPlaceOrder(customer))
                    return;

                // Create and process order
                var orderResult = ProcessNewOrder(customer);
                if (orderResult.Success)
                {
                    EnqueueOrder(customer, customer.CurrentOrder);
                    DisplayOrderConfirmation(customer, customer.CurrentOrder);
                }
            }
            catch (Exception ex)
            {
                _consoleHelper.DisplayErrorMessage($"Order creation failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Modifies an existing order with comprehensive options
        /// </summary>
        public void ModifyOrderDetails()
        {
            try
            {
                _consoleHelper.DisplaySectionHeader("MODIFY ORDER DETAILS");

                var customersWithOrders = GetCustomersWithCurrentOrders();
                if (!customersWithOrders.Any())
                {
                    _consoleHelper.DisplayInfoMessage("No customers with current orders found.");
                    return;
                }

                var selectedCustomer = SelectCustomerWithOrder(customersWithOrders);
                if (selectedCustomer?.CurrentOrder == null) return;

                DisplayCurrentOrderSummary(selectedCustomer.CurrentOrder);

                var modifications = ProcessOrderModifications(selectedCustomer.CurrentOrder);
                if (modifications.HasChanges)
                {
                    UpdateOrderInQueue(selectedCustomer);
                    _consoleHelper.DisplaySuccessMessage("Order modified successfully!");
                    DisplayCurrentOrderSummary(selectedCustomer.CurrentOrder);
                }
            }
            catch (Exception ex)
            {
                _consoleHelper.DisplayErrorMessage($"Order modification failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Processes the next order in queue with comprehensive checkout
        /// </summary>
        public void ProcessOrderAndCheckout()
        {
            try
            {
                _consoleHelper.DisplaySectionHeader("PROCESS ORDER AND CHECKOUT");

                var orderToProcess = GetNextOrderFromQueue();
                if (orderToProcess == null)
                {
                    _consoleHelper.DisplayInfoMessage("No orders in queue to process.");
                    return;
                }

                var customer = FindCustomerByOrder(orderToProcess.Item2);
                if (customer == null)
                {
                    _consoleHelper.DisplayErrorMessage("Customer not found for order. Removing from queue.");
                    return;
                }

                var checkoutResult = ProcessCheckout(customer, orderToProcess.Item2);
                if (checkoutResult.Success)
                {
                    CompleteOrder(customer, orderToProcess.Item2, checkoutResult.FinalTotal);
                    _consoleHelper.DisplaySuccessMessage("Order completed successfully!");
                }
            }
            catch (Exception ex)
            {
                _consoleHelper.DisplayErrorMessage($"Order processing failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Displays comprehensive monthly charges breakdown and analytics
        /// </summary>
        public void DisplayMonthlyCharges()
        {
            try
            {
                _consoleHelper.DisplaySectionHeader("MONTHLY CHARGES BREAKDOWN");

                var year = GetValidBusinessYear();
                if (!year.HasValue) return;

                var analytics = GenerateYearlyAnalytics(year.Value);
                if (analytics == null)
                {
                    _consoleHelper.DisplayInfoMessage($"No completed orders found for year {year.Value}.");
                    return;
                }

                DisplayComprehensiveAnalytics(analytics);
            }
            catch (Exception ex)
            {
                _consoleHelper.DisplayErrorMessage($"Error generating analytics: {ex.Message}");
            }
        }

        #endregion

        #region Private Helper Methods - Order Creation

        /// <summary>
        /// Checks if a customer can place a new order
        /// </summary>
        private bool CanCustomerPlaceOrder(Customer customer)
        {
            if (customer.CurrentOrder != null)
            {
                _consoleHelper.DisplayWarningMessage(
                    $"Customer {customer.Name} already has an ongoing order (ID: {customer.CurrentOrder.Id}). " +
                    "Please process the current order first or modify it.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Processes the creation of a new order
        /// </summary>
        private OrderCreationResult ProcessNewOrder(Customer customer)
        {
            try
            {
                _consoleHelper.DisplayProgress("Creating new order");

                customer.MakeOrder();
                _consoleHelper.CompleteProgress();

                var iceCreamsAdded = AddIceCreamsToOrder(customer.CurrentOrder);

                return new OrderCreationResult
                {
                    Success = iceCreamsAdded > 0,
                    IceCreamsAdded = iceCreamsAdded,
                    Message = iceCreamsAdded > 0 ? "Order created successfully" : "No ice creams added to order"
                };
            }
            catch (Exception ex)
            {
                return new OrderCreationResult
                {
                    Success = false,
                    Message = $"Order creation failed: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Adds ice creams to an order with user interaction
        /// </summary>
        private int AddIceCreamsToOrder(Order order)
        {
            int iceCreamsAdded = 0;
            bool addMore = true;

            while (addMore)
            {
                try
                {
                    var iceCream = CreateIceCreamWithGuidance();
                    if (iceCream != null)
                    {
                        order.AddiceCream(iceCream);
                        iceCreamsAdded++;

                        _consoleHelper.DisplaySuccessMessage($"Ice cream #{iceCreamsAdded} added to order!");
                        DisplayIceCreamSummary(iceCream, iceCreamsAdded);

                        if (iceCreamsAdded >= 5) // Reasonable limit
                        {
                            _consoleHelper.DisplayInfoMessage("Maximum 5 ice creams per order reached.");
                            addMore = false;
                        }
                        else
                        {
                            addMore = _consoleHelper.GetYesNoInput(
                                "Would you like to add another ice cream to this order?", false);
                        }
                    }
                    else
                    {
                        addMore = false;
                    }
                }
                catch (Exception ex)
                {
                    _consoleHelper.DisplayErrorMessage($"Error adding ice cream: {ex.Message}");
                    addMore = _consoleHelper.GetYesNoInput("Would you like to try adding another ice cream?", false);
                }
            }

            return iceCreamsAdded;
        }

        /// <summary>
        /// Creates an ice cream with step-by-step guidance
        /// </summary>
        private IceCream CreateIceCreamWithGuidance()
        {
            try
            {
                _consoleHelper.DisplaySeparator("Ice Cream Configuration");

                // Step 1: Get ice cream base option
                var option = SelectIceCreamOption();
                if (string.IsNullOrEmpty(option)) return null;

                // Step 2: Get number of scoops
                var scoops = SelectScoopCount();
                if (!scoops.HasValue) return null;

                // Step 3: Select flavours
                var flavours = SelectFlavours(scoops.Value);
                if (flavours == null || !flavours.Any()) return null;

                // Step 4: Select toppings
                var toppings = SelectToppings();

                // Step 5: Configure specific options
                var iceCream = CreateSpecificIceCream(option, scoops.Value, flavours, toppings);

                if (iceCream != null)
                {
                    var price = iceCream.CalculatePrice(_dataManager.OptionsDict,
                        _dataManager.AllFlavours, _dataManager.AllToppings);
                    _consoleHelper.DisplayInfoMessage($"Ice cream configured! Estimated price: ${price:F2}");
                }

                return iceCream;
            }
            catch (Exception ex)
            {
                _consoleHelper.DisplayErrorMessage($"Error creating ice cream: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Allows user to select ice cream base option with validation
        /// </summary>
        private string SelectIceCreamOption()
        {
            var options = AppConstants.VALID_ICE_CREAM_OPTIONS;
            var descriptions = new[]
            {
                "Cup - Classic ice cream in a cup",
                "Cone - Ice cream in a wafer cone (optional chocolate dip)",
                "Waffle - Ice cream in a premium waffle cone"
            };

            Console.WriteLine("\nAvailable Ice Cream Options:");
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"  {i + 1}. {descriptions[i]}");
            }

            var selection = _consoleHelper.GetValidIntegerInput(
                "Select ice cream option", 1, options.Length);

            return selection.HasValue ? options[selection.Value - 1] : null;
        }

        /// <summary>
        /// Allows user to select number of scoops
        /// </summary>
        private int? SelectScoopCount()
        {
            Console.WriteLine($"\nScoops available: {AppConstants.MIN_SCOOPS} to {AppConstants.MAX_SCOOPS}");
            Console.WriteLine("More scoops = more flavours = more deliciousness!");

            return _consoleHelper.GetValidIntegerInput(
                "Number of scoops", AppConstants.MIN_SCOOPS, AppConstants.MAX_SCOOPS);
        }

        /// <summary>
        /// Allows user to select flavours based on scoop count
        /// </summary>
        private List<Flavour> SelectFlavours(int scoopCount)
        {
            try
            {
                Console.WriteLine($"\n--- FLAVOUR SELECTION ({scoopCount} scoop{(scoopCount > 1 ? "s" : "")} to configure) ---");

                DisplayAvailableFlavours();

                var selectedFlavours = new List<Flavour>();

                for (int scoop = 1; scoop <= scoopCount; scoop++)
                {
                    Console.WriteLine($"\nConfiguring scoop #{scoop}:");

                    var flavour = SelectSingleFlavour(scoop);
                    if (flavour == null)
                    {
                        _consoleHelper.DisplayWarningMessage("Flavour selection cancelled.");
                        return null;
                    }

                    selectedFlavours.Add(flavour);
                    _consoleHelper.DisplaySuccessMessage(
                        $"Scoop #{scoop}: {flavour.Type} {(flavour.Premium ? "(Premium - +$2.00)" : "(Regular)")}");
                }

                return selectedFlavours;
            }
            catch (Exception ex)
            {
                _consoleHelper.DisplayErrorMessage($"Error selecting flavours: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Displays available flavours organized by category
        /// </summary>
        private void DisplayAvailableFlavours()
        {
            var regularFlavours = _dataManager.AllFlavours.Where(f => !f.Premium).ToList();
            var premiumFlavours = _dataManager.AllFlavours.Where(f => f.Premium).ToList();

            Console.WriteLine("\nREGULAR FLAVOURS (Included in base price):");
            foreach (var flavour in regularFlavours)
            {
                Console.WriteLine($"  • {flavour.Type}");
            }

            Console.WriteLine($"\nPREMIUM FLAVOURS (+${AppConstants.PREMIUM_FLAVOUR_COST:F2} each):");
            foreach (var flavour in premiumFlavours)
            {
                Console.WriteLine($"  • {flavour.Type}");
            }
        }

        /// <summary>
        /// Allows user to select a single flavour with validation
        /// </summary>
        private Flavour SelectSingleFlavour(int scoopNumber)
        {
            const int maxAttempts = 3;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                var flavourName = _consoleHelper.GetUserInput($"Enter flavour for scoop #{scoopNumber}: ");

                if (string.IsNullOrWhiteSpace(flavourName))
                {
                    _consoleHelper.DisplayWarningMessage("Flavour name cannot be empty.");
                    continue;
                }

                var matchedFlavour = _dataManager.AllFlavours.FirstOrDefault(f =>
                    f.Type.Equals(flavourName.Trim(), StringComparison.OrdinalIgnoreCase));

                if (matchedFlavour != null)
                {
                    return new Flavour(matchedFlavour.Type, matchedFlavour.Premium, 1);
                }

                var remainingAttempts = maxAttempts - attempt;
                if (remainingAttempts > 0)
                {
                    _consoleHelper.DisplayWarningMessage(
                        $"Flavour '{flavourName}' not found. {remainingAttempts} attempts remaining.");

                    // Show similar flavours if any
                    var similarFlavours = FindSimilarFlavours(flavourName);
                    if (similarFlavours.Any())
                    {
                        Console.WriteLine("Did you mean:");
                        foreach (var similar in similarFlavours.Take(3))
                        {
                            Console.WriteLine($"  • {similar}");
                        }
                    }
                }
            }

            _consoleHelper.DisplayErrorMessage("Failed to select flavour after maximum attempts.");
            return null;
        }

        /// <summary>
        /// Finds flavours similar to the input string
        /// </summary>
        private List<string> FindSimilarFlavours(string input)
        {
            return _dataManager.AllFlavours
                .Where(f => f.Type.Contains(input, StringComparison.OrdinalIgnoreCase) ||
                           input.Contains(f.Type, StringComparison.OrdinalIgnoreCase))
                .Select(f => f.Type)
                .ToList();
        }

        /// <summary>
        /// Allows user to select toppings with enhanced UX
        /// </summary>
        private List<Topping> SelectToppings()
        {
            try
            {
                Console.WriteLine("\n--- TOPPING SELECTION ---");
                Console.WriteLine($"Available toppings (+${AppConstants.TOPPING_COST:F2} each):");

                foreach (var topping in _dataManager.AllToppings)
                {
                    Console.WriteLine($"  • {topping.Type}");
                }

                var selectedToppings = new List<Topping>();

                while (true)
                {
                    Console.WriteLine($"\nCurrently selected: {(selectedToppings.Any() ? string.Join(", ", selectedToppings.Select(t => t.Type)) : "None")}");

                    var toppingInput = _consoleHelper.GetUserInput(
                        "Enter topping name (or 'done' to finish, 'none' for no toppings): ");

                    if (string.IsNullOrWhiteSpace(toppingInput) ||
                        toppingInput.Equals("done", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    if (toppingInput.Equals("none", StringComparison.OrdinalIgnoreCase))
                    {
                        selectedToppings.Clear();
                        break;
                    }

                    var matchedTopping = _dataManager.AllToppings.FirstOrDefault(t =>
                        t.Type.Equals(toppingInput.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (matchedTopping != null)
                    {
                        if (!selectedToppings.Any(t => t.Type.Equals(matchedTopping.Type, StringComparison.OrdinalIgnoreCase)))
                        {
                            selectedToppings.Add(new Topping(matchedTopping.Type));
                            _consoleHelper.DisplaySuccessMessage($"Added {matchedTopping.Type}!");
                        }
                        else
                        {
                            _consoleHelper.DisplayWarningMessage($"{matchedTopping.Type} already selected.");
                        }
                    }
                    else
                    {
                        _consoleHelper.DisplayWarningMessage($"Topping '{toppingInput}' not found.");
                    }
                }

                var toppingCost = selectedToppings.Count * AppConstants.TOPPING_COST;
                if (selectedToppings.Any())
                {
                    _consoleHelper.DisplayInfoMessage(
                        $"Selected {selectedToppings.Count} topping(s). Additional cost: ${toppingCost:F2}");
                }

                return selectedToppings;
            }
            catch (Exception ex)
            {
                _consoleHelper.DisplayErrorMessage($"Error selecting toppings: {ex.Message}");
                return new List<Topping>();
            }
        }

        /// <summary>
        /// Creates specific ice cream instance based on option
        /// </summary>
        private IceCream CreateSpecificIceCream(string option, int scoops, List<Flavour> flavours, List<Topping> toppings)
        {
            switch (option.ToLower())
            {
                case "cup":
                    return new Cup(scoops, flavours, toppings);

                case "cone":
                    var isDipped = _consoleHelper.GetYesNoInput(
                        $"Would you like the cone dipped in chocolate? (+${2.0:F2})", false);
                    return new Cone(scoops, flavours, toppings, isDipped);

                case "waffle":
                    var waffleFlavour = SelectWaffleFlavour();
                    return waffleFlavour != null ? new Waffle(scoops, flavours, toppings, waffleFlavour) : null;

                default:
                    _consoleHelper.DisplayErrorMessage($"Unknown ice cream option: {option}");
                    return null;
            }
        }

        /// <summary>
        /// Allows user to select waffle flavour
        /// </summary>
        private string SelectWaffleFlavour()
        {
            Console.WriteLine("\nAvailable waffle flavours:");
            for (int i = 0; i < AppConstants.VALID_WAFFLE_FLAVOURS.Length; i++)
            {
                Console.WriteLine($"  {i + 1}. {AppConstants.VALID_WAFFLE_FLAVOURS[i]}");
            }

            var selection = _consoleHelper.GetValidIntegerInput(
                "Select waffle flavour", 1, AppConstants.VALID_WAFFLE_FLAVOURS.Length);

            return selection.HasValue ? AppConstants.VALID_WAFFLE_FLAVOURS[selection.Value - 1] : null;
        }

        #endregion

        #region Private Helper Methods - Order Management

        /// <summary>
        /// Enqueues order into appropriate queue based on customer tier
        /// </summary>
        private void EnqueueOrder(Customer customer, Order order)
        {
            if (customer.Rewards.Tier.Equals(AppConstants.TIER_GOLD, StringComparison.OrdinalIgnoreCase))
            {
                _dataManager.GoldQueue.Enqueue(order);
                _consoleHelper.DisplayInfoMessage("Order added to Gold member priority queue! 🏆");
            }
            else
            {
                _dataManager.OrdinaryQueue.Enqueue(order);
                _consoleHelper.DisplayInfoMessage("Order added to regular queue.");
            }
        }

        /// <summary>
        /// Displays order confirmation with details
        /// </summary>
        private void DisplayOrderConfirmation(Customer customer, Order order)
        {
            _consoleHelper.DisplaySeparator("ORDER CONFIRMATION");

            Console.WriteLine($"Customer: {customer.Name} (ID: {customer.MemberId})");
            Console.WriteLine($"Order ID: {order.Id}");
            Console.WriteLine($"Order Time: {order.TimeReceived:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Queue: {(customer.Rewards.Tier.Equals(AppConstants.TIER_GOLD, StringComparison.OrdinalIgnoreCase) ? "Gold Priority" : "Regular")}");

            Console.WriteLine($"\nItems in order ({order.IceCreamList.Count}):");
            for (int i = 0; i < order.IceCreamList.Count; i++)
            {
                var iceCream = order.IceCreamList[i];
                var price = iceCream.CalculatePrice(_dataManager.OptionsDict, _dataManager.AllFlavours, _dataManager.AllToppings);
                Console.WriteLine($"  {i + 1}. {iceCream.Option} ({iceCream.Scoops} scoops) - ${price:F2}");
            }

            var estimatedTotal = order.CalculateTotal(_dataManager.OptionsDict, _dataManager.AllFlavours,
                _dataManager.AllToppings, customer.Rewards, false);
            Console.WriteLine($"\nEstimated Total: ${estimatedTotal:F2}");

            if (customer.IsBirthday())
            {
                Console.WriteLine("🎂 Birthday discount will be applied during checkout!");
            }

            if (customer.Rewards.PunchCard >= AppConstants.PUNCH_CARD_LIMIT)
            {
                Console.WriteLine("🎫 Punch card reward available!");
            }
        }

        /// <summary>
        /// Displays ice cream summary after creation
        /// </summary>
        private void DisplayIceCreamSummary(IceCream iceCream, int itemNumber)
        {
            Console.WriteLine($"\n--- Ice Cream #{itemNumber} Summary ---");
            Console.WriteLine($"Type: {iceCream.Option}");
            Console.WriteLine($"Scoops: {iceCream.Scoops}");

            if (iceCream.Flavours.Any())
            {
                Console.WriteLine("Flavours:");
                foreach (var flavour in iceCream.Flavours)
                {
                    Console.WriteLine($"  • {flavour.Type} {(flavour.Premium ? "(Premium)" : "")}");
                }
            }

            if (iceCream.Toppings.Any())
            {
                Console.WriteLine("Toppings:");
                foreach (var topping in iceCream.Toppings)
                {
                    Console.WriteLine($"  • {topping.Type}");
                }
            }

            // Display special properties
            if (iceCream is Cone cone)
            {
                Console.WriteLine($"Chocolate Dipped: {(cone.Dipped ? "Yes" : "No")}");
            }
            else if (iceCream is Waffle waffle)
            {
                Console.WriteLine($"Waffle Flavour: {waffle.WaffleFlavour}");
            }

            var price = iceCream.CalculatePrice(_dataManager.OptionsDict, _dataManager.AllFlavours, _dataManager.AllToppings);
            Console.WriteLine($"Price: ${price:F2}");
        }

        #endregion

        #region Private Helper Methods - Order Display

        /// <summary>
        /// Displays orders in a queue with color coding
        /// </summary>
        private void DisplayOrderQueue(string queueName, Queue<Order> queue, ConsoleColor color)
        {
            var originalColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = color;
                Console.WriteLine($"\n--- {queueName} ---");
                Console.ForegroundColor = originalColor;

                if (!queue.Any())
                {
                    _consoleHelper.DisplayInfoMessage("No orders in queue.");
                    return;
                }

                foreach (var (order, position) in queue.Select((o, i) => (o, i + 1)))
                {
                    Console.WriteLine($"\n{position}. Order ID: {order.Id}");
                    Console.WriteLine($"   Time: {order.TimeReceived:HH:mm:ss}");
                    Console.WriteLine($"   Items: {order.IceCreamList.Count}");

                    var customer = FindCustomerByOrder(order);
                    if (customer != null)
                    {
                        Console.WriteLine($"   Customer: {customer.Name}");
                        var total = order.CalculateTotal(_dataManager.OptionsDict, _dataManager.AllFlavours,
                            _dataManager.AllToppings, customer.Rewards, false);
                        Console.WriteLine($"   Est. Total: ${total:F2}");
                    }
                }
            }
            finally
            {
                Console.ForegroundColor = originalColor;
            }
        }

        /// <summary>
        /// Gets customers who have current orders
        /// </summary>
        private List<Customer> GetCustomersWithCurrentOrders()
        {
            return _dataManager.Customers.Where(c => c.CurrentOrder != null).ToList();
        }

        /// <summary>
        /// Allows user to select a customer with an order
        /// </summary>
        private Customer SelectCustomerWithOrder(List<Customer> customersWithOrders)
        {
            Console.WriteLine("\nCustomers with current orders:");
            for (int i = 0; i < customersWithOrders.Count; i++)
            {
                var customer = customersWithOrders[i];
                Console.WriteLine($"{i + 1,3}. {customer.Name} (ID: {customer.MemberId}) - " +
                                $"{customer.CurrentOrder.IceCreamList.Count} item(s)");
            }

            var selection = _consoleHelper.GetValidIntegerInput(
                "Select customer", 1, customersWithOrders.Count);

            return selection.HasValue ? customersWithOrders[selection.Value - 1] : null;
        }

        /// <summary>
        /// Displays current order summary for modification
        /// </summary>
        private void DisplayCurrentOrderSummary(Order order)
        {
            _consoleHelper.DisplaySeparator("CURRENT ORDER SUMMARY");

            Console.WriteLine($"Order ID: {order.Id}");
            Console.WriteLine($"Order Time: {order.TimeReceived:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Items in order: {order.IceCreamList.Count}");

            for (int i = 0; i < order.IceCreamList.Count; i++)
            {
                var iceCream = order.IceCreamList[i];
                var price = iceCream.CalculatePrice(_dataManager.OptionsDict, _dataManager.AllFlavours, _dataManager.AllToppings);

                Console.WriteLine($"\n{i + 1}. {iceCream.Option} - {iceCream.Scoops} scoop(s) - ${price:F2}");

                if (iceCream.Flavours.Any())
                {
                    var flavourList = string.Join(", ", iceCream.Flavours.Select(f =>
                        f.Premium ? $"{f.Type} (Premium)" : f.Type));
                    Console.WriteLine($"   Flavours: {flavourList}");
                }

                if (iceCream.Toppings.Any())
                {
                    var toppingList = string.Join(", ", iceCream.Toppings.Select(t => t.Type));
                    Console.WriteLine($"   Toppings: {toppingList}");
                }
            }
        }

        #endregion

        #region Helper Classes

        /// <summary>
        /// Result object for order creation operations
        /// </summary>
        private class OrderCreationResult
        {
            public bool Success { get; set; }
            public int IceCreamsAdded { get; set; }
            public string Message { get; set; }
        }

        /// <summary>
        /// Result object for order modification operations
        /// </summary>
        private class OrderModificationResult
        {
            public bool HasChanges { get; set; }
            public string Summary { get; set; }
        }

        /// <summary>
        /// Result object for checkout operations
        /// </summary>
        private class CheckoutResult
        {
            public bool Success { get; set; }
            public double FinalTotal { get; set; }
            public string Summary { get; set; }
        }

        #endregion

        // Note: This is Part 1 of the OrderService. Part 2 will include:
        // - Order modification methods
        // - Checkout processing
        // - Analytics and reporting
        // - Additional utility methods

        #region Placeholder Methods (to be implemented in Part 2)

        private OrderModificationResult ProcessOrderModifications(Order order)
        {
            // Implementation in Part 2
            return new OrderModificationResult { HasChanges = false };
        }

        private void UpdateOrderInQueue(Customer customer)
        {
            // Implementation in Part 2
        }

        private (string, Order) GetNextOrderFromQueue()
        {
            // Implementation in Part 2
            return (null, null);
        }

        private Customer FindCustomerByOrder(Order order)
        {
            return _dataManager.Customers.FirstOrDefault(c => c.CurrentOrder?.Id == order.Id);
        }

        private CheckoutResult ProcessCheckout(Customer customer, Order order)
        {
            // Implementation in Part 2
            return new CheckoutResult { Success = false };
        }

        private void CompleteOrder(Customer customer, Order order, double finalTotal)
        {
            // Implementation in Part 2
        }

        private int? GetValidBusinessYear()
        {
            return _consoleHelper.GetValidIntegerInput("Enter year", 2000, DateTime.Now.Year);
        }

        private object GenerateYearlyAnalytics(int year)
        {
            // Implementation in Part 2
            return null;
        }

        private void DisplayComprehensiveAnalytics(object analytics)
        {
            // Implementation in Part 2
        }

        #endregion
    }
}