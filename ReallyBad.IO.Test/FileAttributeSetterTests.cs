// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.IO.Test
//     File:       FileAttributeSetterTests.cs
// 
//     Created:    09/14/2004 9:14 AM
//     Updated:    05/06/2021 11:05 PM
// 

using System;
using System.IO.Abstractions;

using Xunit;

#nullable enable

namespace ReallyBad.IO.Test
{

    public class FileAttributeSetterTests
    {

        [Fact]
        public void SetCreateDateTest()
        {
            var fileSystem = new FileSystem();
            var fileAttributeSetter = new FileAttributeSetter( fileSystem )
            {
                CreateDateTime = DateTime.Parse( "2004-09-14 09:14 AM" ),
                IsRecurse = true,
                IsIgnoreErrors = true,
                FileSearch = "*.*",
                DirectorySearch = "*",
                Root = @"D:\Projects\Really Bad\ReallyBad.Libraries",
            };
            fileAttributeSetter.Execute();
            fileAttributeSetter.Root = @"D:\Projects\Really Bad\FileOrganizer";
            fileAttributeSetter.Execute();
        }

    }

}
