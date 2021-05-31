using System;
using System.IO.Abstractions;

using Microsoft.Extensions.Logging;

using ReallyBad.Core.Text;
using ReallyBad.Core.Validation;

#nullable enable

namespace ReallyBad.IO
{

	public class DirectoryCleaner
	{

		private ILogger<DirectoryCleaner> _logger;

		private string _root = string.Empty;

		public string Root
		{
			get => _root;
			set
			{
				ArgumentValidator.ValidateNotEmpty( value, nameof( value ) );
				_root = value;
			}
		}

		private IFileSystem FileSystem { get; }

		public IDirectoryInfo RootDirectoryInfo => FileSystem.DirectoryInfo.FromDirectoryName( Root );

		public DirectoryCleaner( ILogger<DirectoryCleaner> logger, IFileSystem fileSystem, string? root = null )
		{
			_logger = logger;
			FileSystem = fileSystem;
			if ( !root.NullOrEmpty() )
			{
				Root = root ?? throw new ArgumentNullException( nameof( root ) );
			}
		}

		public void Clean()
		{
			ArgumentValidator.ValidateNotEmpty( Root, nameof( Root ) );
			Clean( RootDirectoryInfo );
		}

		private void Clean( IDirectoryInfo directoryInfo )
		{

			//Debug.WriteLine( $"Cleaning {directoryInfo}" );

			if ( !directoryInfo.Empty() )
			{
				foreach ( var subDirectoryInfo in directoryInfo.GetDirectories() )
				{
					Clean( subDirectoryInfo );
				}
			}

			directoryInfo.Refresh();

			if ( !directoryInfo.Empty() )
			{
				return;
			}

			_logger.LogInformation( $"Deleting {directoryInfo}" );
			directoryInfo.Delete();

		}

	}

}
