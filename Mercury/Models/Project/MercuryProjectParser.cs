using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Models.Project
{
	static class MercuryProjectParser
	{
		public static MercuryProject TryToParseFile(string absoluteFilePath)
		{
			string contents = File.ReadAllText(absoluteFilePath);
			return TryToParseContents(contents);
		}

		public static MercuryProject TryToParseContents(string fileContents)
		{
			MercuryProject project = Newtonsoft.Json.JsonConvert.DeserializeObject<MercuryProject>(fileContents);

			/// TODO: Make sure we correctly import the plugins.

			return project;
		}
	}
}
