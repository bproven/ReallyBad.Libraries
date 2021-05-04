// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.IO
//     File:       FileSystemInfoWrapper.cs
// 
//     Created:    05/03/2021 4:10 PM
//     Updated:    05/03/2021 4:22 PM
// 

using System;
using System.IO;
using System.Runtime.Serialization;

#nullable enable

namespace ReallyBad.IO
{

	public abstract class FileSystemInfoWrapper : MarshalByRefObject, ISerializable, IFileSystemInfo
	{

		public static IFileSystemInfo Create( FileSystemInfo fileSystemInfo )
		{
			return fileSystemInfo switch
			{
				FileInfo fileInfo => new FileInfoWrapper( fileInfo ),
				DirectoryInfo directoryInfo => new DirectoryInfoWrapper( directoryInfo ),
				_ => throw new InvalidOperationException( "Input is not a FileInfo or DirectoryInfo object." )
			};
		}

		protected FileSystemInfoWrapper( FileSystemInfo fileSystemInfo )
			=> FileSystemInfo = fileSystemInfo;

		public FileSystemInfo FileSystemInfo { get; init; }

		public FileAttributes Attributes
		{
			get => FileSystemInfo.Attributes;
			set => FileSystemInfo.Attributes = value;
		}

		public DateTime CreationTime
		{
			get => FileSystemInfo.CreationTime;
			set => FileSystemInfo.CreationTime = value;
		}

		public DateTime CreationTimeUtc
		{
			get => FileSystemInfo.CreationTimeUtc;
			set => FileSystemInfo.CreationTimeUtc = value;
		}

		public bool Exists => FileSystemInfo.Exists;

		public string Extension => FileSystemInfo.Extension;

		public string FullName => FileSystemInfo.FullName;

		public DateTime LastAccessTime
		{
			get => FileSystemInfo.LastAccessTime;
			set => FileSystemInfo.LastAccessTime = value;
		}

		public DateTime LastAccessTimeUtc
		{
			get => FileSystemInfo.LastAccessTimeUtc;
			set => FileSystemInfo.LastAccessTimeUtc = value;
		}

		public DateTime LastWriteTime
		{
			get => FileSystemInfo.LastWriteTime;
			set => FileSystemInfo.LastWriteTime = value;
		}

		public DateTime LastWriteTimeUtc
		{
			get => FileSystemInfo.LastWriteTimeUtc;
			set => FileSystemInfo.LastWriteTimeUtc = value;
		}

		public string Name => FileSystemInfo.Name;

		public void Delete() => FileSystemInfo.Delete();

		public virtual void Refresh() => FileSystemInfo.Refresh();

		public void GetObjectData( SerializationInfo info, StreamingContext context )
			=> FileSystemInfo.GetObjectData( info, context );

		public override string ToString() => FileSystemInfo.ToString();

	}

}
