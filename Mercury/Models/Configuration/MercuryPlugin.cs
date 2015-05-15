using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Models.Configuration
{
	public class MercuryPlugin
	{
		public string Name { get; set; }
		public string SourceDirectory { get; set; }
		public List<MercuryPlugin> Dependencies { get; set; }

		public MercuryPlugin()
		{
			Dependencies = new List<MercuryPlugin>();
		}
	}
}
