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

using Java.IO;
using System.IO;

namespace LargeImageView.StreamPipe
{
    public interface IOutputStreamPipe
    {
        /**
     * Obtain the {@code OutputStreamPipe}.
     * Call it before opening {@code OutputStream}.
     */
        void Obtain();

        /**
         * Release the {@code OutputStreamPipe}.
         */
        void Release();

        /**
         * Open {@code OutputStream}
         *
         * @return the {@code OutputStream}
         * @throws IOException
         */
        Stream Open();

        /**
         * Close the {@code OutputStream} opened before.
         */
        void Close();
    }
}