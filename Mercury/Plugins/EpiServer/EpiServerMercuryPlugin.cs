using Mercury.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Plugins.EpiServer
{
	public class EpiServerMercuryPlugin : MercuryPlugin
	{
		public EpiServerMercuryPlugin() : base("EpiServer", @"Episerver\source\")
		{
			ListenForFilenamePattern("Web.config");
		}

		protected override string OnFilenamePatternMatched(string filename, string contents)
		{
			Console.WriteLine("EpiServerMercuryPlugin ChanceToProcessFile(\"{0}\")", filename);
			return contents;
		}
	}
}