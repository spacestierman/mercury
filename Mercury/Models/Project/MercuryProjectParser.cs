using Mercury.Common;
using Mercury.Models.Configuration;
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

			foreach (KeyValuePair<string, object> setting in project._settings)
			{
				project.Settings.Set(setting.Key, setting.Value);
			}

			foreach (MercuryPluginData pluginData in project._plugins)
			{
				MercuryPlugin plugin = (MercuryPlugin)TryToBuildObjectFromTypeString(pluginData.Type);
				if (plugin == null)
				{
					throw new FormattedException("Unable to determine the plugin type \"{0}\".", pluginData.Type);
				}

				plugin.Settings.Merge(project.Settings);
				foreach (KeyValuePair<string, object> setting in pluginData.Settings)
				{
					plugin.Settings.Set(setting.Key, setting.Value);
				}

				project.Plugins.Add(plugin);
			}

			return project;
		}

		private static object TryToBuildObjectFromTypeString(string typeName)
		{
			Type type = Type.GetType(typeName);
			object instance = Activator.CreateInstance(type);
			return instance;
		}
	}
}
