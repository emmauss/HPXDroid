using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Java.Lang;

namespace LargeImageView
{
    public abstract class RegionDecoder
    {
        public Bitmap DecodeRegion(Rect rect, int sample)
        {
            try
            {
                return DecodeRegionInternal(rect, sample);
            }
            catch (Throwable t)
            {
                return null;
            }
        }

        public abstract int Width {

            get;
        }

        public abstract int Height
        {
            get;
        }

        
    protected abstract Bitmap DecodeRegionInternal(Rect rect, int sample);

        public abstract void Recycle();
    }
}