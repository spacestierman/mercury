using Mercury.Models.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Models.Project
{
	public class MercuryProject
	{
		public string Name { get; set; }
		
		public MercurySettings Settings { get; set; }
		[JsonProperty(PropertyName = "settings")]
		internal Dictionary<string, object> _settings;

		public List<MercuryPlugin> Plugins { get; set; }
		[JsonProperty(PropertyName = "plugins")]
		internal IEnumerable<MercuryPluginData> _plugins;

		public MercuryProject()
		{
			Settings = new MercurySettings();
			Plugins = new List<MercuryPlugin>();
		}
	}
}
