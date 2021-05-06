using Microsoft.Extensions.Logging;

using ReallyBad.Core.Validation;

#nullable enable

namespace ReallyBad.Core.Logging
{

	public static class Logger
	{

		public static ILogger CreateLogger( string name, LogLevel level = LogLevel.Debug )
		{
			ArgumentValidator.ValidateNotEmpty( name, nameof( name ) );
			using var loggerFactory = LoggerFactory.Create( builder =>
			{
				builder
					.AddFilter( "Microsoft", LogLevel.Warning )
					.AddFilter( "System", LogLevel.Warning )
					.AddFilter( name, level );
#if DEBUG
				builder
					.AddDebug();
#endif
			} );
			return loggerFactory.CreateLogger( name );
		}

	}

}
