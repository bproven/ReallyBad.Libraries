// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core
//     File:       ByteArrayExtensions.cs
// 
//     Created:    04/29/2020 5:05 PM
//     Updated:    05/02/2021 1:19 AM
// 

using System;
using System.Text;

#nullable enable

namespace ReallyBad.Core.Binary
{

	public static class ByteArrayExtensions
	{

		public static string GetEncoded( this byte[] bytes, Encoding encoding )
			=> encoding.GetString( bytes );

		public static string GetBase64( this byte[] bytes )
			=> bytes.Length > 0 ? Convert.ToBase64String( bytes ) : string.Empty;

		public static bool ValueEquals( this byte[]? bytes, byte[]? other )
		{
			var result = bytes == other;
			if ( result )
			{
				return true;
			}

			if ( bytes == null || other == null )
			{
				return false;
			}

			if ( bytes.Length != other.Length )
			{
				return false;
			}

			var compare = true;
			for ( var i = 0; compare && i < bytes.Length; i++ )
			{
				compare = bytes[ i ] == other[ i ];
			}

			result = compare;
			return result;
		}

	}

}
