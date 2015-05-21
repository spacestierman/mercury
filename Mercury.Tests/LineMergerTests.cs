using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mercury.Core;

namespace Mercury.Tests
{
	[TestClass]
	public class LineMergerTests
	{
		[TestMethod]
		public void TestNoChangesNeeded()
		{
			const string CONTENTS = "Here\r\nIs\r\nA\r\nTest";
			string a = string.Copy(CONTENTS);
			string b = string.Copy(CONTENTS);

			string merged = LineMerger.Merge(a, b);
			Assert.AreEqual(CONTENTS, merged);
		}

		[TestMethod]
		public void TestInAButNotB()
		{
			string a = "Here\r\nIs\r\nSomething in\r\nA\r\nTest";
			string b = "Here\r\nIs\r\nA\r\nTest";

			string c = LineMerger.Merge(a, b);
			Assert.AreEqual(a, c);
		}

		[TestMethod]
		public void TestInBButNotA()
		{
			string a = "Here\r\nIs\r\nA\r\nTest";
			string b = "Here\r\nIs\r\nSomething in\r\nA\r\nTest";

			string c = LineMerger.Merge(a, b);
			Assert.AreEqual(a, c);
		}

		[TestMethod]
		public void ExampleMultipleMerge()
		{
			const string a = "1\r\n2\r\n3\r\n4\r\n5\r\n6\r\n7\r\n8\r\n9\r\n10";
			const string b = "1\r\nA\r\nB\r\nC\r\n2\r\nD\r\nE\r\nF\r\n3\r\n5\r\n6\r\n7\r\n8";
			
			string merged = LineMerger.Merge(a, b);
			const string expected = "1\r\nA\r\nB\r\nC\r\n2\r\nD\r\nE\r\nF\r\n3\r\n4\r\n5\r\n6\r\n7\r\n8\r\n9\r\n10";
			Assert.AreEqual(expected, merged);
		}
	}
}
