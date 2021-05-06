// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.IO
//     File:       Directory.cs
// 
//     Created:    05/01/2021 1:21 AM
//     Updated:    05/02/2021 9:27 PM
// 

using System.Collections.Generic;
using System.IO;
using System.Linq;

using ReallyBad.Core.Collection;
using ReallyBad.Core.Validation;

#nullable enable

namespace ReallyBad.IO
{

	public class Directory
	{


		private readonly IList<FileSystemInfo> _allFiles = new List<FileSystemInfo>();

		private readonly string _path = string.Empty;

		private DirectoryInfo? _directoryInfo;

		public Directory()
		{
		}

		public Directory( string path )
		{
			ArgumentValidator.ValidateNotEmpty( path, nameof( path ) );
			Path = path;
		}

		public string Path
		{
			get => _path;

			init
			{
				ArgumentValidator.ValidateNotEmpty( value, nameof( value ) );
				_path = value;
			}
		}

		public DirectoryInfo DirectoryInfo => _directoryInfo ??= new DirectoryInfo( Path );

		public IEnumerable<FileInfo> Files => _allFiles.OfType<FileInfo>();

		public IEnumerable<DirectoryInfo> Directories => _allFiles.OfType<DirectoryInfo>();

		public void Scan()
		{
			_allFiles.AddCollection( DirectoryInfo.GetFileSystemInfos() );
		}

	}

}
