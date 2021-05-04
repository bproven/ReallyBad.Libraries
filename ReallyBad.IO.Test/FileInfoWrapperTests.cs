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
using System.Runtime.Versioning;

using Xunit;

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
			var fileInfo = new FileInfo( filePath );
			using var stream = fileInfo.OpenWrite();
			using var writer = new StreamWriter( stream );
			writer.WriteLine( Line1 );
			writer.Flush();
		}

		[Fact]
		public void CtorStringTest()
		{
			var fileInfo = new FileInfoWrapper( filePath );
			Assert.Equal( FileName, fileInfo.Name );
			Assert.Equal( filePath, fileInfo.FullName );
		}

		[Fact]
		public void CtorFileInfoTest()
		{
			var fileInfo = new FileInfoWrapper( new FileInfo( filePath ) );
			Assert.Equal( FileName, fileInfo.Name );
			Assert.Equal( filePath, fileInfo.FullName );
		}

		[Fact]
		public void DirectoryTest()
		{
			var fileInfo = new FileInfoWrapper( new FileInfo( filePath ) );
			var dirInfo = fileInfo.Directory;
			Assert.NotNull( dirInfo );
			Assert.Equal( TestSub, dirInfo.FullName );
		}

		[Fact]
		public void DirectoryNameTest()
		{
			var fileInfo = new FileInfoWrapper( filePath );
			Assert.Equal( TestSub, fileInfo.DirectoryName );
		}

		[Fact]
		public void IsReadOnlyTest()
		{
			var fileInfo = new FileInfo( filePath )
			{
				IsReadOnly = true,
			};

			try
			{
				var fileInfo2 = new FileInfoWrapper( filePath );
				Assert.True( fileInfo2.IsReadOnly );
			}
			finally
			{
				fileInfo.IsReadOnly = false;
			}
		}

		[Fact]
		public void LengthTest()
		{
			var fileInfo = new FileInfo( filePath );
			var fileInfo2 = new FileInfoWrapper( filePath );
			Assert.Equal( fileInfo.Length, fileInfo2.Length );
		}

		private static string ReadToEnd( IFileInfo fileInfo )
		{
			var reader = fileInfo.OpenText();
			var fileContent = reader.ReadToEnd();
			reader.Close();

			return fileContent;
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
			var fileInfo = new FileInfoWrapper( filePath );
			WriteLine( fileInfo, Line2 );
			var fileContent = ReadToEnd( fileInfo );
			Assert.Equal( $"{Line1}{Environment.NewLine}{Line2}{Environment.NewLine}", fileContent );
		}

		[Fact]
		public void CopyToTest()
		{
			var fileInfo = new FileInfoWrapper( filePath );
			const string fileName2 = "test1.txt";
			var filePath2 = Path.Combine( TestSub, fileName2 );
			fileInfo.CopyTo( filePath2 );
			Assert.True( File.Exists( filePath ) );
			Assert.True( File.Exists( filePath2 ) );
			var fileInfo2 = new FileInfoWrapper( filePath2 );
			var content = ReadToEnd( fileInfo2 );
			Assert.Equal( Line1 + Environment.NewLine, content );
		}

		[Fact]
		public void CopyToOverwriteTest()
		{
			var fileInfo = new FileInfoWrapper( filePath );
			const string fileName2 = "test1.txt";
			var filePath2 = Path.Combine( TestSub, fileName2 );
			var fileInfo2 = new FileInfoWrapper( filePath2 );
			WriteLine( fileInfo2, Line2 );
			Assert.True( File.Exists( filePath2 ) );
			var content = ReadToEnd( fileInfo2 );
			Assert.Equal( Line2 + Environment.NewLine, content );
			fileInfo.CopyTo( filePath2, true );
			Assert.True( File.Exists( filePath ) );
			Assert.True( File.Exists( filePath2 ) );
			content = ReadToEnd( fileInfo2 );
			Assert.Equal( Line1 + Environment.NewLine, content );
		}

		[Fact]
		public void CreateTest()
		{
			var filePath3 = Path.Combine( TestSub, "test3.txt" );
			var fileInfo = new FileInfoWrapper( filePath3 );
			var stream = fileInfo.Create();
			var writer = new StreamWriter( stream );
			writer.WriteLine( Line1 );
			writer.Flush();
			stream.Flush();
			writer.Close();
			stream.Close();
			Assert.True( File.Exists( filePath3 ) );
		}

		[Fact]
		public void CreateTextTest()
		{
			var filePath3 = Path.Combine( TestSub, "test4.txt" );
			var fileInfo = new FileInfoWrapper( filePath3 );
			using var writer = fileInfo.CreateText();
			writer.WriteLine( Line1 );
			writer.Flush();
			Assert.True( File.Exists( filePath3 ) );
		}

		[Fact]
		[SupportedOSPlatform( "windows" )]
		public void EncryptTest()
		{
			var fileInfo = new FileInfoWrapper( filePath );
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
			var fileInfo2 = new FileInfoWrapper( filePath );
			fileInfo2.Decrypt();
		}

		[Fact]
		public void MoveToTest()
		{
			var fileInfo = new FileInfoWrapper( filePath );
			var fileName2 = "test1.txt";
			var filePath2 = Path.Combine( TestSub, fileName2 );
			fileInfo.MoveTo( filePath2 );
			Assert.False( File.Exists( filePath ) );
			Assert.True( File.Exists( filePath2 ) );
			var fileInfo2 = new FileInfoWrapper( filePath2 );
			var content = ReadToEnd( fileInfo2 );
			Assert.Equal( Line1 + Environment.NewLine, content );
		}

		[Fact]
		public void MoveToOverwriteTest()
		{
			var fileInfo = new FileInfoWrapper( filePath );
			var fileName2 = "test1.txt";
			var filePath2 = Path.Combine( TestSub, fileName2 );
			var fileInfo2 = new FileInfoWrapper( filePath2 );
			WriteLine( fileInfo2, Line2 );
			Assert.True( File.Exists( filePath2 ) );
			var content = ReadToEnd( fileInfo2 );
			Assert.Equal( Line2 + Environment.NewLine, content );
			fileInfo.MoveTo( filePath2, true );
			Assert.False( File.Exists( filePath ) );
			Assert.True( File.Exists( filePath2 ) );
			content = ReadToEnd( fileInfo2 );
			Assert.Equal( Line1 + Environment.NewLine, content );
		}

		[Fact]
		public void OpenFileModeTest()
		{
			var fileInfo = new FileInfoWrapper( filePath );
			using var stream = fileInfo.Open( FileMode.Open );
			using var reader = new StreamReader( stream );
			var content = reader.ReadToEnd();
			Assert.Equal( Line1 + Environment.NewLine, content );
		}

		[Fact]
		public void OpenFileModeFileAccessTest()
		{
			var fileInfo = new FileInfoWrapper( filePath );
			using var stream = fileInfo.Open( FileMode.Open, FileAccess.Read );
			using var reader = new StreamReader( stream );
			var content = reader.ReadToEnd();
			Assert.Equal( Line1 + Environment.NewLine, content );
		}

		[Fact]
		public void OpenFileModeFileAccessFileShareTest()
		{
			var fileInfo = new FileInfoWrapper( filePath );
			using var stream = fileInfo.Open( FileMode.Open, FileAccess.Read, FileShare.None );
			using var reader = new StreamReader( stream );
			var content = reader.ReadToEnd();
			Assert.Equal( Line1 + Environment.NewLine, content );
		}

	}

}
