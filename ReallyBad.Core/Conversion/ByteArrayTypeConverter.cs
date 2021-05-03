// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core
//     File:       ByteArrayTypeConverter.cs
// 
//     Created:    05/01/2021 11:46 PM
//     Updated:    05/02/2021 7:45 PM
// 

using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

using ReallyBad.Core.Text;

#nullable enable

namespace ReallyBad.Core.Conversion
{

	public class ByteArrayTypeConverter : ByteConverter
	{

		public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType )
			=> sourceType == typeof( string ) || base.CanConvertFrom( context, sourceType );

		public override object ConvertFrom( ITypeDescriptorContext context, CultureInfo culture, object? value )
		{
			return value switch
			{
				null => Array.Empty<byte>(),
				string stringValue => stringValue.NullOrEmpty()
					? Array.Empty<byte>()
					: Encoding.UTF8.GetBytes( stringValue ),
				_ => base.ConvertFrom( context, culture, value )!,
			};
		}

		public override object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, object? value,
			Type destinationType )
		{
			if ( destinationType != typeof( string ) )
			{
				return base.ConvertTo( context, culture, value, destinationType )!;
			}

			return value switch
			{
				null => string.Empty,
				byte[] array => Encoding.UTF8.GetString( array, 0, array.Length ),
				_ => base.ConvertTo( context, culture, value, destinationType )!,
			};
		}

	}

}
