using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace assinfo
{
    class Program
    {
        private static bool showTypes = false;

        static void Main(string[] args)
        {
            if ( !args.Any() )
            {
                RunPath( Environment.CurrentDirectory );

                return;
            }

            var path = args.First();

            if ( ( args.Length > 1 ) && args[1].Equals( "--show-types" ) )
            {
                showTypes = true;
            }

            if ( path.Equals( "." ) || Directory.Exists( path ) )
            {
                RunPath( Path.GetFullPath( path ) );

                return;
            }

            RunFile( path );
        }

        static void RunFile( string path )
        {
            var assemblyName = path;

            if ( !assemblyName.EndsWith( ".dll" ) )
            {
                assemblyName = string.Concat( assemblyName, ".dll" );
            }

            assemblyName = Path.GetFullPath( assemblyName );

            try
            {
                var assembly = Assembly.LoadFile( assemblyName );
                var name = assembly.GetName();

                Console.Out.WriteLine( name.Name );
                Console.Out.WriteLine( "  " + name.Version.ToString() );

                if ( showTypes )
                {
                    var types = assembly.GetTypes();

                    foreach ( var type in types )
                    {
                        Console.Out.WriteLine( "  " + type.FullName );
                    }
                }
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.Message );
            }
        }

        static void RunPath( string path )
        {
            var files = Directory.GetFiles( path, "*.dll" );

            if ( !files.Any() )
            {
                Console.Out.WriteLine( "no assemblies found" );
            }

            foreach ( var file in files )
            {
                RunFile( file );
            }
        }
    }
}
