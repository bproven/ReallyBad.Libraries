// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.IO.Test
//     File:       FileSystemInfoWrapperTests.cs
// 
//     Created:    05/03/2021 11:13 PM
//     Updated:    05/03/2021 11:19 PM
// 

using System;
using System.IO;

using Xunit;

#nullable enable

namespace ReallyBad.IO.Test
{

	public class FileSystemInfoWrapperTests : IDisposable
	{

		protected const string SearchPattern = "*.*";

		protected const SearchOption DirectorySearchOption = SearchOption.AllDirectories;

		protected readonly EnumerationOptions EnumerationOptions = new()
		{
			RecurseSubdirectories = true,
		};

		protected readonly string Root = Environment.GetFolderPath( Environment.SpecialFolder.Personal );
		protected readonly string TestPath;
		protected readonly string TestSub;

		public FileSystemInfoWrapperTests()
		{
			var path = Guid.NewGuid().ToString();
			TestPath = Path.Combine( Root, path );

			var sub = Guid.NewGuid().ToString();

			var root = new DirectoryInfo( Root );

			var testDir = System.IO.Directory.Exists( TestPath )
				? new DirectoryInfo( TestPath )
				: root.CreateSubdirectory( path );

			var subDirInfo = testDir.CreateSubdirectory( sub );

			TestSub = subDirInfo.FullName;
		}

		public void Dispose()
		{
			System.IO.Directory.Delete( TestPath, true );
		}

		protected void FileSystemInfoRefreshTest( FileSystemInfoWrapper testFileSystemInfo, FileSystemInfo checkFileSystemInfo )
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

		public class BadFileInfo : FileSystemInfo
		{
			public override void Delete() => throw new NotImplementedException(); 
			public override bool Exists => throw new NotImplementedException();
			public override string Name => throw new NotImplementedException();
		}

		[Fact]
		public void CreateFileSystemInfoTest()
		{
			Assert.Throws<ArgumentException>( () => FileSystemInfoWrapper.Create( new BadFileInfo() ) );
		}
		
	}

}
