using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Core
{
	public class Pluralizer
	{
		public static string MakeSingular(string input)
		{
			string output = string.Copy(input);
			if (input.EndsWith("ies"))
			{
				output = input.Substring(0, input.Length - 3);
				output += "y";
			}
			else if (input.EndsWith("s"))
			{
				output = input.Substring(0, input.Length - 1);
			}
			return output;
		}

		public static string MakePlural(string input)
		{
			string output = string.Copy(input);
			if (input.EndsWith("y"))
			{
				output = input.Substring(0, input.Length - 1);
				output += "ies";
			}
			else if (!input.EndsWith("s"))
			{
				output += "s";
			}

			return output;
		}
	}
}
