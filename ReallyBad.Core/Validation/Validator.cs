// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core
//     File:       Validator.cs
// 
//     Created:    05/02/2021 8:52 PM
//     Updated:    05/02/2021 9:17 PM
// 

using System;
using System.Collections.Generic;
using System.Linq;

using ReallyBad.Core.Text;

#nullable enable

namespace ReallyBad.Core.Validation
{

	public class Validator
	{

		private static readonly DateTime emptyDateTime = new();

		private static bool IsNotEmpty( object? item )
		{
			switch ( item )
			{
				case null:
				case string s when s.IsNullOrEmpty():
				case DateTime dateTime when dateTime == emptyDateTime:
				case Guid guid when guid == Guid.Empty:

					return false;

				default:

					return true;
			}
		}

		private static bool IsNotEmpty<T>( IEnumerable<T> enumerable ) => enumerable.Any();

		public static void ValidateNotEmpty<T>( IEnumerable<T> enumerable, string name )
		{
			if ( !IsNotEmpty( enumerable ) )
			{
				throw new ArgumentNullException( $"{name} is empty.", name );
			}
		}

		public static void ValidateNotEmpty( object? item, string name )
		{
			if ( !IsNotEmpty( item ) )
			{
				throw new ArgumentNullException( $"{name} is empty.", name );
			}
		}

	}

}
