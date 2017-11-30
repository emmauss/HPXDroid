using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.IO;
using Java.IO;

namespace LargeImageView.StreamPipe
{
    public interface IInputStreamPipe
    {
        /**
     * Obtain the {@code InputStreamPipe}.
     * Call it before opening {@code InputStream}.
     */
        void Obtain();

        /**
         * Release the {@code InputStreamPipe}.
         */
        void Release();

        /**
         * Open {@code InputStream}
         *
         * @return the {@code InputStream}
         * @throws IOException
         */
        Stream Open();

        /**
         * Close the {@code InputStream} opened before.
         */
        void Close();
    }
}