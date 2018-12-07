using System;
using System.Collections.Generic;
using System.Text;

namespace TestGenerator
{
	public class TestClassInfo
	{
		public string FileName
		{
			get;
		}

		public string Content
		{
			get; set;
		}

		public TestClassInfo(string fileName, string content)
		{
			FileName = fileName;
			Content = content;
		}
	}
}
