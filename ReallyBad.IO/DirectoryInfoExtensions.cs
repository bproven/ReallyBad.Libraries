// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.IO
//     File:       DirectoryInfoExtensions.cs
// 
//     Created:    05/01/2021 1:49 AM
//     Updated:    05/02/2021 9:39 PM
// 

using System.IO;
using System.Linq;

#nullable enable

namespace ReallyBad.IO
{

	public static class DirectoryInfoExtensions
	{

		public static bool IsEmpty( this DirectoryInfo directoryInfo )
			=> !directoryInfo.EnumerateFileSystemInfos().Any();

	}

}
