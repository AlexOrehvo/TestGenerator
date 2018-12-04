using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TestGenerator
{
	public class TestGenerator
	{
		private int maxOriginalFileCount;
		private int maxProcessedFileCount;
		private int maxWritetableFileCount;
		private List<string> fileNameList;

		public TestGenerator(List<string> fileNameList, int maxOriginalFileCount,
			int maxProcessedFileCount, int maxWritetableFileCount)
		{
			this.fileNameList = fileNameList;
			this.maxOriginalFileCount = maxOriginalFileCount;
			this.maxProcessedFileCount = maxProcessedFileCount;
			this.maxWritetableFileCount = maxWritetableFileCount;
		}

		public async Task Generate(AsyncWriter writer)
		{
			DataflowLinkOptions linkOptions = new DataflowLinkOptions
			{
				PropagateCompletion = true
			};

			ExecutionDataflowBlockOptions maxReadableFilesTasks = new ExecutionDataflowBlockOptions
			{
				MaxDegreeOfParallelism = maxOriginalFileCount
			};

			ExecutionDataflowBlockOptions maProcessedFilesTasks = new ExecutionDataflowBlockOptions
			{
				MaxDegreeOfParallelism = maxProcessedFileCount
			};

			ExecutionDataflowBlockOptions maxWritetableFilesTasks = new ExecutionDataflowBlockOptions
			{
				MaxDegreeOfParallelism = maxWritetableFileCount
			};

			AsyncReader ar = new AsyncReader();
		}
	}
}
