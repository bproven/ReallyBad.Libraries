using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable enable

namespace ReallyBad.IO
{

	public class FileSystemInfoEnumerable<TInterface, TType> : IEnumerable<TInterface>
		where TInterface : class, IFileSystemInfo
		where TType : FileSystemInfo
	{

		private readonly IEnumerable<TType> _enumerable;

		public FileSystemInfoEnumerable( IEnumerable<TType> enumerable ) => _enumerable = enumerable;

		public IEnumerator<TInterface> GetEnumerator()
			=> new FileSystemInfoEnumerator<TInterface,TType>( _enumerable.GetEnumerator() );

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	}

}
