using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using FlooringMastery.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlooringMastery.BLL
{
    public class OrderManager
    {
        private IOrderRepository _orderTestRepository;

        public OrderManager(IOrderRepository orderRepository)
        {
            _orderTestRepository = orderRepository;
        }



        public OrderLookupResponse LookupOrderNumber(int orderNumber)
        {
            OrderLookupResponse response = new OrderLookupResponse();

            response.Order = _orderTestRepository.LoadOrderNumber(orderNumber);

            if(response.Order == null)
            {
                response.Success = false;
                response.Message = $"Order number {orderNumber} does not exist.";
            }
            else
            {
                response.Success = true;
            }
            return response;
        }
        
        public OrderLookupResponse LookupOrderDate(DateTime orderCheck)
        {
            OrderLookupResponse response = new OrderLookupResponse();

            response.Orders = _orderTestRepository.LoadOrderDate(orderCheck);

            if (!response.Orders.Any())
            {
                response.Success = false;
                response.Message = $"No orders from {orderCheck.ToString("M/d/yy")} exist.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public ProductLookupResponse LookupProducts()
        {
            ProductLookupResponse response = new ProductLookupResponse();

            response.Product = _orderTestRepository.LoadProducts();

            return response;
        }

        public TaxesLookupResponse LookupTaxes()
        {
            TaxesLookupResponse response = new TaxesLookupResponse();

            response.Taxes = _orderTestRepository.LoadTaxFile();

            return response;

        }

        public OrderLookupResponse LookupLastOrder(DateTime orderDate)
        {
            OrderLookupResponse response = new OrderLookupResponse();

            response.Order = _orderTestRepository.LoadLastOrder(orderDate);

            return response;
        }

        public DateTime GetDate()
        {
            Console.Write("Enter a future order date in the format mm/dd/yyyy: ");
            while (true)
            {
                DateTime futureDate;
                string userInput = Console.ReadLine();
                if (DateTime.TryParse(userInput, out futureDate))
                {
                    if (futureDate <= DateTime.Today)
                    {
                        Console.Write("Error: Enter a future date: ");
                    }
                    else
                    {
                        return futureDate;
                    }
                }
                else
                {
                    Console.Write("Error: Invalid date format.  Please enter a proper date: ");
                }
            }
        }

        public string GetName(Order order)
        {
            Console.WriteLine("Enter a customer name.");
            Console.Write("This may include alphanumeric characters as well as commas and periods: ");

            while (true)
            {
                string customerName = Console.ReadLine();

                if(order.Edit && customerName == "")
                {
                    return order.CustomerName;
                }
                if (Regex.IsMatch(customerName, @"^[a-zA-Z0-9,.\s]+$") && customerName != "")
                {
                    return customerName;
                }
                else
                {
                    Console.Write("Invalid input. Enter a valid name: ");
                }
            }
        }

        public string GetState(TaxesLookupResponse taxes, Order order)
        {
            Console.Write("Enter a state in XX format: ");
            while (true)
            {
                string state = Console.ReadLine();

                if(order.Edit && state == "")
                {
                    return order.State;
                }
                if (taxes.Taxes.Any(x => x.StateAbbreviation == state))
                {
                    order.Recalculate = true;
                    return state;
                }
                else
                {
                    Console.Write("Error: State does not exist in database. Enter another state: ");
                }
            }
        }

        public string GetProduct(ProductLookupResponse products, Order order)
        {
            Console.WriteLine("------------------------");

            string productType;

            foreach (var x in products.Product)
            {
                Console.WriteLine($"Product type: {x.ProductType}");
                Console.WriteLine($"Cost per square foot: {x.CostPerSquareFoot}");
                Console.WriteLine($"Labor cost per square foot: {x.CostPerSquareFoot}");
                Console.WriteLine("------------------------");
            }
            Console.Write("Select a product: ");
            while (true)
            {
                productType = Console.ReadLine();
                if(order.Edit && productType == "")
                {
                    return order.ProductType;
                }
                if (products.Product.Any(x => x.ProductType == productType))
                {
                    order.Recalculate = true;
                    return productType;
                }
                else
                {
                    Console.Write("Error: Product does not exist. Select another product: ");
                }
            }
        }

        public decimal GetArea(Order order)
        {
            string areaCheck;
            decimal area;

            Console.Write("Enter the area: ");
            while (true)
            {
                areaCheck = Console.ReadLine();
                if(order.Edit && areaCheck == "")
                {
                    return order.Area;
                }
                if (decimal.TryParse(areaCheck, out area) && area >= 100)
                {
                    order.Recalculate = true;
                    return area;
                }
                else
                {
                    Console.Write("Error: Please enter a number that is at least 100: ");
                }
            }
        }

        public Order AddOrderInformation(TaxesLookupResponse taxes, ProductLookupResponse products)
        {
            Order order = new Order();

            order.Edit = false;

            order.OrderDate = GetDate();
            order.CustomerName = GetName(order);
            order.State = GetState(taxes, order);
            order.ProductType = GetProduct(products, order);
            order.Area = GetArea(order);

            order = OrderCalculations(order, taxes, products);

            return order ;
        }
        
        public void AddNewOrder(Order order)
        {
            _orderTestRepository.SaveNewOrder(order);
        }

        public void RemoveOrder(Order order)
        {
            _orderTestRepository.RemoveOrder(order);
        }

        public Order EditOrderInformation(Order order, TaxesLookupResponse taxes, ProductLookupResponse products)
        {
            Console.Clear();
            order.Edit = true;
            order.Recalculate = false;

            order.CustomerName = GetName(order);
            order.State = GetState(taxes, order);
            order.ProductType = GetProduct(products, order);
            order.Area = GetArea(order);

            if(order.Recalculate)
            {
                order = OrderCalculations(order, taxes, products);
            }

            return order;
        }

        public Order OrderCalculations(Order order, TaxesLookupResponse taxes, ProductLookupResponse products)
        {

            Taxes stateTax = taxes.Taxes.Where(x => x.StateAbbreviation == order.State).First();
            Products productPrices = products.Product.Where(x => x.ProductType == order.ProductType).First();

            if (!order.Edit)
                order.OrderNumber = _orderTestRepository.LoadLastOrder(order.OrderDate).OrderNumber + 1;
            else
            order.TaxRate = stateTax.TaxRate;
            order.CostPerSquareFoot = productPrices.CostPerSquareFoot;
            order.LaborCostPerSquareFoot = productPrices.LaborCostPerSquareFoot;
            order.MaterialCost = Math.Round(productPrices.CostPerSquareFoot * order.Area, 2);
            order.LaborCost = Math.Round(productPrices.LaborCostPerSquareFoot * order.Area, 2);
            order.Tax = Math.Round((order.LaborCost + order.MaterialCost) * (stateTax.TaxRate / 100), 2);
            order.Total = Math.Round(order.LaborCost + order.MaterialCost + order.Tax, 2);

            return order;
        }

        public void SaveChanges(Order editOrder)
        {
            _orderTestRepository.EditOrder(editOrder);
        }
    }
}
