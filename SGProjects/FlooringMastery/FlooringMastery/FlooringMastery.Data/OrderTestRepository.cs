using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using FlooringMastery.Models.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Data
{
    public class OrderTestRepository : IOrderRepository
    {
        private static List<Order> _orders = new List<Order>();
        private static List<Products> _products = new List<Products>();
        private static List<Taxes> _taxes = new List<Taxes>();

        static OrderTestRepository()
        {
            Order newOrder = new Order();
            newOrder.OrderDate = DateTime.Parse("01/01/2017");
            newOrder.OrderNumber = 1;
            newOrder.CustomerName = "Fish";
            newOrder.State = "MN";
            newOrder.TaxRate = decimal.Parse("6.25");
            newOrder.ProductType = "Wood";
            newOrder.Area = decimal.Parse("100");
            newOrder.CostPerSquareFoot = decimal.Parse("5.15");
            newOrder.LaborCostPerSquareFoot = decimal.Parse("4.75");
            newOrder.MaterialCost = decimal.Parse("515.00");
            newOrder.LaborCost = decimal.Parse("475.00");
            newOrder.Tax = decimal.Parse("61.88");
            newOrder.Total = decimal.Parse("1051.88");

            _orders.Add(newOrder);

            Taxes taxRates = new Taxes();
            taxRates.StateAbbreviation = "MN";
            taxRates.StateName = "Minnesota";
            taxRates.TaxRate = decimal.Parse("6.25");

            _taxes.Add(taxRates);

            Products productTypes = new Products();
            productTypes.ProductType = "Wood";
            productTypes.CostPerSquareFoot = decimal.Parse("5.15");
            productTypes.LaborCostPerSquareFoot = decimal.Parse("4.75");

            _products.Add(productTypes);
        }


        public Order LoadOrderNumber(int OrderNumber)
        {
            return _orders.SingleOrDefault(x => x.OrderNumber == OrderNumber);
        }

        public Order LoadLastOrder(DateTime orderDate)
        {
            return _orders.Last();
        }

        public IEnumerable<Order> LoadOrderDate(DateTime OrderDate)
        {
            return _orders.Where(x => x.OrderDate == OrderDate);
        }

        public void SaveOrder(IEnumerable<Order> orders)
        {
            _orders = orders.ToList();
        }

        public void SaveNewOrder(Order order)
        {
            _orders.Add(order);
        }

        public IEnumerable<Products> LoadProducts()
        {
            return _products;
        }

        public IEnumerable<Taxes> LoadTaxFile()
        {
            return _taxes;
        }

        public void RemoveOrder(Order order)
        {
            _orders.Remove(order);
        }

        public void EditOrder(Order editOrder)
        {
            foreach(var x in _orders)
            {
                if(x.OrderNumber == editOrder.OrderNumber)
                {
                    x.CustomerName = editOrder.CustomerName;
                    x.State = editOrder.State;
                    x.TaxRate = editOrder.TaxRate;
                    x.ProductType = editOrder.ProductType;
                    x.Area = editOrder.Area;
                    x.CostPerSquareFoot = editOrder.CostPerSquareFoot;
                    x.LaborCostPerSquareFoot = editOrder.LaborCostPerSquareFoot;
                    x.MaterialCost = editOrder.MaterialCost;
                    x.LaborCost = editOrder.LaborCost;
                    x.Tax = editOrder.Tax;
                    x.Total = editOrder.Total;                    
                }
            }
        }
    }
}
