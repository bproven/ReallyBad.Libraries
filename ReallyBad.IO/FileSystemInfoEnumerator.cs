using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable enable

namespace ReallyBad.IO
{

	public class FileSystemInfoEnumerator<TInterface,TType> : IEnumerator<TInterface>
		where TInterface : class, IFileSystemInfo
		where TType : FileSystemInfo
	{

		private readonly IEnumerator<TType> _enumerator;

		public FileSystemInfoEnumerator( IEnumerator<TType> enumerator ) 
			=> _enumerator = enumerator;

		private TInterface? _current;

		public TInterface Current => _current ??= (TInterface)FileSystemInfoWrapper.Create( _enumerator.Current );

		object IEnumerator.Current => Current;

		public bool MoveNext()
		{
			var result = _enumerator.MoveNext();
			_current = null;
			return result;
		}

		public void Reset()
		{
			_enumerator.Reset();
			_current = null;
		}

		private bool disposedValue;

		protected virtual void Dispose( bool disposing )
		{
			if ( disposedValue )
			{
				return;
			}

			if ( disposing )
			{
				_enumerator.Dispose();
			}
			disposedValue = true;
		}

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

	}

}
