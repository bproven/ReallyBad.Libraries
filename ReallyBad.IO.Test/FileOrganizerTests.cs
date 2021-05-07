using System;
using System.IO.Abstractions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

using ReallyBad.Core.File;

using Xunit;

namespace ReallyBad.IO.Test
{

	public class FileOrganizerTests
	{

		private readonly IServiceProvider serviceProvider;
		private readonly FileOrganizer fileOrganizer;
		private readonly IFileSystem fileSystem;

		public FileOrganizer GetFileOrganizer() => serviceProvider.GetService<FileOrganizer>();

		public FileOrganizerTests()
		{

			serviceProvider = new ServiceCollection()
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
				})
				.AddTransient<IFileSystem,FileSystem>()
				.AddTransient<FileOrganizer>()
				.AddTransient<IImageFileInfoProvider,MockImageFileInfoProvider>()
				.BuildServiceProvider();

			fileOrganizer = serviceProvider
				.GetService<FileOrganizer>();

			fileSystem = serviceProvider
				.GetService<IFileSystem>();

		}

		[Fact]
		public void DateFormatDirectoryTest()
		{
			Assert.True( FileOrganizer.DateFormatDirectory( "2018-12-31", "yyyy-MM-dd" ) );
			Assert.True( FileOrganizer.DateFormatDirectory( "18-12-31", "yy-MM-dd" ) );
			Assert.False( FileOrganizer.DateFormatDirectory( "201-12-31", "yyyy-MM-dd" ) );
			Assert.False( FileOrganizer.DateFormatDirectory( "201x-12-31", "yyyy-MM-dd" ) );
			Assert.NotNull( fileOrganizer );
			Assert.True( fileOrganizer.DateFormatDirectory( "2018-12-31" ) );
			Assert.False( fileOrganizer.DateFormatDirectory( "201-12-31" ) );
			Assert.False( fileOrganizer.DateFormatDirectory( "201x-12-31" ) );
		}


		[Fact]
		public void GetRelativePathTest()
		{
			var root = @"C:\Root";
			var sub = @"Sub1\Sub2";
			var subFolder = root + @"\" + sub;
			var relative = fileOrganizer.GetRelativePath( subFolder, root );
			Assert.Equal( sub, relative );
		}

		[Fact]
		public void GetDestinationPathTest()
		{
			//string source = @"C:\Source";
			var dest = @"C:\Dest";
			var formatted = "2018-12-31";
			var fileText = "file.txt";
			var backslash = @"\";
			var expected = dest + backslash + formatted + backslash + fileText;
			var destinationPath = fileOrganizer.GetDestinationPath( fileText, dest, formatted );
			Assert.Equal( expected, destinationPath );
		}

		[Fact]
		public void GetDestinationParentDirectoryTest()
		{

			const string backslash = @"\";
			const string source = @"C:\Source";
			const string dest = @"C:\Dest";
			const string fileText = "file.txt";
			const string file = source + backslash + fileText;

			var fileInfo = fileSystem.FileInfo.FromFileName( file );

			fileOrganizer.SourceRootDirectory = source;
			fileOrganizer.DestRootDirectory = dest;

			var destParent = fileOrganizer.GetDestinationParentDirectory( fileInfo );

			Assert.Equal( dest, destParent );

			// test one level sub

			var sub = backslash + "Sub";
			var sourceSub = source + sub;
			var destSub = dest + sub;
			var fileSub = sourceSub + backslash + fileText;

			var fileInfoSub = fileSystem.FileInfo.FromFileName( fileSub );

			var fileOrganizerSub = GetFileOrganizer();

			Assert.NotNull( fileOrganizerSub );

			fileOrganizerSub.SourceRootDirectory = source;
			fileOrganizerSub.DestRootDirectory = dest;
			fileOrganizerSub.IsPreserveTree = true;

			var destSubParent = fileOrganizerSub
				.GetDestinationParentDirectory( fileInfoSub );

			Assert.Equal( destSub, destSubParent );

			// test 2 level sub

			var sourceSub2 = source + sub + "1" + sub + "2";
			var destSub2 = dest + sub + "1" + sub + "2";
			var fileSub2 = sourceSub2 + backslash + fileText;

			var fileInfoSub2 = fileSystem.FileInfo.FromFileName( fileSub2 );

			var fileOrganizerSub2 = GetFileOrganizer();

			Assert.NotNull( fileOrganizerSub2 );

			fileOrganizerSub2.SourceRootDirectory = source;
			fileOrganizerSub2.DestRootDirectory = dest;
			fileOrganizerSub2.IsPreserveTree = true;

			var destSubParent2 = fileOrganizerSub2.GetDestinationParentDirectory( fileInfoSub2 );

			Assert.Equal( destSub2, destSubParent2 );

			// test 2 level in formatted directory

			var sourceSub3 = source + sub + "1" + sub + "2" + backslash + "2019-01-01";
			var destSub3 = dest + sub + "1" + sub + "2";
			var fileSub3 = sourceSub3 + backslash + fileText;

			var fileInfoSub3 = fileSystem.FileInfo.FromFileName( fileSub3 );

			var fileOrganizerSub3 = GetFileOrganizer();

			Assert.NotNull( fileOrganizerSub3 );

			fileOrganizerSub3.SourceRootDirectory = source;
			fileOrganizerSub3.DestRootDirectory = dest;
			fileOrganizerSub3.IsPreserveTree = true;

			var destSubParent3 = fileOrganizerSub3.GetDestinationParentDirectory( fileInfoSub3 );

			Assert.Equal( destSub3, destSubParent3 );

		}

	}

}
