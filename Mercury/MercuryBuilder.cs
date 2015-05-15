using DiffMatchPatch;
using Mercury.Common;
using Mercury.Core;
using Mercury.Models;
using Mercury.Models.Configuration;
using Mercury.Models.Project;
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
		private string _coreDirectory;
		private MercuryConfiguration _configuration;

        public MercuryBuilder(string coreDirectory)
        {
			if (string.IsNullOrEmpty(coreDirectory))
			{
				throw new ArgumentRequiredException("coreDirectory");
			}

			_coreDirectory = coreDirectory;
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

			bool successfullyDeletedAllFiles = TryToEmptyDirectory(outputDirectory);
			if (!successfullyDeletedAllFiles)
			{
				throw new Exception("Unable to delete all of the files in the output directory.");
			}

			foreach (MercuryPlugin plugin in project.Plugins)
			{
				string[] pluginDirectories = GetAllDirectoriesInPluginSourceDirectory(plugin);
				foreach (string pluginDirectory in pluginDirectories)
				{
					FilesystemHelper.EnsureAllDirectoriesExist(pluginDirectory);
				}

				string pluginRootDirectory = GetPluginSourceDirectoryPath(plugin);
				string[] pluginFiles = GetAllFilesInPluginSourceDirectory(plugin);
				foreach (string pluginFile in pluginFiles)
				{
					string relativePath = GetPathRelativeTo(pluginFile, pluginRootDirectory);
					string outputFilePath = outputDirectory + @"\" + relativePath;
					string outputDirectoryPath = FilesystemHelper.GetDirectoryForFilePath(outputFilePath);

					string contents = Render.FileToString(pluginFile, plugin.Settings.ToObject());
					if (File.Exists(outputFilePath))
					{
						 string existingFileContents = File.ReadAllText(outputFilePath);
						diff_match_patch dmp = new diff_match_patch();
						List<Diff> differences = dmp.diff_lineMode(existingFileContents, contents);
						differences = differences.Where(x => x.operation != Operation.DELETE).ToList(); // Since we're "merging" whole different features, we don't want to remove lines. So we remove any deletes from the diff list.
						List<Patch> patches = dmp.patch_make(existingFileContents, differences);
						string patchesText = dmp.patch_toText(patches);
						object[] results = dmp.patch_apply(patches, existingFileContents);
						contents = (string)results[0];
					}

					foreach (MercuryPlugin otherPlugin in project.Plugins)
					{
						contents = otherPlugin.ChanceToProcessFile(relativePath, contents);
					}

					File.WriteAllText(outputFilePath, contents);
				}
			}
		}

		private bool TryToEmptyDirectory(string directory)
		{
			bool deletedAllFiles = true;
			string[] files = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
			foreach (string file in files)
			{
				try
				{
					File.Delete(file);
				}
				catch
				{
					deletedAllFiles = false;
				}
			}

			string[] directories = Directory.GetDirectories(directory);
			foreach (string directoryPath in directories)
			{
				try
				{
					Directory.Delete(directoryPath, true);
				}
				catch
				{
					deletedAllFiles = false;
				}
			}

			return deletedAllFiles;
		}

		private string GetPluginSourceDirectoryPath(MercuryPlugin plugin)
		{
			string pluginDirectory = _coreDirectory + @"\Plugins\" + plugin.SourceDirectory;
			return pluginDirectory;
		}

		private string[] GetAllDirectoriesInPluginSourceDirectory(MercuryPlugin plugin)
		{
			string pluginDirectory = GetPluginSourceDirectoryPath(plugin);
			return Directory.GetDirectories(pluginDirectory, "*", SearchOption.AllDirectories);
		}

		private string[] GetAllFilesInPluginSourceDirectory(MercuryPlugin plugin)
		{
			string pluginDirectory = GetPluginSourceDirectoryPath(plugin);
			return Directory.GetFiles(pluginDirectory, "*", SearchOption.AllDirectories);
		}

		private string GetPathRelativeTo(string absolutePath, string relativeFromPath)
		{
			if (!absolutePath.StartsWith(relativeFromPath))
			{
				throw new ArgumentException("absolutePath must be relative to the supplied path");
			}

			/// TODO: Add in better checks for trailing slashes, etc
			string result = absolutePath.Substring(relativeFromPath.Length);
			return result;
		}
    }
}
