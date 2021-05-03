// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core
//     File:       IImageFileInfoProvider.cs
// 
//     Created:    04/29/2020 5:05 PM
//     Updated:    05/02/2021 7:43 PM
// 

using System;
using System.IO;

#nullable enable

namespace ReallyBad.Core.File
{

	public interface IImageFileInfoProvider
	{

		DateTime GetDateTaken( string filePath );

		DateTime GetDateTaken( FileInfo fileInfo );

	}

}
