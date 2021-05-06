// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core
//     File:       BinaryData.cs
// 
//     Created:    04/29/2020 5:05 PM
//     Updated:    05/02/2021 1:38 AM
// 

using System;
using System.Text;

using ReallyBad.Core.Validation;

#nullable enable

namespace ReallyBad.Core.Binary
{

	public class BinaryData
	{

		public BinaryData( byte[] data ) => Data = data;

		public BinaryData( int size ) => Data = new byte[ size ];

		public BinaryData( string text, Encoding encoding )
		{
			ArgumentValidator.ValidateNotEmpty( text, nameof( text ) );
			Encoding = encoding;
			Data = Encoding.GetBytes( text );
		}

		public BinaryData( string text )
		{
			ArgumentValidator.ValidateNotEmpty( text, nameof( text ) );
			Data = Convert.FromBase64String( text );
		}

		/// <summary>
		///     The actual binary data
		/// </summary>
		public byte[] Data { get; init; }

		/// <summary>
		///     Number of bits in the Data
		/// </summary>
		public int NumberOfBits => Data.Length * 8;

		/// <summary>
		///     The encoding to use for encoding/decoding
		/// </summary>
		public Encoding Encoding { get; set; } = Encoding.UTF8;

		/// <summary>
		///     Text encoded data
		/// </summary>
		public string Text => Data.GetEncoded( Encoding );

		/// <summary>
		///     Base64 representation of the Data value
		/// </summary>
		public string Base64 => Data.GetBase64();

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public override string ToString() => Text;

		public override bool Equals( object? obj )
		{
			var result = obj switch
			{
				BinaryData other1 => Data.Equals( other1 ),
				byte[] other2 => Data.Equals( other2 ),
				_ => false,
			};

			return result;
		}

		public override int GetHashCode() => Data.GetHashCode();

		public bool Equals( byte[] other ) => Data.ValueEquals( other );

		public bool Equals( BinaryData other ) => Equals( other.Data );

		public static bool operator ==( BinaryData data, byte[] other ) => data.Data.ValueEquals( other );

		public static bool operator !=( BinaryData data, byte[] other ) => !( data == other );

	}

}
