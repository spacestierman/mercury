using Mercury.Common;
using Mercury.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Plugins
{
	public class EntitiesAndSettingsMustacheModel
	{
		public IEnumerable<MercuryEntity> Entities { get; private set; }
		public object Settings { get; private set; }

		public EntitiesAndSettingsMustacheModel(IEnumerable<MercuryEntity> entities, MercurySettings settings)
		{
			if (entities == null)
			{
				throw new ArgumentRequiredException("entities");
			}
			Entities = entities;

			if (settings == null)
			{
				throw new ArgumentRequiredException("settings");
			}
			Settings = settings.ToObject();
		}
	}
}
