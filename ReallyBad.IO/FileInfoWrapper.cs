// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.IO
//     File:       FileInfoWrapper.cs
// 
//     Created:    05/03/2021 4:53 PM
//     Updated:    05/03/2021 5:23 PM
// 

using System.IO;
using System.Runtime.Versioning;

#nullable enable

namespace ReallyBad.IO
{

	public class FileInfoWrapper : FileSystemInfoWrapper, IFileInfo
	{

		public FileInfoWrapper( string fileName )
			: this( new FileInfo( fileName ) )
		{
		}

		public FileInfoWrapper( FileInfo fileInfo )
			: base( fileInfo )
		{
		}

		private FileInfo FileInfo => (FileInfo)FileSystemInfo;

		private IDirectoryInfo? _directory;

		public IDirectoryInfo? Directory => _directory ??= FileInfo.Directory is null ? null : new DirectoryInfoWrapper( FileInfo.Directory );

		public string? DirectoryName => FileInfo.DirectoryName;

		public bool IsReadOnly
		{
			get => FileInfo.IsReadOnly;
			set => FileInfo.IsReadOnly = value;
		}

		public long Length => FileInfo.Length;

		public StreamWriter AppendText() => FileInfo.AppendText();

		public IFileInfo CopyTo( string destFileName ) => new FileInfoWrapper( FileInfo.CopyTo( destFileName ) );

		public IFileInfo CopyTo( string destFileName, bool overwrite )
			=> new FileInfoWrapper( FileInfo.CopyTo( destFileName, overwrite ) );

		public FileStream Create() => FileInfo.Create();

		public StreamWriter CreateText() => FileInfo.CreateText();

		[SupportedOSPlatform( "windows" )]
		public void Decrypt() => FileInfo.Decrypt();

		[SupportedOSPlatform( "windows" )]
		public void Encrypt() => FileInfo.Encrypt();

		public void MoveTo( string destFileName ) => FileInfo.MoveTo( destFileName );

		public void MoveTo( string destFileName, bool overwrite ) => FileInfo.MoveTo( destFileName, overwrite );

		public FileStream Open( FileMode mode ) => FileInfo.Open( mode );

		public FileStream Open( FileMode mode, FileAccess access ) => FileInfo.Open( mode, access );

		public FileStream Open( FileMode mode, FileAccess access, FileShare share )
			=> FileInfo.Open( mode, access, share );

		public FileStream OpenRead() => FileInfo.OpenRead();

		public StreamReader OpenText() => FileInfo.OpenText();

		public FileStream OpenWrite() => FileInfo.OpenWrite();

		public IFileInfo Replace( string destinationFileName, string? destinationBackupFileName )
			=> new FileInfoWrapper( FileInfo.Replace( destinationFileName, destinationBackupFileName ) );

		public IFileInfo Replace( string destinationFileName, string? destinationBackupFileName,
			bool ignoreMetadataErrors ) => new FileInfoWrapper( FileInfo.Replace( destinationFileName,
			destinationBackupFileName, ignoreMetadataErrors ) );

		public override void Refresh()
		{
			base.Refresh();
			_directory = null;
		}

	}

}
