// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core
//     File:       ObjectExtensions.cs
// 
//     Created:    05/02/2021 6:45 PM
//     Updated:    05/02/2021 6:53 PM
// 

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

using ReallyBad.Core.Logging;
using ReallyBad.Core.Validation;

#nullable enable

namespace ReallyBad.Core.Reflection
{

	public static class ObjectExtensions
	{

		private static readonly string name = typeof( ObjectExtensions ).FullName ?? string.Empty;

		private static ILogger? _log;

		private static ILogger Log => _log ??= Logger.CreateLogger( name );

		public static object? GetPropertyValueObject( this object item, string propertyName,
			object? defaultValue )
		{
			Validator.ValidateNotEmpty( propertyName, nameof( propertyName ) );
			return item.GetType().GetProperty( propertyName )?.GetValue( item ) ?? defaultValue;
		}

		public static T? GetPropertyValue<T>( this object item, string propertyName, T? defaultValue )
		{
			Validator.ValidateNotEmpty( propertyName, nameof( propertyName ) );
			return (T?)item.GetPropertyValueObject( propertyName, defaultValue );
		}

		public static T? GetPropertyValue<T>( this object item, string propertyName )
		{
			Validator.ValidateNotEmpty( propertyName, nameof( propertyName ) );
			return item.GetPropertyValue( propertyName, default( T ) );
		}

		public static bool SetPropertyValue( this object item, string propertyName, object? value,
			bool throwIfNotFound = true )
		{
			Validator.ValidateNotEmpty( propertyName, nameof( propertyName ) );
			var propertyInfo = item.GetType().GetProperty( propertyName );
			var result = false;

			if ( propertyInfo is not null )
			{
				result = true;

				try
				{
					propertyInfo.SetValue( item, value );
				}
				catch ( ArgumentException ex )
				{
					Log.LogError( ex.Message, ex );

					if ( throwIfNotFound )
					{
						throw;
					}
				}
			}
			else if ( throwIfNotFound )
			{
				throw new MissingMemberException( $"Property {propertyName} not found." );
			}

			return result;
		}

		public static object? GetPropertyValueByAssignableType( this object item, Type type )
			=> item.GetType()
				.GetProperties()
				.SingleOrDefault( pi => type.IsAssignableFrom( pi.PropertyType ) )
				?.GetValue( item );

		public static T? GetPropertyValueByAssignableType<T>( this object item )
			=> (T?)item.GetPropertyValueByAssignableType( typeof( T ) );

		public static object? GetGenericPropertyValueByType( this object item, Type genericType,
			ICollection<Type> types )
			=> item.GetType()
				.GetProperties()
				.SingleOrDefault( pi => pi.PropertyType.IsGenericType &&
				                        pi.PropertyType.GetGenericTypeDefinition() == genericType &&
				                        pi.PropertyType.GenericTypeArguments.Length == types.Count &&
				                        pi.PropertyType.GenericTypeArguments.SequenceEqual( types ) )
				?.GetValue( item );

		public static object? GetGenericPropertyValueByType( this object item, Type genericType, params Type[] types )
		{
			Validator.ValidateNotEmpty( types, nameof( types ) );
			return item.GetGenericPropertyValueByType( genericType, (ICollection<Type>)types );
		}

		public static TGenericType? GetGenericPropertyValueByType<TGenericType>( this object item )
			=> (TGenericType?)item.GetGenericPropertyValueByType( typeof( TGenericType ).GetGenericTypeDefinition(),
				typeof( TGenericType ).GetGenericArguments() );

	}

}
