// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.IO
//     File:       FileOrganizer.cs
// 
//     Created:    09/14/2004 9:14 AM
//     Updated:    05/06/2021 11:04 PM
// 

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO.Abstractions;
using System.Linq;

using Microsoft.Extensions.Logging;

using ReallyBad.Core.File;
using ReallyBad.Core.Text;
using ReallyBad.Core.Validation;

#nullable enable

namespace ReallyBad.IO
{

    public class FileOrganizer
    {

        private const string DefaultPathFormat = "yyyy-MM-dd";

        private readonly ILogger<FileOrganizer> log;

        private string _pathFormat = DefaultPathFormat;

        private int errorCount;

        private int existsCount;

        private int moveCount;

        private int totalCount;

        public FileOrganizer( ILogger<FileOrganizer> logger, IImageFileInfoProvider imageFileInfoProvider )
        {
            log = logger;
            ImageFileInfoProvider = imageFileInfoProvider;
        }

        private FileSystem FileSystem { get; } = new();

        private IPath Path => FileSystem.Path;

        public string SourceRootDirectory { get; set; } = string.Empty;

        public string DestRootDirectory { get; set; } = string.Empty;

        public string PathFormat
        {
            get => _pathFormat;

            set
            {
                ArgumentValidator.ValidateNotEmpty( value, nameof( value ) );
                _pathFormat = value;
            }
        }

        public bool IsPause { get; set; } = false;

        public bool IsVerbose { get; set; } = false;

        public bool IsDebug { get; set; } = true;

        public bool IsPreserveTree { get; set; } = false;

        public bool IsRecurse { get; set; } = false;

        public bool IsClean { get; set; } = false;

        public int Limit { get; set; } = 8000;

        private IImageFileInfoProvider ImageFileInfoProvider { get; }

        public void Organize()
        {
            var rootDirectoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( SourceRootDirectory );
            Organize( rootDirectoryInfo );

            if ( IsVerbose )
            {
                log.LogDebug(
                    $"{totalCount} total files.  {moveCount} moved, {errorCount} error{( errorCount == 1 ? string.Empty : "s" )}, {existsCount} not moved because they already existed in the destination." );
            }
        }

        [ExcludeFromCodeCoverage]
        private void Log( string message )
        {
            ArgumentValidator.ValidateNotEmpty( message, nameof( message ) );

            if ( IsVerbose )
            {
                log.LogInformation( message );
            }
        }

        public static bool DateFormatDirectory( string directoryName, string pathFormat )
        {
            ArgumentValidator.ValidateNotEmpty( directoryName, nameof( directoryName ) );
            ArgumentValidator.ValidateNotEmpty( pathFormat, nameof( pathFormat ) );

            return DateTime.TryParseExact( directoryName, pathFormat, CultureInfo.CurrentCulture,
                DateTimeStyles.AssumeLocal, out var _ );
        }

        /// <summary>
        /// Tests if a directory name is in date format.
        /// </summary>
        /// <param name="directoryName"></param>
        /// <returns>Boolean indicating whether the past in value is in date format.</returns>
        public bool DateFormatDirectory( string directoryName )
        {
            ArgumentValidator.ValidateNotEmpty( directoryName, nameof( directoryName ) );

            return DateFormatDirectory( directoryName, PathFormat );
        }

        public string GetRelativePath( string subFolder, string folder )
        {
            var pathUri = new Uri( subFolder );

            // Folders must end in a slash
            if ( !folder.EndsWith( Path.DirectorySeparatorChar.ToString() ) )
            {
                folder += Path.DirectorySeparatorChar;
            }

            if ( !subFolder.EndsWith( Path.DirectorySeparatorChar.ToString() ) )
            {
                //subFolder += Path.DirectorySeparatorChar;
            }

            var folderUri = new Uri( folder );
            var result = Uri.UnescapeDataString( folderUri.MakeRelativeUri( pathUri )
                .ToString()
                .Replace( '/', Path.DirectorySeparatorChar ) );
            result = result.TrimEnd( Path.DirectorySeparatorChar.ToString() );

            return result;
        }

        public string GetDestinationParentDirectory( IFileInfo sourceFileInfo )
        {
            var destinationParentDirectory = DestRootDirectory;

            if ( !IsPreserveTree )
            {
                return destinationParentDirectory;
            }

            var sourceDirectory =
                GetRelativePath( sourceFileInfo.Directory?.FullName ?? string.Empty, SourceRootDirectory );
            var sourceDirectoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( sourceDirectory );

            if ( DateFormatDirectory( sourceDirectoryInfo.Name ) )
            {
                sourceDirectory = GetRelativePath( sourceFileInfo.Directory?.Parent?.FullName ?? string.Empty,
                    SourceRootDirectory );
            }

            destinationParentDirectory = Path.Combine( destinationParentDirectory, sourceDirectory );

            return destinationParentDirectory;
        }

        public string GetDestinationPath( string sourceFileName, string destinationParentDirectory,
            string destinationRelativePath )
        {
            ArgumentValidator.ValidateNotEmpty( sourceFileName, nameof( sourceFileName ) );
            ArgumentValidator.ValidateNotEmpty( destinationParentDirectory, nameof( destinationParentDirectory ) );
            ArgumentValidator.ValidateNotEmpty( destinationRelativePath, nameof( destinationRelativePath ) );

            return Path.Combine( Path.Combine( destinationParentDirectory, destinationRelativePath ), sourceFileName );
        }

        public IFileInfo GetDestinationFileInfo( string sourceFileName, string destinationParentDirectory,
            string destinationRelativePath )
        {
            ArgumentValidator.ValidateNotEmpty( sourceFileName, nameof( sourceFileName ) );
            ArgumentValidator.ValidateNotEmpty( destinationParentDirectory, nameof( destinationParentDirectory ) );
            ArgumentValidator.ValidateNotEmpty( destinationRelativePath, nameof( destinationRelativePath ) );
            var destFullPath =
                GetDestinationPath( sourceFileName, destinationParentDirectory, destinationRelativePath );

            return FileSystem.FileInfo.FromFileName( destFullPath );
        }

        public IFileInfo GetDestinationFileInfo( IFileInfo sourceFileInfo, string destinationParentDirectory,
            string destinationRelativePath )
        {
            ArgumentValidator.ValidateNotEmpty( destinationParentDirectory, nameof( destinationParentDirectory ) );
            ArgumentValidator.ValidateNotEmpty( destinationRelativePath, nameof( destinationRelativePath ) );

            return GetDestinationFileInfo( sourceFileInfo.Name, destinationParentDirectory, destinationRelativePath );
        }

        public IFileInfo GetDestinationFileInfo( IFileInfo sourceFileInfo, string destinationRelativePath )
        {
            ArgumentValidator.ValidateNotEmpty( destinationRelativePath, nameof( destinationRelativePath ) );
            var destinationParentDirectory = GetDestinationParentDirectory( sourceFileInfo );

            return GetDestinationFileInfo( sourceFileInfo, destinationParentDirectory, destinationRelativePath );
        }

        public IFileInfo GetDestinationFileInfo( IFileInfo sourceFileInfo )
        {
            var destinationRelativePath = ImageFileInfoProvider.GetDateTaken( sourceFileInfo ).ToString( PathFormat );
            var destinationParentDirectory = GetDestinationParentDirectory( sourceFileInfo );

            return GetDestinationFileInfo( sourceFileInfo, destinationParentDirectory, destinationRelativePath );
        }

        private void Move( IFileInfo sourceFileInfo, IFileInfo destFileInfo )
        {
            if ( IsDebug )
            {
                Log( $"DEBUG Not Moving {sourceFileInfo.Name} to {destFileInfo.FullName} " );
            }
            else
            {
                if ( string.Compare( sourceFileInfo.FullName, destFileInfo.FullName,
                    StringComparison.CurrentCultureIgnoreCase ) == 0 )
                {
                    Log( $"Not Moving {sourceFileInfo.Name} to {destFileInfo.FullName} " );
                }
                else
                {
                    Log( $"Moving {sourceFileInfo.Name} to {destFileInfo.FullName} " );

                    if ( !( destFileInfo.Directory?.Exists ?? true ) )
                    {
                        destFileInfo.Directory.Create();
                    }

                    if ( destFileInfo.Exists )
                    {
                        Log(
                            $"Not Moving {sourceFileInfo.Name} to {destFileInfo.FullName} because the destination file already exists." );
                        existsCount++;
                    }
                    else
                    {
                        sourceFileInfo.MoveTo( destFileInfo.FullName );
                        moveCount++;
                    }
                }
            }
        }

        private void Move( IFileInfo sourceFileInfo )
        {
            var destination = GetDestinationFileInfo( sourceFileInfo );
            Move( sourceFileInfo, destination );
        }

        private void Organize( IDirectoryInfo sourceDirectoryInfo )
        {
            if ( IsRecurse )
            {
                foreach ( var directoryInfo in sourceDirectoryInfo.GetDirectories( "*.*" ).OrderBy( d => d.Name ) )
                {
                    Organize( directoryInfo );
                }

                if ( IsPause )
                {
                    Console.Write( "Press any key to continue ..." );
                    Console.ReadKey();
                    Console.WriteLine();
                }
            }

            //foreach ( IFileInfo fileInfo in sourceDirectoryInfo.GetFiles( "*.*" ).OrderBy( f => f.Name ) )
            foreach ( var fileInfo in sourceDirectoryInfo.GetFiles( "*.*" ).Take( Limit ) )
            {
                try
                {
                    totalCount++;
                    Move( fileInfo );
                }
                catch ( Exception ex )
                {
                    errorCount++;
                    log.LogError( $"Error moving {fileInfo.Name}. {ex.Message}", ex );
                }
            }

            if ( !IsClean )
            {
                return;
            }

            if ( !DateFormatDirectory( sourceDirectoryInfo.Name ) || !sourceDirectoryInfo.Empty() )
            {
                return;
            }

            if ( IsDebug )
            {
                Log( $"DEBUG Not Deleting {sourceDirectoryInfo.FullName} " );
            }
            else
            {
                Log( $"Deleting {sourceDirectoryInfo.FullName} " );

                //sourceDirectoryInfo.Delete();
            }
        }

    }

}
