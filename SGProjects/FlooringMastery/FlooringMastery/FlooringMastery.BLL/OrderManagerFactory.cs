using FlooringMastery.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.BLL
{
    public class OrderManagerFactory
    {
        public static OrderManager Create()
        {
            string mode = ConfigurationManager.AppSettings["Mode"].ToString();

            switch(mode)
            {
                case "Test":
                return new OrderManager(new OrderTestRepository());
                case "Prod":
                return new OrderManager(new OrderProdRespository());
                default:
                    throw new Exception("Invalid mode. Check app config");
            }

        }
    }
}
