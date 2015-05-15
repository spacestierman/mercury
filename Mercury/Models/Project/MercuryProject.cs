using Mercury.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Models.Project
{
	public class MercuryProject
	{
		public string Name { get; set; }
		public List<MercuryPlugin> Plugins { get; set; }

		public MercuryProject()
		{
			Plugins = new List<MercuryPlugin>();
		}
	}
}
