using Mercury.Common;
using Mercury.Models.Configuration;
using Mercury.Models.Project;
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
        public MercuryBuilder(string coreSetupAbsoluteFilePath)
        {

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

			bool successfullyDeletedAllFiles = TryToDeleteAllFilesInDirectory(outputDirectory);
			if (!successfullyDeletedAllFiles)
			{
				throw new Exception("Unable to delete all of the files in the output directory.");
			}

			foreach (MercuryPlugin plugin in project.Plugins)
			{

			}
		}

		private bool TryToDeleteAllFilesInDirectory(string directory)
		{
			bool deletedAllFiles = true;
			string[] files = Directory.GetFiles(directory);
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

			return deletedAllFiles;
		}
    }
}
