using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Core
{
	internal static class FilesystemHelper
	{
		private const string FORWARD_SLASH = @"\";
		private const string BACK_SLASH = "/";

		public static void EnsureAllDirectoriesExist(string directoryPath)
		{
			directoryPath = EnsureForwardSlashes(directoryPath);
			directoryPath = EnsureTrailingForwardSlash(directoryPath);

			IEnumerable<string> directories = GetAllDirectories(directoryPath);
			foreach (string directory in directories)
			{
				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}
			}
		}

		public static string EnsureForwardSlashes(string path)
		{
			string output = path.Replace(BACK_SLASH, FORWARD_SLASH);
			return output;
		}

		public static string EnsureTrailingForwardSlash(string path)
		{
			string output = EnsureForwardSlashes(path);
			if (!output.EndsWith(FORWARD_SLASH))
			{
				output = output + FORWARD_SLASH;
			}

			return output;
		}

		public static string GetDirectoryForFilePath(string filePath)
		{
			string output = EnsureForwardSlashes(filePath);
			return Path.GetDirectoryName(filePath);
		}

		public static IEnumerable<string> GetAllDirectories(string path)
		{
			path = EnsureForwardSlashes(path);
			string[] tokens = path.Split(new string[] { FORWARD_SLASH }, StringSplitOptions.RemoveEmptyEntries);

			List<string> directories = new List<string>();
			StringBuilder builder = new StringBuilder();
			for (var i = 0; i < tokens.Length; i++)
			{
				string directory = tokens[i] + FORWARD_SLASH;
				builder.Append(directory);

				directories.Add(builder.ToString());
			}

			return directories;
		}
	}
}
