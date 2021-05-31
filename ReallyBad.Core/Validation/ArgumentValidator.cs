// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core
//     File:       ArgumentValidator.cs
// 
//     Created:    09/14/2004 9:14 AM
//     Updated:    05/06/2021 10:59 PM
// 

using System;
using System.Collections.Generic;
using System.Linq;

using ReallyBad.Core.Text;

#nullable enable

namespace ReallyBad.Core.Validation
{

    public class ArgumentValidator
    {

        private static readonly DateTime emptyDateTime = new();

        private static bool NotEmpty( object? item )
        {
            switch ( item )
            {
                case null:
                case string s when s.NullOrEmpty():
                case DateTime dateTime when dateTime == emptyDateTime:
                case Guid guid when guid == Guid.Empty:

                    return false;

                default:

                    return true;
            }
        }

        private static bool NotEmpty<T>( IEnumerable<T> enumerable ) => enumerable.Any();

        public static void ValidateNotEmpty<T>( IEnumerable<T> enumerable, string name )
        {
            if ( !NotEmpty( enumerable ) )
            {
                throw new ArgumentNullException( $"{name} is empty.", name );
            }
        }

        public static void ValidateNotEmpty( object? item, string name )
        {
            if ( !NotEmpty( item ) )
            {
                throw new ArgumentNullException( $"{name} is empty.", name );
            }
        }

    }

}
