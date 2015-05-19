using Mercury.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Models
{
	public class MercuryProject
	{
		public string Name { get; set; }
		
		public MercurySettings Settings { get; set; }
		[JsonProperty(PropertyName = "settings")]
		internal Dictionary<string, object> _settings; // Ignore the compile warning, these are filled in by Newtonsoft

		public List<MercuryPlugin> Plugins { get; set; }
		[JsonProperty(PropertyName = "plugins")]
		internal IEnumerable<MercuryPluginJson> _plugins; // Ignore the compile warning, these are filled in by Newtonsoft

		public IEnumerable<MercuryEntity> Entities { get; set; }
		[JsonProperty(PropertyName = "entities")]
		internal IEnumerable<MercuryEntityData> _entities; // Ignore the compile warning, these are filled in by Newtonsoft

		public MercuryProject()
		{
			Settings = new MercurySettings();
			Plugins = new List<MercuryPlugin>();
			Entities = new List<MercuryEntity>();
		}
	}
}
