using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Java.Lang;

namespace LargeImageView
{
    public class BitmapSource : ImageSource
    {
        private Bitmap mBitmap;
        private Paint mPaint;
        private Matrix mMatrix;
        private readonly bool mIsOwner;

    public BitmapSource(Bitmap bitmap) : this (bitmap, false)
        {
            
        }

        public BitmapSource(Bitmap bitmap, bool isOwner)
        {
            mBitmap = bitmap;
            mIsOwner = isOwner;
            mMatrix = new Matrix();
            mPaint = new Paint(PaintFlags.FilterBitmap | Android.Graphics.PaintFlags.Dither);
        }



        public override bool Ready => true;

        public override int Width => mBitmap.Width;

        public override int Height => mBitmap.Height;

        public override void Draw(Canvas canvas, RectF src, RectF dst)
        {
            if (mBitmap != null)
            {
                mMatrix.SetRectToRect(src, dst, Matrix.ScaleToFit.Fill);
                canvas.DrawBitmap(mBitmap, mMatrix, mPaint);
            }
        }

        public override void Initialize()
        {
        }

        public override void Recycle()
        {
            if (mBitmap != null)
            {
                if (mIsOwner)
                {
                    mBitmap.Recycle();
                }
                mBitmap = null;
                mMatrix = null;
                mPaint = null;
            }
        }

        public override void SetMaxBitmapSize(int maxSize)
        {
        }

        public override void SetWindowSize(int w, int h)
        {
        }
    }
}