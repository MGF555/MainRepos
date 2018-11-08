using FlooringMastery.BLL;
using FlooringMastery.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.UI.Workflows
{
    public class OrderLookupWorkflow
    {
        public void Execute()
        {
            OrderManager manager = OrderManagerFactory.Create();


            Console.Clear();
            Console.WriteLine("Lookup an order");
            Console.WriteLine("-------------------------------------");
            Console.Write("Enter the order date: ");
            string date = Console.ReadLine();

            DateTime orderDate;

            if (DateTime.TryParse(date, out orderDate))
            {
                OrderLookupResponse response = manager.LookupOrderDate(orderDate);

                if (response.Success)
                {
                    foreach(var x in response.Orders)
                    {
                        ConsoleIO.DisplayOrderDetails(x);
                        Console.WriteLine("***************************");
                    }
                }
                else
                {
                    Console.Write("Error: ");
                    Console.WriteLine(response.Message);
                }
            }
            else
            {
                Console.WriteLine("Error: The date entered is not a valid date");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
