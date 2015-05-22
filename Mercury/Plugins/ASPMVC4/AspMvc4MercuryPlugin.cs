using Mercury.Core;
using Mercury.Models;
using Nustache.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Plugins.ASPMVC4
{
	public class AspMvc4MercuryPlugin : MustachePlugin
	{
		public AspMvc4MercuryPlugin() : base("ASP.NET MVC 4", @"ASPMVC4\")
		{
		}

		public override BuildPlan Build(IEnumerable<MercuryEntity> entities, FileContentsProvider fileContentsProvider)
		{
			BuildPlan plan = base.Build(entities, fileContentsProvider);

			/// TODO: Build Controllers, ViewModels, etc
			return plan;
		}
	}
}