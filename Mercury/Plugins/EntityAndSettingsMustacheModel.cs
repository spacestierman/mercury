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
		public MercuryEntity Entity { get; set; }
		public object Settings { get; set; }
	}
}
