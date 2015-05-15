using Mercury.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Plugins.EpiServer
{
	public class AspNetMvc3MercuryPlugin : MercuryPlugin
	{
		public AspNetMvc3MercuryPlugin() : base("ASP.NET MVC 4", @"ASPNETMVC4\source\")
		{
		}
	}
}