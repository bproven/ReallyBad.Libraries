// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core
//     File:       AssemblyExtensions.cs
// 
//     Created:    05/01/2021 11:53 PM
//     Updated:    05/02/2021 9:22 PM
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using ReallyBad.Core.Text;
using ReallyBad.Core.Validation;

#nullable enable

namespace ReallyBad.Core.Reflection
{

	public static class AssemblyExtensions
	{

		private const string NamespaceSeparator = ".";

		private static string GetNamespaceName( Assembly assembly, string nameSpace )
			=> $"{assembly.GetName().Name}.{nameSpace}";

		private static string GetResourceName( Assembly assembly, string nameSpace, string resourceName )
		{
			var separator = nameSpace.NullOrEmpty() ? string.Empty : NamespaceSeparator;
			var namespaceName = GetNamespaceName( assembly, nameSpace );

			return$"{namespaceName}{separator}{resourceName}";
		}

		public static string GetResource( this Assembly assembly, string nameSpace, string resourceName )
		{
			Validator.ValidateNotEmpty( resourceName, nameof( resourceName ) );

			return assembly.GetResource( GetResourceName( assembly, nameSpace, resourceName ) );
		}

		public static string GetResource( this Assembly assembly, string resourceFullName )
		{
			using var stream = assembly.GetManifestResourceStream( resourceFullName );

			if ( stream == null )
			{
				throw new FileNotFoundException( $"Could not find embedded resource: {resourceFullName}" );
			}

			using var reader = new StreamReader( stream );
			var resource = reader.ReadToEnd();

			return resource;
		}

		public static IEnumerable<string> GetResourcesWithExtensions( this Assembly assembly,
			IEnumerable<string> extensions, string? nameSpace = null )
		{
			static bool CheckExtension( string fileName, IEnumerable<string>? extensions )
			{
				Validator.ValidateNotEmpty( fileName, nameof( fileName ) );

				var extensionList = extensions?.ToList() ?? new List<string>();
				var count = extensionList.Count;

				if ( count <= 0 )
				{
					return true;
				}

				var fileInfo = new FileInfo( fileName );
				var result = extensionList.Contains( fileInfo.Extension );

				return result;
			}

			return GetResources( assembly, nameSpace, s => CheckExtension( s, extensions ) );
		}

		public static IEnumerable<string> GetResources( this Assembly assembly, string? nameSpace = null,
			Func<string, bool>? predicate = null )
		{
			var searchNameSpace = string.IsNullOrEmpty( nameSpace ) ? string.Empty : nameSpace;
			searchNameSpace = GetNamespaceName( assembly, searchNameSpace );

			static bool DefaultPredicate( string s ) => true;

			var defaultPredicate = predicate ?? DefaultPredicate;

			return assembly
				.GetManifestResourceNames()
				.Where( n =>
				{
					var result = n.StartsWith( searchNameSpace );

					if ( !result )
					{
						return false;
					}

					var s = n.TrimStart( searchNameSpace );
					s = s.TrimStart( NamespaceSeparator );
					result = defaultPredicate( s );

					return result;
				} )
				.Select( f =>
				{
					var result = f.TrimStart( searchNameSpace );
					result = result.TrimStart( NamespaceSeparator );

					return result;
				} );
		}

	}

}
