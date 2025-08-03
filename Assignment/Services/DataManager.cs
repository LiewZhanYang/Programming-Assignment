//==========================================================
// Student Number : S10259432
// Student Name : Liew Zhan Yang
// Partner Number : S10257777
// Partner Name : Amicus Lee Ming Ge
//==========================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Assignment
{
    public class DataManager
    {
        public List<Customer> Customers { get; private set; }
        public List<Flavour> AllFlavours { get; private set; }
        public List<Topping> AllToppings { get; private set; }
        public Dictionary<string, double> OptionsDict { get; private set; }
        public Queue<Order> GoldQueue { get; private set; }
        public Queue<Order> OrdinaryQueue { get; private set; }

        public DataManager()
        {
            LoadAllData();
        }

        private void LoadAllData()
        {
            AllFlavours = ReadFlavoursCsv(AppConstants.FLAVOURS_FILE);
            AllToppings = ReadToppingsCsv(AppConstants.TOPPINGS_FILE);
            OptionsDict = ReadOptionsCsv(AppConstants.OPTIONS_FILE);
            Customers = new List<Customer>();
            ReadCustomerCsv();
            GoldQueue = new Queue<Order>();
            OrdinaryQueue = new Queue<Order>();
        }

        private List<Flavour> ReadFlavoursCsv(string filePath)
        {
            var flavours = new List<Flavour>();

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Warning: {filePath} not found. Using default flavours.");
                return GetDefaultFlavours();
            }

            try
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines.Skip(1)) // Skip header
                {
                    var parts = line.Split(',');
                    if (parts.Length >= 2)
                    {
                        var flavourName = parts[0].Trim();
                        if (double.TryParse(parts[1], out double cost))
                        {
                            flavours.Add(new Flavour(flavourName, cost > 0, 1));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading flavours: {ex.Message}");
                return GetDefaultFlavours();
            }

            return flavours;
        }

        private List<Topping> ReadToppingsCsv(string filePath)
        {
            var toppings = new List<Topping>();

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Warning: {filePath} not found. Using default toppings.");
                return GetDefaultToppings();
            }

            try
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines.Skip(1)) // Skip header
                {
                    var parts = line.Split(',');
                    if (parts.Length >= 1)
                    {
                        var type = parts[0].Trim();
                        if (!string.IsNullOrEmpty(type))
                        {
                            toppings.Add(new Topping(type));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading toppings: {ex.Message}");
                return GetDefaultToppings();
            }

            return toppings;
        }

        private Dictionary<string, double> ReadOptionsCsv(string filePath)
        {
            var optionsDict = new Dictionary<string, double>();

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Warning: {filePath} not found. Using default options.");
                return GetDefaultOptions();
            }

            try
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines.Skip(1)) // Skip header
                {
                    var parts = line.Split(',');
                    if (parts.Length >= 5)
                    {
                        var key = $"{parts[0].Trim()}-{parts[1].Trim()}-{parts[2].Trim()}-{parts[3].Trim()}";
                        if (double.TryParse(parts[4], out double cost))
                        {
                            optionsDict[key] = cost;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading options: {ex.Message}");
                return GetDefaultOptions();
            }

            return optionsDict;
        }

        private void ReadCustomerCsv()
        {
            if (!File.Exists(AppConstants.CUSTOMERS_FILE))
            {
                Console.WriteLine($"Warning: {AppConstants.CUSTOMERS_FILE} not found. Starting with empty customer list.");
                return;
            }

            try
            {
                using (StreamReader sr = new StreamReader(AppConstants.CUSTOMERS_FILE))
                {
                    sr.ReadLine(); // Skip header

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var data = line.Split(',');
                        if (data.Length >= 6)
                        {
                            try
                            {
                                string name = data[0].Trim();
                                int memberId = int.Parse(data[1]);
                                DateTime dob = DateTime.ParseExact(data[2], AppConstants.DATE_FORMAT, CultureInfo.InvariantCulture);
                                string tier = data[3].Trim();
                                int points = int.Parse(data[4]);
                                int punchCard = int.Parse(data[5]);

                                var rewards = new PointCard(points, punchCard) { Tier = tier };
                                var customer = new Customer(name, memberId, dob)
                                {
                                    Rewards = rewards,
                                    OrderHistory = new List<Order>()
                                };

                                Customers.Add(customer);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error parsing customer line: {line}. Error: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading customers: {ex.Message}");
            }
        }

        public void SaveCustomerToCsv(Customer customer)
        {
            try
            {
                string newLine = $"{customer.Name},{customer.MemberId},{customer.Dob.ToString(AppConstants.DATE_FORMAT)}," +
                               $"{customer.Rewards.Tier},{customer.Rewards.Points},{customer.Rewards.PunchCard}";

                using (StreamWriter sw = File.AppendText(AppConstants.CUSTOMERS_FILE))
                {
                    sw.WriteLine(newLine);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving customer: {ex.Message}");
            }
        }

        private List<Flavour> GetDefaultFlavours()
        {
            return new List<Flavour>
            {
                new Flavour("Vanilla", false, 1),
                new Flavour("Chocolate", false, 1),
                new Flavour("Strawberry", false, 1),
                new Flavour("Durian", true, 1),
                new Flavour("Ube", true, 1),
                new Flavour("Sea Salt", true, 1)
            };
        }

        private List<Topping> GetDefaultToppings()
        {
            return new List<Topping>
            {
                new Topping("Sprinkles"),
                new Topping("Mochi"),
                new Topping("Sago"),
                new Topping("Oreos")
            };
        }

        private Dictionary<string, double> GetDefaultOptions()
        {
            return new Dictionary<string, double>
            {
                { "Cup-1---", 4.0 },
                { "Cup-2---", 5.5 },
                { "Cup-3---", 6.5 },
                { "Cone-1-FALSE--", 4.0 },
                { "Cone-1-TRUE--", 6.0 },
                { "Cone-2-FALSE--", 5.5 },
                { "Cone-2-TRUE--", 7.5 },
                { "Cone-3-FALSE--", 6.5 },
                { "Cone-3-TRUE--", 8.5 },
                { "Waffle-1--Original", 7.0 },
                { "Waffle-1--Red Velvet", 10.0 },
                { "Waffle-1--Charcoal", 10.0 },
                { "Waffle-1--Pandan", 10.0 },
                { "Waffle-2--Original", 8.5 },
                { "Waffle-2--Red Velvet", 11.5 },
                { "Waffle-2--Charcoal", 11.5 },
                { "Waffle-2--Pandan", 11.5 },
                { "Waffle-3--Original", 9.5 },
                { "Waffle-3--Red Velvet", 12.5 },
                { "Waffle-3--Charcoal", 12.5 },
                { "Waffle-3--Pandan", 12.5 }
            };
        }
    }
}