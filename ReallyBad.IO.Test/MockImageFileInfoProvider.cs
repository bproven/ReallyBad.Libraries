using System;
using System.IO.Abstractions;

using ReallyBad.Core.File;

namespace ReallyBad.IO.Test
{

    public class MockImageFileInfoProvider : IImageFileInfoProvider
    {

        public DateTime DateTime { get; set; } = new ( 2019, 1, 1, 23, 00, 00 );

        public DateTime GetDateTaken( string filePath ) => DateTime;

        public DateTime GetDateTaken( IFileInfo fileInfo ) => DateTime;

    }

}
