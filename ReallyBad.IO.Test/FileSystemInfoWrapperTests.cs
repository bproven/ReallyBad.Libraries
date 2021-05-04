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

	}

}
