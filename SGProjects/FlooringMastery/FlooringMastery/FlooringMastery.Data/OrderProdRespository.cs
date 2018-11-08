using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Data
{
    public class OrderProdRespository : IOrderRepository
    {
        private List<Order> _orders = new List<Order>();
        private List<Products> _products = new List<Products>();
        private List<Taxes> _taxes = new List<Taxes>();


        public void GetOrder(DateTime orderDate)
        {
            FileInfo orderData = new FileInfo($@".\Orders_{orderDate.ToString("MMddyyyy")}.txt");

            if (!File.Exists($@".\Orders_{orderDate.ToString("MMddyyyy")}.txt"))
            {
                return;
            }
            using (TextReader orderGet = orderData.OpenText())
            {
                string line = orderGet.ReadLine();
                while ((line = orderGet.ReadLine()) != null)
                {
                    line = line.Replace("\"", "");
                    string[] column = line.Split(',');
                    Order a = new Order();
                    a.OrderDate = orderDate;
                    a.OrderNumber = int.Parse(column[0]);
                    a.CustomerName = column[1];
                    a.State = column[2];
                    a.TaxRate = decimal.Parse(column[3]);
                    a.ProductType = column[4];
                    a.Area = decimal.Parse(column[5]);
                    a.CostPerSquareFoot = decimal.Parse(column[6]);
                    a.LaborCostPerSquareFoot = decimal.Parse(column[7]);
                    a.MaterialCost = decimal.Parse(column[8]);
                    a.LaborCost = decimal.Parse(column[9]);
                    a.Tax = decimal.Parse(column[10]);
                    a.Total = decimal.Parse(column[11]);

                    _orders.Add(a);
                }

            }
        }

        public IEnumerable<Taxes> TaxInfo()
        {
            FileInfo taxData = new FileInfo(@".\Taxes.txt");

            if (!File.Exists(@".\Taxes.txt"))
            {
                File.Create(@".\Taxes.txt");
            }
            using (TextReader taxGet = taxData.OpenText())
            {
                string line = taxGet.ReadLine();
                while ((line = taxGet.ReadLine()) != null)
                {
                    line = line.Replace("\"", "");
                    string[] column = line.Split(',');
                    Taxes a = new Taxes();
                    a.StateAbbreviation = column[0];
                    a.StateName = column[1];
                    a.TaxRate = decimal.Parse(column[2]);

                    _taxes.Add(a);
                }
            }
            return _taxes;
        }

        public IEnumerable<Products> ProductInfo()
        {
            FileInfo productData = new FileInfo(@".\Products.txt");

            if (!File.Exists(@".\Products.txt"))
            {
                File.Create(@".\Products.txt");
            }

            using (TextReader prodGet = productData.OpenText())
            {
                string line = prodGet.ReadLine();
                while ((line = prodGet.ReadLine()) != null)
                {
                    line = line.Replace("\"", "");
                    string[] column = line.Split(',');
                    Products a = new Products();
                    a.ProductType = column[0];
                    a.CostPerSquareFoot = decimal.Parse(column[1]);
                    a.LaborCostPerSquareFoot = decimal.Parse(column[2]);

                    _products.Add(a);
                }
            }
            return _products;
        }

        public Order LoadOrderNumber(int OrderNumber)
        {
            return _orders.SingleOrDefault(x => x.OrderNumber == OrderNumber);
        }

        public Order LoadLastOrder(DateTime orderDate)
        {
            GetOrder(orderDate);
            if(_orders.Count == 0)
            {
                Order voidOrder = new Order();
                voidOrder.OrderNumber = 0;
                _orders.Add(voidOrder);
            }
            return _orders.Last();
        }

        public IEnumerable<Order> LoadOrderDate(DateTime OrderDate)
        {
            GetOrder(OrderDate);
            return _orders.Where(x => x.OrderDate == OrderDate);
        }

        public void SaveOrder(IEnumerable<Order> orders)
        {
            _orders = orders.ToList();
        }

        public void SaveNewOrder(Order order)
        {
            _orders.Add(order);
            bool newFile = false;

            if (!File.Exists($@".\Orders_{order.OrderDate.ToString("MMddyyyy")}.txt"))
            {
                File.Create($@".\Orders_{order.OrderDate.ToString("MMddyyyy")}.txt").Close();
                newFile = true;
            }

            using(StreamWriter newOrderWriter = File.AppendText($@".\Orders_{order.OrderDate.ToString("MMddyyyy")}.txt"))
            {
                if (newFile)
                {
                    newOrderWriter.WriteLine("OrderNumber,CustomerName,State,TaxRate,ProductType,Area,CostPerSquareFoot,LaborCostPerSquareFoot,MaterialCost,LaborCost,Tax,Total");
                }
                newOrderWriter.WriteLine($"{order.OrderNumber},{order.CustomerName},{order.State},{order.TaxRate},{order.ProductType},{order.Area},{order.CostPerSquareFoot},{order.LaborCostPerSquareFoot},{order.MaterialCost},{order.LaborCost},{order.Tax},{order.Total}");
            }
        }

        public IEnumerable<Products> LoadProducts()
        {
            return ProductInfo();
        }

        public IEnumerable<Taxes> LoadTaxFile()
        {
            return TaxInfo();
        }

        public void RemoveOrder(Order order)
        {
            _orders.Remove(order);
            if (File.Exists($@".\Orders_{order.OrderDate.ToString("MMddyyyy")}.txt"))
            {
                File.Delete($@".\Orders_{order.OrderDate.ToString("MMddyyyy")}.txt");
            }

            if(_orders.Count == 0)
            {
                return; //This prevents a new file from being created if there are no more orders for the date.
            }

            using (StreamWriter orderWriter = new StreamWriter($@".\Orders_{order.OrderDate.ToString("MMddyyyy")}.txt"))
            {
                orderWriter.WriteLine("OrderNumber,CustomerName,State,TaxRate,ProductType,Area,CostPerSquareFoot,LaborCostPerSquareFoot,MaterialCost,LaborCost,Tax,Total");
                foreach(var orderWrite in _orders)
                {
                    orderWriter.WriteLine($"{orderWrite.OrderNumber},{orderWrite.CustomerName},{orderWrite.State},{orderWrite.TaxRate},{orderWrite.ProductType},{orderWrite.Area},{orderWrite.CostPerSquareFoot},{orderWrite.LaborCostPerSquareFoot},{orderWrite.MaterialCost},{orderWrite.LaborCost},{orderWrite.Tax},{orderWrite.Total}");
                }
            }
        }

        public void EditOrder(Order editOrder)
        {
            if (File.Exists($@".\Orders_{editOrder.OrderDate.ToString("MMddyyyy")}.txt"))
            {
                File.Delete($@".\Orders_{editOrder.OrderDate.ToString("MMddyyyy")}.txt");
            }

            using (StreamWriter orderWriter = new StreamWriter($@".\Orders_{editOrder.OrderDate.ToString("MMddyyyy")}.txt"))
            {
                orderWriter.WriteLine("OrderNumber,CustomerName,State,TaxRate,ProductType,Area,CostPerSquareFoot,LaborCostPerSquareFoot,MaterialCost,LaborCost,Tax,Total");
                foreach (var writeOrder in _orders)
                {
                    if (writeOrder.OrderNumber == editOrder.OrderNumber)
                    {
                        writeOrder.CustomerName = editOrder.CustomerName;
                        writeOrder.State = editOrder.State;
                        writeOrder.TaxRate = editOrder.TaxRate;
                        writeOrder.ProductType = editOrder.ProductType;
                        writeOrder.Area = editOrder.Area;
                        writeOrder.CostPerSquareFoot = editOrder.CostPerSquareFoot;
                        writeOrder.LaborCostPerSquareFoot = editOrder.LaborCostPerSquareFoot;
                        writeOrder.MaterialCost = editOrder.MaterialCost;
                        writeOrder.LaborCost = editOrder.LaborCost;
                        writeOrder.Tax = editOrder.Tax;
                        writeOrder.Total = editOrder.Total;
                    }
                        orderWriter.WriteLine($"{writeOrder.OrderNumber},{writeOrder.CustomerName},{writeOrder.State},{writeOrder.TaxRate},{writeOrder.ProductType},{writeOrder.Area},{writeOrder.CostPerSquareFoot},{writeOrder.LaborCostPerSquareFoot},{writeOrder.MaterialCost},{writeOrder.LaborCost},{writeOrder.Tax},{writeOrder.Total}");
                }
            }
        }

    }
}
