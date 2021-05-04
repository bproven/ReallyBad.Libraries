// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.IO
//     File:       IFileInfo.cs
// 
//     Created:    05/03/2021 4:31 PM
//     Updated:    05/03/2021 5:23 PM
// 

using System.IO;
using System.Runtime.Versioning;

#nullable enable

namespace ReallyBad.IO
{

	public interface IFileInfo : IFileSystemInfo
	{

		IDirectoryInfo? Directory { get; }

		string? DirectoryName { get; }

		bool IsReadOnly { get; set; }

		long Length { get; }

		StreamWriter AppendText();

		IFileInfo CopyTo( string destFileName );

		IFileInfo CopyTo( string destFileName, bool overwrite );

		FileStream Create();

		StreamWriter CreateText();

		[SupportedOSPlatform( "windows" )]
		void Decrypt();

		[SupportedOSPlatform( "windows" )]
		void Encrypt();

		void MoveTo( string destFileName );

		void MoveTo( string destFileName, bool overwrite );

		FileStream Open( FileMode mode );

		FileStream Open( FileMode mode, FileAccess access );

		FileStream Open( FileMode mode, FileAccess access, FileShare share );

		FileStream OpenRead();

		StreamReader OpenText();

		FileStream OpenWrite();

		IFileInfo Replace( string destinationFileName, string? destinationBackupFileName );

		IFileInfo Replace( string destinationFileName, string? destinationBackupFileName, bool ignoreMetadataErrors );

	}

}
