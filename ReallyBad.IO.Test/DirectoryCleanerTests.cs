using System;
using System.IO.Abstractions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

using Xunit;

#nullable enable

namespace ReallyBad.IO.Test
{

	public class DirectoryCleanerTests
	{

		private DirectoryCleaner directoryCleaner;

		public DirectoryCleanerTests()
		{
			IServiceProvider serviceProvider = new ServiceCollection()
				.AddLogging( builder =>
				{
					builder
						.AddDebug()
						.AddSimpleConsole( options =>
						{
							options.ColorBehavior = LoggerColorBehavior.Default;
							options.SingleLine = true;
							options.TimestampFormat = "yyyy-MM-dd";
						} );
				} )
				.AddTransient<IFileSystem, FileSystem>()
				.AddTransient<DirectoryCleaner>()
				.BuildServiceProvider() ?? throw new ArgumentNullException( nameof( serviceProvider ) );

			directoryCleaner = serviceProvider
				.GetService<DirectoryCleaner>() ?? throw new ArgumentNullException( nameof( directoryCleaner ) );
		}

		[Fact]
		public void DirectoryCleanerTest()
		{
			directoryCleaner.Root = @"D:\Users\Bob\Downloads";
			directoryCleaner.Clean();
		}

	}

}
