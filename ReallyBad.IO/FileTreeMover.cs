using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

using Microsoft.Extensions.Logging;

using ReallyBad.Core.Validation;

#nullable enable

namespace ReallyBad.IO
{

    public class FileTreeMover 
    {

        private readonly ILogger<FileOrganizer> log;

        public FileTreeMover( ILogger<FileOrganizer> logger ) => log = logger;

        private FileSystem FileSystem { get; } = new();

        private IPath Path => FileSystem.Path;

        public string SourceRootDirectory { get; set; } = string.Empty;

        public string DestRootDirectory { get; set; } = string.Empty;

        public IList<string> Extensions { get; } = new List<string>();

        private IDirectoryInfo? _sourceDirectoryInfo;

        public IDirectoryInfo SourceDirectoryInfo
        {
            get
            {
                ArgumentValidator.ValidateNotEmpty( SourceRootDirectory, nameof( SourceRootDirectory ) );
                return _sourceDirectoryInfo ??= FileSystem.DirectoryInfo.FromDirectoryName( SourceRootDirectory );
            }
        }

        private IDirectoryInfo? _destDirectoryInfo;

        public IDirectoryInfo DestDirectoryInfo
        {
            get
            {
                ArgumentValidator.ValidateNotEmpty( DestRootDirectory, nameof( DestRootDirectory ) );
                return _destDirectoryInfo ??= FileSystem.DirectoryInfo.FromDirectoryName( DestRootDirectory );
            }
        }

        public void Move()
        {
            if ( !Extensions.Any() )
            {
                Extensions.Add( "*" );
            }
            Move( SourceDirectoryInfo, DestDirectoryInfo );
        }

        private void Move( IDirectoryInfo sourceDirectoryInfo, IDirectoryInfo destDirectoryInfo )
        {

            log.LogDebug( $"Moving {sourceDirectoryInfo} to {destDirectoryInfo}" );

            var destDirectory = destDirectoryInfo.ToString();

            foreach ( var extension in Extensions )
            {
                foreach ( var sourceFileInfo in sourceDirectoryInfo.EnumerateFiles( $"*.{extension}" ) )
                {
                    var destFileName = Path.Combine( destDirectory, sourceFileInfo.Name );
                    var i = 0;
                    while ( File.Exists( destFileName ) && i < 1000 )
                    {
                        destFileName = Path.Combine( destDirectory,
                            $"{sourceFileInfo.BaseName()} ({i}){sourceFileInfo.Extension}" );
                        i++;
                    }
                    if ( i == 1000 )
                    {
                        throw new InvalidOperationException( $"Too many duplicate file names for {sourceFileInfo.FullName} in {destDirectoryInfo}" );
                    }
                    log.LogDebug( $"Moving {sourceFileInfo.FullName} to {destFileName}" );
                    sourceFileInfo.MoveTo( destFileName );
                }
            }

            foreach ( var sourceSubDirectoryInfo in sourceDirectoryInfo.EnumerateDirectories() )
            {
                var destSubDirectoryName = Path.Combine( destDirectory, sourceSubDirectoryInfo.Name );
                var destSubDirectoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( destSubDirectoryName );

                if ( sourceSubDirectoryInfo.Empty() )
                {
                    continue;
                }

                if ( !destSubDirectoryInfo.Exists )
                {
                    destSubDirectoryInfo.Create();
                }

                Move( sourceSubDirectoryInfo, destSubDirectoryInfo );
            }

        }

    }

}