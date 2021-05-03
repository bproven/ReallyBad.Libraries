// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core
//     File:       ImageFileInfoProvider.cs
// 
//     Created:    04/29/2020 5:05 PM
//     Updated:    05/02/2021 7:43 PM
// 

using System;
using System.IO;

using ExifLib;

using Microsoft.Extensions.Logging;

using ReallyBad.Core.Logging;
using ReallyBad.Core.Validation;

#nullable enable

namespace ReallyBad.Core.File
{

	/// <summary>
	/// </summary>
	public class ImageFileInfoProvider : IImageFileInfoProvider
	{

		private readonly ILogger<ImageFileInfoProvider> log;

		public ImageFileInfoProvider( ILogger<ImageFileInfoProvider> logger ) => log = logger;

		public ImageFileInfoProvider()
			: this( Logger.CreateLogger<ImageFileInfoProvider>() )
		{
		}

		/// <summary>
		/// Gets the Date Taken from the file specified by fileInfo.  If the image does not contain a date taken, returns the
		/// LastWriteTime.
		/// </summary>
		/// <param name="filePath">The file to retrieve the date taken for.</param>
		/// <returns>The date taken for the image.</returns>
		public DateTime GetDateTaken( string filePath )
		{
			Validator.ValidateNotEmpty( filePath, nameof( filePath ) );
			return GetDateTaken( new FileInfo( filePath ) );
		}

		/// <summary>
		/// Gets the Date Taken from the file specified by fileInfo.  If the image does not contain a date taken, returns the
		/// LastWriteTime.
		/// </summary>
		/// <param name="fileInfo">The file to retrieve the date taken for.</param>
		/// <returns>The date taken for the image.</returns>
		public DateTime GetDateTaken( FileInfo fileInfo )
		{
			using var reader = new ExifReader( fileInfo.FullName );

			if ( reader.GetTagValue<DateTime>( ExifTags.DateTimeDigitized, out var returnDateTime ) )
			{
				return returnDateTime;
			}

			returnDateTime = fileInfo.LastWriteTime;
			log.LogWarning( $"Image date not found for {fileInfo.FullName}" );

			return returnDateTime;
		}

	}

}
