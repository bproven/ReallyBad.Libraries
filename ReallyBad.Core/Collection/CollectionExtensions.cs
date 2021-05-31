// 
//     Created By: Bob Provencher
//     User Name:  bob
// 
//     Solution:   ReallyBad.Libraries
//     Project:    ReallyBad.Core
//     File:       CollectionExtensions.cs
// 
//     Created:    09/14/2004 9:14 AM
//     Updated:    05/06/2021 10:57 PM
// 

using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace ReallyBad.Core.Collection
{

    public static class CollectionExtensions
    {

        public static void AddCollection<T>( this ICollection<T> collection, IEnumerable<T> otherCollection )
        {
            foreach ( var item in otherCollection )
            {
                collection.Add( item );
            }
        }

        /// <summary>
        /// Removes all items from a collection
        /// </summary>
        /// <typeparam name="T">The type of the collection</typeparam>
        /// <param name="collection">The collection to remove items from</param>
        public static void RemoveAll<T>( this ICollection<T> collection )
        {
            foreach ( var item in collection.ToList() )
            {
                collection.Remove( item );
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>( this ICollection<T> collection ) => collection.Count == 0;

    }

}
