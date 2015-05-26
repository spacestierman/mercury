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
			plan.Merge(BuildWebProjectFile(entities, fileContentsProvider));
			plan.Merge(BuildControllerPlans(entities, fileContentsProvider));
			plan.Merge(BuildInputModelPlans(entities, fileContentsProvider));
			//plan.Merge(BuildInputValidatorPlans(entities, fileContentsProvider));
			plan.Merge(BuildSqlImportPlans(entities, fileContentsProvider));

			/// TODO: Build Controllers, ViewModels, etc
			return plan;
		}

		private BuildPlan BuildWebProjectFile(IEnumerable<MercuryEntity> entities, FileContentsProvider fileContentsProvider)
		{
			BuildPlan plan = new BuildPlan();

			string template = fileContentsProvider.GetFileContents(GetTemplateFilePath("Web.csproj.template"));
			foreach (MercuryEntity entity in entities)
			{
				string filePath = Settings.Get("Namespace") + @".Web\" + Settings.Get("Namespace") + ".Web.csproj";
				string contents = BuildEntityFileContents(entity, template);

				plan.Add(new SaveFileContentsBuildStep(filePath, contents));
			}

			return plan;
		}

		private BuildPlan BuildControllerPlans(IEnumerable<MercuryEntity> entities, FileContentsProvider fileContentsProvider)
		{
			BuildPlan plan = new BuildPlan();

			string template = fileContentsProvider.GetFileContents(GetTemplateFilePath("Controller.cs.template"));
			foreach (MercuryEntity entity in entities)
			{
				string filePath = Settings.Get("Namespace") + @".Web\Controllers\" + entity.NameSingular + "Controller.cs";
				string contents = BuildEntityFileContents(entity, template);

				plan.Add(new SaveFileContentsBuildStep(filePath, contents));
			}

			return plan;
		}

		private BuildPlan BuildInputModelPlans(IEnumerable<MercuryEntity> entities, FileContentsProvider fileContentsProvider)
		{
			BuildPlan plan = new BuildPlan();

			string template = fileContentsProvider.GetFileContents(GetTemplateFilePath("InputModel.cs.template"));
			foreach (MercuryEntity entity in entities)
			{
				string filePath = Settings.Get("Namespace") + @".Web\Models\" + entity.NameSingular + @"\" + entity.NameSingular + @"InputModel.cs";
				string contents = BuildEntityFileContents(entity, template);

				plan.Add(new SaveFileContentsBuildStep(filePath, contents));
			}

			return plan;
		}

		private BuildPlan BuildInputValidatorPlans(IEnumerable<MercuryEntity> entities, FileContentsProvider fileContentsProvider)
		{
			BuildPlan plan = new BuildPlan();

			string template = fileContentsProvider.GetFileContents(GetTemplateFilePath("InputModelValidator.cs.template"));
			foreach (MercuryEntity entity in entities)
			{
				string filePath = Settings.Get("Namespace") + @".Web\Models\" + entity.NameSingular + @"\" + entity.NameSingular + @"InputModel.cs";
				string contents = BuildEntityFileContents(entity, template);

				plan.Add(new SaveFileContentsBuildStep(filePath, contents));
			}

			return plan;
		}

		private BuildPlan BuildSqlImportPlans(IEnumerable<MercuryEntity> entities, FileContentsProvider fileContentsProvider)
		{
			string template = fileContentsProvider.GetFileContents(GetTemplateFilePath("SqlImport.sql.template"));
			EntitiesAndSettingsMustacheModel model = new EntitiesAndSettingsMustacheModel(entities, Settings);
			string filePath = Settings.Get("Namespace") + @".Data\SqlImport.sql";
			string contents = ApplyMustache(template, model);

			BuildPlan plan = new BuildPlan();
			plan.Add(new SaveFileContentsBuildStep(filePath, contents));

			return plan;
		}

		private string BuildEntityFileContents(MercuryEntity entity, string template)
		{
			EntityAndSettingsMustacheModel model = new EntityAndSettingsMustacheModel(entity, Settings);
			return ApplyMustache(template, model);
		}
	}
}