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

    }
    else if (option == 5)
    {

    }
    else if (option == 6)
    {

    }






}