using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Models.Configuration
{
	public class MercuryPluginData
	{
		public string Type { get; set; }
		public Dictionary<string, object> Settings { get; set; }
	}
}
