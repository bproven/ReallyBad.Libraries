// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core
//     File:       ImageFileInfoProvider.cs
// 
//     Created:    09/14/2004 9:14 AM
//     Updated:    05/06/2021 10:58 PM
// 

using System;
using System.IO.Abstractions;

using ExifLib;

using Microsoft.Extensions.Logging;

using ReallyBad.Core.Validation;

#nullable enable

namespace ReallyBad.Core.File
{

    /// <summary>
    /// </summary>
    public class ImageFileInfoProvider : IImageFileInfoProvider
    {

        private readonly IFileSystem _fileSystem;

        private readonly ILogger<ImageFileInfoProvider> _log;

        public ImageFileInfoProvider( ILogger<ImageFileInfoProvider> logger, IFileSystem fileSystem )
        {
            _log = logger;
            _fileSystem = fileSystem;
        }

        //public ImageFileInfoProvider()
        //	: this( Logger.CreateLogger<ImageFileInfoProvider>() )
        //{
        //}

        /// <summary>
        /// Gets the Date Taken from the file specified by fileInfo.  If the image does not contain a date taken, returns the
        /// LastWriteTime.
        /// </summary>
        /// <param name="filePath">The file to retrieve the date taken for.</param>
        /// <returns>The date taken for the image.</returns>
        public DateTime GetDateTaken( string filePath )
        {
            ArgumentValidator.ValidateNotEmpty( filePath, nameof( filePath ) );

            return GetDateTaken( _fileSystem.FileInfo.FromFileName( filePath ) );
        }

        /// <summary>
        /// Gets the Date Taken from the file specified by fileInfo.  If the image does not contain a date taken, returns the
        /// LastWriteTime.
        /// </summary>
        /// <param name="fileInfo">The file to retrieve the date taken for.</param>
        /// <returns>The date taken for the image.</returns>
        public DateTime GetDateTaken( IFileInfo fileInfo )
        {
            using var reader = new ExifReader( fileInfo.FullName );

            if ( reader.GetTagValue<DateTime>( ExifTags.DateTimeDigitized, out var returnDateTime ) )
            {
                return returnDateTime;
            }

            returnDateTime = fileInfo.LastWriteTime;
            _log.LogWarning( $"Image date not found for {fileInfo.FullName}" );

            return returnDateTime;
        }

    }

}
