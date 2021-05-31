// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core
//     File:       IImageFileInfoProvider.cs
// 
//     Created:    09/14/2004 9:14 AM
//     Updated:    05/06/2021 10:58 PM
// 

using System;
using System.IO.Abstractions;

#nullable enable

namespace ReallyBad.Core.File
{

    public interface IImageFileInfoProvider
    {

        DateTime GetDateTaken( string filePath );

        DateTime GetDateTaken( IFileInfo fileInfo );

    }

}
