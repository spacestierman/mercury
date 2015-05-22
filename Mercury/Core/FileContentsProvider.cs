using Mercury.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Core
{
	public class FileContentsProvider
	{
		private string _absoluteRootPath;

		public FileContentsProvider(string absoluteRootPath)
		{
			if (string.IsNullOrEmpty(absoluteRootPath))
			{
				throw new ArgumentRequiredException("absoluteRootPath");
			}
			_absoluteRootPath = FilesystemHelper.EnsureTrailingForwardSlash(absoluteRootPath);
		}

		public string GetFileContents(string relativePath)
		{
			string mergedPath = FilesystemHelper.MergePaths(_absoluteRootPath, relativePath);
			return File.ReadAllText(mergedPath);
		}

		public IEnumerable<string> GetFilesUnder(string relativePath, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories)
		{
			string mergedPath = GetMergedPath(relativePath);
			string[] files = Directory.GetFiles(mergedPath, searchPattern, searchOption);
			return MakePathsRelative(files);
		}

		public IEnumerable<string> GetDirectoriesUnder(string relativePath, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories)
		{
			string mergedPath = GetMergedPath(relativePath);
			string[] directories = Directory.GetDirectories(mergedPath, searchPattern, searchOption);
			return MakePathsRelative(directories);
		}

		private IEnumerable<string> MakePathsRelative(string[] paths)
		{
			List<string> relativePaths = new List<string>();
			foreach (string path in paths)
			{
				string temp = FilesystemHelper.EnsureForwardSlashes(path);
				temp = temp.Substring(_absoluteRootPath.Length);
				relativePaths.Add(temp);
			}

			return relativePaths;
		}

		public string GetMergedPath(string relativePath)
		{
			string path = FilesystemHelper.MergePaths(_absoluteRootPath, relativePath);
			return path;
		}
	}
}