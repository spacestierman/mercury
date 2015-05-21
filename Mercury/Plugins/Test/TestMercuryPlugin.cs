using Mercury.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Plugins.Test
{
	public class TestMercuryPlugin : MercuryPlugin
	{
		const string BUILD_NAME = "MvcApplicationName";

		public TestMercuryPlugin() : base("Test Plugin", @"Test\source\", @"Test\templates\")
		{

		}

		override public string ChanceToChangeDirectoryName(string directoryPath)
		{
			if (directoryPath.IndexOf(BUILD_NAME) >= 0)
			{
				directoryPath = directoryPath.Replace(BUILD_NAME, Settings.GetAsString("Namespace"));
			}

			return directoryPath;
		}

		override public string ChanceToChangeFileName(string filePath)
		{
			if (filePath.IndexOf(BUILD_NAME) >= 0)
			{
				filePath = filePath.Replace(BUILD_NAME, Settings.GetAsString("Namespace"));
			}

			return filePath;
		}
	}
}
