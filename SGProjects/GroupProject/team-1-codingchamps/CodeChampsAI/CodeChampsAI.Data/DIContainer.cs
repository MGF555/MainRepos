using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace CodeChampsAI.Data
{
	public static class DIContainer
	{
		public static IKernel Kernel = new StandardKernel();

		static DIContainer()
		{
			string dataType = ConfigurationManager.AppSettings["Mode"].ToString();

			if (dataType == "MockRepository")
				Kernel.Bind<IRepository>().To<MockRepository>();
			else if (dataType == "EFRepo")
				Kernel.Bind<IRepository>().To<EFRepo>();
			else
				throw new Exception("Data type key in app.config not set properly!");

		}
	}
}
