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
    public class RemoveOrderWorkflow
    {
        public void Execute()
        {
            OrderManager manager = OrderManagerFactory.Create();

            Console.Clear();
            Console.WriteLine("Remove an order");
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

            if(int.TryParse(orderNumCheck, out int orderNumber))
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

            Console.WriteLine("Order details");
            Console.WriteLine("*******************");
            ConsoleIO.DisplayOrderDetails(order);
            Console.WriteLine("*******************");
            Console.Write("Delete this order? This cannot be undone. Y/N: ");

            while (true)
            {
                string userSelection = Console.ReadLine();

                if (userSelection == "Y")
                {
                    manager.RemoveOrder(order);
                    Console.WriteLine("Order deleted. Press any key to continue");
                    Console.ReadKey();
                    return;
                }
                else if (userSelection == "N")
                {
                    Console.WriteLine("Order deletion cancelled. Press any key to continue...");
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
