using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Android.Annotation;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V4.View.Animation;

using Java.Lang.Annotation;
using Java.Lang;

namespace LargeImageView
{
    public class LargeImageView : View, ImageSource.ICallback, GestureRecognizer.IListener
    {
        public const int ORIENTATION_0 = 0;
        public const int ORIENTATION_90 = 1;
        public const int ORIENTATION_180 = 2;
        public const int ORIENTATION_270 = 3;

        private const float MAX_SCALE = 8.0f;
        private const float MIN_SCALE = 1.0f / 8.0f;

        private static ITimeInterpolator FAST_SLOW_INTERPOLATOR = new LinearOutSlowInInterpolator();


        private int mOrientation = ORIENTATION_0;
        private float mScale;

        private ImageSource mImage;
        private int mImageWidth;
        private int mImageHeight;

        // Window width of Image
        private int mWindowWidth = 0;
        // Window height of Image
        private int mWindowHeight = 0;
        // Max bitmap width and height
        private int mMaxBitmapSize = 0;

        // True to update mSrcActual and mDstActual
        private bool mRectDirty;

        // The area in view for whole Image
        private RectF mDst = new RectF();
        // The area in image to draw
        private RectF mSrcActual = new RectF();
        // The area in view to draw
        private RectF mDstActual = new RectF();

        // The scale to fit view.
        private float mFitScale;
        // The max value that scale can be.
        private float mMaxScale;
        // The min value that scale can be.
        private float mMinScale;
        // fit width, fit height, 1.0f
        private float[] mScaleArray = new float[3];

        // Current running animator count
        private int mAnimating;

        private GestureRecognizer mGestureRecognizer;

        private SmoothScaler mSmoothScaler;
        private ImageFling mImageFling;

        private IImageInitListener mImageInitListener;

        private Android.Graphics.PointF mTempPointF = new Android.Graphics.PointF();
        // The dump drawable to call
        // scheduleDrawable and unscheduleDrawable.
        private Drawable mDumpDrawable;







        public LargeImageView(Context context) : base(context)
        {
            Initialize(context);
        }

        public LargeImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(context);
        }

        public LargeImageView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize(context);
        }

        public void SetImageInitListener(IImageInitListener imageInitListener)
        {
            mImageInitListener = imageInitListener;
        }

        protected override bool VerifyDrawable(Drawable who)
        {
            // We use dump drawable to call scheduleDrawable and unscheduleDrawable,
            // so verify dump drawable here.
            return base.VerifyDrawable(who) || (mDumpDrawable != null && who == mDumpDrawable);
        }


        private void UpdateScale()
        {
            int wWidth = mWindowWidth;
            int wHeight = mWindowHeight;
            if (wWidth <= 0 || wHeight <= 0)
            {
                return;
            }
            int iWidth = mImageWidth;
            int iHeight = mImageHeight;
            if (iWidth <= 0 || iHeight <= 0)
            {
                return;
            }

            float widthScale = (float)wWidth / iWidth;
            float heightScale = (float)wHeight / iHeight;
            float fitScale = System.Math.Min(widthScale, heightScale);

            mFitScale = fitScale;
            mMaxScale = System.Math.Max(MAX_SCALE, fitScale);
            mMinScale = System.Math.Min(MIN_SCALE, fitScale);

            float[] scaleArray = mScaleArray;
            scaleArray[0] = Clamp(widthScale, mMinScale, mMaxScale);
            scaleArray[1] = Clamp(heightScale, mMinScale, mMaxScale);
            scaleArray[2] = 1.0f;
            Array.Sort(scaleArray);
        }

        // Fit windows center
        private void InitializePosition()
        {
            int wWidth = mWindowWidth;
            int wHeight = mWindowHeight;
            if (wWidth <= 0 || wHeight <= 0)
            {
                return;
            }
            int iWidth = mImageWidth;
            int iHeight = mImageHeight;
            if (iWidth <= 0 || iHeight <= 0)
            {
                return;
            }

            // Set scale
            mScale = mFitScale;
            float dWidth = iWidth * mScale;
            float dHeight = iHeight * mScale;

            // Set mDst.left and mDst.right
            RectF dst = mDst;
            dst.Left = (wWidth - dWidth) / 2;
            dst.Top = (wHeight - dHeight) / 2;

            // Set mDst.right and mDst.bottom
            dst.Right = dst.Left + dWidth;
            dst.Bottom = dst.Top + dHeight;

            // Adjust position
            AdjustPosition();

            mRectDirty = true;
        }

        // Make sure scale is in [mMinScale, mMaxScale]
        private void AdjustScale()
        {
            int iWidth = mImageWidth;
            int iHeight = mImageHeight;
            if (iWidth <= 0 || iHeight <= 0)
            {
                return;
            }
            RectF dst = mDst;
            if (dst.IsEmpty)
            {
                return;
            }

            float oldScale = mScale;
            mScale = Clamp(oldScale, mMinScale, mMaxScale);
            if (oldScale == mScale)
            {
                return;
            }

            dst.Right = dst.Left + mScale * iWidth;
            dst.Bottom = dst.Top + mScale * iHeight;
            mRectDirty = true;
        }

        // If target is smaller then view, make it in screen center.
        // If target is larger then view, make it fill screen.
        private void AdjustPosition()
        {
            int wWidth = mWindowWidth;
            int wHeight = mWindowHeight;
            if (wWidth <= 0 || wHeight <= 0)
            {
                return;
            }
            RectF dst = mDst;
            float dWidth = dst.Width();
            float dHeight = dst.Height();
            if (dWidth <= 0 || dHeight <= 0)
            {
                return;
            }

            if (dWidth > wWidth)
            {
                float fixXOffset = dst.Left;
                if (fixXOffset > 0)
                {
                    dst.Left -= fixXOffset;
                    dst.Right -= fixXOffset;
                    mRectDirty = true;
                }
                else if ((fixXOffset = wWidth - dst.Right) > 0)
                {
                    dst.Left += fixXOffset;
                    dst.Right += fixXOffset;
                    mRectDirty = true;
                }
            }
            else
            {
                float left = (wWidth - dWidth) / 2;
                dst.OffsetTo(left, dst.Top);
                mRectDirty = true;
            }
            if (dHeight > wHeight)
            {
                float fixYOffset = dst.Top;
                if (fixYOffset > 0)
                {
                    dst.Top -= fixYOffset;
                    dst.Bottom -= fixYOffset;
                    mRectDirty = true;
                }
                else if ((fixYOffset = wHeight - dst.Bottom) > 0)
                {
                    dst.Top += fixYOffset;
                    dst.Bottom += fixYOffset;
                    mRectDirty = true;
                }
            }
            else
            {
                float top = (wHeight - dHeight) / 2;
                dst.OffsetTo(dst.Left, top);
                mRectDirty = true;
            }
        }

        // Update mWindowWidth and mWindowHeight
        private void SetWindowSize(int w, int h)
        {
            switch (mOrientation)
            {
                case ORIENTATION_0:
                case ORIENTATION_180:
                    mWindowWidth = w;
                    mWindowHeight = h;
                    break;
                case ORIENTATION_90:
                case ORIENTATION_270:
                    mWindowWidth = h;
                    mWindowHeight = w;
                    break;
                default:
                    throw new IllegalStateException("Unknown orientation: " + mOrientation);
            }
        }


        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            CancelAllAnimator();

            // Assign window width and height
            SetWindowSize(w, h);

            if (mImage != null)
            {
                mImage.SetWindowSize(w, h);
                UpdateScale();
                if (mDst.IsEmpty)
                {
                    InitializePosition();
                }
                else
                {
                    AdjustScale();
                    AdjustPosition();
                }
            }
        }


        public int Orientation
        {
            get
            {
                return mOrientation;
            }
            set
            {
                if (mOrientation == value)
                {
                    return;
                }
                mOrientation = value;

                CancelAllAnimator();

                // Window size might be still the same, check it
                int oldWWidth = mWindowWidth;
                int oldWHeight = mWindowHeight;
                SetWindowSize(Width, Height);
                if (oldWWidth != mWindowWidth || oldWHeight != mWindowHeight)
                {
                    mImage.SetWindowSize(mWindowWidth, mWindowHeight);
                    UpdateScale();
                    InitializePosition();
                }
                Invalidate();
            }
        }

        private void Translate(float dx, float dy)
        {
            int wWidth = mWindowWidth;
            int wHeight = mWindowHeight;
            if (wWidth <= 0 || wHeight <= 0)
            {
                return;
            }
            RectF dst = mDst;
            float dWidth = dst.Width();
            float dHeight = dst.Height();
            if (dWidth <= 0 || dHeight <= 0)
            {
                return;
            }

            float remainX;
            float remainY;

            if (dWidth > wWidth)
            {
                dst.Left -= dx;
                dst.Right -= dx;

                float fixXOffset = dst.Left;
                if (fixXOffset > 0)
                {
                    dst.Left -= fixXOffset;
                    dst.Right -= fixXOffset;
                    remainX = -fixXOffset;
                }
                else if ((fixXOffset = wWidth - dst.Right) > 0)
                {
                    dst.Left += fixXOffset;
                    dst.Right += fixXOffset;
                    remainX = fixXOffset;
                }
                else
                {
                    remainX = 0;
                }
            }
            else
            {
                remainX = dx;
            }
            if (dHeight > wHeight)
            {
                dst.Top -= dy;
                dst.Bottom -= dy;

                float fixYOffset = dst.Top;
                if (fixYOffset > 0)
                {
                    dst.Top -= fixYOffset;
                    dst.Bottom -= fixYOffset;
                    remainY = -fixYOffset;
                }
                else if ((fixYOffset = wHeight - dst.Bottom) > 0)
                {
                    dst.Top += fixYOffset;
                    dst.Bottom += fixYOffset;
                    remainY = fixYOffset;
                }
                else
                {
                    remainY = 0;
                }
            }
            else
            {
                remainY = dy;
            }

            // Check requestDisallowInterceptTouchEvent
            // Don't call requestDisallowInterceptTouchEvent when animated
            // Only call requestDisallowInterceptTouchEvent when on room for scroll left or right
            if (mAnimating == 0 && dx == remainX)
            {
                IViewParent parent = Parent;
                if (parent != null)
                {
                    parent.RequestDisallowInterceptTouchEvent(false);
                }
            }

            if (dx != remainX || dy != remainY)
            {
                mRectDirty = true;
                Invalidate();
            }
        }

        private void SetScale(float x, float y, float scale)
        {
            int iWidth = mImageWidth;
            int iHeight = mImageHeight;
            if (iWidth <= 0 || iHeight <= 0)
            {
                return;
            }
            RectF dst = mDst;
            float dWidth = dst.Width();
            float dHeight = dst.Height();
            if (dWidth <= 0 || dHeight <= 0)
            {
                return;
            }

            scale = Clamp(scale, mMinScale, mMaxScale);
            if (mScale == scale)
            {
                return;
            }

            float sScale = scale / mScale;
            mScale = scale;
            dst.Left = (x - ((x - dst.Left) * sScale));
            dst.Top = (y - ((y - dst.Top) * sScale));
            dst.Right = dst.Left + (iWidth * scale);
            dst.Bottom = dst.Top + (iHeight * scale);

            // Adjust position
            AdjustPosition();

            mRectDirty = true;
            Invalidate();
        }

        private void Scale(float x, float y, float scale)
        {
            SetScale(x, y, mScale * scale);
        }

        private void CancelAllAnimator()
        {
            if (mSmoothScaler != null)
            {
                mSmoothScaler.Cancel();
            }
            if (mImageFling != null)
            {
                mImageFling.Cancel();
            }
        }

        private void ScaleToNextLevel(float x, float y)
        {
            if (!Ready)
            {
                return;
            }

            float scale = mScale;
            float endScale = mScaleArray[0];
            foreach (float value in mScaleArray)
            {
                if (scale < value - 0.01f)
                {
                    endScale = value;
                    break;
                }
            }

            if (mSmoothScaler == null)
            {
                mSmoothScaler = new SmoothScaler(this);
            }
            mSmoothScaler.StartSmoothScaler(x, y, endScale);
        }

        private void Fling(float velocityX, float velocityY)
        {
            int wWidth = mWindowWidth;
            int wHeight = mWindowHeight;
            if (wWidth <= 0 || wHeight <= 0)
            {
                return;
            }
            RectF dst = mDst;
            if (dst.IsEmpty)
            {
                return;
            }

            float minX, maxX;
            float minY, maxY;

            minX = (dst.Right > wWidth) ? (wWidth - dst.Right) : 0;
            maxX = dst.Left < 0 ? -dst.Left : 0;
            minY = (dst.Bottom > wHeight) ? (wHeight - dst.Bottom) : 0;
            maxY = dst.Top < 0 ? -dst.Top : 0;

            if (mImageFling == null)
            {
                mImageFling = new ImageFling(this);
            }
            mImageFling.StartFling(velocityX, minX, maxX, velocityY, minY, maxY);
        }

        // Return true if image is shown
        private bool Ready
        {
            get
            {
                return !mDst.IsEmpty;
            }
        }

        /**
         * Set the {@code ImageSource} to show.
         */
        public void SetImage(ImageSource image)
        {
            if (mImage != null)
            {
                CancelAllAnimator();
                mImage.SetCallback(null);
                UnscheduleImage(mImage);
                if (ViewCompat.IsAttachedToWindow(this))
                {
                    mImage.SetVisible(false);
                }
                mImage.Recycle();
                mDst.SetEmpty();
                mSrcActual.SetEmpty();
                mDstActual.SetEmpty();
            }

            int oldIWidth = mImageWidth;
            int oldIHeight = mImageHeight;

            mImage = image;

            if (image != null)
            {
                image.SetCallback(this);
                if (ViewCompat.IsAttachedToWindow(this))
                {
                    image.SetVisible(Visibility == ViewStates.Visible);
                }
                image.SetWindowSize(mWindowWidth, mWindowHeight);
                if (mMaxBitmapSize != 0)
                {
                    image.SetMaxBitmapSize(mMaxBitmapSize);
                }
                if (image.Ready)
                {
                    OnImageReady(image);
                    // Let onImageReady handle next process
                    return;
                }
                else
                {
                    image.Initialize();
                }
            }

            // ImageSource is null or not ready
            mImageWidth = 0;
            mImageHeight = 0;
            if (oldIWidth != mImageWidth || oldIHeight != mImageHeight)
            {
                RequestLayout();
            }
            Invalidate();
        }

        /**
         * Return {@code ImageSource}.
         */
        public ImageSource Image
        {
            get
            {
                return mImage;
            }
        }



        public void InvalidateImage(ImageSource who)
        {
            if (who == mImage)
            {
                Invalidate();
            }
        }

        private void EnsureDumpDrawable()
        {
            if (mDumpDrawable == null)
            {
                mDumpDrawable = new DumpDrawable();
                mDumpDrawable.SetCallback(this);
            }
        }

        public void OnCancel()
        {
            if (mImage != null)
            {
                mImage.OnTouchEnd();
            }
        }

        public void OnDoubleTap(float x, float y)
        {
            mTempPointF.Set(x, y);
            TransformPoint(mTempPointF);
            ScaleToNextLevel(mTempPointF.X, mTempPointF.Y);
        }

        public void OnDown()
        {
            CancelAllAnimator();

            if (mImage != null)
            {
                mImage.OnTouchStart();
            }
        }

        public void OnFling(float x, float y, float velocityX, float velocityY)
        {
            mTempPointF.Set(velocityX, velocityY);
            TransformDistance(mTempPointF);
            Fling(mTempPointF.X, mTempPointF.Y);
        }

        public void OnImageFailed(ImageSource who)
        {
            if (who != mImage)
            {
                return;
            }

            if (mImageInitListener != null)
            {
                mImageInitListener.OnImageInitFailed();
            }
        }

        public void OnImageReady(ImageSource who)
        {
            if (who != mImage)
            {
                return;
            }

            int oldIWidth = mImageWidth;
            int oldIHeight = mImageHeight;

            mImageWidth = who.Width;
            mImageHeight = who.Height;

            UpdateScale();
            InitializePosition();

            if (oldIWidth != mImageWidth || oldIHeight != mImageHeight)
            {
                RequestLayout();
            }
            Invalidate();

            if (mImageInitListener != null)
            {
                mImageInitListener.OnImageInitSuccessful();
            }
        }

        public void OnLongPress(float x, float y)
        {

        }

        public void OnScale(float x, float y, float scale)
        {
            mTempPointF.Set(x, y);
            TransformPoint(mTempPointF);
            Scale(mTempPointF.X, mTempPointF.Y, scale);
        }

        public void OnScroll(float x, float y, float dx, float dy, float totalX, float totalY)
        {
            mTempPointF.Set(dx, dy);
            TransformDistance(mTempPointF);
            Translate(mTempPointF.X, mTempPointF.Y);
        }

        public void OnSingleTap(float x, float y)
        {

        }

        protected override void OnAnimationStart()
        {
            ++mAnimating;

            if (mImage != null)
            {
                mImage.OnAnimatorStart();
            }
        }

        protected override void OnAnimationEnd()
        {
            --mAnimating;

            if (mImage != null)
            {
                mImage.OnAnimatorEnd();
            }
        }

        public void OnUp()
        {
            if (mImage != null)
            {
                mImage.OnTouchEnd();
            }
        }

        public void ScheduleImage(ImageSource who, Runnable what, long when)
        {
            if (who == mImage)
            {
                // Use dump drawable to call scheduleDrawable
                EnsureDumpDrawable();
                ScheduleDrawable(mDumpDrawable, what, when);
            }
        }

        public void UnscheduleImage(ImageSource who, Runnable what)
        {
            if (who == mImage)
            {
                // Use dump drawable to call unscheduleDrawable
                EnsureDumpDrawable();
                UnscheduleDrawable(mDumpDrawable, what);
            }
        }

        public void UnscheduleImage(ImageSource who)
        {
            if (who == mImage)
            {
                // Use dump drawable to call unscheduleDrawable
                EnsureDumpDrawable();
                UnscheduleDrawable(mDumpDrawable);
            }
        }

        private void ApplyRectInWindow()
        {
            RectF dst = mDst;
            RectF dstActual = mDstActual;
            RectF srcActual = mSrcActual;

            dstActual.Set(dst);
            if (dstActual.Intersect(0, 0, mWindowWidth, mWindowHeight))
            {
                if (dst.Equals(dstActual))
                {
                    // Still dst
                    srcActual.Set(0, 0, mImageWidth, mImageHeight);
                }
                else
                {
                    srcActual.Left = Lerp(0.0f, mImageWidth,
                            Norm(dst.Left, dst.Right, dstActual.Left));
                    srcActual.Right = Lerp(0.0f, mImageWidth,
                            Norm(dst.Left, dst.Right, dstActual.Right));
                    srcActual.Top = Lerp(0.0f, mImageHeight,
                            Norm(dst.Top, dst.Bottom, dstActual.Top));
                    srcActual.Bottom = Lerp(0.0f, mImageHeight,
                            Norm(dst.Top, dst.Bottom, dstActual.Bottom));
                }
            }
            else
            {
                // Can't be seen, set src and dst empty
                srcActual.SetEmpty();
                dstActual.SetEmpty();
            }

            mRectDirty = false;
        }

        private int GetMaxBitmapSize(Canvas canvas)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.IceCreamSandwich)
            {
                int maxSize = System.Math.Min(canvas.MaximumBitmapWidth, canvas.MaximumBitmapHeight);
                // If hardware acceleration is not enabled,
                // getMaximumBitmapWidth() and getMaximumBitmapHeight()
                // will return Integer.MAX_VALUE.
                // In that case, use 2048 as default.
                if (maxSize == Integer.MaxValue)
                {
                    maxSize = 2048;
                }
                return maxSize;
            }
            else
            {
                // Before ICE_CREAM_SANDWICH, hardware acceleration is not supported,
                // bitmap max size is not limited.
                // Use 2048 as default.
                return 2048;
            }
        }

        protected override void OnDraw(Canvas canvas)
        {
            // Set max bitmap size
            if (mMaxBitmapSize == 0)
            {
                mMaxBitmapSize = GetMaxBitmapSize(canvas);
                if (mImage != null)
                {
                    mImage.SetMaxBitmapSize(mMaxBitmapSize);
                }
            }

            if (mImage == null || mDst.IsEmpty)
            {
                return;
            }
            if (mRectDirty)
            {
                ApplyRectInWindow();
            }
            if (!mSrcActual.IsEmpty)
            {
                int saved = TransformCanvas(canvas);
                mImage.Draw(canvas, mSrcActual, mDstActual);
                if (saved != 0)
                {
                    canvas.RestoreToCount(saved);
                }
            }

        }

        public override ViewStates Visibility
        {
            get => base.Visibility;
            set
            {
                base.Visibility = value;
                if (mImage != null)
                {
                    mImage.SetVisible(ViewCompat.IsAttachedToWindow(this) && Visibility == ViewStates.Visible);
                }
            }
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            if (mImage != null)
            {
                mImage.SetVisible(Visibility == ViewStates.Visible);
            }
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            if (mImage != null)
            {
                mImage.SetVisible(false);
            }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            // Always call parent.requestDisallowInterceptTouchEvent(true)
            // When get edge, translate() will call parent.requestDisallowInterceptTouchEvent(false)
            IViewParent parent = Parent;
            if (parent != null)
            {
                parent.RequestDisallowInterceptTouchEvent(true);
            }

            mGestureRecognizer.OnTouchEvent(e);
            return true;
        }




        private void Initialize(Context context)
        {
            mGestureRecognizer = new GestureRecognizer(context, this);
        }


        private abstract class BaseAnimator : ValueAnimator, Animator.IAnimatorListener,
            ValueAnimator.IAnimatorUpdateListener
        {

            private LargeImageView mView;

            public BaseAnimator(LargeImageView view)
            {
                mView = view;
                AddListener(this);
                AddUpdateListener(this);
            }


            public void OnAnimationCancel(Animator animation)
            {

            }

            public void OnAnimationEnd(Animator animation)
            {
                mView.OnAnimationEnd();
            }

            public void OnAnimationRepeat(Animator animation)
            {

            }

            public void OnAnimationStart(Animator animation)
            {
                mView.OnAnimationStart();
            }

            public abstract void OnAnimationUpdate(ValueAnimator animation);
        }

        private class SmoothScaler : BaseAnimator
        {

            private LargeImageView mView;
            private float mX;
            private float mY;
            private float mStartScale;
            private float mEndScale;

            public SmoothScaler(LargeImageView view) : base(view)
            {
                mView = view;
                SetDuration(300);
                SetInterpolator(FAST_SLOW_INTERPOLATOR);
                SetFloatValues(0.0f, 1.0f);
            }

            public void StartSmoothScaler(float x, float y, float scale)
            {
                if (!mView.Ready)
                {
                    return;
                }
                mView.CancelAllAnimator();
                mX = x;
                mY = y;
                mStartScale = mView.mScale;
                mEndScale = scale;
                Start();
            }
            public override void OnAnimationUpdate(ValueAnimator animation)
            {
                float value = (float)AnimatedValue;
                mView.SetScale(mX, mY, Lerp(mStartScale, mEndScale, value));
            }

        }

        private class ImageFling : BaseAnimator
        {

            private LargeImageView mView;
            private Fling mFling;

            private float mDx;
            private float mDy;
            private float mLastX;
            private float mLastY;

            public ImageFling(LargeImageView view) : base(view)
            {
                mView = view;
                mFling = new Fling(view.Context);
                base.SetInterpolator(global::LargeImageView.Fling.FLING_INTERPOLATOR);
                SetFloatValues(0.0f, 1.0f);
            }

            public void StartFling(float velocityX, float minX, float maxX,
                    float velocityY, float minY, float maxY)
            {
                Fling fling = mFling;
                mDx = (float)(fling.GetSplineFlingDistance(velocityX) * System.Math.Sign(velocityX));
                mDy = (float)(fling.GetSplineFlingDistance(velocityY) * System.Math.Sign(velocityY));
                mLastX = 0;
                mLastY = 0;
                int durationX = fling.GetSplineFlingDuration(velocityX);
                int durationY = fling.GetSplineFlingDuration(velocityY);

                if (mDx < minX)
                {
                    durationX = fling.AdjustDuration(0, mDx, minX, durationX);
                    mDx = minX;
                }
                if (mDx > maxX)
                {
                    durationX = fling.AdjustDuration(0, mDx, maxX, durationX);
                    mDx = maxX;
                }
                if (mDy < minY)
                {
                    durationY = fling.AdjustDuration(0, mDy, minY, durationY);
                    mDy = minY;
                }
                if (mDy > maxY)
                {
                    durationY = fling.AdjustDuration(0, mDy, maxY, durationY);
                    mDy = maxY;
                }

                if (mDx == 0 && mDy == 0)
                {
                    return;
                }

                SetDuration(System.Math.Max(durationX, durationY));
                Start();
            }
            public override void OnAnimationUpdate(ValueAnimator animation)
            {
                float value = (float)AnimatedValue;
                float x = mDx * value;
                float y = mDy * value;
                float offsetX = x - mLastX;
                float offsetY = y - mLastY;
                mView.Translate(-offsetX, -offsetY);
                mLastX = x;
                mLastY = y;
            }
        }


        ////////////////////
        // Transform for orientation
        ////////////////////

        private int TransformCanvas(Canvas canvas)
        {
            int saved;
            switch (mOrientation)
            {
                case ORIENTATION_0:
                    saved = 0;
                    break;
                case ORIENTATION_90:
                    saved = canvas.Save();
                    canvas.Translate(Width, 0);
                    canvas.Rotate(90);
                    break;
                case ORIENTATION_180:
                    saved = canvas.Save();
                    canvas.Translate(Width, Height);
                    canvas.Rotate(180);
                    break;
                case ORIENTATION_270:
                    saved = canvas.Save();
                    canvas.Translate(0, Height);
                    canvas.Rotate(-90);
                    break;
                default:
                    throw new IllegalStateException("Unknown orientation: " + mOrientation);
            }
            return saved;
        }

        private void TransformPoint(Android.Graphics.PointF p)
        {
            switch (mOrientation)
            {
                case ORIENTATION_0:
                    break;
                case ORIENTATION_90:
                    p.Set(p.Y, Width - p.X);
                    break;
                case ORIENTATION_180:
                    p.Set(Width - p.X, Height - p.Y);
                    break;
                case ORIENTATION_270:
                    p.Set(Height - p.Y, p.X);
                    break;
                default:
                    throw new IllegalStateException("Unknown orientation: " + mOrientation);
            }
        }

        private void TransformDistance(Android.Graphics.PointF p)
        {
            switch (mOrientation)
            {
                case ORIENTATION_0:
                    break;
                case ORIENTATION_90:
                    p.Set(p.Y, -p.X);
                    break;
                case ORIENTATION_180:
                    p.Set(-p.X, -p.Y);
                    break;
                case ORIENTATION_270:
                    p.Set(-p.Y, p.X);
                    break;
                default:
                    throw new IllegalStateException("Unknown orientation: " + mOrientation);
            }
        }


        ////////////////////
        // Math function
        ////////////////////


        private static float Lerp(float start, float stop, float amount)
        {
            return start + (stop - start) * amount;
        }


        private static float Norm(float start, float stop, float value)
        {
            if (stop == start)
            {
                if (stop == value)
                {
                    return 1.0f;
                }
                else
                {
                    return float.NaN;
                }
            }
            else
            {
                return (value - start) / (stop - start);
            }
        }


        private static float Clamp(float x, float min, float max)
        {
            if (x > max) return max;
            if (x < min) return min;
            return x;
        }

        // The dump drawable to call scheduleDrawable and unscheduleDrawable.
        private class DumpDrawable : Drawable
        {
            public override int Opacity => (int)Format.Opaque;

            public override void Draw(Canvas canvas)
            {
            }

            public override void SetAlpha(int alpha)
            {
            }

            public override void SetColorFilter(ColorFilter colorFilter)
            {
            }
        }

        /**
         * Listener for init {@code ImageSource}.
         */
        public interface IImageInitListener
        {

            void OnImageInitSuccessful();

            void OnImageInitFailed();
        }

    }
}
