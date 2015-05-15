using Mercury.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Plugins.ASPMVC4
{
	public class AspMvc4MercuryPlugin : MercuryPlugin
	{
		const string BUILD_NAME = "MvcApplicationName";

		public AspMvc4MercuryPlugin() : base("ASP.NET MVC 4", @"ASPMVC4\source\")
		{
		}

		public override string ChanceToChangeDirectoryName(string directoryPath)
		{
			if (directoryPath.IndexOf(BUILD_NAME) >= 0)
			{
				directoryPath = directoryPath.Replace(BUILD_NAME, Settings.GetAsString("Namespace"));
			}

			return directoryPath;
		}

		public override string ChanceToChangeFileName(string filePath)
		{
			if (filePath.IndexOf(BUILD_NAME) >= 0)
			{
				filePath = filePath.Replace(BUILD_NAME, Settings.GetAsString("Namespace"));
			}

			return filePath;
		}
	}
}