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
	public class TemplateGeneratorTest
	{
		private string path;
		private AsyncReader ar;
		private TestClassTemplate testClassTemplate;
		private TemplateGenerator generator;
		private SyntaxNode root;
		private string sourceCode;
		private SyntaxNode generatedRoot;

		[TestInitialize]
		public void Initialize()
		{
			path = Environment.CurrentDirectory + "\\..\\..\\..\\TemplateGeneratorTest.cs";
			ar = new AsyncReader();
			sourceCode = ar.Read(path).Result;

			generator = new TemplateGenerator();

			root = CSharpSyntaxTree.ParseText(sourceCode).GetRoot();
		}

		[TestMethod]
		public void GetTemplateTest()
		{
			testClassTemplate = generator.GetTemplate(sourceCode);
			Assert.IsNotNull(testClassTemplate);
			generatedRoot = CSharpSyntaxTree.ParseText(testClassTemplate.Content).GetRoot();
			List<ClassDeclarationSyntax> generatedClasses = new List<ClassDeclarationSyntax>(generatedRoot.DescendantNodes().OfType<ClassDeclarationSyntax>());
			List<ClassDeclarationSyntax> sourceClasses = new List<ClassDeclarationSyntax>(root.DescendantNodes().OfType<ClassDeclarationSyntax>());
			Assert.AreEqual(sourceClasses.Count, generatedClasses.Count);

			for (int i = 0; i < sourceClasses.Count; i++)
			{
				ClassDeclarationSyntax sourceClass = sourceClasses[i];
				ClassDeclarationSyntax generatedClass = generatedClasses[i];

				List<MethodDeclarationSyntax> sourceMethods = new List<MethodDeclarationSyntax>(
					sourceClass.DescendantNodes().OfType<MethodDeclarationSyntax>()
					.Where(method => method.Modifiers.
						Any(modifer => modifer.ToString() == "public")));
				List<MethodDeclarationSyntax> generatedMethods = new List<MethodDeclarationSyntax>(
					generatedClass.DescendantNodes().OfType<MethodDeclarationSyntax>()
					.Where(method => method.Modifiers.
						Any(modifer => modifer.ToString() == "public")));
				Assert.AreEqual(sourceMethods.Count, generatedMethods.Count);

				for (int j = 0; j < sourceMethods.Count; j++)
				{
					MethodDeclarationSyntax sourceMethod = sourceMethods[j];
					MethodDeclarationSyntax generatedMethod = generatedMethods[j];
					Assert.IsTrue(generatedMethod.Identifier.ToString().Contains(sourceMethod.Identifier.ToString()));
				}
			}
		}
	}
}