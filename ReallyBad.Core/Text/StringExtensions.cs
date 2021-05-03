// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core
//     File:       StringExtensions.cs
// 
//     Created:    04/29/2020 5:05 PM
//     Updated:    05/02/2021 7:41 PM
// 

using System;
using System.ComponentModel;

using Microsoft.Extensions.Logging;

using ReallyBad.Core.Conversion;
using ReallyBad.Core.Logging;

#nullable enable

namespace ReallyBad.Core.Text
{

	public static class StringExtensions
	{

		private static readonly string name = typeof( StringExtensions ).FullName ?? string.Empty;

		private static ILogger? _log;

		private static ILogger Log => _log ??= Logger.CreateLogger( name );

		public static string TrimStart( this string s, string trim )
		{
			var result = s;

			if ( result.StartsWith( trim ) )
			{
				result = result.Substring( trim.Length );
			}

			return result;
		}

		public static string TrimEnd( this string s, string trim )
		{
			var result = s;

			if ( result.EndsWith( trim ) )
			{
				result = result.Substring( 0, result.Length - trim.Length );
			}

			return result;
		}

		private static TypeConverter GetConverter( Type type ) => type == typeof( byte[] )
			? new ByteArrayTypeConverter()
			: TypeDescriptor.GetConverter( type );

		public static object? GetValue( this string stringValue, Type type, bool isIgnoreErrors = false )
			=> stringValue.GetValue( type, GetDefaultValue( type ), isIgnoreErrors );

		public static object? GetValue( this string stringValue, Type type, object? def,
			bool isIgnoreErrors = false )
		{
			var result = def;

			if ( string.IsNullOrEmpty( stringValue ) )
			{
				return result;
			}

			try
			{
				var converter = GetConverter( type );
				result = converter.ConvertFrom( stringValue );
			}
			catch ( NotSupportedException ex )
			{
				Log.LogError( $"Can not convert string '{stringValue}' to {type.Name}", ex );

				if ( !isIgnoreErrors )
				{
					throw;
				}
			}
			catch ( ArgumentException ex )
			{
				Log.LogError( ex.InnerException is FormatException
						? $"Invalid format attempting to convert string '{stringValue}' to {type.Name}, {ex.Message}"
						: $"Invalid argument {stringValue}, {ex.Message}",
					ex );

				if ( !isIgnoreErrors )
				{
					throw;
				}
			}
			catch ( Exception ex )
			{
				Log.LogError( ex.Message, ex );

				if ( !isIgnoreErrors )
				{
					throw;
				}
			}

			return result;
		}

		private static object? GetDefaultValue( Type type )
		{
			var def = type.IsValueType ? Activator.CreateInstance( type ) : null;

			if ( type.IsArray )
			{
				def = Activator.CreateInstance( type, 0 ); // empty array
			}

			return def;
		}

		public static T? GetDefaultValue<T>() => (T?)GetDefaultValue( typeof( T ) );

		public static T? GetValue<T>( this string stringValue, T? defaultValue, bool isIgnoreErrors = false )
		{
			var value = stringValue.GetValue( typeof( T ), defaultValue, isIgnoreErrors );

			return value != null ? (T?)value : defaultValue;
		}

		public static T? GetValue<T>( this string stringValue, bool isIgnoreErrors = false )
			=> stringValue.GetValue( GetDefaultValue<T>(), isIgnoreErrors );

		public static bool NullOrEmpty( this string? s ) => string.IsNullOrEmpty( s );

		public static string Safe( this string? s, string? def = null )
		{
			var result = s;

			if ( !string.IsNullOrEmpty( result ) )
			{
				return result;
			}

			if ( string.IsNullOrEmpty( def ) )
			{
				def = string.Empty;
			}

			result = def;

			return result;
		}

	}

}
