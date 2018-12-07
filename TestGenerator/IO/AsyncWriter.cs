using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestGenerator
{
	public class AsyncWriter
	{
		private string outPutDirectory;

		public AsyncWriter(string outPutDirectory)
		{
			this.outPutDirectory = outPutDirectory;
		}

		public async Task Write(TestClassInfo classTemplate)
		{
			if (classTemplate == null)
			{
				return;
			}

			string filePath = outPutDirectory + "\\" + classTemplate.FileName;
			using (StreamWriter sw = new StreamWriter(filePath))
			{
				await sw.WriteAsync(classTemplate.Content);
			}
		}
	}
}
