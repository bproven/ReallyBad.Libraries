// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.IO
//     File:       FileSystemInfoExtensions.cs
// 
//     Created:    09/14/2004 9:14 AM
//     Updated:    05/06/2021 11:04 PM
// 

using System.IO;
using System.IO.Abstractions;

using ReallyBad.Core.Text;

#nullable enable

namespace ReallyBad.IO
{

    public static class FileSystemInfoExtensions
    {

        public static bool AttributeSet( this IFileSystemInfo fileSystemInfo, FileAttributes fileAttributes )
            => ( fileSystemInfo.Attributes & fileAttributes ) != 0;

        public static bool ReadOnly( this IFileSystemInfo fileSystemInfo )
            => fileSystemInfo.AttributeSet( FileAttributes.ReadOnly );

        public static bool Hidden( this IFileSystemInfo fileSystemInfo )
            => fileSystemInfo.AttributeSet( FileAttributes.Hidden );

        public static bool System( this IFileSystemInfo fileSystemInfo )
            => fileSystemInfo.AttributeSet( FileAttributes.System );

        public static string BaseName( this IFileSystemInfo fileSystemInfo ) 
            => fileSystemInfo.Extension.NullOrEmpty() ? fileSystemInfo.Name : fileSystemInfo.Name.Replace( fileSystemInfo.Extension, string.Empty );

    }

}
