using System;
using System.Collections.Generic;
using TestGenerator;

namespace TestGeneratorDemo
{
	class Program
	{
		static void Main()
		{
			List<string> inputFiles = new List<string>();
			inputFiles.Add("../../../../Classes/Class1.cs");
			inputFiles.Add("../../../../Classes/Classes.cs");

			TestGenerator.TestGenerator generator =
				new TestGenerator.TestGenerator(inputFiles, 2, 2, 2);

			AsyncWriter asyncWriter = new AsyncWriter("../../../../Tests");

			generator.Generate(asyncWriter).Wait();

			Console.WriteLine("Generation completed");
			Console.ReadKey();
		}
	}
}
