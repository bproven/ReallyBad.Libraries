// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core
//     File:       EnumExtensions.cs
// 
//     Created:    05/01/2021 11:56 PM
//     Updated:    05/02/2021 9:36 PM
// 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

using ReallyBad.Core.Validation;

#nullable enable

namespace ReallyBad.Core.Reflection
{

	public static class EnumExtensions
	{

		public static string GetDescription( this Enum value )
		{
			var enumValue = value.ToString();

			var fi = value.GetType().GetField( enumValue );

			if ( fi is null )
			{
				return enumValue;
			}

			var attributes = (DescriptionAttribute[])fi.GetCustomAttributes( typeof( DescriptionAttribute ), false );

			return attributes.Length > 0 ? attributes[ 0 ].Description : value.ToString();
		}

		/// <summary>
		/// Get the EnumMember.Value for an enum
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumValue( this Enum value )
		{
			var enumValue = value.ToString();

			var fi = value.GetType().GetField( enumValue );

			if ( fi is null )
			{
				return enumValue;
			}

			var attributes = (EnumMemberAttribute[])fi.GetCustomAttributes( typeof( EnumMemberAttribute ), false );

			if ( !attributes.Any() )
			{
				return value.ToString();
			}

			var enumMemberAttribute = attributes.First();

			return enumMemberAttribute.Value ?? string.Empty;
		}

		/// <summary>
		/// Get the enum with the corresponding EnumMember.Value
		/// </summary>
		/// <param name="enumMemberValue"></param>
		/// <returns></returns>
		public static T FromValue<T>( this string enumMemberValue )
			where T : struct, Enum, IConvertible
		{
			ArgumentValidator.ValidateNotEmpty( enumMemberValue, nameof( enumMemberValue ) );

			var t = typeof( T );
			var enumValues = t.GetEnumValues();

			IList<T> enums = enumValues.Cast<T>().ToList();

			// try to match EnumMember.Value
			foreach ( var e in enums )
			{
				if ( e.GetEnumValue() == enumMemberValue )
				{
					return e;
				}
			}

			// try to match Value
			foreach ( var e in enums )
			{
				if ( t.GetEnumName( e ) == enumMemberValue )
				{
					return e;
				}
			}

			// match number
			if ( !int.TryParse( enumMemberValue, out var value ) )
			{
				throw new ArgumentException( $"{enumMemberValue} is not a valid value for {t.Name}" );
			}


			var underlyingType = t.GetEnumUnderlyingType();

			foreach ( var e in enums )
			{
				if ( e.ToType( underlyingType, null ).Equals( value ) )
				{
					return e;
				}
			}

			throw new ArgumentException( $"{enumMemberValue} is not a valid value for {t.Name}" );
		}

	}

}
