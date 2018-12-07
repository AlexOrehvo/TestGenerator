using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TestGenerator;
using System.IO;

namespace TestGeneratorTests
{
	[TestClass]
	public class TestGeneratorTest
	{
		string resultPath;
		private AsyncWriter aw;
		private List<string> files;
		private int maxReadableCount;
		private int maxProcessableCount;
		private int maxWritableCount;
		private TestGenerator.TestGenerator testsGenerator;

		[TestInitialize]
		public void Initialize()
		{
			resultPath = Environment.CurrentDirectory + "\\Tests";
			aw = new AsyncWriter(resultPath);

			files = new List<string>();
			string pathToFile1 = Environment.CurrentDirectory + "\\..\\..\\..\\TestGeneratorTest.cs";
			files.Add(pathToFile1);

			maxReadableCount = 3;
			maxProcessableCount = 3;
			maxWritableCount = 3;
			testsGenerator = new TestGenerator.TestGenerator(files, maxReadableCount, maxProcessableCount, maxWritableCount);
		}

		[TestMethod]
		public void GenerateTest()
		{
			testsGenerator.Generate(aw).Wait();
			int currentCountOfFiles = Directory.GetFiles(resultPath).Length;
			int expectedCount = files.Count;
			Assert.AreEqual(expectedCount, currentCountOfFiles);
			foreach (string filePath in files)
			{
				string pathToResFile = resultPath + "\\" + Path.GetFileNameWithoutExtension(filePath) + "Tests.cs";
				File.Delete(pathToResFile);
			}
		}

	}
}
