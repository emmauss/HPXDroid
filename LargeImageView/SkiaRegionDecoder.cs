using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Graphics;
using Android.Views;
using Android.Widget;

using Java.Lang;

namespace LargeImageView
{
    public class SkiaRegionDecoder : RegionDecoder
    {
        private BitmapRegionDecoder mDecoder;
        private readonly Bitmap.Config mConfig;

        public SkiaRegionDecoder(BitmapRegionDecoder decoder, Bitmap.Config config)
        {
            mDecoder = decoder;
            mConfig = config;
        }

        public override int Width => mDecoder.Width;

        public override int Height => mDecoder.Height;

        public override void Recycle()
        {
            if (mDecoder != null)
            {
                mDecoder.Recycle();
                mDecoder = null;
            }
        }

        protected override Bitmap DecodeRegionInternal(Rect rect, int sample)
        {
            BitmapRegionDecoder decoder = mDecoder;
            if (decoder != null)
            {
                BitmapFactory.Options options = new BitmapFactory.Options
                {
                    InSampleSize = sample,
                    InPreferredConfig = mConfig
                };
                return decoder.DecodeRegion(rect, options);
            }
            else
            {
                return null;
            }
        }
    }
}