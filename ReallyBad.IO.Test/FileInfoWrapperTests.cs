// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.IO.Test
//     File:       FileInfoWrapperTests.cs
// 
//     Created:    05/03/2021 11:20 PM
//     Updated:    05/04/2021 12:14 AM
// 

using System;
using System.IO;
using System.IO.Abstractions;
using System.Runtime.Versioning;

using ReallyBad.Core.Text;

using Xunit;

#nullable enable

namespace ReallyBad.IO.Test
{

	public class FileInfoWrapperTests : FileSystemInfoWrapperTests
	{

		private const string FileName = "test.txt";
		private const string Line1 = "This is a test file.";
		private const string Line2 = "This is an appended line.";
		private readonly string filePath;

		public FileInfoWrapperTests()
		{
			filePath = Path.Combine( TestSub, FileName );
			CreateTestFile( filePath, Line1 );
		}

		private void CreateTestFile( string fullFileName, string? contents = null )
		{
			var fileInfo = new FileInfo( fullFileName );
			using var stream = fileInfo.OpenWrite();
			using var writer = new StreamWriter( stream );
			if ( !contents.NullOrEmpty() )
			{
				writer.WriteLine( contents );
			}
			writer.Flush();
		}

		[Fact]
		public void CtorStringTest()
		{
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath );
			Assert.Equal( FileName, fileInfo.Name );
			Assert.Equal( filePath, fileInfo.FullName );
		}

		[Fact]
		public void CtorFileInfoTest()
		{
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath );
			Assert.Equal( FileName, fileInfo.Name );
			Assert.Equal( filePath, fileInfo.FullName );
		}

		[Fact]
		public void DirectoryTest()
		{
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath );
			var dirInfo = fileInfo.Directory;
			Assert.NotNull( dirInfo );
			Assert.Equal( TestSub, dirInfo?.FullName );
		}

		[Fact]
		public void DirectoryNameTest()
		{
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath );
			Assert.Equal( TestSub, fileInfo.DirectoryName );
		}

		[Fact]
		public void IsReadOnlyTest()
		{

			var testFileInfo = FileSystem.FileInfo.FromFileName( filePath );
			var checkFileInfo = new FileInfo( filePath );

			Assert.False( testFileInfo.IsReadOnly );
			Assert.False( checkFileInfo.IsReadOnly );

			testFileInfo.IsReadOnly = true;

			checkFileInfo.Refresh();

			Assert.True( testFileInfo.IsReadOnly );
			Assert.True( checkFileInfo.IsReadOnly );

			testFileInfo.IsReadOnly = false;

			checkFileInfo.Refresh();

			Assert.False( testFileInfo.IsReadOnly );
			Assert.False( checkFileInfo.IsReadOnly );

		}

		[Fact]
		public void LengthTest()
		{
			var fileInfo = new FileInfo( filePath );
			var fileInfo2 = FileSystem.FileInfo.FromFileName( filePath );
			Assert.Equal( fileInfo.Length, fileInfo2.Length );
		}

		private static string ReadToEnd( FileInfo fileInfo )
		{
			using var reader = fileInfo.OpenText();
			var fileContent = reader.ReadToEnd();
			return fileContent;
		}

		private static void VerifyContents( FileInfo fileInfo, string expected )
		{
			var contents = ReadToEnd( fileInfo );
			Assert.Equal( expected, contents );
		}

		private static void VerifyContents( string filePath, string expected )
		{
			var fileInfo = new FileInfo( filePath );
			VerifyContents( fileInfo, expected );
		}

		private static void WriteLine( IFileInfo fileInfo, string line )
		{
			using var writer = fileInfo.AppendText();
			writer.WriteLine( line );
			writer.Flush();
		}

		[Fact]
		public void AppendTest()
		{
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath );
			WriteLine( fileInfo, Line2 );
			VerifyContents( filePath, $"{Line1}{Environment.NewLine}{Line2}{Environment.NewLine}" );
		}

		[Fact]
		public void CopyToTest()
		{
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath );
			const string fileName2 = "test1.txt";
			var filePath2 = Path.Combine( TestSub, fileName2 );
			fileInfo.CopyTo( filePath2 );
			Assert.True( File.Exists( filePath ) );
			Assert.True( File.Exists( filePath2 ) );
			VerifyContents( filePath2, Line1 + Environment.NewLine );
		}

		[Fact]
		public void CopyToOverwriteTest()
		{
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath );
			const string fileName2 = "test1.txt";
			var filePath2 = Path.Combine( TestSub, fileName2 );
			var fileInfo2 = FileSystem.FileInfo.FromFileName( filePath2 );
			WriteLine( fileInfo2, Line2 );
			Assert.True( File.Exists( filePath2 ) );
			VerifyContents( filePath2, Line2 + Environment.NewLine );
			fileInfo.CopyTo( filePath2, true );
			Assert.True( File.Exists( filePath ) );
			Assert.True( File.Exists( filePath2 ) );
			VerifyContents( filePath2, Line1 + Environment.NewLine );
		}

		[Fact]
		public void CreateTest()
		{
			var filePath3 = Path.Combine( TestSub, "test3.txt" );
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath3 );
			using var stream = fileInfo.Create();
			using var writer = new StreamWriter( stream );
			writer.WriteLine( Line1 );
			writer.Flush();
			stream.Flush();
			Assert.True( File.Exists( filePath3 ) );
		}

		[Fact]
		public void CreateTextTest()
		{
			var filePath3 = Path.Combine( TestSub, "test4.txt" );
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath3 );
			using var writer = fileInfo.CreateText();
			writer.WriteLine( Line1 );
			writer.Flush();
			Assert.True( File.Exists( filePath3 ) );
		}

		[Fact]
		[SupportedOSPlatform( "windows" )]
		public void EncryptTest()
		{
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath );
			fileInfo.Encrypt();
			var fileInfo2 = new FileInfo( filePath );
			fileInfo2.Decrypt();
		}

		[Fact]
		[SupportedOSPlatform( "windows" )]
		public void DecryptTest()
		{
			var fileInfo = new FileInfo( filePath );
			fileInfo.Encrypt();
			var fileInfo2 = FileSystem.FileInfo.FromFileName( filePath );
			fileInfo2.Decrypt();
		}

		[Fact]
		public void MoveToTest()
		{
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath );
			var fileName2 = "test1.txt";
			var filePath2 = Path.Combine( TestSub, fileName2 );
			fileInfo.MoveTo( filePath2 );
			Assert.False( File.Exists( filePath ) );
			Assert.True( File.Exists( filePath2 ) );
			VerifyContents( filePath2, Line1 + Environment.NewLine );
		}

		[Fact]
		public void MoveToOverwriteTest()
		{
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath );
			var fileName2 = "test1.txt";
			var filePath2 = Path.Combine( TestSub, fileName2 );
			var fileInfo2 = FileSystem.FileInfo.FromFileName( filePath2 );
			WriteLine( fileInfo2, Line2 );
			Assert.True( File.Exists( filePath2 ) );
			VerifyContents( filePath2, Line2 + Environment.NewLine );
			fileInfo.MoveTo( filePath2, true );
			Assert.False( File.Exists( filePath ) );
			Assert.True( File.Exists( filePath2 ) );
			VerifyContents( filePath2, Line1 + Environment.NewLine );
		}

		private void ReadToEndTest( Func<IFileInfo,Stream> streamFactory )
		{
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath );
			using var stream = streamFactory( fileInfo );
			using var reader = new StreamReader( stream );
			var content = reader.ReadToEnd();
			Assert.Equal( Line1 + Environment.NewLine, content );
		}

		[Fact]
		public void OpenFileModeTest()
		{
			ReadToEndTest( fi => fi.Open( FileMode.Open ) );
		}

		[Fact]
		public void OpenFileModeFileAccessTest()
		{
			ReadToEndTest( fi => fi.Open( FileMode.Open, FileAccess.Read ) );
		}

		[Fact]
		public void OpenFileModeFileAccessFileShareTest()
		{
			ReadToEndTest( fi => fi.Open( FileMode.Open, FileAccess.Read, FileShare.None ) );
		}

		[Fact]
		public void OpenReadTest()
		{
			ReadToEndTest( fi => fi.OpenRead() );
		}

		[Fact]
		private void OpenTextTest()
		{
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath );
			using var reader = fileInfo.OpenText();
			var content = reader.ReadToEnd();
			Assert.Equal( Line1 + Environment.NewLine, content );
		}

		[Fact]
		public void OpenWriteTest()
		{
			var filePath3 = Path.Combine( TestSub, "test4.txt" );
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath3 );
			using var stream = fileInfo.OpenWrite();
			using var writer = new StreamWriter( stream );
			writer.WriteLine( Line1 );
			writer.Flush();
			writer.Close();
			Assert.True( File.Exists( filePath3 ) );
			VerifyContents( filePath3, Line1 + Environment.NewLine );
		}

		[Fact]
		public void ReplaceTest()
		{
			string replaceFileName = Path.Combine( TestSub, "test4.txt" );
			CreateTestFile( replaceFileName, Line2 );
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath );
			fileInfo.Replace( replaceFileName, null );
			VerifyContents( replaceFileName, Line1 + Environment.NewLine );
		}

		[Fact]
		public void ReplaceBackupTest()
		{
			string replaceFileName = Path.Combine( TestSub, "test4.txt" );
			string backupFileName = Path.Combine( TestSub, "test4.txt.bak" );
			CreateTestFile( replaceFileName, Line2 );
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath );
			fileInfo.Replace( replaceFileName, backupFileName );
			VerifyContents( replaceFileName, Line1 + Environment.NewLine );
			Assert.True( File.Exists( backupFileName ) );
			VerifyContents( backupFileName, Line2 + Environment.NewLine );
		}

		[Fact]
		public void ReplaceBackupIgnoreTest()
		{
			string replaceFileName = Path.Combine( TestSub, "test4.txt" );
			string backupFileName = Path.Combine( TestSub, "test4.txt.bak" );
			CreateTestFile( replaceFileName, Line2 );
			var fileInfo = FileSystem.FileInfo.FromFileName( filePath );
			fileInfo.Replace( replaceFileName, backupFileName, true );
			VerifyContents( replaceFileName, Line1 + Environment.NewLine );
			Assert.True( File.Exists( backupFileName ) );
			VerifyContents( backupFileName, Line2 + Environment.NewLine );
		}

		[Fact]
		public void RefreshTest()
		{
			var testFileInfo = FileSystem.FileInfo.FromFileName( filePath );
			var checkFileInfo = new FileInfo( filePath );
			FileSystemInfoRefreshTest( testFileInfo, checkFileInfo );
		}

		[Fact]
		public void DateTests()
		{
			var testFileInfo = FileSystem.FileInfo.FromFileName( filePath );
			var checkFileInfo = new FileInfo( filePath );
			Assert.Equal( checkFileInfo.CreationTime, testFileInfo.CreationTime );
			Assert.Equal( checkFileInfo.CreationTimeUtc, testFileInfo.CreationTimeUtc );
			Assert.Equal( checkFileInfo.LastAccessTime, testFileInfo.LastAccessTime );
			Assert.Equal( checkFileInfo.LastAccessTimeUtc, testFileInfo.LastAccessTimeUtc );
			Assert.Equal( checkFileInfo.LastWriteTime, testFileInfo.LastWriteTime );
			Assert.Equal( checkFileInfo.LastWriteTimeUtc, testFileInfo.LastWriteTimeUtc );
			var now = DateTime.Now;
			testFileInfo.CreationTime = now;
			testFileInfo.LastAccessTime = now;
			testFileInfo.LastWriteTime = now;
			var utcNow = DateTime.UtcNow.AddMinutes( 30 );
			testFileInfo.CreationTimeUtc = utcNow;
			testFileInfo.LastAccessTimeUtc = utcNow;
			testFileInfo.LastWriteTimeUtc = utcNow;
			checkFileInfo.Refresh();
			Assert.Equal( checkFileInfo.CreationTime, testFileInfo.CreationTime );
			Assert.Equal( checkFileInfo.CreationTimeUtc, testFileInfo.CreationTimeUtc );
			Assert.Equal( checkFileInfo.LastAccessTime, testFileInfo.LastAccessTime );
			Assert.Equal( checkFileInfo.LastAccessTimeUtc, testFileInfo.LastAccessTimeUtc );
			Assert.Equal( checkFileInfo.LastWriteTime, testFileInfo.LastWriteTime );
			Assert.Equal( checkFileInfo.LastWriteTimeUtc, testFileInfo.LastWriteTimeUtc );
		}

		[Fact]
		public void ToStringTest()
		{
			var testFileInfo = FileSystem.FileInfo.FromFileName( filePath );
			Assert.EndsWith( FileName, testFileInfo?.ToString() ?? throw new ArgumentNullException() );
		}

		[Fact]
		public void ExtensionTest()
		{
			var testFileInfo = FileSystem.FileInfo.FromFileName( filePath );
			Assert.Equal( ".txt", testFileInfo.Extension );
		}

		[Fact]
		public void ExistsTest()
		{
			var testFileInfo = FileSystem.FileInfo.FromFileName( filePath );
			Assert.True( testFileInfo.Exists );
		}

	}

}
