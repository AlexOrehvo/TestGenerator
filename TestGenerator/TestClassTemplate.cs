using System;
using System.Collections.Generic;
using System.Text;

namespace TestGenerator
{
	class TestClassTemplate
	{
		public string FileName
		{
			get;
		}

		public string Content
		{
			get; set;
		}

		public TestClassTemplate(string fileName, string content)
		{
			FileName = fileName;
			Content = content;
		}
	}
}
