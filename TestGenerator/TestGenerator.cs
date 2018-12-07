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
			DataflowLinkOptions linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
			ExecutionDataflowBlockOptions maxReadableFilesTasks = new ExecutionDataflowBlockOptions
			{
				MaxDegreeOfParallelism = maxOriginalFileCount
			};

			ExecutionDataflowBlockOptions maxProcessableTasks = new ExecutionDataflowBlockOptions
			{
				MaxDegreeOfParallelism = maxProcessedFileCount
			};

			ExecutionDataflowBlockOptions maxWritableTasks = new ExecutionDataflowBlockOptions
			{
				MaxDegreeOfParallelism = maxWritetableFileCount
			};

			AsyncReader asyncReader = new AsyncReader();
			TemplateGenerator templateGenerator = new TemplateGenerator();

			TransformBlock<string, string> readingBlock =
				new TransformBlock<string, string>(new Func<string, Task<String>>(asyncReader.Read), maxReadableFilesTasks);

			TransformBlock<string, TestClassInfo> processingBlock =
				new TransformBlock<string, TestClassInfo>(new Func<string, TestClassInfo>(templateGenerator.GetTemplate), maxProcessableTasks);

			ActionBlock<TestClassInfo> writingBlock = new ActionBlock<TestClassInfo>(
				(classTemplate) => writer.Write(classTemplate), maxWritableTasks);

			readingBlock.LinkTo(processingBlock, linkOptions);
			processingBlock.LinkTo(writingBlock, linkOptions);

			foreach (string filePath in fileNameList)
			{
				readingBlock.Post(filePath);
			}

			readingBlock.Complete();

			await writingBlock.Completion;

		}
	}
}
