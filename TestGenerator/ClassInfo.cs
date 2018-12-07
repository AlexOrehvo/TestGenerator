using System;
using System.Collections.Generic;
using System.Text;

namespace TestGenerator
{
	public class ClassInfo
	{
		public string Name
		{
			get; set;
		}

		public string Namespace
		{
			get; set;
		}

		public List<string> MethodList
		{
			get; set;
		}

		public ClassInfo(string name, string nmspace, List<string> methodList)
		{
			Name = name;
			Namespace = nmspace;
			MethodList = methodList;
		}
	}
}
