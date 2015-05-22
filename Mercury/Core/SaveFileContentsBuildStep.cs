using Mercury.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Core
{
	public class SaveFileContentsBuildStep : IBuildStep
	{
		public string RelativeFilePath { get; set; }
		public string Contents { get; set; }

		public SaveFileContentsBuildStep(string relativeFilePath, string contents)
		{
			if (string.IsNullOrEmpty(relativeFilePath))
			{
				throw new ArgumentRequiredException("relativeFilePath");
			}
			RelativeFilePath = relativeFilePath;

			if (contents == null)
			{
				throw new ArgumentException("contents must not be null.  Use string.Empty to indicate no content.");
			}
			Contents = contents;
		}

		public void Execute(FileContentsProvider outputProvider)
		{
			string outputPath = outputProvider.GetMergedPath(RelativeFilePath);
			FilesystemHelper.EnsureDirectoryExistsAndWriteOrMergeFileContents(outputPath, Contents);
		}

		public bool ConflictsWith(IBuildStep buildStep)
		{
			if (!(buildStep is SaveFileContentsBuildStep))
			{
				return false;
			}

			SaveFileContentsBuildStep asSaveFile = (SaveFileContentsBuildStep)buildStep;
			return asSaveFile.RelativeFilePath.CompareTo(RelativeFilePath) == 0;
		}

		public IBuildStep MergeWith(IEnumerable<IBuildStep> otherSteps)
		{
			string useContent = Contents;
			foreach (SaveFileContentsBuildStep step in otherSteps)
			{
				useContent = LineMerger.Merge(useContent, step.Contents);
			}

			return new SaveFileContentsBuildStep(RelativeFilePath, useContent);
		}
	}
}