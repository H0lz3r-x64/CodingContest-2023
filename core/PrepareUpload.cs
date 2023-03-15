using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using com.knapp.CodingContest.solution;

namespace com.knapp.CodingContest.core
{
    /// <summary>
    /// Helper class to create zip for upload
    /// </summary>
    public static class PrepareUpload
    {
        /// <summary>
        /// **KNAPP use only
        /// Create uploadable zip file
        /// 
        /// </summary>
        public static void CreateZipFile()
        {
            string resultsFile = Path.Combine( Settings.OutputPath, Settings.outResultFilename );
            string propertiesFile = Path.Combine( Settings.OutputPath, Settings.outPropertyFilename );
            string zipFile = Path.Combine( Settings.OutputPath, Settings.outZipFilename );

            if ( File.Exists( zipFile ) )
            {
                File.Delete( zipFile );
            }

            using ( ZipArchive archive = ZipFile.Open( zipFile, ZipArchiveMode.Create ) )
            {
                archive.CreateEntryFromFile( resultsFile, Settings.outResultFilename );

                archive.CreateEntryFromFile( propertiesFile, Settings.outPropertyFilename );

                AddSourceDirectoryToZipFile( Settings.zipSourceDirectory, archive, Settings.sourceDirectory );
            }
        }
        /// <summary>
        /// ** KNAPP use only
        /// Add a source folder to the upload zip
        /// bin and obj folders are ignored
        /// </summary>
        /// <param name="directoryPrefix">prefix of the directory</param>
        /// <param name="archive">zip archive to add the contents of the directory to</param>
        /// <param name="pathName">path to add to the zip file</param>
        private static void AddSourceDirectoryToZipFile( string directoryPrefix, ZipArchive archive, string pathName )
        {
            foreach ( var file in Directory.GetFiles( pathName ) )
            {
                string zipName = file.Substring( Settings.sourceDirectory.Length );

                archive.CreateEntryFromFile( file, directoryPrefix + zipName );
            }

            foreach ( var subDir in Directory.GetDirectories( pathName ) )
            {
                if ( !subDir.EndsWith( "bin" )
                    && !subDir.EndsWith( "obj" ) 
                    && !subDir.EndsWith(".vs"))
                {
                    AddSourceDirectoryToZipFile( directoryPrefix, archive, subDir );
                }
            }
        }

        /// <summary>
        /// ** KNAPP use only
        /// Write the created results to the export file
        /// </summary>
        /// <param name="operations">enumerable with operations created during optimization</param>
        internal static void WriteResult( Solution solution, MyOperations operations )
        {
            using ( StreamWriter writer = new StreamWriter( Path.Combine( Settings.OutputPath, Settings.outResultFilename ) ) )
            {
                foreach ( var operation in operations.GetResults() )
                {
                    writer.WriteLine( operation.ToResultString() );
                }
            }
        }
    }
}
