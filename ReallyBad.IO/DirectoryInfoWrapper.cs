using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable enable

namespace ReallyBad.IO
{

	public class DirectoryInfoWrapper : FileSystemInfoWrapper, IDirectoryInfo
	{

		public DirectoryInfo DirectoryInfo => (DirectoryInfo)FileSystemInfo;

		public DirectoryInfoWrapper( string path )
			: this( new DirectoryInfo( path ) )
		{
		}

		public DirectoryInfoWrapper( DirectoryInfo directoryInfo )
			: base( directoryInfo )
		{
		}

		private IDirectoryInfo? _parent;

		public IDirectoryInfo? Parent
			=> _parent ??= DirectoryInfo.Parent is null ? null : new DirectoryInfoWrapper( DirectoryInfo.Parent );

		private IDirectoryInfo? _root;

		public IDirectoryInfo Root => _root ??= new DirectoryInfoWrapper( DirectoryInfo.Root );

		public void Create() => DirectoryInfo.Create();

		public IDirectoryInfo CreateSubdirectory( string path )
			=> new DirectoryInfoWrapper( DirectoryInfo.CreateSubdirectory( path ) );

		public void Delete( bool recursive ) => DirectoryInfo.Delete( recursive );

		public IEnumerable<IDirectoryInfo> EnumerateDirectories( string searchPattern, SearchOption searchOption )
			=> new FileSystemInfoEnumerable<IDirectoryInfo,DirectoryInfo>( DirectoryInfo.EnumerateDirectories( searchPattern, searchOption ) );

		public IEnumerable<IDirectoryInfo> EnumerateDirectories( string searchPattern, EnumerationOptions enumerationOptions )
			=> new FileSystemInfoEnumerable<IDirectoryInfo, DirectoryInfo>( DirectoryInfo.EnumerateDirectories( searchPattern, enumerationOptions ) );

		public IEnumerable<IDirectoryInfo> EnumerateDirectories()
			=> new FileSystemInfoEnumerable<IDirectoryInfo, DirectoryInfo>( DirectoryInfo.EnumerateDirectories() );

		public IEnumerable<IDirectoryInfo> EnumerateDirectories( string searchPattern )
			=> new FileSystemInfoEnumerable<IDirectoryInfo, DirectoryInfo>( DirectoryInfo.EnumerateDirectories( searchPattern ) );

		public IEnumerable<IFileInfo> EnumerateFiles()
			=> new FileSystemInfoEnumerable<IFileInfo,FileInfo>( DirectoryInfo.EnumerateFiles() );

		public IEnumerable<IFileInfo> EnumerateFiles( string searchPattern )
			=> new FileSystemInfoEnumerable<IFileInfo, FileInfo>( DirectoryInfo.EnumerateFiles( searchPattern ) );

		public IEnumerable<IFileInfo> EnumerateFiles( string searchPattern, EnumerationOptions enumerationOptions )
			=> new FileSystemInfoEnumerable<IFileInfo, FileInfo>( DirectoryInfo.EnumerateFiles( searchPattern, enumerationOptions ) );

		public IEnumerable<IFileInfo> EnumerateFiles( string searchPattern, SearchOption searchOption )
			=> new FileSystemInfoEnumerable<IFileInfo, FileInfo>( DirectoryInfo.EnumerateFiles( searchPattern, searchOption ) );

		public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos( string searchPattern, SearchOption searchOption )
			=> new FileSystemInfoEnumerable<IFileSystemInfo, FileSystemInfo>( DirectoryInfo.EnumerateFileSystemInfos( searchPattern, searchOption ) );

		public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos()
			=> new FileSystemInfoEnumerable<IFileSystemInfo, FileSystemInfo>( DirectoryInfo.EnumerateFileSystemInfos() );

		public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos( string searchPattern )
			=> new FileSystemInfoEnumerable<IFileSystemInfo, FileSystemInfo>( DirectoryInfo.EnumerateFileSystemInfos( searchPattern ) );

		public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos( string searchPattern, EnumerationOptions enumerationOptions )
			=> new FileSystemInfoEnumerable<IFileSystemInfo, FileSystemInfo>( DirectoryInfo.EnumerateFileSystemInfos( searchPattern, enumerationOptions ) );

		public IDirectoryInfo[] GetDirectories( string searchPattern, SearchOption searchOption )
			=> DirectoryInfo.GetDirectories( searchPattern, searchOption )
				.Select( d => (IDirectoryInfo)new DirectoryInfoWrapper( d ) )
				.ToArray();

		public IDirectoryInfo[] GetDirectories( string searchPattern, EnumerationOptions enumerationOptions )
			=> DirectoryInfo.GetDirectories( searchPattern, enumerationOptions )
				.Select( d => (IDirectoryInfo)new DirectoryInfoWrapper( d ) )
				.ToArray();

		public IDirectoryInfo[] GetDirectories()
			=> DirectoryInfo.GetDirectories()
				.Select( d => (IDirectoryInfo)new DirectoryInfoWrapper( d ) )
				.ToArray();

		public IDirectoryInfo[] GetDirectories( string searchPattern )
			=> DirectoryInfo.GetDirectories( searchPattern )
				.Select( d => (IDirectoryInfo)new DirectoryInfoWrapper( d ) )
				.ToArray();

		public IFileInfo[] GetFiles()
			=> DirectoryInfo.GetFiles()
				.Select( f => (IFileInfo)new FileInfoWrapper( f ) )
				.ToArray();

		public IFileInfo[] GetFiles( string searchPattern )
			=> DirectoryInfo.GetFiles( searchPattern )
				.Select( f => (IFileInfo)new FileInfoWrapper( f ) )
				.ToArray();

		public IFileInfo[] GetFiles( string searchPattern, EnumerationOptions enumerationOptions )
			=> DirectoryInfo.GetFiles( searchPattern, enumerationOptions )
				.Select( f => (IFileInfo)new FileInfoWrapper( f ) )
				.ToArray();

		public IFileInfo[] GetFiles( string searchPattern, SearchOption searchOption )
			=> DirectoryInfo.GetFiles( searchPattern, searchOption )
				.Select( f => (IFileInfo)new FileInfoWrapper( f ) )
				.ToArray();

		public IFileSystemInfo[] GetFileSystemInfos( string searchPattern, SearchOption searchOption )
			=> DirectoryInfo.GetFileSystemInfos( searchPattern, searchOption )
				.Select( Create )
				.ToArray();

		public IFileSystemInfo[] GetFileSystemInfos()
			=> DirectoryInfo.GetFileSystemInfos()
				.Select( Create )
				.ToArray();

		public IFileSystemInfo[] GetFileSystemInfos( string searchPattern )
			=> DirectoryInfo.GetFileSystemInfos( searchPattern )
				.Select( Create )
				.ToArray();

		public IFileSystemInfo[] GetFileSystemInfos( string searchPattern, EnumerationOptions enumerationOptions )
			=> DirectoryInfo.GetFileSystemInfos( searchPattern, enumerationOptions )
				.Select( Create )
				.ToArray();

		public void MoveTo( string destDirName ) => DirectoryInfo.MoveTo( destDirName );

		public override void Refresh()
		{
			base.Refresh();
			_parent = null;
			_root = null;
		}

	}

}
