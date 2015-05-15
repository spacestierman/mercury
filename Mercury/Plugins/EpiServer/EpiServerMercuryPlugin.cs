using Mercury.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Core.Plugins.EpiServer
{
	public class EpiServerMercuryPlugin : MercuryPlugin
	{
		public string Namespace { get; set; }

		public EpiServerMercuryPlugin() : base("EpiServer", @"Episerver\source\")
		{
			ListenForFilenamePattern("Web.config");
		}

		public override void LoadSettings(MercurySettings settings)
		{
			base.LoadSettings(settings);

			const string KEY_NAMESPACE = "Namespace";
			if (settings.Contains(KEY_NAMESPACE))
			{
				Namespace = settings.GetAsString(KEY_NAMESPACE);
			}
		}

		protected override string OnFilenamePatternMatched(string filename, string contents)
		{
			Console.WriteLine("EpiServerMercuryPlugin ChanceToProcessFile(\"{0}\")", filename);
			return contents;
		}
	}
}