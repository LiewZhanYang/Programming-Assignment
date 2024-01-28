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







































void displaymenu()
{
    Console.WriteLine("------ Menu ------");
    Console.WriteLine("1 ) List All Customer");
    Console.WriteLine("2 ) List All Current Order");
    Console.WriteLine("3 ) Register A New Customer");
    Console.WriteLine("4 ) Create A Customer's Order");
    Console.WriteLine("5 ) Display Order Details Of A Customer");
    Console.WriteLine("6 ) Modify Order Details");
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
        SelectCustomer(customers);
        CreateCustomerOrder();


    }
    else if (option == 5)
    {

    }
    else if (option == 6)
    {

    }






}