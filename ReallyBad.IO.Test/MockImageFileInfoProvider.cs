// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.IO.Test
//     File:       MockImageFileInfoProvider.cs
// 
//     Created:    09/14/2004 9:14 AM
//     Updated:    05/06/2021 11:11 PM
// 

using System;
using System.IO.Abstractions;

using ReallyBad.Core.File;

#nullable enable

namespace ReallyBad.IO.Test
{

    public class MockImageFileInfoProvider : IImageFileInfoProvider
    {

        public DateTime DateTime { get; set; } = new(2019, 1, 1, 23, 00, 00);

        public DateTime GetDateTaken( string filePath ) => DateTime;

        public DateTime GetDateTaken( IFileInfo fileInfo ) => DateTime;

    }

}
