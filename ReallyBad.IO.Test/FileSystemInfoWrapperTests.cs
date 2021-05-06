// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.IO.Test
//     File:       FileSystemInfoWrapperTests.cs
//
//     Created:    05/03/2021 11:19 PM
//     Updated:    05/03/2021 11:19 PM
// 

using System;
using System.IO;
using System.IO.Abstractions;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

#nullable enable

namespace ReallyBad.IO.Test
{

	public class FileSystemInfoWrapperTests : IDisposable
	{

		protected const string SearchPattern = "*";

		protected const SearchOption DirectorySearchOption = SearchOption.AllDirectories;

		protected readonly EnumerationOptions EnumerationOptions = new()
		{
			RecurseSubdirectories = true,
		};

		protected readonly string Root
			= Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), "Temp" );
		protected readonly string TestPath;
		protected readonly string TestSub;

		protected readonly IFileSystem FileSystem;

		private FileInfo CreateFile( DirectoryInfo dirInfo, string fileName, string? contents = null ) 
			=> CreateFile( dirInfo.FullName, fileName, contents );

		private FileInfo CreateFile( string path, string fileName, string? contents = null )
		{
			var newFilePath = Path.Combine( path, fileName );
			var fileInfo = new FileInfo( newFilePath );
			using var stream = fileInfo.OpenWrite();
			using var writer = new StreamWriter( stream );
			writer.WriteLine( contents ?? "File Contents" );

			return fileInfo;
		}

		private DirectoryInfo CreateSub( DirectoryInfo directoryInfo, string name ) => directoryInfo.CreateSubdirectory( name );

		private DirectoryInfo CreateSub( string path, string name ) => CreateSub( new DirectoryInfo( path ), name );

		public FileSystemInfoWrapperTests()
		{

			var container = new ServiceCollection();

			container
				.AddSingleton<IFileSystem, FileSystem>();

			var provider = container
				.BuildServiceProvider();

			FileSystem = provider.GetService<IFileSystem>() ?? throw new InvalidOperationException( "Resolve failed" );

			var rootDir = new DirectoryInfo( Root );

			if ( !rootDir.Exists )
			{
				rootDir.Create();
			}

			// create directory for this test run
			var testDir = CreateSub( Root, Guid.NewGuid().ToString() );

			TestPath = testDir.FullName;

			CreateFile( TestPath, $"{Guid.NewGuid()}.txt" );

			var subDir = CreateSub( testDir, Guid.NewGuid().ToString() );

			TestSub = subDir.FullName;

			CreateFile( TestSub, $"{Guid.NewGuid()}.txt" );

			var newSubDir = CreateSub( subDir, Guid.NewGuid().ToString() );

			CreateFile( newSubDir, $"{Guid.NewGuid()}.txt" );

		}

		public void Dispose()
		{
			Directory.Delete( TestPath, true );
		}

		protected void FileSystemInfoRefreshTest( IFileSystemInfo testFileSystemInfo, FileSystemInfo checkFileSystemInfo )
		{

			// load the attributes
			Assert.False( ( testFileSystemInfo.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );
			Assert.False( ( checkFileSystemInfo.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );

			// set RO from other
			checkFileSystemInfo.Attributes |= FileAttributes.ReadOnly;

			// still off in this one.
			Assert.False( ( testFileSystemInfo.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );
			// on
			Assert.True( ( checkFileSystemInfo.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );

			// refresh
			testFileSystemInfo.Refresh();

			// now it's on in both
			Assert.True( ( checkFileSystemInfo.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );
			Assert.True( ( testFileSystemInfo.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );

			// turn off
			testFileSystemInfo.Attributes &= ~FileAttributes.ReadOnly;

			// still thinks its on
			Assert.True( ( checkFileSystemInfo.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );
			// off 
			Assert.False( ( testFileSystemInfo.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );

			// refresh
			checkFileSystemInfo.Refresh();

			// now it's off again
			Assert.False( ( testFileSystemInfo.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );
			Assert.False( ( checkFileSystemInfo.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );

		}

	}

}
