using Mercury.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Core
{
	public class EnsureDirectoryExistsBuildStep : IBuildStep
	{
		public string RelativePath { get; set; }

		public EnsureDirectoryExistsBuildStep(string relativePath)
		{
			if (string.IsNullOrEmpty(relativePath))
			{
				throw new ArgumentRequiredException("relativePath");
			}
			RelativePath = relativePath;
		}

		public void Execute(FileContentsProvider outputProvider)
		{
			string outputPath = outputProvider.GetMergedPath(RelativePath);
			FilesystemHelper.EnsureAllDirectoriesExist(outputPath);
		}

		public bool ConflictsWith(IBuildStep buildStep)
		{
			if (!(buildStep is EnsureDirectoryExistsBuildStep))
			{
				return false;
			}

			EnsureDirectoryExistsBuildStep asDirectoryStep = (EnsureDirectoryExistsBuildStep)buildStep;
			return RelativePath.CompareTo(asDirectoryStep.RelativePath) == 0;
		}

		public IBuildStep MergeWith(IEnumerable<IBuildStep> otherSteps)
		{
			return new EnsureDirectoryExistsBuildStep(RelativePath);
		}
	}
}
