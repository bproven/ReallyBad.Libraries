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

		protected void FileSystemInfoRefreshTest( FileSystemInfoWrapper testFileSystemInfo, FileSystemInfo checkFileSystemInfo )
		{

			// load the attributes
			Assert.False( ( testFileSystemInfo.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );

			// set RO from another object
			checkFileSystemInfo.Attributes |= FileAttributes.ReadOnly;

			// still off in this one.
			Assert.False( ( testFileSystemInfo.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );

			// refresh
			testFileSystemInfo.Refresh();

			// now it's on in this one
			Assert.True( ( testFileSystemInfo.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );

			// turn off
			checkFileSystemInfo.Attributes &= ~FileAttributes.ReadOnly;

			// still thinks its on
			Assert.True( ( testFileSystemInfo.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );

			// refresh
			testFileSystemInfo.Refresh();

			// now it's off again
			Assert.False( ( testFileSystemInfo.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );

		}



		public void Dispose()
		{
			System.IO.Directory.Delete( TestPath, true );
		}

	}

}
