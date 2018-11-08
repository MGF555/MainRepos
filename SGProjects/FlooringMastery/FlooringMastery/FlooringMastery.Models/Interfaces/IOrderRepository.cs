using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Models.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> LoadOrderDate(DateTime OrderDate);
        Order LoadOrderNumber(int OrderNumber);
        void SaveOrder(IEnumerable<Order> order);
        IEnumerable<Products> LoadProducts();
        IEnumerable<Taxes> LoadTaxFile();
        Order LoadLastOrder(DateTime OrderDate);
        void SaveNewOrder(Order order);
        void RemoveOrder(Order order);
        void EditOrder(Order editOrder);
    }
}
