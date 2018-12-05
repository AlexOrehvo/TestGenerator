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

			ExecutionDataflowBlockOptions maxProcessedFilesTasks = new ExecutionDataflowBlockOptions
			{
				MaxDegreeOfParallelism = maxProcessedFileCount
			};

			ExecutionDataflowBlockOptions maxWritetableFilesTasks = new ExecutionDataflowBlockOptions
			{
				MaxDegreeOfParallelism = maxWritetableFileCount
			};

			AsyncReader ar = new AsyncReader();
			TemplateGenerator generator = new TemplateGenerator();

			TransformBlock<string, string> readingBlock =
				new TransformBlock<string, string>(new Func<string, Task<String>>(ar.Read), maxReadableFilesTasks);

			TransformBlock<string, TestClassTemplate> processingBlock =
				new TransformBlock<string, TestClassTemplate>(new Func<string, TestClassTemplate>(generator.GetTemplate), maxProcessedFilesTasks);

			ActionBlock<TestClassTemplate> writingBlock = new ActionBlock<TestClassTemplate>(
				(classTemplate) => writer.Write(classTemplate), maxWritetableFilesTasks);

			readingBlock.LinkTo(processingBlock, linkOptions);
			processingBlock.LinkTo(writingBlock, linkOptions);

			foreach (string filePath in fileNames)
			{
				readingBlock.Post(filePath);
			}

			readingBlock.Complete();

			await writingBlock.Completion;
		}
	}
}
