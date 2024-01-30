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

//test the data from the csv file

var flavours = ReadFlavoursCsv("flavours.csv");

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
            Premium = cost > 0, // If the cost is greater than 0, it's considered a premium flavour
            // Assuming Quantity is set elsewhere, as it's not part of the CSV
        };

        flavours.Add(flavour);
    }

    return flavours;
}

var toppings = ReadToppingsCsv("toppings.csv");

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

var optionsDict = ReadOptionsCsv("options.csv");

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

List<Customer> customers = new List<Customer>();

void ReadCustomerCsv(List<Customer> customers)
{
    using(StreamReader sr = new StreamReader("customers.csv"))
    {

        sr.ReadLine();

        string line;
        while((line = sr.ReadLine()) != null)
        {
            string[] data = line.Split(',');

            string name = data[0];
            int memberId = int.Parse(data[1]);
            DateTime dob = DateTime.ParseExact(data[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string tier = data[3];
            int points = int.Parse(data[4]);
            int punchCard = int.Parse(data[5]);


            PointCard rewards = new PointCard(points,punchCard)
            {
                Tier = tier

            };

            Customer customer = new Customer(name, memberId, dob)
            {
                Rewards = rewards,
                OrderHistory = new List<Order>()



            };

            customers.Add(customer);



        }

    }

}

ReadCustomerCsv(customers);

void ListAllCustomers(List<Customer> customers)
{
    Console.WriteLine("{0,-20}{1,-20}{2,-20}{3,-20}{4,-20}{5,-20}",
        "Name", "Member ID", "DOB", "MemberShipStatus", "MemberShipPoints", "PunchCard"); 
    foreach (Customer customer in customers) 
    {
        Console.WriteLine("{0,-20}{1,-20}{2,-20}{3,-20}{4,-20}{5,-20}",
            customer.Name, customer.MemberId, customer.Dob.ToString("dd/MM/yyyy")
            ,customer.Rewards.Tier ,customer.Rewards.Points,customer.Rewards.PunchCard);

    }




}


//option 2

void ListAllCurrentOrders(List<Customer> customers)
{
    Console.WriteLine("Gold Member Orders: ");
    Console.WriteLine("{0,-20}{1,-20}{2,-20}","Order ID","Customer","Time Received");
    foreach (Customer customer in customers.Where(c => c.Rewards.Tier == "Gold"))
    {
        foreach(var order in customer.OrderHistory.Where(o => o.TimeFulfilled == null))
        {
            Console.WriteLine("{0,-20}{1,-20}{2,-20}", order.Id,customer.Name,order.TimeReceived);
        }
    
    
    
    }

    Console.WriteLine("\nRegular Member Orders: ");
    Console.WriteLine("{0,-20}{1,-20}{2,-20}", "Order ID", "Customer", "Time Received");
    foreach (Customer customer in customers.Where(c => c.Rewards.Tier != "Gold"))
    {
        foreach (var order in customer.OrderHistory.Where(o => o.TimeFulfilled == null))
        {
            Console.WriteLine("{0,-20}{1,-20}{2,-20}", order.Id, customer.Name, order.TimeReceived);
        }



    }
}



// Option 3

void RegisterNewCustomer(List<Customer> customers)
{
    Console.Write("Enter customer's name: ");
    string name = Console.ReadLine();

    Console.Write("Enter customer's ID number: ");
    int memberId = Convert.ToInt32(Console.ReadLine());

    Console.Write("Enter customer's date of birth (dd/MM/yyyy): ");
    DateTime dob = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

    Customer newCustomer = new Customer(name, memberId, dob)
    {
        Rewards = new PointCard()

    };

    customers.Add(newCustomer);

    AppendCustomerToCsv(newCustomer);

    Console.WriteLine("Customer registration successful !");


}

void AppendCustomerToCsv(Customer customer)
{
    string newLine = $"{customer.Name},{ customer.MemberId},{ customer.Dob.ToString("dd/MM/yyyy")}," +
        $" {customer.Rewards.Tier},{customer.Rewards.Points},{customer.Rewards.PunchCard}";

    using (StreamWriter sw = File.AppendText("customers.csv"))
    {
        sw.WriteLine(newLine);
    }


}



// option 4

Customer? SelectCustomer(List<Customer> customers)
{
    Console.Write("Enter the ID of the customer: ");
    int id = Convert.ToInt32(Console.ReadLine());


    Customer? selectedCustomer = customers.FirstOrDefault(c => c.MemberId == id);
    if (selectedCustomer != null)
    {
        return selectedCustomer;
        
    }
    else
    {
        Console.WriteLine("No customer found with the provided ID.");
        return null;
    }
 

}

Order CreateCustomerOrder()
{
    Order order = new Order();

    bool addMore;
    do
    {
        Console.Write("Enter ice cream option (Cup/Cone/Waffle): ");
        string option = Console.ReadLine();

        Console.Write("Enter number of scoops: ");
        int scoops = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter flavour: ");
        string flavourName = Console.ReadLine();

        Console.Write("Is this a premium flavour? (Y/N): ");
        bool isPremium = Console.ReadLine().Trim().ToUpper() == "Y";

        int quantity = 1;

        Flavour flavour = new Flavour(flavourName,isPremium,quantity);

        Console.Write("Enter topping: ");
        string topping = Console.ReadLine();

        IceCream iceCream = null;
        switch (option.ToLower())
        {
            case "cup":
                List<Flavour> flavours = new List<Flavour> {flavour};
                List<Topping> toppings = new List<Topping>{new Topping(topping)};

                iceCream = new Cup("Cup", scoops, flavours, toppings);
                break;

        }

        if (iceCream != null)
        {
            order.IceCreamList.Add(iceCream);
        }


        Console.Write("Would you like to add another ice cream to the order? (Y/N): ");
        addMore = Console.ReadLine().ToUpper() == "Y";

    }while (addMore);

    return order;



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

    Console.WriteLine("Order has been made successfully!");







}


//option 5 

void DisplayOrderCustomer(List<Customer> customers)
{
    ListAllCustomers(customers);

    Console.Write("Select a customer id you want to retrived Order History: ");
    int id = Convert.ToInt32(Console.ReadLine());


    Customer selectedCustomer = customers.FirstOrDefault(c => c.MemberId == id);


    if(selectedCustomer != null)
    {
        Console.WriteLine($"Order for {selectedCustomer.Name}:");
        foreach (var order in selectedCustomer.OrderHistory)
        {
            Console.WriteLine($"Order ID: {order.Id}, Received: {order.TimeReceived}");
        }


      
    }
    else 
    {
        Console.WriteLine("Customer not found.");

    }

    








}


//option 6
void ModifyOrderDetails(List<Customer> customers)
{
    ListAllCustomers(customers);
    Customer selectedCustomer = SelectCustomer(customers);

    if(selectedCustomer == null || selectedCustomer.CurrentOrder == null)
    {
        Console.WriteLine("Invalid customer or no current order.");
        return;



    }

    Console.WriteLine("Current Ice Creams in the Order:");
    for(int i = 0;i < selectedCustomer.CurrentOrder.IceCreamList.Count;i++)
    {

        Console.WriteLine($"{i + 1}.{selectedCustomer.CurrentOrder.IceCreamList[i]}");

    }

    Console.WriteLine("Choose an action: " +
        "\n[1] Modify an Ice"+"\n[2] Add a New Ice Cream"+"\n[3] Delete an Ice Cream");

    int action = Convert.ToInt32(Console.ReadLine());

    switch(action) 
    {
        case 1:
            ModifyIceCream(selectedCustomer.CurrentOrder);
            break;
        case 2:
            IceCream newIceCream = CreateCustomerOrder().IceCreamList.FirstOrDefault();
            if(newIceCream != null)
            {
                selectedCustomer.CurrentOrder.IceCreamList.Add(newIceCream);
            }
            break;
        case 3:
            DeleteIceCream(selectedCustomer.CurrentOrder);
            break;
    
    
    
    }

    Console.WriteLine("Updated Order:");
    foreach(var iceCream in selectedCustomer.CurrentOrder.IceCreamList)
    {
        Console.WriteLine(iceCream);
    }






}

void ModifyIceCream(Order order)
{
    Console.Write("Select the number of the ice cream to modify: ");
    int index = Convert.ToInt32(Console.ReadLine()) - 1;

    if(index >= 0 && index < order.IceCreamList.Count)
    {
        IceCream iceCreamToModify = order.IceCreamList[index];

        Console.Write("Enter new ice cream option (Cup/Cone/Waffle): ");
        string option = Console.ReadLine();

        Console.Write("Enter new number of scoops: ");
        int scoops = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter new flavour: ");
        string flavourName = Console.ReadLine();

        Console.Write("Is this a premium flavour? (Y/N): ");
        bool isPremium = Console.ReadLine().Trim().ToUpper() == "Y";

        Console.Write("Enter new topping: ");
        string toppingName = Console.ReadLine();

        iceCreamToModify.Option = option;
        iceCreamToModify.Scoops = scoops;
        iceCreamToModify.Flavours = new List<Flavour> {new Flavour(flavourName, isPremium ,1 )};
        iceCreamToModify.Toppings = new List<Topping> { new Topping(toppingName) };




    }
    else
    {
        Console.WriteLine("Invalid selection");
    }




}


void DeleteIceCream(Order order)
{
    Console.Write("Select the number of the ice cream to delete: ");
    int index = Convert.ToInt32(Console.ReadLine()) - 1;

    if (index >= 0 && index < order.IceCreamList.Count)
    {
        if (order.IceCreamList.Count > 1)
        {
            order.IceCreamList.RemoveAt(index);



        }
        else
        {
            Console.WriteLine("Cannot delete the only ice cream in the order.");

        }
    }
    else 
    {
        Console.WriteLine("Invalid selection");

    }





}




//Advance feature
//Option 7 

void ProcessAndCheckOutOrder()
{
    if (goldMemberOrders.Count == 0 && regularOrders.Count == 0) 
    {
        Console.WriteLine("There are ");



    }



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
    Console.WriteLine("7 ) ");
    Console.WriteLine("0 EXIT");



}
















while (true)
{
    displaymenu();

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
        ListAllCurrentOrders(customers);


    }
    else if (option == 3) 
    {
        RegisterNewCustomer(customers);

    }
    else if (option == 4)
    {
        ListAllCustomers(customers);
        Customer selectedCustomer = SelectCustomer(customers);
        if (selectedCustomer != null) 
        {
            Order newOrder = CreateCustomerOrder();
            ProcessOrder(selectedCustomer, newOrder);
        
        }
        else
        {
            Console.WriteLine("Customer not found.");
        }
        
        
        
       
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
        
    
    }






}