using Mercury.Common;
using Mercury.Core;
using Mercury.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mercury.Models
{
	public class MercuryPlugin
	{
		public string Name { get; private set; }
		public string RelativeDirectory { get; private set; }
		public List<MercuryPlugin> Dependencies { get; private set; } /// TODO: Implement
		public MercurySettings Settings { get; set; }

		public MercuryPlugin(string name, string relativeDirectory)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentRequiredException("name");
			}
			Name = name;

			if (string.IsNullOrEmpty(relativeDirectory))
			{
				throw new ArgumentRequiredException("relativeDirectory");
			}
			RelativeDirectory = relativeDirectory;

			Dependencies = new List<MercuryPlugin>();
			Settings = new MercurySettings();
		}

		public virtual BuildPlan Build(IEnumerable<MercuryEntity> entities, FileContentsProvider fileContentsProvider)
		{
			BuildPlan plan = new BuildPlan();
			return plan; // Subclasses override and add details to this plan
		}
	}
}
