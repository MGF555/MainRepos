using DVDLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDLibrary.Data
{
    public static class DvdRepositoryFactory
    {
        public static IDvdRepository GetRepository()
        {
            switch (Settings.GetRepositoryType())
            {
                case "DvdRepositoryMock":
                    return new DvdRepositoryMock();
                case "DvdRepositoryEF":
                    return new DvdRepositoryEF();
                case "DvdRepositoryADO":
                    return new DvdRepositoryADO();
                default:
                    throw new Exception("Error: Valid configuration file not found");
            }
        }
    }
}
