using DiffMatchPatch;
using Mercury.Common;
using Mercury.Core;
using Mercury.Models;
using Nustache.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury
{
    public class MercuryBuilder
    {
		private const string TEMPLATE_EXTENSION = "template";
		private const string TEMPLATE_INDICATOR = "." + TEMPLATE_EXTENSION;

		private string _rootDirectory;

        public MercuryBuilder(string rootDirectory)
        {
			if (string.IsNullOrEmpty(rootDirectory))
			{
				throw new ArgumentRequiredException("rootDirectory");
			}

			_rootDirectory = FilesystemHelper.EnsureTrailingForwardSlash(rootDirectory);
        }

        public void Build(string projectJsonAbsoluteFilePath, string outputDirectory)
        {
			MercuryProject project = MercuryProjectParser.TryToParseFile(projectJsonAbsoluteFilePath);
			Build(project, outputDirectory);
        }

		public void Build(MercuryProject project, string outputDirectory)
		{
			if (project == null)
			{
				throw new ArgumentRequiredException("project");
			}
			if (string.IsNullOrEmpty(outputDirectory))
			{
				throw new ArgumentRequiredException("outputDirectory");
			}
			if (!Directory.Exists(outputDirectory))
			{
				try
				{
					Directory.CreateDirectory(outputDirectory);
				}
				catch
				{
					throw new ArgumentException("outputDirectory did not exist and could not be created.");
				}
			}

			bool successfullyDeletedAllFiles = FilesystemHelper.TryToEmptyDirectory(outputDirectory);
			if (!successfullyDeletedAllFiles)
			{
				throw new Exception("Unable to delete all of the files in the output directory.");
			}

			
			BuildPlan projectBuildPlan = new BuildPlan();
			foreach (MercuryPlugin plugin in project.Plugins)
			{
				string pluginDirectory = FilesystemHelper.MergePaths(GetPluginsDirectory(), plugin.RelativeDirectory);
				FileContentsProvider pluginProvider = new FileContentsProvider(pluginDirectory);
				BuildPlan pluginPlan = plugin.Build(project.Entities, pluginProvider);
				projectBuildPlan.Merge(pluginPlan);
			}

			FileContentsProvider outputProvider = new FileContentsProvider(outputDirectory);
			projectBuildPlan.Execute(outputProvider);
		}

		private string GetPluginsDirectory()
		{
			return FilesystemHelper.MergePaths(_rootDirectory, @"plugins\");
		}
    }
}
