// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core
//     File:       ByteArrayExtensions.cs
// 
//     Created:    09/14/2004 9:14 AM
//     Updated:    05/06/2021 10:57 PM
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
