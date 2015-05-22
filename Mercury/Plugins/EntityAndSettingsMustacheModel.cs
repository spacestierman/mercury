using Mercury.Common;
using Mercury.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Plugins
{
	public class EntityAndSettingsMustacheModel
	{
		public MercuryEntity Entity { get; private set; }
		public object Settings { get; private set; }

		public EntityAndSettingsMustacheModel(MercuryEntity entity, MercurySettings settings)
		{
			if (entity == null)
			{
				throw new ArgumentRequiredException("entity");
			}
			Entity = entity;

			if (settings == null)
			{
				throw new ArgumentRequiredException("settings");
			}
			Settings = settings.ToObject();
		}
	}
}
