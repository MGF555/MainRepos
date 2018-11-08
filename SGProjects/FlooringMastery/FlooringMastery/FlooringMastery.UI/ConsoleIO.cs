using FlooringMastery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.UI
{
    public class ConsoleIO
    {
        public static void DisplayOrderDetails(Order order)
        {
            Console.WriteLine($"{ order.OrderNumber} | {order.OrderDate.ToString("M/d/yyyy")}");
            Console.WriteLine(order.CustomerName);
            Console.WriteLine(order.State);
            Console.WriteLine($"Product: {order.ProductType}");
            Console.WriteLine($"Materials: {order.MaterialCost:c}");
            Console.WriteLine($"Labor: {order.LaborCost:c}");
            Console.WriteLine($"Tax: {order.Tax:c}");
            Console.WriteLine($"Total: {order.Total:c}");
        }

        public static void DisplayProductDetails(Products products)
        {
            Console.WriteLine($"Product type: {products.ProductType}");
            Console.WriteLine($"Cost per square foot: {products.CostPerSquareFoot}");
            Console.WriteLine($"Labor cost per square foot: {products.CostPerSquareFoot}");
        }
    }
}
