// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core
//     File:       StringTypeConverter.cs
// 
//     Created:    05/02/2021 6:20 PM
//     Updated:    05/02/2021 6:44 PM
// 

using System;
using System.ComponentModel;

using Microsoft.Extensions.Logging;

using ReallyBad.Core.Logging;
using ReallyBad.Core.Text;

#nullable enable

namespace ReallyBad.Core.Conversion
{

	public class StringTypeConverter
	{

		private readonly ILogger<StringTypeConverter> log;

		public StringTypeConverter( ILogger<StringTypeConverter> logger ) => log = logger;

		/// <summary>
		/// True to return the default value on exception.
		/// </summary>
		public bool IsIgnoreErrors { get; set; } = true;

		/// <summary>
		/// Convert the input string to the desired type and return it,
		/// </summary>
		/// <typeparam name="T">The type to convert the string to.</typeparam>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value if the input is empty, or there is an error converting the string.</param>
		/// <param name="isIgnoreErrors">true to ignore conversion exceptions and return the defaultValue.</param>
		/// <returns>The converted value or the defaultValue if there was an error.</returns>
		public T? ConvertTo<T>( string value, T? defaultValue, bool isIgnoreErrors )
		{
			var result = defaultValue;

			if ( value.NullOrEmpty() )
			{
				return result;
			}

			try
			{
				result = (T)TypeDescriptor.GetConverter( typeof( T ) ).ConvertFrom( value );
			}
			catch ( NotSupportedException ex )
			{
				log.LogError( $"Error: can not convert string '{value}' to {typeof( T ).Name}", ex );

				if ( !isIgnoreErrors )
				{
					throw;
				}
			}
			catch ( FormatException ex )
			{
				log.LogError( $"Error: invalid format attempting to convert string '{value}' to {typeof( T ).Name}", ex );

				if ( !isIgnoreErrors )
				{
					throw;
				}
			}
			catch ( Exception ex )
			{
				log.LogError( $"Error: {ex.Message}", ex );

				if ( !isIgnoreErrors )
				{
					throw;
				}
			}

			return result;
		}

		/// <summary>
		/// Convert the input string to the desired type and return it,
		/// </summary>
		/// <typeparam name="T">The type to convert the string to.</typeparam>
		/// <param name="value">The value to convert.</param>
		/// <param name="defaultValue">The default value if the input is empty, or there is an error converting the string.</param>
		/// <returns>The converted value or the defaultValue if there was an error.</returns>
		public T? ConvertTo<T>( string value, T? defaultValue )
			=> ConvertTo( value, defaultValue, IsIgnoreErrors );

		/// <summary>
		/// Convert the input string to the desired type and return it,
		/// </summary>
		/// <typeparam name="T">The type to convert the string to.</typeparam>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value or the default value for the type T if there was an error.</returns>
		public T? ConvertTo<T>( string value ) => ConvertTo<T>( value, default );

	}

}
