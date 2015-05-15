using Mercury.Common;
using Mercury.Models.Project;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mercury.Models.Configuration
{
	public class MercuryPlugin
	{
		public string Name { get; set; }
		public string SourceDirectory { get; set; }
		
		public MercurySettings Settings { get; set; }

		public List<MercuryPlugin> Dependencies { get; set; }


		private List<Regex> _listenForFilenamePatterns;

		public MercuryPlugin(string name, string sourceDirectory)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentRequiredException("name");
			}
			if (string.IsNullOrEmpty(sourceDirectory))
			{
				throw new ArgumentRequiredException("sourceDirectory");
			}

			Name = name;
			SourceDirectory = sourceDirectory;

			Settings = new MercurySettings();

			Dependencies = new List<MercuryPlugin>();
			_listenForFilenamePatterns = new List<Regex>();
		}

		public virtual void LoadSettings(MercurySettings projectSettings)
		{
			// For subclasses to pull values out of
		}

		public void ListenForFilenamePattern(string pattern)
		{
			if (string.IsNullOrEmpty(pattern))
			{
				throw new ArgumentRequiredException("pattern");
			}

			Regex regex = new Regex(pattern);
			ListenForFilenamePattern(regex);
		}

		public void ListenForFilenamePattern(Regex pattern)
		{
			if (pattern == null)
			{
				throw new ArgumentRequiredException("pattern");
			}

			if (_listenForFilenamePatterns.Contains(pattern))
			{
				throw new ArgumentException("pattern is already in the set");
			}

			_listenForFilenamePatterns.Add(pattern);
		}

		public virtual string ChanceToProcessFile(string filename, string contents)
		{
			Console.WriteLine("MercuryPlugin ChanceToProcessFile(\"{0}\", contents)", filename);
			string output = contents;
			foreach (Regex regex in _listenForFilenamePatterns)
			{
				if (regex.IsMatch(filename))
				{
					output = OnFilenamePatternMatched(filename, contents);
				}
			}

			return output;
		}

		protected virtual string OnFilenamePatternMatched(string filename, string contents) 
		{
			return contents; // For subclasses to implement
		}

		public virtual string ChanceToChangeDirectoryName(string directoryPath)
		{
			return directoryPath; // For subclasses to implement
		}

		public virtual string ChanceToChangeFileName(string filePath)
		{
			return filePath; // For subclasses to implement
		}
	}
}
