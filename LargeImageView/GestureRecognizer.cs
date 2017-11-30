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
using Android.Support.V4.View;
namespace LargeImageView
{
    class GestureRecognizer : Java.Lang.Object, GestureDetector.IOnGestureListener,
        GestureDetector.IOnDoubleTapListener, ScaleGestureDetector.IOnScaleGestureListener
    {
        private readonly GestureDetectorCompat mGestureDetector;
        private readonly ScaleGestureDetector mScaleDetector;
        private readonly IListener mListener;

        private bool mScale;

        public GestureRecognizer(Context context, IListener listener)
        {
            mListener = listener;
            Handler handler = new Handler(Looper.MainLooper);
            mGestureDetector = new GestureDetectorCompat(context, this, handler);
            mGestureDetector.SetOnDoubleTapListener(this);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                mScaleDetector = new ScaleGestureDetector(context, this, handler);
            }
            else
            {
                mScaleDetector = new ScaleGestureDetector(context, this);
            }
        }

        public void OnTouchEvent(MotionEvent ev)
        {
            // If pointer count is more than 1, must be scale action
            switch (ev.ActionMasked)
            {
                case MotionEventActions.Up:
                    mListener.OnUp();
                    break;
                case MotionEventActions.Cancel:
                    mListener.OnCancel();
                    break;
                case MotionEventActions.PointerDown:
                    mScale = ev.PointerCount > 1;
                    break;
                case MotionEventActions.PointerUp:
                    mScale = (ev.PointerCount - 1) > 1;
                    break;
            }

            mGestureDetector.OnTouchEvent(ev);
            mScaleDetector.OnTouchEvent(ev);
        }


        public bool OnDoubleTap(MotionEvent e)
        {
            return true;
        }

        public bool OnDoubleTapEvent(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Up)
            {
                mListener.OnDoubleTap(e.RawX, e.RawY);
            }
            return true;
        }

        public bool OnDown(MotionEvent e)
        {
            mScale = false;
            mListener.OnDown();
            return true;
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            if (!mScale)
            {
                mListener.OnFling(e2.RawX, e2.RawY, velocityX, velocityY);
            }
            return true;
        }

        public void OnLongPress(MotionEvent e)
        {
            if (!mScale)
            {
                mListener.OnLongPress(e.RawX, e.RawY);
            }
        }

        public bool OnScale(ScaleGestureDetector detector)
        {
            mScale = true;
            mListener.OnScale(detector.FocusX,
                    detector.FocusY, detector.ScaleFactor);
            return true;
        }

        public bool OnScaleBegin(ScaleGestureDetector detector)
        {
            mScale = true;
            return true;
        }

        public void OnScaleEnd(ScaleGestureDetector detector)
        {

        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            if (!mScale)
            {
                mListener.OnScroll(e2.RawX, e2.RawY, distanceX, distanceY,
                        e2.RawX - e1.RawX, e2.RawY - e1.RawY);
            }
            return true;
        }

        public void OnShowPress(MotionEvent e)
        {
        }

        public bool OnSingleTapConfirmed(MotionEvent e)
        {
            mListener.OnSingleTap(e.RawX, e.RawY);
            return true;
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            return true;
        }


        public interface IListener
        {
            void OnDown();
            void OnUp();
            void OnCancel();
            void OnSingleTap(float x, float y);
            void OnDoubleTap(float x, float y);
            void OnLongPress(float x, float y);
            void OnScroll(float x, float y, float dx, float dy, float totalX, float totalY);
            void OnFling(float x, float y, float velocityX, float velocityY);
            void OnScale(float x, float y, float scale);
        }

    }
}