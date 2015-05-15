using Mercury.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Models.Configuration
{
	public class MercurySettings
	{
		public static MercurySettings Merge(MercurySettings settingsA, MercurySettings settingsB, bool overwriteExistingKeys = false)
		{
			MercurySettings merged = new MercurySettings();
			merged.Merge(settingsA, overwriteExistingKeys);
			merged.Merge(settingsB, overwriteExistingKeys);
			return merged;
		}

		private Dictionary<string, object> _settings;

		public MercurySettings()
		{
			_settings = new Dictionary<string, object>();
		}

		public void Merge(MercurySettings otherSettings, bool overwriteExistingKeys = false)
		{
			foreach (KeyValuePair<string, object> kvp in otherSettings._settings)
			{
				if (overwriteExistingKeys || !Contains(kvp.Key))
				{
					Set(kvp.Key, kvp.Value);
				}
			}
		}

		public void Set(string name, object value)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentRequiredException("name");
			}
			if (value == null)
			{
				throw new ArgumentRequiredException("value");
			}

			_settings[name.ToLower()] = value;
		}

		public object ToObject()
		{
			IDictionary<string, Object> value = new ExpandoObject() as IDictionary<string, Object>;
			foreach (KeyValuePair<string, object> kvp in _settings)
			{
				value.Add(kvp.Key, kvp.Value);
			}
			return value;
		}

		public bool Contains(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentRequiredException("name");
			}

			object value = TryToGet(name);
			return value != null;
		}

		public object Get(string name)
		{
			object value = TryToGet(name);
			if (value == null)
			{
				throw new FormattedException("No key named \"{0}\" found!", name);
			}

			return value;
		}

		public object TryToGet(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentRequiredException("name");
			}

			string key = name.ToLower();
			if (!_settings.ContainsKey(key))
			{
				return null;
			}

			return _settings[key];
		}

		public int GetAsInteger(string name)
		{
			object value = Get(name);
			if (value is int)
			{
				return (int)value;
			}

			return Convert.ToInt32(value);
		}

		public string GetAsString(string name)
		{
			object value = Get(name);
			if (value is string)
			{
				return (string)value;
			}

			return Convert.ToString(value);
		}

		public MercurySettings Clone()
		{
			MercurySettings clone = new MercurySettings();
			foreach (KeyValuePair<string, object> kvp in _settings)
			{
				clone._settings[kvp.Key] = kvp.Value;
			}
			return clone;
		}
	}
}