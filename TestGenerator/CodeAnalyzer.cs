using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestGenerator
{
	class CodeAnalyzer
	{
		SyntaxTree syntaxThree;
		SyntaxNode syntaxRoot;
		List<ClassDeclarationSyntax> classList;

		public List<ClassInfo> Parse(string sourceCode) {
			syntaxRoot = syntaxThree.GetRoot();
			return GetClasses();
		}

		private List<ClassInfo> GetClasses()
		{
			classList = new List<ClassDeclarationSyntax>(syntaxRoot.DescendantNodes().
				OfType<ClassDeclarationSyntax>());
			List<ClassInfo> classesInfo = new List<ClassInfo>();
			foreach (ClassDeclarationSyntax cls in classList)
			{
				string className = cls.Identifier.ToString();
				NamespaceDeclarationSyntax nmspace = (NamespaceDeclarationSyntax)cls.Parent;
				string nmspaceName = nmspace.Name.ToString();
				classesInfo.Add(new ClassInfo(className, nmspaceName, GetMethods(cls)));
			}
			return classesInfo;
		}

		private List<string> GetMethods(ClassDeclarationSyntax cls)
		{
			return new List<string>(
				cls.DescendantNodes().OfType<MethodDeclarationSyntax>().Where(method => method.Modifiers.
				Any(m => m.ToString() == "public")).Select(e => e.Identifier.ToString()));
		}
	}
}
