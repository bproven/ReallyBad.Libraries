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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

using Xunit;

#nullable enable

namespace ReallyBad.IO.Test
{

	public class DirectoryInfoWrapperTests : FileSystemInfoWrapperTests
	{

		[Fact]
		public void CtorPathTest()
		{
			var expected = new DirectoryInfo( TestSub );
			var actual = FileSystem.DirectoryInfo.FromDirectoryName( TestSub );
			Assert.Equal( expected.FullName, actual.FullName );
		}

		[Fact]
		public void ParentTest()
		{
			var subDir = FileSystem.DirectoryInfo.FromDirectoryName( TestSub );
			Assert.NotNull( subDir.Parent );
			var parent = subDir.Parent;
			Assert.Equal( TestPath, parent?.FullName );
		}

		[Fact]
		public void RootTest()
		{
			var subDir = FileSystem.DirectoryInfo.FromDirectoryName( TestSub );
			var root = subDir.Root;
			var subDir1 = new DirectoryInfo( TestSub );
			Assert.Equal( subDir1.Root.FullName, root.FullName );
		}

		[Fact]
		public void CreateTest()
		{
			var subDir = Guid.NewGuid().ToString();
			var path = Path.Combine( TestSub, subDir );
			var dir = FileSystem.DirectoryInfo.FromDirectoryName( path );
			dir.Create();
			Assert.True( Directory.Exists( path ) );
		}

		[Fact]
		public void DeleteTest()
		{
			var subDir = Guid.NewGuid().ToString();
			var path = Path.Combine( TestSub, subDir );
			var dir = new DirectoryInfo( path );
			dir.Create();
			var dir2 = FileSystem.DirectoryInfo.FromDirectoryName( path );
			dir2.Delete();
			Assert.False( Directory.Exists( path ) );
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
			var dir2 = FileSystem.DirectoryInfo.FromDirectoryName( path );
			dir2.Delete( true );
			Assert.False( Directory.Exists( path ) );
		}

		[Fact]
		public void RefreshTest()
		{
			var subDir = Guid.NewGuid().ToString();
			var path = Path.Combine( TestSub, subDir );
			var checkFileSystemInfo = new DirectoryInfo( path );
			checkFileSystemInfo.Create();
			var testFileSystemInfo = FileSystem.DirectoryInfo.FromDirectoryName( path );
			FileSystemInfoRefreshTest( testFileSystemInfo, checkFileSystemInfo );
		}

		[Fact]
		public void CreateSubdirectoryTest()
		{
			var dir = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			var testPath = Guid.NewGuid().ToString();
			var path = Path.Combine( TestPath, testPath );
			dir.CreateSubdirectory( testPath );
			Assert.True( Directory.Exists( path ) );
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
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.EnumerateDirectories(), () => directoryInfo.EnumerateDirectories() );
		}

		[Fact]
		public void EnumDirectoriesSearchPatternTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.EnumerateDirectories( SearchPattern ),
				() => directoryInfo.EnumerateDirectories( SearchPattern ) );
		}

		[Fact]
		public void EnumDirectoriesSearchPatternSearchOptionTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.EnumerateDirectories( SearchPattern, DirectorySearchOption ),
				() => directoryInfo.EnumerateDirectories( SearchPattern, DirectorySearchOption ) );
		}

		[Fact]
		public void EnumDirectoriesSearchPatternEnumOptionTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual(
				() => expectedDirectoryInfo.EnumerateDirectories( SearchPattern, EnumerationOptions ),
				() => directoryInfo.EnumerateDirectories( SearchPattern, EnumerationOptions ) );
		}

		[Fact]
		public void EnumFilesSearchPatternEnumOptionsTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.EnumerateFiles( SearchPattern, EnumerationOptions ),
				() => directoryInfo.EnumerateFiles( SearchPattern, EnumerationOptions ) );
		}

		[Fact]
		public void EnumFilesSearchPatternSearchOptionsTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.EnumerateFiles( SearchPattern, DirectorySearchOption ),
				() => directoryInfo.EnumerateFiles( SearchPattern, DirectorySearchOption ) );
		}

		[Fact]
		public void EnumFilesSearchPatternTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.EnumerateFiles( SearchPattern ),
				() => directoryInfo.EnumerateFiles( SearchPattern ) );
		}

		[Fact]
		public void EnumFilesTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.EnumerateFiles(),
				() => directoryInfo.EnumerateFiles() );
		}

		[Fact]
		public void EnumFileSystemInfosTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.EnumerateFileSystemInfos(),
				() => directoryInfo.EnumerateFileSystemInfos() );
		}

		[Fact]
		public void EnumFileSystemInfosSearchPatternTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.EnumerateFileSystemInfos( SearchPattern ),
				() => directoryInfo.EnumerateFileSystemInfos( SearchPattern ) );
		}

		[Fact]
		public void EnumFileSystemInfosSearchPatternSearchOptionTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.EnumerateFileSystemInfos( SearchPattern, DirectorySearchOption ),
				() => directoryInfo.EnumerateFileSystemInfos( SearchPattern, DirectorySearchOption ) );
		}

		[Fact]
		public void EnumFileSystemInfosSearchPatternEnumOptionTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.EnumerateFileSystemInfos( SearchPattern, EnumerationOptions ),
				() => directoryInfo.EnumerateFileSystemInfos( SearchPattern, EnumerationOptions ) );
		}

		[Fact]
		public void GetDirectoriesTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.GetDirectories(), () => directoryInfo.GetDirectories() );
		}

		[Fact]
		public void GetDirectoriesSearchPatternTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.GetDirectories( SearchPattern ),
				() => directoryInfo.GetDirectories( SearchPattern ) );
		}

		[Fact]
		public void GetDirectoriesSearchPatternSearchOptionTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.GetDirectories( SearchPattern, DirectorySearchOption ),
				() => directoryInfo.GetDirectories( SearchPattern, DirectorySearchOption ) );
		}

		[Fact]
		public void GetDirectoriesSearchPatternEnumOptionsTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.GetDirectories( SearchPattern, EnumerationOptions ),
				() => directoryInfo.GetDirectories( SearchPattern, EnumerationOptions ) );
		}

		[Fact]
		public void GetFilesTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.GetFiles(), () => directoryInfo.GetFiles() );
		}

		[Fact]
		public void GetFilesSearchPatternTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.GetFiles( SearchPattern ),
				() => directoryInfo.GetFiles( SearchPattern ) );
		}

		[Fact]
		public void GetFilesSearchPatternSearchOptionTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.GetFiles( SearchPattern, DirectorySearchOption ),
				() => directoryInfo.GetFiles( SearchPattern, DirectorySearchOption ) );
		}

		[Fact]
		public void GetFilesSearchPatternEnumOptionsTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.GetFiles( SearchPattern, EnumerationOptions ),
				() => directoryInfo.GetFiles( SearchPattern, EnumerationOptions ) );
		}

		[Fact]
		public void GetFileSystemInfosTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.GetFileSystemInfos(), () => directoryInfo.GetFileSystemInfos() );
		}

		[Fact]
		public void GetFileSystemInfosSearchPatternTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.GetFileSystemInfos( SearchPattern ),
				() => directoryInfo.GetFileSystemInfos( SearchPattern ) );
		}

		[Fact]
		public void GetFileSystemInfosSearchPatternSearchOptionTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.GetFileSystemInfos( SearchPattern, DirectorySearchOption ),
				() => directoryInfo.GetFileSystemInfos( SearchPattern, DirectorySearchOption ) );
		}

		[Fact]
		public void GetFileSystemInfosSearchPatternEnumOptionsTest()
		{
			var expectedDirectoryInfo = new DirectoryInfo( TestPath );
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			EnumEqual( () => expectedDirectoryInfo.GetFileSystemInfos( SearchPattern, EnumerationOptions ),
				() => directoryInfo.GetFileSystemInfos( SearchPattern, EnumerationOptions ) );
		}

		[Fact]
		public void MoveToTest()
		{
			var dirPath = Guid.NewGuid().ToString();
			var subDir = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			var subDir2 = subDir.CreateSubdirectory( dirPath );
			var oldPath = subDir2.FullName;
			subDir2.MoveTo( Path.Combine( TestSub, dirPath ) );
			Assert.False( Directory.Exists( oldPath ) );
			Assert.True( Directory.Exists( subDir2.FullName ) );
		}

		[Fact]
		public void EnumerableTest()
		{
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			IEnumerable enumerable = directoryInfo.EnumerateDirectories();
			Assert.NotNull( enumerable );
			IEnumerator enumerator = enumerable.GetEnumerator();
			Assert.NotNull( enumerator );
			enumerator.MoveNext();
			var obj1 = enumerator.Current;
			Assert.NotNull( obj1 );
			Assert.IsType<DirectoryInfoWrapper>( obj1 );
		}

		[Fact]
		public void EnumeratorResetTest()
		{
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			IEnumerable enumerable = directoryInfo.EnumerateDirectories();
			IEnumerator enumerator = enumerable.GetEnumerator();
			enumerator.MoveNext();
			Assert.Throws<NotSupportedException>( () => enumerator.Reset() );
		}

		[Fact]
		public void DisposeTest()
		{
			var directoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( TestPath );
			IEnumerable enumerable = directoryInfo.EnumerateDirectories();
			IEnumerator enumerator = enumerable.GetEnumerator();
			enumerator.MoveNext();
			( enumerator as IDisposable )?.Dispose();
			( enumerator as IDisposable )?.Dispose();
		}

	}

}
