using Mercury.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Core
{
	/// <summary>
	/// Great read on some techniques to find differences and merge
	/// http://xmailserver.org/diff2.pdf
	/// 
	/// This class takes two files, and does a line-by-line comparison to merge them together.  
	/// Sample use:
	/// 
	/// 1\n2\n3\n4\n\5
	/// 
	/// merged with 
	/// 
	/// 1\nA\nB\nC\n2\n3\n4
	/// 
	/// becomes:
	/// 1\nA\nB\nC\n2\n3\n4\n5
	/// </summary>
	public class LineMerger
	{
		private const string NEWLINE = "\r\n";

		public static string Merge(string contentA, string contentB)
		{
			if (contentA == null)
			{
				throw new ArgumentRequiredException("contentA");
			}
			if (contentB == null)
			{
				throw new ArgumentRequiredException("contentB");
			}

			if (contentA.CompareTo(contentB) == 0) // Optimization to prevent further work
			{
				return contentA;
			}

			string[] linesInA = GetLines(contentA);
			string[] linesInB = GetLines(contentB);

			LineMatchSet matches = GetMatchPoints(linesInA, linesInB);
			if (matches.Count() <= 0)
			{
				return String.Join(NEWLINE, linesInA, linesInB); // If there are no matches, just jam the two files together
			}

			StringBuilder content = new StringBuilder();
			foreach (LineMatch match in matches)
			{
				LineMatch previous = matches.GetPreviousMatch(match);
				if (previous == null)
				{
					AppendRange(linesInA, 0, match.IndexA, content);
					AppendRange(linesInB, 0, match.IndexB, content);
				}
				else
				{
					AppendRange(linesInA, previous.IndexA + 1, match.IndexA, content);
					AppendRange(linesInB, previous.IndexB + 1, match.IndexB, content);
				}

				string matchingLine = linesInA[match.IndexA]; // Doesn't matter which one we pull from, since it's the matching line
				content.AppendLine(matchingLine);
			}

			// Append the suffixes, if any
			AppendRange(linesInA, matches.LastMatch.IndexA + 1, linesInA.Length, content);
			AppendRange(linesInB, matches.LastMatch.IndexB + 1, linesInB.Length, content);

			string output = content.ToString();
			output = output.Trim();
			return output;
		}

		private static LineMatchSet GetMatchPoints(string[] a, string[] b)
		{
			LineMatchSet list = new LineMatchSet();

			string[] longerLines = a.Count() >= b.Count() ? a : b;
			string[] shorterLines = longerLines == a ? b : a;

			for (int i = 0; i < shorterLines.Length; i++)
			{
				string line = shorterLines[i];
				int? match = TryToGetNextMatchingLineIndex(line, longerLines, list.LastMatch == null ? 0 : list.LastMatch.IndexB);
				if (match.HasValue)
				{
					list.Add(new LineMatch(i, match.Value));
				}
			}

			if (shorterLines != a)
			{
				list.ReverseAll();
			}

			return list;
		}

		private static void AppendRange(string[] pullFrom, int startIndex, int endBeforeIndex, StringBuilder appendTo)
		{
			for (int i = startIndex; i < endBeforeIndex; i++)
			{
				string line = pullFrom[i];
				appendTo.AppendLine(line);
			}
		}

		private static string[] GetLines(string contents)
		{
			string[] tokens = contents.Split(new string[] { NEWLINE }, StringSplitOptions.None);  // Do NOT remove empty entries!  We don't want to magically skip empty lines.
			return tokens;
		}

		private static int? TryToGetNextMatchingLineIndex(string lookFor, string[] availableLines, int startFrom = 0)
		{
			int loopUntil = availableLines.Count();
			for (int i = startFrom; i < loopUntil; i++)
			{
				string line = availableLines[i];
				if (line.CompareTo(lookFor) == 0)
				{
					return i;
				}
			}

			return null;
		}
	}

	class LineMatchSet : List<LineMatch>
	{
		public LineMatchSet()
		{ }

		public LineMatch LastMatch
		{
			get { 
				if (this.Count() <= 0)
				{
					return null;
				}

				return this.Last();
			}
		}

		public LineMatch GetPreviousMatch(LineMatch match)
		{
			int index = this.IndexOf(match);
			if (index <= 0)
			{
				return null;
			}

			return this.ElementAt(index - 1);
		}

		public void ReverseAll()
		{
			foreach (LineMatch match in this)
			{
				match.Reverse();
			}
		}
	}

	class LineMatch
	{
		public int IndexA { get; private set; }
		public int IndexB { get; private set; }

		public LineMatch(int indexA, int indexB)
		{
			IndexA = indexA;
			IndexB = indexB;
		}

		public void Reverse()
		{
			int temp = IndexA;
			IndexA = IndexB;
			IndexB = temp;
		}
	}
}
