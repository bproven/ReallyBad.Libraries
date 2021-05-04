// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.IO.Test
//     File:       DirectoryInfoWrapperTests.cs
// 
//     Created:    05/03/2021 6:20 PM
//     Updated:    05/03/2021 11:19 PM
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Xunit;

#nullable enable

namespace ReallyBad.IO.Test
{

	public class DirectoryInfoWrapperTests : FileSystemInfoWrapperTests
	{

		[Fact]
		public void CtorInfoTest()
		{
			var expected = new DirectoryInfo( Root );
			var actual = new DirectoryInfoWrapper( expected );
			Assert.Same( expected, actual.DirectoryInfo );
		}

		[Fact]
		public void CtorPathTest()
		{
			var expected = new DirectoryInfo( Root );
			var actual = new DirectoryInfoWrapper( Root );
			Assert.Equal( expected.FullName, actual.FullName );
		}

		[Fact]
		public void ParentTest()
		{
			var subDir = new DirectoryInfoWrapper( TestSub );
			Assert.NotNull( subDir.Parent );
			var parent = subDir.Parent;
			Assert.Equal( TestPath, parent?.FullName );
		}

		[Fact]
		public void RootTest()
		{
			var subDir = new DirectoryInfoWrapper( TestSub );
			var root = subDir.Root;
			var subDir1 = new DirectoryInfo( TestSub );
			Assert.Equal( subDir1.Root.FullName, root.FullName );
		}

		[Fact]
		public void CreateTest()
		{
			var subDir = Guid.NewGuid().ToString();
			var path = Path.Combine( TestSub, subDir );
			var dir = new DirectoryInfoWrapper( path );
			dir.Create();
			Assert.True( System.IO.Directory.Exists( path ) );
		}

		[Fact]
		public void DeleteTest()
		{
			var subDir = Guid.NewGuid().ToString();
			var path = Path.Combine( TestSub, subDir );
			var dir = new DirectoryInfo( path );
			dir.Create();
			var dir2 = new DirectoryInfoWrapper( path );
			dir2.Delete();
			Assert.False( System.IO.Directory.Exists( path ) );
		}

		[Fact]
		public void DeleteRecursiveTest()
		{
			var subDir = Guid.NewGuid().ToString();
			var path = Path.Combine( TestSub, subDir );
			var testDir = new DirectoryInfo( path );
			testDir.Create();
			var subDir2 = Guid.NewGuid().ToString();
			testDir.CreateSubdirectory( subDir2 );
			var dir2 = new DirectoryInfoWrapper( path );
			dir2.Delete( true );
			Assert.False( System.IO.Directory.Exists( path ) );
		}

		[Fact]
		public void RefreshTest()
		{
			var subDir = Guid.NewGuid().ToString();
			var path = Path.Combine( TestSub, subDir );
			var dir = new DirectoryInfo( path );
			dir.Create();
			var dir2 = new DirectoryInfoWrapper( path );

			// load the attributes
			Assert.False( ( dir2.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );

			// set RO from another object
			dir.Attributes |= FileAttributes.ReadOnly;

			// still off in this one.
			Assert.False( ( dir2.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );

			// refresh
			dir2.Refresh();

			// now it's on in this one
			Assert.True( ( dir2.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );

			// turn off
			dir.Attributes &= ~FileAttributes.ReadOnly;

			// still thinks its on
			Assert.True( ( dir2.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );

			// refresh
			dir2.Refresh();

			// now it's off again
			Assert.False( ( dir2.Attributes & FileAttributes.ReadOnly ) == FileAttributes.ReadOnly );
		}

		[Fact]
		public void CreateSubdirectoryTest()
		{
			var dir = new DirectoryInfoWrapper( TestPath );
			var testPath = Guid.NewGuid().ToString();
			var path = Path.Combine( TestPath, testPath );
			dir.CreateSubdirectory( testPath );
			Assert.True( System.IO.Directory.Exists( path ) );
		}

		private IList<TOut> ToList<TIn, TOut>( IEnumerable<TIn> input, Func<TIn, TOut> select )
		{
			return input.Select( select ).OrderBy( s => s ).ToList();
		}

		private void AssertEqual( IEnumerable<FileSystemInfo> expected, IEnumerable<IFileSystemInfo> actual )
		{
			var expectedList = ToList( expected, f => f.Name );
			Assert.True( expectedList.Any() );
			var actualList = ToList( actual, f => f.Name );
			Assert.True( actualList.Any() );
			Assert.Equal( expectedList, actualList );
		}

		private void EnumEqual<TInterface, TType>( Func<IEnumerable<TType>> expected,
			Func<IEnumerable<TInterface>> actual )
			where TType : FileSystemInfo
			where TInterface : class, IFileSystemInfo
		{
			var expectedDirectories = expected();
			var directories = actual();
			AssertEqual( expectedDirectories, directories );
		}

		[Fact]
		public void EnumDirectoriesTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.EnumerateDirectories(), () => directoryInfo.EnumerateDirectories() );
		}

		[Fact]
		public void EnumDirectoriesSearchPatternTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.EnumerateDirectories( SearchPattern ),
				() => directoryInfo.EnumerateDirectories( SearchPattern ) );
		}

		[Fact]
		public void EnumDirectoriesSearchPatternSearchOptionTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.EnumerateDirectories( SearchPattern, DirectorySearchOption ),
				() => directoryInfo.EnumerateDirectories( SearchPattern, DirectorySearchOption ) );
		}

		[Fact]
		public void EnumDirectoriesSearchPatternEnumOptionTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual(
				() => expectedDirectoryInfo.EnumerateDirectories( SearchPattern, EnumerationOptions ),
				() => directoryInfo.EnumerateDirectories( SearchPattern, EnumerationOptions ) );
		}

		[Fact]
		public void EnumFilesSearchPatternEnumOptionsTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.EnumerateFiles( SearchPattern, EnumerationOptions ),
				() => directoryInfo.EnumerateFiles( SearchPattern, EnumerationOptions ) );
		}

		[Fact]
		public void EnumFilesSearchPatternSearchOptionsTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.EnumerateFiles( SearchPattern, DirectorySearchOption ),
				() => directoryInfo.EnumerateFiles( SearchPattern, DirectorySearchOption ) );
		}

		[Fact]
		public void EnumFilesSearchPatternTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.EnumerateFiles( SearchPattern ),
				() => directoryInfo.EnumerateFiles( SearchPattern ) );
		}

		[Fact]
		public void EnumFilesTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.EnumerateFiles(),
				() => directoryInfo.EnumerateFiles() );
		}

		[Fact]
		public void EnumFileSystemInfosTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.EnumerateFileSystemInfos(),
				() => directoryInfo.EnumerateFileSystemInfos() );
		}

		[Fact]
		public void EnumFileSystemInfosSearchPatternTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.EnumerateFileSystemInfos( SearchPattern ),
				() => directoryInfo.EnumerateFileSystemInfos( SearchPattern ) );
		}

		[Fact]
		public void EnumFileSystemInfosSearchPatternSearchOptionTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.EnumerateFileSystemInfos( SearchPattern, DirectorySearchOption ),
				() => directoryInfo.EnumerateFileSystemInfos( SearchPattern, DirectorySearchOption ) );
		}

		[Fact]
		public void EnumFileSystemInfosSearchPatternEnumOptionTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.EnumerateFileSystemInfos( SearchPattern, EnumerationOptions ),
				() => directoryInfo.EnumerateFileSystemInfos( SearchPattern, EnumerationOptions ) );
		}

		[Fact]
		public void GetDirectoriesTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.GetDirectories(), () => directoryInfo.GetDirectories() );
		}

		[Fact]
		public void GetDirectoriesSearchPatternTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.GetDirectories( SearchPattern ),
				() => directoryInfo.GetDirectories( SearchPattern ) );
		}

		[Fact]
		public void GetDirectoriesSearchPatternSearchOptionTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.GetDirectories( SearchPattern, DirectorySearchOption ),
				() => directoryInfo.GetDirectories( SearchPattern, DirectorySearchOption ) );
		}

		[Fact]
		public void GetDirectoriesSearchPatternEnumOptionsTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.GetDirectories( SearchPattern, EnumerationOptions ),
				() => directoryInfo.GetDirectories( SearchPattern, EnumerationOptions ) );
		}

		[Fact]
		public void GetFilesTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.GetFiles(), () => directoryInfo.GetFiles() );
		}

		[Fact]
		public void GetFilesSearchPatternTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.GetFiles( SearchPattern ),
				() => directoryInfo.GetFiles( SearchPattern ) );
		}

		[Fact]
		public void GetFilesSearchPatternSearchOptionTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.GetFiles( SearchPattern, DirectorySearchOption ),
				() => directoryInfo.GetFiles( SearchPattern, DirectorySearchOption ) );
		}

		[Fact]
		public void GetFilesSearchPatternEnumOptionsTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.GetFiles( SearchPattern, EnumerationOptions ),
				() => directoryInfo.GetFiles( SearchPattern, EnumerationOptions ) );
		}

		[Fact]
		public void GetFileSystemInfosTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.GetFileSystemInfos(), () => directoryInfo.GetFileSystemInfos() );
		}

		[Fact]
		public void GetFileSystemInfosSearchPatternTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.GetFileSystemInfos( SearchPattern ),
				() => directoryInfo.GetFileSystemInfos( SearchPattern ) );
		}

		[Fact]
		public void GetFileSystemInfosSearchPatternSearchOptionTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.GetFileSystemInfos( SearchPattern, DirectorySearchOption ),
				() => directoryInfo.GetFileSystemInfos( SearchPattern, DirectorySearchOption ) );
		}

		[Fact]
		public void GetFileSystemInfosSearchPatternEnumOptionsTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( Root );
			var directoryInfo = new DirectoryInfoWrapper( Root );
			EnumEqual( () => expectedDirectoryInfo.GetFileSystemInfos( SearchPattern, EnumerationOptions ),
				() => directoryInfo.GetFileSystemInfos( SearchPattern, EnumerationOptions ) );
		}

		[Fact]
		public void MoveToTest()
		{
			var dirPath = Guid.NewGuid().ToString();
			var subDir = new DirectoryInfoWrapper( TestSub );
			var subDir2 = subDir.CreateSubdirectory( dirPath );
			var oldPath = subDir2.FullName;
			subDir2.MoveTo( Path.Combine( TestPath, dirPath ) );
			Assert.False( System.IO.Directory.Exists( oldPath ) );
			Assert.True( System.IO.Directory.Exists( subDir2.FullName ) );
		}

	}

}