// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.IO
//     File:       IFileSystemInfo.cs
// 
//     Created:    05/03/2021 3:55 PM
//     Updated:    05/03/2021 5:23 PM
// 

using System;
using System.IO;
using System.Runtime.Serialization;

namespace ReallyBad.IO
{

	public interface IFileSystemInfo
	{

		FileAttributes Attributes { get; set; }

		DateTime CreationTime { get; set; }

		DateTime CreationTimeUtc { get; set; }

		bool Exists { get; }

		string Extension { get; }

		string FullName { get; }

		DateTime LastAccessTime { get; set; }

		DateTime LastAccessTimeUtc { get; set; }

		DateTime LastWriteTime { get; set; }

		DateTime LastWriteTimeUtc { get; set; }

		string Name { get; }

		void Delete();

		void GetObjectData( SerializationInfo info, StreamingContext context );

		void Refresh();

	}

}
