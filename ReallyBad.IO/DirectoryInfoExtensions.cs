// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.IO
//     File:       DirectoryInfoExtensions.cs
// 
//     Created:    09/14/2004 9:14 AM
//     Updated:    05/06/2021 11:03 PM
// 

using System.IO.Abstractions;
using System.Linq;

#nullable enable

namespace ReallyBad.IO
{

    public static class DirectoryInfoExtensions
    {

        public static bool Empty( this IDirectoryInfo directoryInfo )
            => !directoryInfo.EnumerateFileSystemInfos().Any();

    }

}
