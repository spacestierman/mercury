using Mercury.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Plugins.Test
{
	public class TestMercuryPlugin : MercuryPlugin
	{
		public TestMercuryPlugin() : base("Test Plugin", @"Test\source\")
		{

		}
	}
}
