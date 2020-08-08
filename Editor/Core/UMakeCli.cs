using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMake
{
	public static class UMakeCli
	{
		public sealed class CliErrorException : System.Exception
		{
			public CliErrorException( string message ) : base( message )
			{
			}

			public CliErrorException() : base()
			{
			}

			public CliErrorException( SerializationInfo info, StreamingContext context ) : base( info, context )
			{
			}

			public CliErrorException( string message, System.Exception innerException ) : base( message, innerException )
			{
			}
		}

		public static bool IsInCli = false;
		public static readonly Dictionary<string, string> Args = new Dictionary<string, string>();

		private static void ExecuteOnTarget(TeamCityParameters tcParameters, Action<UMake, UMakeTarget> callback)
		{
			if( callback == null )
				return;

			string targetName = tcParameters.UMakeTarget;

			UMakeTarget target;
			if( !UMake.GetTarget( targetName ).TryGet( out target ) )
				throw new CliErrorException( string.Format( "Could not find target '{0}'.", targetName ) );

			UMake umake;
			if( UMake.Get().TryGet( out umake ) )
				callback( umake, target );
		}

		public static void Build()
		{
			var tcParameters = new TeamCityParameters();
			IsInCli = true;
			
			ExecuteOnTarget( tcParameters, ( umake, target ) =>
			{
				string buildPath = tcParameters.UMakeBuildPath;
				Debug.LogFormat( "\n\nBuilding for target: '{0}' at '{1}'.\n\n", target.name, buildPath );

				target.Build(umake, buildPath);
				target.ExecutePostBuildActions(umake);
			} );
		}
	}
}