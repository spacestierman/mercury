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
	public class AspMvc4MercuryPlugin : MercuryPlugin
	{
		const string BUILD_NAME = "MvcApplicationName";

		public AspMvc4MercuryPlugin() : base("ASP.NET MVC 4", @"ASPMVC4\source\", @"ASPMVC4\templates\")
		{
		}

		public string Namespace {
			get { return Settings.GetAsString("Namespace"); }
		}

		override public string ChanceToChangeDirectoryName(string directoryPath)
		{
			if (directoryPath.IndexOf(BUILD_NAME) >= 0)
			{
				directoryPath = directoryPath.Replace(BUILD_NAME, Settings.GetAsString("Namespace"));
			}

			return directoryPath;
		}

		override public string ChanceToChangeFileName(string filePath)
		{
			if (filePath.IndexOf(BUILD_NAME) >= 0)
			{
				filePath = filePath.Replace(BUILD_NAME, Settings.GetAsString("Namespace"));
			}

			return filePath;
		}

		public override void ChanceToProcessEntities(IEnumerable<MercuryEntity> entities, string coreDirectory, string outputDirectory)
		{
			base.ChanceToProcessEntities(entities, coreDirectory, outputDirectory);

			foreach (MercuryEntity entity in entities)
			{
				EntityAndSettingsMustacheModel model = new EntityAndSettingsMustacheModel();
				model.Entity = entity;
				model.Settings = Settings.ToObject();

				/// TODO: figure out a better way to not hardcode a bunch of these paths
				string controllerCode = Render.FileToString(coreDirectory + @"Plugins\" + TemplateDirectory + "Controller.cs.template", model);
				string controllerFile = outputDirectory + Namespace + @"\Controllers\" + entity.Name + "Controller.cs";

				FilesystemHelper.EnsureDirectoryExistsAndWriteFileContents(controllerFile, controllerCode);
			}
		}
	}
}