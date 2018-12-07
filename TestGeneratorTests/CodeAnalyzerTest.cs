using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TestGenerator;
using System.Linq;

namespace TestGeneratorTests
{
	[TestClass]
	public class CodeAnalyzerTest
	{
		private string path;
		private AsyncReader ar;
		private CodeAnalyzer analyzer;
		private List<ClassInfo> classes;
		private SyntaxNode root;

		[TestInitialize]
		public void Initialize()
		{
			path = Environment.CurrentDirectory + "\\..\\..\\..\\CodeAnalyzerTest.cs";
			ar = new AsyncReader();
			string sourceCode = ar.Read(path).Result;

			analyzer = new CodeAnalyzer();
			classes = analyzer.Parse(sourceCode);

			root = CSharpSyntaxTree.ParseText(sourceCode).GetRoot();
		}

		[TestMethod]
		public void ParseTest()
		{
			foreach (var cls in classes)
			{
				List<ClassDeclarationSyntax> sameClasses = new List<ClassDeclarationSyntax>(
					root.DescendantNodes().OfType<ClassDeclarationSyntax>()
					.Where((classInfo) =>
					(classInfo.Identifier.ToString() == cls.Name)
					&& (((NamespaceDeclarationSyntax)classInfo.Parent).Name.ToString() == cls.Namespace)));
				Assert.AreNotEqual(sameClasses.Count, 0);
			}
		}
	}
}
