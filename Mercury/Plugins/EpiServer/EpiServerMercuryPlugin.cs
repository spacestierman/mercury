using Mercury.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Plugins.EpiServer
{
	public class EpiServerMercuryPlugin : MustachePlugin
	{
		public EpiServerMercuryPlugin() : base("EpiServer", @"Episerver\")
		{

		}
	}
}