// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.IO
//     File:       FileAttributeSetter.cs
// 
//     Created:    09/14/2004 9:14 AM
//     Updated:    05/06/2021 11:04 PM
// 

using System;
using System.Collections.Generic;
using System.IO.Abstractions;

using Microsoft.Extensions.Logging;

using ReallyBad.Core.Validation;

#nullable enable

namespace ReallyBad.IO
{

    public class FileAttributeSetter
    {

        public const string DefaultFileSearchSpec = "*.*";
        public const string DefaultDirectorySearchSpec = "*";
        public const string DefaultIgnorePrefix = ".";

        private readonly IFileSystem _fileSystem;

        private readonly ILogger<FileAttributeSetter>? _logger;

        public FileAttributeSetter( IFileSystem fileSystem, ILogger<FileAttributeSetter>? logger = null )
        {
            _logger = logger;
            _fileSystem = fileSystem;
        }

        public string Root { get; set; } = string.Empty;

        public DateTime? CreateDateTime { get; set; }

        public string FileSearch { get; set; } = DefaultFileSearchSpec;

        public string DirectorySearch { get; set; } = DefaultDirectorySearchSpec;

        public string IgnorePrefix { get; set; } = DefaultIgnorePrefix;

        public bool IsIgnoreSystem { get; set; } = true;

        public bool IsIgnoreHidden { get; set; } = true;

        public bool IsIgnoreReadOnly { get; set; } = true;

        public bool IsRecurse { get; set; } = false;

        public bool IsIgnoreErrors { get; set; } = false;

        private bool Ignore( IFileSystemInfo fileSystemInfo )
        {
            if ( fileSystemInfo.Name.StartsWith( IgnorePrefix ) )
            {
                return true;
            }

            if ( fileSystemInfo.Name.StartsWith( IgnorePrefix ) )
            {
                return true;
            }

            if ( IsIgnoreSystem && fileSystemInfo.System() )
            {
                return true;
            }

            if ( IsIgnoreHidden && fileSystemInfo.Hidden() )
            {
                return true;
            }

            if ( IsIgnoreReadOnly && fileSystemInfo.ReadOnly() )
            {
                return true;
            }

            return false;
        }

        private void SetFileSystem( IFileSystemInfo fileInfo )
        {
            if ( Ignore( fileInfo ) )
            {
                return;
            }

            try
            {
                if ( CreateDateTime.HasValue && fileInfo.CreationTime != CreateDateTime.Value )
                {
                    fileInfo.CreationTime = CreateDateTime.Value;
                }
            }
            catch ( Exception ex )
            {
                if ( !IsIgnoreErrors )
                {
                    throw;
                }

                _logger?.LogError( ex, ex.Message );
            }
        }

        private void SetFileSystem( IEnumerable<IFileSystemInfo> fileSystemInfos )
        {
            foreach ( var fileSystemInfo in fileSystemInfos )
            {
                SetFileSystem( fileSystemInfo );
            }
        }

        private void SetDirectory( IDirectoryInfo directoryInfo )
        {
            if ( Ignore( directoryInfo ) )
            {
                return;
            }

            SetFileSystem( directoryInfo );
            SetFileSystem( directoryInfo.GetFileSystemInfos() );

            if ( !IsRecurse )
            {
                return;
            }

            SetDirectory( directoryInfo.GetDirectories() );
        }

        private void SetDirectory( IEnumerable<IDirectoryInfo> directoryInfos )
        {
            foreach ( var directoryInfo in directoryInfos )
            {
                SetDirectory( directoryInfo );
            }
        }

        private void SetDirectory( string rootPath )
        {
            ArgumentValidator.ValidateNotEmpty( rootPath, nameof( rootPath ) );
            var directoryInfo = _fileSystem.DirectoryInfo.FromDirectoryName( rootPath );
            SetDirectory( directoryInfo );
        }

        public void Execute()
        {
            SetDirectory( Root );
        }

    }

}
