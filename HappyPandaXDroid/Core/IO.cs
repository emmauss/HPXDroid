using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//using System.IO.Compression;

using ICSharpCode.SharpZipLib.GZip;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HappyPandaXDroid.Core
{
    public class IO
    {
        public static class Parcel
        {
            private static Stack<object> parcels;

            static Parcel()
            {
                parcels = new Stack<object>();
            }

            public static void PushParcel(object parcel)
            {
                lock (parcels)
                {
                    parcels.Push(parcel);
                }
            }

            public static object PopParcel()
            {
                lock (parcels)
                {
                    return parcels.Pop();
                }
            }

        }

        public class Compression
        {
            public static byte[] Compress(byte[] source)
            {
                if (source != null & source.Length > 0)
                {
                    byte[] result = null;
                    using(MemoryStream memcopy = new MemoryStream())
                    {
                        using (GZipOutputStream gzip = new GZipOutputStream(memcopy))
                        {
                            gzip.Write(source, 0, source.Length);
                        }
                        result =  memcopy.ToArray();
                    }

                    return result;
                }
                else
                    return new byte[1];
            }


            public static byte[] Decompress(byte[] source)
            {
                try
                {
                    if (source != null && source.Length > 0)
                    {
                        var decompressedstream = new MemoryStream();
                        using (MemoryStream compressedstream = new MemoryStream(source))
                        {
                            using (GZipInputStream gzip = new GZipInputStream(compressedstream))
                            {
                                gzip.CopyTo(decompressedstream);
                            }

                        }

                        return decompressedstream.ToArray();
                    }
                    else
                        return new byte[1];
                }
                catch(Exception ex)
                {
                    return new byte[1];
                }
            }
        }
    }
}