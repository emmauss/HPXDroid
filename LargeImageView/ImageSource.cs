using Android.Graphics;
using Java.Lang;
using System;

namespace LargeImageView
{
    public abstract class ImageSource : Java.Lang.Object
    {
        private WeakReference<ICallback> mCallback;
        private bool mVisible = true;



        public abstract void Initialize();

        public abstract bool Ready {
            get;
        }

        public void SetCallback(ICallback cb)
        {
            mCallback = cb != null ? new WeakReference<ICallback>(cb) : null;
        }

        public ICallback GetCallback()
        {
            ICallback back = null;
            if (mCallback != null)
            {
                mCallback.TryGetTarget(out back);
            }

            return back;
        }

        public void CallSelfReady()
        {
            ICallback callback = GetCallback();
            if (callback != null)
            {
                callback.OnImageReady(this);
            }
        }


        public void CallSelfFailed()
        {
            ICallback callback = GetCallback();
            if (callback != null)
            {
                callback.OnImageFailed(this);
            }
        }

        /**
     * Call {@code invalidateImage} of its callback.
     */
        public void InvalidateSelf()
        {
            ICallback callback = GetCallback();
            if (callback != null)
            {
                callback.InvalidateImage(this);
            }
        }

        /**
         * Call {@code scheduleImage} of its callback.
         */
        public void ScheduleSelf(Runnable what, long when)
        {
            ICallback callback = GetCallback();
            if (callback != null)
            {
                callback.ScheduleImage(this, what, when);
            }
        }

        /**
         * Call {@code unscheduleImage} of its callback.
         */
        public void UnscheduleSelf(Runnable what)
        {
            ICallback callback = GetCallback();
            if (callback != null)
            {
                callback.UnscheduleImage(this, what);
            }
        }

        /**
         * Set whether this ImageSource is visible.  This generally does not impact
         * the ImageSource's behavior, but is a hint that can be used by some
         * ImageSource, for example, to decide whether run animations.
         */
        public virtual bool SetVisible(bool visible)
        {
            bool changed = mVisible != visible;
            if (changed)
            {
                mVisible = visible;
                InvalidateSelf();
            }
            return changed;
        }

        public virtual bool IsVisible()
        {
            return mVisible;
        }

        /**
         * A window is the rect the {@code ImageSource}
         * to show in {@code Canvas}.
         */
        public abstract void SetWindowSize(int w, int h);

        /**
         * From {@link Canvas#getMaximumBitmapWidth()}
         * and {@link Canvas#getMaximumBitmapHeight()}.
         */
        public abstract void SetMaxBitmapSize(int maxSize);

        /**
         * Indicate a animator on this {@code ImageSource} start.
         */
        public virtual void OnAnimatorStart() { }

        /**
         * Indicate a animator on this {@code ImageSource} end.
         */
        public virtual void OnAnimatorEnd() { }

        /**
         * Indicate a finger touch the View.
         */
        public virtual void OnTouchStart() { }

        /**
         * Indicate a finger leave the View.
         */
        public virtual void OnTouchEnd() { }

        /**
         * Return the width of this {@code ImageSource}.
         * If not ready, do what you wanna do.
         */
        public abstract int Width {
            get;
        }

        /**
         * Return the height of this {@code ImageSource}.
         * If not ready, do what you wanna do.
         */
        public abstract int Height
        {
            get;
        }

        /**
         * Render this {@code ImageSource}.
         * If not ready, do what you wanna do.
         */
        public abstract void Draw(Canvas canvas, RectF src, RectF dst);

        /**
         * Recycle this {@code ImageSource}.
         */
        public abstract void Recycle();

        /**
         * Just like {@link android.graphics.drawable.Drawable.Callback}.
         */


        public interface ICallback
        {
            /**
         * Indicate this {@code ImageSource} init done.
         */
            void OnImageReady(ImageSource who);

            /**
             * Indicate this {@code ImageSource} init failed.
             */
            void OnImageFailed(ImageSource who);

            /**
             * Request for rendering this {@code ImageSource}.
             */
            void InvalidateImage(ImageSource who);

            /**
             * Request for posting the {@code Runnable}.
             */
            void ScheduleImage(ImageSource who,Runnable what, long when);

            /**
             * Request for canceling posting the {@code Runnable}.
             */
            void UnscheduleImage(ImageSource who,Runnable what);
        }


    }
}