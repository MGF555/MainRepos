using FlooringMastery.BLL;
using FlooringMastery.Models;
using FlooringMastery.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.UI.Workflows
{
    public class EditOrderWorkflow
    {
        public void Execute()
        {
            OrderManager manager = OrderManagerFactory.Create();

            Console.Clear();
            Console.WriteLine("Edit an order");
            Console.WriteLine("-------------------------------------");
            Console.Write("Enter the order date: ");
            string date = Console.ReadLine();

            OrderLookupResponse response;

            if (DateTime.TryParse(date, out DateTime orderDate))
            {
                response = manager.LookupOrderDate(orderDate);

                if (!response.Success)
                {
                    Console.Write("Error: ");
                    Console.WriteLine(response.Message);
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                Console.WriteLine("Error: The date entered is not a valid date");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter the order number:");
            string orderNumCheck = Console.ReadLine();

            if (int.TryParse(orderNumCheck, out int orderNumber))
            {
                response = manager.LookupOrderNumber(orderNumber);

                if (!response.Success)
                {
                    Console.Write("Error: ");
                    Console.WriteLine(response.Message);
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                Console.WriteLine("Error: Invalid input");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Order order = response.Order;
            ProductLookupResponse products = manager.LookupProducts();
            TaxesLookupResponse taxes = manager.LookupTaxes();
            Console.WriteLine("Order details");
            Console.WriteLine("*******************");
            ConsoleIO.DisplayOrderDetails(order);
            Console.WriteLine("*******************");
            Console.WriteLine("Press any key to continue to the edit screen...");
            Console.ReadKey();
            
            Order orderEdit = manager.EditOrderInformation(order, taxes, products);
            Console.Clear();

            Console.WriteLine("Edited order information");
            ConsoleIO.DisplayOrderDetails(orderEdit);
            Console.Write("Save these changes? Y/N: ");
            while (true)
            {
                string userInput = Console.ReadLine();
                if(userInput == "Y")
                {
                    manager.SaveChanges(orderEdit);
                    Console.WriteLine("Changes have been saved. Press any key to continue...");
                    Console.ReadKey();
                    return;
                }
                else if(userInput == "N")
                {
                    Console.WriteLine("Changes have been discarded. Press any key to continue...");
                    Console.ReadKey();
                    return;
                }
            }
        }
    }
}
