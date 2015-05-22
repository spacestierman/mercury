using Mercury.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Plugins.Test
{
	public class TestMercuryPlugin : MustachePlugin
	{
		public TestMercuryPlugin() : base("Test Plugin", @"Test\")
		{

		}
	}
}
