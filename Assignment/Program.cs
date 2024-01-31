//==========================================================
// Student Number : S10259432
// Student Name : Liew Zhan Yang
// Partner Number : S10257777
// Partner Name : Amicus Lee Ming Ge
//==========================================================

using Assignment;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using static System.Formats.Asn1.AsnWriter;


List<Flavour> allFlavours = ReadFlavoursCsv("flavours.csv");
List<Topping> allToppings = ReadToppingsCsv("toppings.csv");
Dictionary<string, double> optionsDict = ReadOptionsCsv("options.csv");
Queue<Order> goldQueue = new Queue<Order>();
Queue<Order> ordinaryQueue = new Queue<Order>();


//test the data from the csv file


List<Flavour> ReadFlavoursCsv(string filePath)
{
    var flavours = new List<Flavour>();
    var lines = File.ReadAllLines(filePath);

    foreach (var line in lines.Skip(1)) // Skip the header line
    {
        var parts = line.Split(',');
        var flavourName = parts[0];
        var cost = double.Parse(parts[1]);

        var flavour = new Flavour
        {
            Type = flavourName,
            Premium = cost > 0,
           // If the cost is greater than 0, it's considered a premium flavour
            // Assuming Quantity is set elsewhere, as it's not part of the CSV
        };

        flavours.Add(flavour);
    }

    return flavours;
}



List<Topping> ReadToppingsCsv(string filePath)
{
    var toppings = new List<Topping>();
    var lines = File.ReadAllLines(filePath);

    foreach (var line in lines.Skip(1)) // Skip the CSV header
    {
        var parts = line.Split(',');
        if (parts.Length >= 2)
        {
            var type = parts[0].Trim(); // Get the topping type
            // Optionally handle the cost if needed

            toppings.Add(new Topping(type));
        }
    }

    return toppings;
}



Dictionary<string, double> ReadOptionsCsv(string filePath)
{
    var optionsDict = new Dictionary<string, double>();
    var lines = File.ReadAllLines(filePath);

    foreach (var line in lines.Skip(1)) // Skip the CSV header
    {
        var parts = line.Split(',');
        var key = $"{parts[0].Trim()}-{parts[1].Trim()}-{parts[2].Trim()}-{parts[3].Trim()}"; // Composite key
        var cost = double.Parse(parts[4]);

        optionsDict[key] = cost;
    }

    return optionsDict;
}


void DisplayFlavours(List<Flavour> flavours)
{
    Console.WriteLine("Flavours:");
    foreach (var flavour in flavours)
    {
        Console.WriteLine($"Type: {flavour.Type}, Premium: {flavour.Premium}");
    }
}

//DisplayFlavours(flavours);

void DisplayToppings(List<Topping> toppings)
{
    Console.WriteLine("Toppings:");
    foreach (var topping in toppings)
    {
        Console.WriteLine($"Type: {topping.Type}");
    }
}

//DisplayToppings(toppings);

void DisplayOptions(Dictionary<string, double> optionsDict)
{
    Console.WriteLine("Options:");
    foreach (var option in optionsDict)
    {
        Console.WriteLine($"Key: {option.Key}, Cost: {option.Value}");
    }
}

//DisplayOptions(optionsDict);





// option 1

// Create an empty list to store Customer objects
List<Customer> customers = new List<Customer>();

// Read data from a CSV file and populate the 'customers' list
void ReadCustomerCsv(List<Customer> customers)
{
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        // Skip the header line in the CSV file
        sr.ReadLine();

        string line;
        // Read each line of the CSV file
        while ((line = sr.ReadLine()) != null)
        {
            // Split the line into individual data elements
            string[] data = line.Split(',');

            // Extract customer data from the CSV
            string name = data[0];
            int memberId = int.Parse(data[1]);
            DateTime dob = DateTime.ParseExact(data[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string tier = data[3];
            int points = int.Parse(data[4]);
            int punchCard = int.Parse(data[5]);

            // Create a PointCard object for customer rewards
            PointCard rewards = new PointCard(points, punchCard)
            {
                Tier = tier
            };

            // Create a Customer object and set its properties
            Customer customer = new Customer(name, memberId, dob)
            {
                Rewards = rewards,
                OrderHistory = new List<Order>()
            };

            // Add the customer to the 'customers' list
            customers.Add(customer);
        }
    }
}

// Call the 'ReadCustomerCsv' method to populate the 'customers' list
ReadCustomerCsv(customers);

// Display a table of customer information
void ListAllCustomers(List<Customer> customers)
{
    // Display column headers
    Console.WriteLine("{0,-20}{1,-20}{2,-20}{3,-20}{4,-20}{5,-20}",
        "Name", "Member ID", "DOB", "Membership Status", "Membership Points", "PunchCard");

    // Iterate through the 'customers' list and display each customer's information
    foreach (Customer customer in customers)
    {
        // Display customer details in a formatted table
        Console.WriteLine("{0,-20}{1,-20}{2,-20}{3,-20}{4,-20}{5,-20}",
            customer.Name, customer.MemberId, customer.Dob.ToString("dd/MM/yyyy"),
            customer.Rewards.Tier, customer.Rewards.Points, customer.Rewards.PunchCard);
    }
}



//option 2

// Display current orders for Gold and Regular members
void ListAllCurrentOrders(Queue<Order> goldQueue, Queue<Order> ordinaryQueue)
{
    Console.WriteLine();
    Console.WriteLine("Gold Member Orders: ");

    // Iterate through Gold member orders
    foreach (Order order in goldQueue)
    {
        // Display order time
        Console.WriteLine($"Order Time: {order.TimeReceived}");

        // Iterate through each ice cream in the order
        foreach (IceCream iceCream in order.IceCreamList)
        {
            // Display ice cream details
            Console.WriteLine($"Ice Cream Option: {iceCream.Option}, Scoops: {iceCream.Scoops}");

            // Iterate through ice cream flavors
            foreach (var flavour in iceCream.Flavours)
            {
                // Display flavor details
                Console.WriteLine($"Flavour: {flavour.Type}, Premium: {flavour.Premium}");
            }

            // Iterate through ice cream toppings
            foreach (var topping in iceCream.Toppings)
            {
                // Display topping details
                Console.WriteLine($"Topping: {topping.Type}");
            }

            Console.WriteLine(); // Empty line for separation
        }
        Console.WriteLine(); // Empty line for separation between orders
    }

    Console.WriteLine("\nRegular Member Orders: ");

    // Iterate through Regular member orders
    foreach (Order order in ordinaryQueue)
    {
        // Display order time
        Console.WriteLine($"Order Time: {order.TimeReceived}");

        // Iterate through each ice cream in the order
        foreach (IceCream iceCream in order.IceCreamList)
        {
            // Display ice cream details
            Console.WriteLine($"Ice Cream Option: {iceCream.Option}, Scoops: {iceCream.Scoops}");

            // Iterate through ice cream flavors
            foreach (var flavour in iceCream.Flavours)
            {
                // Display flavor details
                Console.WriteLine($"Flavour: {flavour.Type}, Premium: {flavour.Premium}");
            }

            // Iterate through ice cream toppings
            foreach (var topping in iceCream.Toppings)
            {
                // Display topping details
                Console.WriteLine($"Topping: {topping.Type}");
            }

            Console.WriteLine(); // Empty line for separation
        }
        Console.WriteLine(); // Empty line for separation between orders
    }
}



// Option 3

// Register a new customer and add them to the 'customers' list and CSV file
void RegisterNewCustomer(List<Customer> customers)
{
    // Prompt for customer information
    Console.Write("Enter customer's name: ");
    string name = Console.ReadLine();
    int memberId;
    while (true)
    {
        try
        {
            Console.Write("Enter customer's ID number: ");
            memberId = Convert.ToInt32(Console.ReadLine());
            break;
        }
        catch (Exception)
        {
            Console.WriteLine("Please Enter a valid ID number");
        }
    }
    
    Console.Write("Enter customer's date of birth (dd/MM/yyyy): ");
    DateTime dob = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

    // Create a new Customer object with provided information and an empty rewards card
    Customer newCustomer = new Customer(name, memberId, dob)
    {
        Rewards = new PointCard()
    };

    // Add the new customer to the 'customers' list
    customers.Add(newCustomer);

    // Append customer information to the CSV file
    AppendCustomerToCsv(newCustomer);

    Console.WriteLine("Customer registration successful!");
}

// Append customer information to a CSV file
void AppendCustomerToCsv(Customer customer)
{
    // Create a new CSV line from customer details
    string newLine = $"{customer.Name},{customer.MemberId},{customer.Dob.ToString("dd/MM/yyyy")}," +
        $"{customer.Rewards.Tier},{customer.Rewards.Points},{customer.Rewards.PunchCard}";

    // Append the new line to the CSV file
    using (StreamWriter sw = File.AppendText("C:\\Users\\User\\Documents\\Ngee Ann Poly Module\\Sem 2 Ngee Ann IT courses modules\\a Programming 2\\Assignment\\Assignment\\Assignment\\Assignment\\customers.csv"))
    {
        sw.WriteLine(newLine);
    }
}




// option 4

void CreateOrder(List<Customer>customers, Queue<Order> goldQueue, Queue<Order> ordinaryQueue, List<Flavour> flavours, List<Topping> toppings)
{
    ListAllCustomers(customers);

    int memberID;
    Customer customer = null;

    while (true)
    {
        Console.Write("Enter Customer's MemberID: ");
        if (int.TryParse(Console.ReadLine(), out memberID))
        {
            customer = customers.FirstOrDefault(c => c.MemberId == memberID);
            if (customer != null)
            {
                if (customer.CurrentOrder == null)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Continue your order thank you.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Member ID.");
            }
        }
        else
        {
            Console.WriteLine("Please enter valid Member ID.");
        }
    }


    customer.MakeOrder();
    bool addMoreIceCream;

    do
    {
        Console.Write("\nEnter IceCream Option (Cup / Cone / Waffle): ");
        string option = Console.ReadLine();

        int scoops;
        do
        {
            Console.Write("Number of Scoops (max 3): ");
        } while (!int.TryParse(Console.ReadLine(), out scoops) || scoops < 1 || scoops > 3);

        Console.WriteLine("\nRegular :");
        flavours.Where(f => !f.Premium).ToList().ForEach(f => Console.WriteLine(f.Type));

        Console.WriteLine("\nPremium :");
        flavours.Where(f => f.Premium).ToList().ForEach(f => Console.WriteLine(f.Type));

        List<Flavour> selectedFlavours = new List<Flavour>();
        for (int i = 0; i < scoops; i++)
        {
            Console.Write("Enter the Flavour you want: ");
            string flavourName = Console.ReadLine();
            Flavour selectedFlavour = flavours.FirstOrDefault(f => f.Type.Equals(flavourName, StringComparison.OrdinalIgnoreCase));
            if (selectedFlavour != null)
            {
                selectedFlavours.Add(selectedFlavour);
            }
            else
            {
                Console.WriteLine("Invalid Flavour");
                i--;  // Decrement counter to retry this scoop
            }
        }

        Console.WriteLine("\nToppings:");
        Console.WriteLine("-----------");
        toppings.ForEach(t => Console.WriteLine(t.Type));

        List<Topping> selectedToppings = new List<Topping>();
        string toppingInput;
        do
        {
            Console.Write("Enter Toppings ('X' Exit): ");
            toppingInput = Console.ReadLine();
            if (toppingInput.ToUpper() != "X")
            {
                Topping selectedTopping = toppings.FirstOrDefault(t => t.Type.Equals(toppingInput, StringComparison.OrdinalIgnoreCase));
                if (selectedTopping != null)
                {
                    selectedToppings.Add(selectedTopping);
                }
                else
                {
                    Console.WriteLine("Enter valid topping");
                }
            }
        } while (toppingInput.ToUpper() != "X");

        // Inside your do-while loop in the CreateOrder method

        IceCream iceCream = null; // Declare iceCream as null initially
        switch (option.ToLower())
        {
            case "cup":
                iceCream = new Cup(scoops, selectedFlavours, selectedToppings);
                break;
            case "cone":
                Console.Write("Want cone dipped? (Y/N): ");
                string dippedResponse = Console.ReadLine();
                bool isDipped = dippedResponse.Equals("Y", StringComparison.OrdinalIgnoreCase);
                // Assuming Cone constructor similar to Cup
                iceCream = new Cone(scoops, selectedFlavours, selectedToppings,isDipped);
                break;
            case "waffle":
                Console.Write("Enter Waffle Flavour: ");
                string waffleFlavour = Console.ReadLine();
                // Assuming Waffle constructor similar to Cup
                iceCream = new Waffle(scoops, selectedFlavours, selectedToppings,waffleFlavour);
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }

        if (iceCream != null)
        {
            customer.CurrentOrder.AddiceCream(iceCream);
            Console.Write("Add another ice cream to the order? (Y/N): ");
            addMoreIceCream = Console.ReadLine().ToUpper() == "Y";
        }
        else
        {
            // Handle the case when an invalid option is entered, and iceCream remains null
            Console.WriteLine("Please enter a valid ice cream option.");
            // Set addMoreIceCream to false to exit the loop
            addMoreIceCream = false;
        }

    } while (addMoreIceCream);

    if (customer.Rewards.Tier.ToLower() == "gold")
    {
        goldQueue.Enqueue(customer.CurrentOrder);
    }
    else
    {
        ordinaryQueue.Enqueue(customer.CurrentOrder);
    }


    Console.WriteLine("Order has been made successfully");
}


List<Order> goldMemberOrders = new List<Order>();
List<Order> regularOrders = new List<Order>();

void ProcessOrder(Customer customer, Order order)
{
    customer.CurrentOrder = order;
    customer.OrderHistory.Add(order);

    if (customer.Rewards.Tier == "Gold")
    {
        goldMemberOrders.Add(order);



    }
    else
    {
        regularOrders.Add(order);

    }

    customer.CurrentOrder = order;

    Console.WriteLine("Order has been made successfully!");







}


//option 5 

void DisplayOrderCustomer(List<Customer> customers)
{
    ListAllCustomers(customers);

    Console.WriteLine();
    Console.Write("Select a customer id you want to retrieve Order History: ");
    int id = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine();

    Customer selectedCustomer = customers.FirstOrDefault(c => c.MemberId == id);

    if (selectedCustomer != null)
    {
        foreach (Order order in selectedCustomer.OrderHistory)
        {
            Console.WriteLine($"Order Time: {order.TimeReceived}");

            foreach (IceCream iceCream in order.IceCreamList)
            {
                // Simplified display logic for ice cream details
                Console.WriteLine($"Ice Cream Option: {iceCream.Option}, Scoops: {iceCream.Scoops}");
                foreach (var flavour in iceCream.Flavours)
                {
                    Console.WriteLine($"Flavour: {flavour.Type}, Premium: {flavour.Premium}");
                }
                foreach (var topping in iceCream.Toppings)
                {
                    Console.WriteLine($"Topping: {topping.Type}");
                }

                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
    else
    {
        Console.WriteLine("Customer not found.");
    }
}











////option 6
///

Flavour ChooseFlavour(List<Flavour> availableFlavours)
{
    Console.WriteLine("Available Flavours:");
    for (int i = 0; i < availableFlavours.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {availableFlavours[i].Type} {(availableFlavours[i].Premium ? "(Premium)" : "")}");
    }

    Console.Write("Select a flavour by number: ");
    int selection = Convert.ToInt32(Console.ReadLine()) - 1; // Adjusting for zero-based index

    if (selection >= 0 && selection < availableFlavours.Count)
    {
        return availableFlavours[selection];
    }
    else
    {
        Console.WriteLine("Invalid selection.");
        return null; // Or handle invalid selection differently
    }
}

List<Topping> ChooseToppings(List<Topping> availableToppings)
{
    List<Topping> selectedToppings = new List<Topping>();

    Console.WriteLine("Available Toppings:");
    for (int i = 0; i < availableToppings.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {availableToppings[i].Type}");
    }
    Console.WriteLine($"{availableToppings.Count + 1}. Done selecting");

    while (true)
    {
        Console.Write("Select a topping by number (or select 'Done selecting' to finish): ");
        int selection = Convert.ToInt32(Console.ReadLine()) - 1; // Adjusting for zero-based index

        if (selection >= 0 && selection < availableToppings.Count)
        {
            selectedToppings.Add(availableToppings[selection]);
            Console.WriteLine($"Added {availableToppings[selection].Type} to the toppings.");
        }
        else if (selection == availableToppings.Count)
        {
            // User is done selecting toppings
            break;
        }
        else
        {
            Console.WriteLine("Invalid selection. Please try again.");
        }
    }

    return selectedToppings;
}

void AddNewIceCream(Order order, List<Flavour> allFlavours, List<Topping> allToppings)
{
    Console.Write("Pick your ice cream base option (Cup/Cone/Waffle): ");
    string baseOption = Console.ReadLine().ToLower();
    IceCream newIceCream;

    // Prompt for the number of scoops first
    Console.Write("How many scoops of ice cream would you like (1 - 3): ");
    int scoops = Convert.ToInt32(Console.ReadLine());

    // Then, select flavors based on the number of scoops
    List<Flavour> flavours = new List<Flavour>();
    for (int i = 0; i < scoops; i++)
    {
        Flavour flavour = ChooseFlavour(allFlavours);
        if (flavour != null)
        {
            flavours.Add(flavour);
        }
    }

    // Next, choose toppings
    List<Topping> toppings = ChooseToppings(allToppings);

    switch (baseOption)
    {
        case "cup":
            newIceCream = new Cup(scoops, flavours, toppings);
            break;
        case "cone":
            newIceCream = new Cone(scoops, flavours, toppings, false); // Temporarily set dipped to false
            // Ask if the cone is dipped in chocolate after choosing flavours and toppings
            Console.Write("Is the cone dipped in chocolate? (Y/N): ");
            bool isDipped = Console.ReadLine().Trim().ToUpper() == "Y";
            ((Cone)newIceCream).Dipped = isDipped; // Cast to Cone and set the dipped property
            break;
        case "waffle":
            Console.Write("Enter Waffle Flavour: ");
            string waffleFlavour = Console.ReadLine().ToUpper();
            newIceCream = new Waffle(scoops, flavours, toppings, waffleFlavour);
            break;
        default:
            Console.WriteLine("Invalid option. Please select 'Cup', 'Cone', or 'Waffle'.");
            return; // Exit the method if the base option is invalid
    }

    // Finally, add the newly created ice cream to the order
    order.IceCreamList.Add(newIceCream);

    Console.WriteLine("New ice cream added to the order.");
}

void DeleteIceCream(Order order)
{
    if (order.IceCreamList.Count == 0)
    {
        Console.WriteLine("There are no ice creams to delete.");
        return;
    }

    // Check if there's only one ice cream left in the order
    if (order.IceCreamList.Count == 1)
    {
        Console.WriteLine("Cannot delete the only ice cream in the order.");
        return;
    }

    Console.WriteLine("Select which ice cream you would like to delete:");
    for (int i = 0; i < order.IceCreamList.Count; i++)
    {
        // Displaying each ice cream with an index for the user to select
        Console.WriteLine($"{i + 1} ) Ice Cream #{i + 1}");
    }

    Console.Write("Enter the number of the ice cream to delete: ");
    int index = Convert.ToInt32(Console.ReadLine()) - 1; // Convert user input to zero-based index

    if (index >= 0 && index < order.IceCreamList.Count)
    {
        // Remove the selected ice cream from the list
        order.IceCreamList.RemoveAt(index);
        Console.WriteLine("Ice cream successfully deleted!");
    }
    else
    {
        Console.WriteLine("Invalid selection. Please choose a valid ice cream number.");
    }
}




Customer SelectCustomer(List<Customer> customers)
{
    for (int i = 0; i < customers.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {customers[i].Name}");
    }
    Console.WriteLine("Select a customer by number:");
    int selected = Convert.ToInt32(Console.ReadLine()) - 1; // Subtract 1 to match list index
    if (selected >= 0 && selected < customers.Count)
    {
        return customers[selected];
    }
    else
    {
        Console.WriteLine("Invalid selection.");
        return null; // Or handle invalid selection differently
    }
}




void ModifyOrderDetails(List<Customer> customers)
{
    ListAllCustomers(customers);
    Customer selectedCustomer = SelectCustomer(customers);

    if (selectedCustomer == null || selectedCustomer.CurrentOrder == null)
    {
        Console.WriteLine("Invalid customer or no current order.");
        return;



    }

    Console.WriteLine("Current Ice Creams in the Order:");
    for (int i = 0; i < selectedCustomer.CurrentOrder.IceCreamList.Count; i++)
    {

        Console.WriteLine($"{i + 1}.{selectedCustomer.CurrentOrder.IceCreamList[i]}");

    }

    Console.WriteLine("Choose an action: " +
        "\n[1] Modify an Ice" + "\n[2] Add a New Ice Cream" + "\n[3] Delete an Ice Cream");

    int action = Convert.ToInt32(Console.ReadLine());

    switch (action)
    {
        case 1:
            ModifyIceCream(selectedCustomer.CurrentOrder);
            break;
        case 2:
            AddNewIceCream(selectedCustomer.CurrentOrder, allFlavours, allToppings);
            break;

        case 3:
            DeleteIceCream(selectedCustomer.CurrentOrder);
            break;



    }

    Console.WriteLine("Updated Order:");
    foreach (var iceCream in selectedCustomer.CurrentOrder.IceCreamList)
    {
        Console.WriteLine(iceCream);
    }

}

void ModifyIceCream(Order order)
{
    Console.Write("Select the number of the ice cream to modify: ");
    int index = Convert.ToInt32(Console.ReadLine()) - 1;

    if (index >= 0 && index < order.IceCreamList.Count)
    {
        IceCream iceCreamToModify = order.IceCreamList[index];

        Console.WriteLine("Please Choose 1 Option");
        Console.WriteLine("1 ) Option");
        Console.WriteLine("2 ) Number of Scoops");
        Console.WriteLine("3 ) Flavours");
        Console.WriteLine("4 ) Toppings");
        
        Console.Write("Enter the number : ");
        int choice = Convert.ToInt32(Console.ReadLine());

        switch (choice)
        {
            case 1:
                Console.Write("Change your Ice Cream option to Cone / Cup / Waffle: ");
                string newOption = Console.ReadLine().ToLower();
                switch (newOption)
                {
                    case "cone":
                        Console.Write("Is cone dipped ? (Yes == Y) ");
                        bool isDipped = Console.ReadLine().Trim().ToUpper() == "Y";
                        order.IceCreamList[index] = new Cone(iceCreamToModify.Scoops, iceCreamToModify.Flavours, iceCreamToModify.Toppings, isDipped);
                        break;
                    case "cup":
                        order.IceCreamList[index] = new Cup(iceCreamToModify.Scoops, iceCreamToModify.Flavours, iceCreamToModify.Toppings);
                        break;
                    case "waffle":
                        Console.Write("Enter Waffle Flavour: ");
                        string waffleFlavour = Console.ReadLine();
                        order.IceCreamList[index] = new Waffle(iceCreamToModify.Scoops, iceCreamToModify.Flavours, iceCreamToModify.Toppings, waffleFlavour);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please select Cone, Cup, or Waffle.");
                        break;

                }
                break;

            case 2:
                Console.Write("Enter new number of scoops: ");
                int newScoops = Convert.ToInt32(Console.ReadLine());
                iceCreamToModify.Scoops = newScoops;
                break;

            case 3:
                Console.WriteLine("Current Flavours:");
                for (int i = 0; i < iceCreamToModify.Flavours.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {iceCreamToModify.Flavours[i].Type}");
                }
                Console.Write("Do you want to [A]dd a new flavour or [R]emove an existing one? (A/R): ");
                string flavourChoice = Console.ReadLine().Trim().ToUpper();
                if (flavourChoice == "A")
                {
                    Flavour newFlavour = ChooseFlavour(allFlavours);
                    if (newFlavour != null)
                    {
                        iceCreamToModify.Flavours.Add(newFlavour);
                        Console.WriteLine($"Added {newFlavour.Type} to the flavours.");
                    }
                }
                else if (flavourChoice == "R")
                {
                    Console.Write("Enter the number of the flavour to remove: ");
                    int flavourIndex = Convert.ToInt32(Console.ReadLine()) - 1;
                    if (flavourIndex >= 0 && flavourIndex < iceCreamToModify.Flavours.Count)
                    {
                        iceCreamToModify.Flavours.RemoveAt(flavourIndex);
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please select A to add or R to remove.");
                }
                break;

            case 4:
                Console.WriteLine("Current Toppings:");
                for (int i = 0; i < iceCreamToModify.Toppings.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {iceCreamToModify.Toppings[i].Type}");
                }
                Console.WriteLine("Do you want to [A]dd a new topping or [R]emove an existing one? (A/R): ");
                string toppingChoice = Console.ReadLine().Trim().ToUpper();
                if (toppingChoice == "A")
                {
                    List<Topping> newToppings = ChooseToppings(allToppings);
                    // You might want to replace the existing toppings or add to them, depending on your requirement
                    iceCreamToModify.Toppings = newToppings;  // This replaces the existing toppings
                    Console.WriteLine("Toppings updated.");
                }
                else if (toppingChoice == "R")
                {
                    Console.Write("Enter the number of the topping to remove: ");
                    int toppingIndex = Convert.ToInt32(Console.ReadLine()) - 1;
                    if (toppingIndex >= 0 && toppingIndex < iceCreamToModify.Toppings.Count)
                    {
                        iceCreamToModify.Toppings.RemoveAt(toppingIndex);
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please select A to add or R to remove.");
                }
                break;





                break;
            
                Console.WriteLine("Invalid selection.");
                break;
        }
    }
    else
    {
        Console.WriteLine("Invalid selection");
    }
}








////Advance feature
//Option 7 
void ProcessOrderAndCheckout(List<Customer> customers, Dictionary<string, double> optionsDict, List<Flavour> allFlavours, List<Topping> allToppings)
{
    if (customers.Count == 0)
    {
        Console.WriteLine("No customers to process.");
        return;
    }

    // Example of selecting a customer and order to process
    // This should be replaced with your logic for selecting the next order
    Customer currentCustomer = customers.FirstOrDefault(c => c.CurrentOrder != null);
    if (currentCustomer == null)
    {
        Console.WriteLine("No orders to process.");
        return;
    }

    Order currentOrder = currentCustomer.CurrentOrder;

    Console.WriteLine($"Processing order for {currentCustomer.Name}");
    Console.WriteLine($"Membership Status: {currentCustomer.Rewards.Tier}");
    Console.WriteLine($"Available Points: {currentCustomer.Rewards.Points}");

    // Displaying ice creams in the order
    foreach (IceCream iceCream in currentOrder.IceCreamList)
    {
        Console.WriteLine(iceCream);  // Ensure IceCream.ToString() provides detailed information
    }

    double totalBill = currentOrder.CalculateTotal(optionsDict, allFlavours, allToppings, currentCustomer.Rewards, false);

    // Applying Birthday Discount
    if (currentCustomer.IsBirthday())
    {
        IceCream mostExpensiveIceCream = currentOrder.IceCreamList.OrderByDescending(ic => ic.CalculatePrice(optionsDict, allFlavours, allToppings)).First();
        double discount = mostExpensiveIceCream.CalculatePrice(optionsDict, allFlavours, allToppings);
        totalBill -= discount;
        Console.WriteLine($"Happy Birthday {currentCustomer.Name}! You get a ${discount:0.00} discount on the most expensive ice cream.");
    }

    // Applying Punch Card Discount
    if (currentCustomer.Rewards.PunchCard >= 10)
    {
        IceCream firstIceCream = currentOrder.IceCreamList.First();
        double discount = firstIceCream.CalculatePrice(optionsDict, allFlavours, allToppings);
        totalBill -= discount;
        currentCustomer.Rewards.PunchCard = 0; // Resetting the punch card
        Console.WriteLine($"Punch card reward! ${discount:0.00} discount applied.");
    }

    // Redeeming Points
    if (currentCustomer.Rewards.Tier != "Ordinary" && currentCustomer.Rewards.Points > 0)
    {
        Console.Write("Would you like to use points to reduce your bill? (Y/N): ");
        string response = Console.ReadLine().ToUpper();
        if (response == "Y")
        {
            Console.Write($"How many points would you like to redeem? (1 point = $0.02, up to {currentCustomer.Rewards.Points} points): ");
            int pointsToRedeem = Convert.ToInt32(Console.ReadLine());
            double pointsValue = Math.Min(pointsToRedeem, currentCustomer.Rewards.Points) * 0.02;
            totalBill = Math.Max(0, totalBill - pointsValue);
            currentCustomer.Rewards.Points -= pointsToRedeem;
            Console.WriteLine($"You've redeemed {pointsToRedeem} points to reduce your bill by ${pointsValue:0.00}.");
        }
    }

    Console.WriteLine($"Total Bill: ${totalBill:0.00}");
    Console.WriteLine("Press any key to make payment...");
    Console.ReadKey();

    // Updating Customer Records
    currentOrder.TimeFulfilled = DateTime.Now;
    currentCustomer.OrderHistory.Add(currentOrder);
    currentCustomer.CurrentOrder = null; // Clear the current order

    // Updating Punch Card and Points
    foreach (var iceCream in currentOrder.IceCreamList)
    {
        currentCustomer.Rewards.PunchCard = Math.Min(currentCustomer.Rewards.PunchCard + 1, 10);
    }

    int pointsEarned = (int)(totalBill * 10); // Example: 10 points per dollar spent
    currentCustomer.Rewards.Points += pointsEarned;

    Console.WriteLine($"Order completed for {currentCustomer.Name}. Points earned: {pointsEarned}. Current points: {currentCustomer.Rewards.Points}.");
}


////option 8

void DisplayCharges(List<Customer> customers, Dictionary<string, double> optionsDict, List<Flavour> allFlavours, List<Topping> allToppings)
{
    int year;
    Console.Write("Enter the year (ex: 2023): ");
    while (!int.TryParse(Console.ReadLine(), out year) || year.ToString().Length != 4)
    {
        Console.WriteLine("Ensure it is a valid 4 digit number. Please retry.");
    }

    List<Order> consolidatedOrders = new List<Order>();

    // Iterate through all customers to gather orders from the specified year
    foreach (Customer customer in customers)
    {
        consolidatedOrders.AddRange(customer.OrderHistory.Where(o => o.TimeReceived.Year == year && o.TimeFulfilled.HasValue));
    }

    double[] monthlyTotals = new double[12];
    double totalAmt = 0;

    // Compute monthly totals
    foreach (Order order in consolidatedOrders)
    {
        int monthIndex = order.TimeReceived.Month - 1;
        double subtotal = order.CalculateTotal(optionsDict, allFlavours, allToppings, new PointCard(), false); // Assuming a default PointCard with no rewards for simplicity
        monthlyTotals[monthIndex] += subtotal;
        totalAmt += subtotal;
    }

    // Display monthly and total charges
    string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
    for (int i = 0; i < months.Length; i++)
    {
        Console.WriteLine($"{months[i]} {year}: \t${monthlyTotals[i]:0.00}");
    }
    Console.WriteLine($"\nTotal: \t\t${totalAmt:0.00}");
}


















void displaymenu()
{
    Console.WriteLine("------ Menu ------");
    Console.WriteLine("1 ) List All Customer");
    Console.WriteLine("2 ) List All Current Order");
    Console.WriteLine("3 ) Register A New Customer");
    Console.WriteLine("4 ) Create A Customer's Order");
    Console.WriteLine("5 ) Display Order Details Of A Customer");
    Console.WriteLine("6 ) Modify Order Details");
    Console.WriteLine("7 ) Process Order And CheckOut");
    Console.WriteLine("8 ) Display Monthly Charged Amounts Breakdown & Total Chraged Amounts For The Year");
    Console.WriteLine("0 EXIT");



}
















while (true)
{
    displaymenu();

    Console.WriteLine();
    Console.Write("What is your option: ");
    int option = Convert.ToInt32(Console.ReadLine());

    if (option == 0)
    {
        break;
    }
    else if (option == 1)
    {
        ListAllCustomers(customers);


    }
    else if (option == 2)
    {
        ListAllCurrentOrders(goldQueue, ordinaryQueue);


    }
    else if (option == 3)
    {
        RegisterNewCustomer(customers);

    }
    else if (option == 4)
    {
        CreateOrder(customers, goldQueue, ordinaryQueue, allFlavours, allToppings);



    }
    else if (option == 5)
    {
        DisplayOrderCustomer(customers);
    }
    else if (option == 6)
    {
        ModifyOrderDetails(customers);
    }
    else if (option == 7)
    {
        ProcessOrderAndCheckout(customers, optionsDict, allFlavours, allToppings);


    }
    else if (option == 8)
    {
        DisplayCharges(customers, optionsDict, allFlavours, allToppings);
    }
}







