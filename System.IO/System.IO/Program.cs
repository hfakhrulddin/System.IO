using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace System.IO
{
    class Program
    {
        static void Main(string[] args)
        {
            Compress(string inFile, string outFile, bool compress)
        }
        
            public static void Compress(string inFile, string outFile, bool compress)
        {
            // error checking
            if (!File.Exists(inFile))
                throw new ArgumentException("inFile does not exist");
            if (File.Exists(outFile))
                throw new ArgumentException("outFile exists");

            // now with pleasantaries out, let's create our streams
            // input stream
            using (Stream inStream = File.Open(
                            inFile, // the file to open
                            FileMode.Open, // try to open an existing file
                            FileAccess.Read, // read only access
                            FileShare.None)) // lock the file till we are done
            {
                // output stream
                using (Stream outStream = File.Open(
                            outFile, // the file to write to
                            FileMode.Create, // create.overwrite
                            FileAccess.Write, // write access please
                            FileShare.None)) // lock it till we are done
                {
                    // now that we have opened both in/out streams
                    // let's wrap either one of them based on 
                    // compression or decompression
                    using (GZipStream gzipStream = new GZipStream(
                            // select the correct stream and compression type
                            compress ? outStream : inStream,
                            compress ? CompressionMode.Compress :
                                       CompressionMode.Decompress))
                    {
                        // now we have wrapped the correct stream
                        // for reading; do the same for writing
                        Stream readFrom = compress ? inStream : gzipStream;
                        Stream writeTo = compress ? gzipStream : outStream;

                        // since we are reading using base Stream class
                        // we will have to read using byte array
                        // we could have also wrapped this is a higher
                        // stream (Buffered, etc, but that would 
                        // complicate this example further

                        // we will use a buffer size of 16384
                        // as that maximizes the performance
                        byte[] buffer = new byte[16384];
                        int byteCount = 0;

                        // move data from in to out
                        do
                        {
                            // read
                            byteCount = readFrom.Read(buffer, 0, 16384);
                            // write
                            writeTo.Write(buffer, 0, byteCount);
                        } while (byteCount > 0);
                    }
                }
            }
        }
    }
}
