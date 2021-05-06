using System;
using System.IO;

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
				.AddTransient<FileOrganizer>()
				.AddTransient<IImageFileInfoProvider,ImageFileInfoProvider>()
				.BuildServiceProvider();

			fileOrganizer = serviceProvider
				.GetService<FileOrganizer>();

		}

		[Fact]
		public void DateFormatDirectoryTest()
		{
			Assert.True( FileOrganizer.DateFormatDirectory( "2018-12-31", "yyyy-MM-dd" ) );
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
			var relative = FileOrganizer.GetRelativePath( subFolder, root );
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
			var destinationPath = FileOrganizer.GetDestinationPath( fileText, dest, formatted );
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

			var fileInfo = new FileInfo( file );

			fileOrganizer.SourceRootDirectory = source;
			fileOrganizer.DestRootDirectory = dest;

			var destParent = fileOrganizer.GetDestinationParentDirectory( fileInfo );

			Assert.Equal( dest, destParent );

			// test one level sub

			var sub = backslash + "Sub";
			var sourceSub = source + sub;
			var destSub = dest + sub;
			var fileSub = sourceSub + backslash + fileText;

			var fileInfoSub = new FileInfo( fileSub );

			var fileOrganizerSub = serviceProvider
				.GetService<FileOrganizer>();

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

			var fileInfoSub2 = new FileInfo( fileSub2 );

			var fileOrganizerSub2 = serviceProvider
				.GetService<FileOrganizer>();

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

			var fileInfoSub3 = new FileInfo( fileSub3 );

			var fileOrganizerSub3 = serviceProvider
				.GetService<FileOrganizer>();

			Assert.NotNull( fileOrganizerSub3 );

			fileOrganizerSub3.SourceRootDirectory = source;
			fileOrganizerSub3.DestRootDirectory = dest;
			fileOrganizerSub3.IsPreserveTree = true;
			fileOrganizerSub3.ImageFileInfoProvider = new MockImageFileInfoProvider();

			var destSubParent3 = fileOrganizerSub3.GetDestinationParentDirectory( fileInfoSub3 );

			Assert.Equal( destSub3, destSubParent3 );

		}

	}

}
