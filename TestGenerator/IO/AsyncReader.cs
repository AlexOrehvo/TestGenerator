using System.IO;
using System.Threading.Tasks;

namespace TestGenerator
{
	public class AsyncReader
	{
		public async Task<string> Read(string path)
		{
			using (StreamReader sr = new StreamReader(path))
			{
				return await sr.ReadToEndAsync();
			}
		}
	}
}
