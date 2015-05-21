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

		private string _coreDirectory;

        public MercuryBuilder(string coreDirectory)
        {
			if (string.IsNullOrEmpty(coreDirectory))
			{
				throw new ArgumentRequiredException("coreDirectory");
			}

			_coreDirectory = FilesystemHelper.EnsureTrailingForwardSlash(coreDirectory);
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

			HaveEachPluginMoveTheirSourceFilesOver(project.Plugins, outputDirectory);
			GiveEachPluginChanceToProcessEntities(project.Plugins, outputDirectory, project.Entities);
		}

		private bool FileNeedsTemplating(string filepath)
		{
			string extension = FilesystemHelper.GetFileExtension(filepath);
			if (string.IsNullOrEmpty(extension))
			{
				return false;
			}

			return extension.EndsWith(TEMPLATE_EXTENSION);
		}

		private void HaveEachPluginMoveTheirSourceFilesOver(IEnumerable<MercuryPlugin> plugins, string outputDirectory)
		{
			foreach (MercuryPlugin plugin in plugins)
			{
				string pluginRootDirectory = GetPluginSourceDirectoryPath(plugin);
				string[] pluginDirectories = GetAllDirectoriesInPluginSourceDirectory(plugin);
				foreach (string pluginDirectory in pluginDirectories)
				{
					string useDirectory = GetPathRelativeTo(pluginDirectory, pluginRootDirectory);
					useDirectory = outputDirectory + @"\" + plugin.ChanceToChangeDirectoryName(useDirectory);
					FilesystemHelper.EnsureAllDirectoriesExist(useDirectory);
				}

				string[] pluginFiles = GetAllFilesInPluginSourceDirectory(plugin);
				foreach (string pluginFile in pluginFiles)
				{
					string relativePath = GetPathRelativeTo(pluginFile, pluginRootDirectory);
					string outputFilePath = outputDirectory + @"\" + relativePath;
					bool fileNeedsTemplating = FileNeedsTemplating(outputFilePath);
					if (fileNeedsTemplating)
					{
						outputFilePath = outputFilePath.Substring(0, outputFilePath.Length - TEMPLATE_INDICATOR.Length); // Strip out the template extension
					}

					outputFilePath = plugin.ChanceToChangeFileName(outputFilePath);

					string contents = string.Empty;
					if (!fileNeedsTemplating)
					{
						contents = File.ReadAllText(pluginFile);
					}
					else
					{
						contents = Render.FileToString(pluginFile, plugin.Settings.ToObject());	
					}

					foreach (MercuryPlugin otherPlugin in plugins)
					{
						contents = otherPlugin.ChanceToProcessFile(relativePath, contents);
					}

					FilesystemHelper.EnsureDirectoryExistsAndWriteOrMergeFileContents(outputFilePath, contents);
				}
			}
		}

		private string GetPluginSourceDirectoryPath(MercuryPlugin plugin)
		{
			string pluginDirectory = _coreDirectory + @"Plugins\" + FilesystemHelper.EnsureNoPrefixedForwardSlash(plugin.SourceDirectory);
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

		private void GiveEachPluginChanceToProcessEntities(IEnumerable<MercuryPlugin> plugins, string outputDirectory, IEnumerable<MercuryEntity> entities)
		{
			foreach (MercuryPlugin plugin in plugins)
			{
				plugin.ChanceToProcessEntities(entities, _coreDirectory, outputDirectory);
			}
		}
    }
}
