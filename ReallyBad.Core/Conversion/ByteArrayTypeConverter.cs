// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core
//     File:       ByteArrayTypeConverter.cs
// 
//     Created:    09/14/2004 9:14 AM
//     Updated:    05/06/2021 10:58 PM
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
