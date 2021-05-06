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
using System.IO.Abstractions;
using System.Linq;

using Microsoft.Extensions.Logging;

using ReallyBad.Core.File;
using ReallyBad.Core.Validation;

#nullable enable

namespace ReallyBad.IO
{

	public class FileSequencer
	{

		private readonly ILogger<FileSequencer> log;

		public FileSequencer( ILogger<FileSequencer> logger, IImageFileInfoProvider imageFileInfoProvider )
		{
			log = logger;
			ImageFileInfoProvider = imageFileInfoProvider;
			log.LogDebug( "ctor" );
		}

		private FileSystem FileSystem { get; } = new();

		private IPath Path => FileSystem.Path;

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

			log.LogDebug( "Sequence" );

			var sourceDirectoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( Source );

			var sourceFiles = new List<IFileInfo>();
			sourceFiles.AddRange( sourceDirectoryInfo.GetFiles() );

			var infoList = new List<Info>();

			foreach ( var sourceFile in sourceFiles )
			{
				var info = new Info( sourceFile, ImageFileInfoProvider.GetDateTaken( sourceFile ) );
				infoList.Add( info );
			}

			Console.WriteLine( $"Found {infoList.Count} source files." );

			var i = 1;

			Console.WriteLine( "Sorting..." );
			var orderedList = infoList.OrderBy( i1 => i1.Taken ).ThenBy( i1 => i1.FileInfo.Name ).ToList();
			Console.WriteLine( "Done." );

			var destDateTime = orderedList.FirstOrDefault()?.Taken ?? DateTime.Now;
			var destDir = Path.Combine( Dest, $"{destDateTime:yyyy-MM-dd}" );
			var destDirectoryInfo = FileSystem.DirectoryInfo.FromDirectoryName( destDir );

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
				var destFileInfo = FileSystem.FileInfo.FromFileName( destPath );
				destFileInfo.CreationTime = fileInfo.CreationTime;
				destFileInfo.LastAccessTime = fileInfo.LastAccessTime;
				destFileInfo.LastWriteTime = fileInfo.LastWriteTime;
				i++;
			}
		}

		private class Info
		{

			public IFileInfo FileInfo { get; } 

			public DateTime Taken { get; }

			public Info( IFileInfo fileInfo, DateTime taken )
			{
				FileInfo = fileInfo;
				Taken = taken;
			}

		}

	}

}
