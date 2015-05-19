using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Models
{
	public class MercuryValidationRuleJson
	{
		public string Type { get; set; }

		[JsonProperty(PropertyName = "settings")]
		public Dictionary<string, object> _settings; // Ignore the compile warning, these are filled in by Newtonsoft
	}
}
