// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.IO
//     File:       FileSequencer.cs
// 
//     Created:    05/01/2021 1:49 AM
//     Updated:    05/02/2021 9:36 PM
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.Logging;

using ReallyBad.Core.File;
using ReallyBad.Core.Validation;

#nullable enable

namespace ReallyBad.IO
{

	public class FileSequencer
	{

		private ILogger<FileSequencer> log;

		public FileSequencer( ILogger<FileSequencer> logger, IImageFileInfoProvider imageFileInfoProvider )
		{
			log = logger;
			ImageFileInfoProvider = imageFileInfoProvider;
		}

		private string _dest = string.Empty;

		private string _prefix = "IMGS";

		public IImageFileInfoProvider ImageFileInfoProvider { get; set; }

		public string Source { get; set; } = string.Empty;

		public string Dest
		{
			get => _dest;

			set
			{
				ArgumentValidator.ValidateNotEmpty( value, nameof( value ) );
				_dest = value;
			}
		}

		public string Prefix
		{
			get => _prefix;

			set
			{
				ArgumentValidator.ValidateNotEmpty( value, nameof( value ) );
				_prefix = value;
			}
		}

		public void Sequence()
		{
			var sourceDirectoryInfo = new DirectoryInfo( Source );

			var sourceFiles = new List<FileInfo>();
			sourceFiles.AddRange( sourceDirectoryInfo.GetFiles() );

			var infoList = new List<Info>();

			foreach ( var sourceFile in sourceFiles )
			{
				var info = new Info
				{
					FileInfo = sourceFile,
					Taken = ImageFileInfoProvider.GetDateTaken( sourceFile ),
				};
				infoList.Add( info );
			}

			Console.WriteLine( $"Found {infoList.Count} source files." );

			var i = 1;

			Console.WriteLine( "Sorting..." );
			var orderedList = infoList.OrderBy( i1 => i1.Taken ).ThenBy( i1 => i1.FileInfo.Name ).ToList();
			Console.WriteLine( "Done." );

			var destDateTime = orderedList.FirstOrDefault()?.Taken ?? DateTime.Now;
			var destDir = Path.Combine( Dest, $"{destDateTime:yyyy-MM-dd}" );
			var destDirectoryInfo = new DirectoryInfo( destDir );

			if ( !destDirectoryInfo.Exists )
			{
				destDirectoryInfo.Create();
			}

			foreach ( var info in orderedList )
			{
				var fileInfo = info.FileInfo;
				var src = fileInfo.Name;
				var dest = $"{Prefix}_{i:00000}{fileInfo.Extension}";
				var taken = info.Taken;
				Console.WriteLine( $"{taken:hh:mm:ss.fff}: {src} -> {dest}" );
				var destPath = Path.Combine( destDir, dest );
				fileInfo.CopyTo( destPath );
				var destFileInfo = new FileInfo( destPath )
				{
					CreationTime = fileInfo.CreationTime,
					LastAccessTime = fileInfo.LastAccessTime,
					LastWriteTime = fileInfo.LastWriteTime,
				};
				i++;
			}
		}

		private class Info
		{

			public FileInfo FileInfo { get; set; } = new FileInfo( string.Empty );

			public DateTime Taken { get; set; }

		}

	}

}
