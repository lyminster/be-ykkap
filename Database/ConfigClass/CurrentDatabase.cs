using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ConfigClass
{
	public class CurrentDatabase
	{
		public static Database.DBConnection CurrentConnection;

		public static Database.DBTransaction CurrentTransaction;
	}

}
