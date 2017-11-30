using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Graphics;
using Android.Widget;
using Java.Lang;

namespace LargeImageView
{
    public abstract class WrapperSource : ImageSource, ImageSource.ICallback
    {
        private ImageSource mBase;
        private int mWindowWidth;
        private int mWindowHeight;
        private int mMaxBitmapSize;

        /**
     * Set base ImageSource.
     * Only call it once!
     */
        protected ImageSource ImageSource
        {
            set
            {
                if (mBase != null)
                {
                    throw new IllegalStateException("Can't set ImageSource twice");
                }
                mBase = value;
                base.SetCallback(this);
                base.SetVisible(IsVisible());
                SetWindowSize(mWindowWidth, mWindowHeight);
                SetMaxBitmapSize(mMaxBitmapSize);
                // Ready
                CallSelfReady();
            }
        }

        public override bool SetVisible(bool visible)
        {
            if (mBase != null)
            {
                return mBase.SetVisible(visible);
            }
            else
            {
                return base.SetVisible(visible);
            }
        }

        public override bool IsVisible()
        {
            if (mBase != null)
            {
                return mBase.IsVisible();
            }
            else
            {
                return  base.IsVisible();
            }
        }



        public override bool Ready => mBase != null;

        public override int Width
        {
            get
            {
                if (mBase != null)
                {
                    return mBase.Width;
                }
                else
                {
                    return 0;
                }
            }
        }

        public override int Height
        {
            get
            {
                if (mBase != null)
                {
                    return mBase.Height;
                }
                else
                {
                    return 0;
                }
            }
        }

        public override void Draw(Canvas canvas, RectF src, RectF dst)
        {
            if (mBase != null)
            {
                mBase.Draw(canvas, src, dst);
            }
        }

        public override void Initialize()
        {
            OnInitialize();
        }

        protected abstract void OnInitialize();

        public void InvalidateImage(ImageSource who)
        {
            InvalidateSelf();
        }

        public void OnImageFailed(ImageSource who)
        {
            CallSelfFailed();
        }

        public void OnImageReady(ImageSource who)
        {
            CallSelfReady();
        }

        public override void Recycle()
        {
            if (mBase != null)
            {
                mBase.Recycle();
            }
        }

        public void ScheduleImage(ImageSource who, Runnable what, long when)
        {
            ScheduleSelf(what, when);
        }

        public override void SetMaxBitmapSize(int maxSize)
        {
            if (mBase != null)
            {
                mBase.SetMaxBitmapSize(maxSize);
            }
            else
            {
                mMaxBitmapSize = maxSize;
            }
        }

        public override void SetWindowSize(int w, int h)
        {
            if (mBase != null)
            {
                mBase.SetWindowSize(w, h);
            }
            else
            {
                mWindowWidth = w;
                mWindowHeight = h;
            }
        }

        public void UnscheduleImage(ImageSource who, Runnable what)
        {
            UnscheduleSelf(what);
        }

        public override void OnAnimatorStart()
        {
            if (mBase != null)
            {
                mBase.OnAnimatorStart();
            }
        }

        public override void OnAnimatorEnd()
        {
            if (mBase != null)
            {
                mBase.OnAnimatorEnd();
            }
        }

        public override void OnTouchStart()
        {
            if (mBase != null)
            {
                mBase.OnTouchStart();
            }
        }

        public override void OnTouchEnd()
        {
            if (mBase != null)
            {
                mBase.OnTouchEnd();
            }
        }
    }
}