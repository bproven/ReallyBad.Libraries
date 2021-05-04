using System.Collections.Generic;
using System.IO;

#nullable enable

namespace ReallyBad.IO
{

	public interface IDirectoryInfo : IFileSystemInfo
	{

		DirectoryInfo DirectoryInfo { get; }

		IDirectoryInfo? Parent { get; }

		IDirectoryInfo Root { get; }

		void Create();

		IDirectoryInfo CreateSubdirectory( string path );

		void Delete( bool recursive );

		IEnumerable<IDirectoryInfo> EnumerateDirectories( string searchPattern, SearchOption searchOption );

		IEnumerable<IDirectoryInfo> EnumerateDirectories( string searchPattern, EnumerationOptions enumerationOptions );

		IEnumerable<IDirectoryInfo> EnumerateDirectories();

		IEnumerable<IDirectoryInfo> EnumerateDirectories( string searchPattern );

		IEnumerable<IFileInfo> EnumerateFiles();

		IEnumerable<IFileInfo> EnumerateFiles( string searchPattern );

		IEnumerable<IFileInfo> EnumerateFiles( string searchPattern, EnumerationOptions enumerationOptions );

		IEnumerable<IFileInfo> EnumerateFiles( string searchPattern, SearchOption searchOption );

		IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos( string searchPattern, SearchOption searchOption );

		IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos();

		IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos( string searchPattern );

		IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos( string searchPattern, EnumerationOptions enumerationOptions );

		IDirectoryInfo[] GetDirectories( string searchPattern, SearchOption searchOption );

		IDirectoryInfo[] GetDirectories( string searchPattern, EnumerationOptions enumerationOptions );

		IDirectoryInfo[] GetDirectories();

		IDirectoryInfo[] GetDirectories( string searchPattern );

		IFileInfo[] GetFiles();

		IFileInfo[] GetFiles( string searchPattern );

		IFileInfo[] GetFiles( string searchPattern, EnumerationOptions enumerationOptions );

		IFileInfo[] GetFiles( string searchPattern, SearchOption searchOption );

		IFileSystemInfo[] GetFileSystemInfos( string searchPattern, SearchOption searchOption );

		IFileSystemInfo[] GetFileSystemInfos();

		IFileSystemInfo[] GetFileSystemInfos( string searchPattern );

		IFileSystemInfo[] GetFileSystemInfos( string searchPattern, EnumerationOptions enumerationOptions );

		void MoveTo( string destDirName );

	}

}
