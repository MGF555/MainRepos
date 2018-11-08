using FlooringMastery.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using FlooringMastery.Models;
using FlooringMastery.Models.Responses;

namespace FlooringMastery.UI.Workflows
{
    public class AddOrderWorkflow
    {
        public void Execute()
        {
            OrderManager manager = OrderManagerFactory.Create();

            Console.Clear();
            Console.WriteLine("Add an order");
            Console.WriteLine("--------------------");

            ProductLookupResponse products = manager.LookupProducts();  
            TaxesLookupResponse taxes = manager.LookupTaxes();
            Order order = manager.AddOrderInformation(taxes, products);

            Console.Clear();
            Console.WriteLine("Order details");
            Console.WriteLine("--------------");
            ConsoleIO.DisplayOrderDetails(order);
            Console.WriteLine("--------------");
            Console.WriteLine("Save order? Enter Y/N");
            while (true)
            {
                string prompt = Console.ReadLine();
                if (prompt == "Y")
                {
                    manager.AddNewOrder(order);
                    Console.WriteLine("Order saved. Press any key to continue...");
                    Console.ReadKey();
                    return;
                }
                else if(prompt == "N")
                {
                    Console.WriteLine("Order discarded. Press any key to continue...");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Console.Write("Invalid input. Enter Y or N: ");
                }
            }

        }
    }
}
