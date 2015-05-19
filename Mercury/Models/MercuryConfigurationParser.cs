using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Models
{
	public class MercuryConfigurationParser
	{
		public static MercuryConfiguration TryToParseFile(string absoluteFilePath)
		{
			string contents = File.ReadAllText(absoluteFilePath);
			return TryToParseContents(contents);
		}

		public static MercuryConfiguration TryToParseContents(string fileContents)
		{
			MercuryConfiguration project = Newtonsoft.Json.JsonConvert.DeserializeObject<MercuryConfiguration>(fileContents);

			/// TODO: Make sure we correctly import the plugins.

			return project;
		}
	}
}
