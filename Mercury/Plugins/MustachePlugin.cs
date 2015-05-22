using Mercury.Common;
using Mercury.Core;
using Mercury.Models;
using Nustache.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Plugins
{
	public class MustachePlugin : MercuryPlugin
	{
		public MustachePlugin(string name, string relativeDirectory)
			: base(name, relativeDirectory)
		{
		}

		public override BuildPlan Build(IEnumerable<MercuryEntity> entities, FileContentsProvider fileContentsProvider)
		{
			BuildPlan plan = base.Build(entities, fileContentsProvider);

			IEnumerable<string> sourceDirectories = fileContentsProvider.GetDirectoriesUnder(GetRelativeSourceDirectory());
			foreach (string directory in sourceDirectories)
			{
				string outputPath = RemoveSourceDirectoryFromPath(directory);
				if (PathNeedsTemplating(outputPath))
				{
					outputPath = ApplyMustache(outputPath, Settings);
				}

				EnsureDirectoryExistsBuildStep step = new EnsureDirectoryExistsBuildStep(outputPath);
				plan.Add(step);
			}

			IEnumerable<string> sourceFiles = fileContentsProvider.GetFilesUnder(GetRelativeSourceDirectory());
			foreach (string file in sourceFiles)
			{
				string outputFilePath = RemoveSourceDirectoryFromPath(file);
				bool fileNeedsTemplating = FileNeedsTemplating(outputFilePath);
				if (fileNeedsTemplating)
				{
					outputFilePath = RemoveTemplateFileExtension(outputFilePath);
				}

				if (PathNeedsTemplating(outputFilePath))
				{
					outputFilePath = ApplyMustache(outputFilePath, Settings);
				}

				string contents = fileContentsProvider.GetFileContents(file);
				if (fileNeedsTemplating)
				{
					contents = ApplyMustache(contents, Settings);
				}

				plan.Add(new SaveFileContentsBuildStep(outputFilePath, contents));
			}

			return plan;
		}

		private string GetRelativeTemplateDirectory()
		{
			return @"templates\";
		}

		private string GetRelativeSourceDirectory()
		{
			return @"source\";
		}

		private string RemoveSourceDirectoryFromPath(string path)
		{
			string sourceDirectory = GetRelativeSourceDirectory();
			if (!path.StartsWith(sourceDirectory))
			{
				return path;
			}

			return path.Substring(sourceDirectory.Length);
		}

		const string TEMPLATE_INDICATOR = "template";
		const string TEMPLATE_EXTENSION = "." + TEMPLATE_INDICATOR;
		private bool FileNeedsTemplating(string filepath)
		{
			string extension = FilesystemHelper.GetFileExtension(filepath);
			if (string.IsNullOrEmpty(extension))
			{
				return false;
			}

			return extension.CompareTo(TEMPLATE_INDICATOR) == 0;
		}

		private string RemoveTemplateFileExtension(string file)
		{
			return file.Substring(0, file.Length - TEMPLATE_EXTENSION.Length);
		}

		private bool PathNeedsTemplating(string path)
		{
			List<string> indicators = new List<string>() { "{{", "}}" };
			foreach (string indicator in indicators)
			{
				if (!path.Contains(indicator))
				{
					return false;
				}
			}

			return true;
		}

		private string ApplyMustache(string content, MercurySettings settings)
		{
			string output = Render.StringToString(content, settings.ToObject());
			return output;
		}
	}
}