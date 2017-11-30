using Android.Graphics;
using Android.OS;
using LargeImageView.StreamPipe;
using System;
using System.IO;
using Java.Lang;

namespace LargeImageView
{
    public class AutoSource : WrapperSource
    {

        private bool mInit;
        private int mMaxBitmapSize;
        private int mBitmapLimit;
        private InitTask mTask;
        private IInputStreamPipe mPipe;


        protected int MaxBitmapSize
        {
            get
            {
                return mMaxBitmapSize;
            }
        }

        protected int BitmapLimit
        {
            get
            {
                return mBitmapLimit;
            }
        }

        protected IInputStreamPipe InputStreamPipe
        {
            get
            {
                return mPipe;
            }
        }

        protected void ClearInputStreamPipe()
        {
            mPipe = null;
        }

        private void InitializeSource()
        {
            if (!mInit || mBitmapLimit == 0 || mTask != null || Ready)
            {
                return;
            }
            mTask = new InitTask(this);
            mTask.Execute();
        }

        private void OnInitializeDone(ImageSource imageSource)
        {
            mTask = null;
            if (imageSource != null)
            {
                ImageSource = imageSource;
            }
            else
            {
                CallSelfFailed();
            }
        }

        public AutoSource(IInputStreamPipe pipe)
        {
            mPipe = pipe;
        }


        protected override void OnInitialize()
        {
            mInit = true;
            InitializeSource();
        }

        public override void SetMaxBitmapSize(int maxSize)
        {
            base.SetMaxBitmapSize(maxSize);
            mMaxBitmapSize = maxSize;
            mBitmapLimit = maxSize / 2;
            InitializeSource();
        }

        public override void Recycle()
        {
            base.Recycle();
            if (mTask != null)
            {
                mTask.Cancel(false);
                mTask = null;
            }
            mPipe = null;
        }


        /**
     * Decode InputStreamPipe to ImageSource.
     * Called in non-UI thread.
     */
        protected ImageSource Decode()
        {
            IInputStreamPipe pipe = mPipe;
            if (pipe == null)
            {
                return null;
            }

            try
            {
                pipe.Obtain();

                // Decode image info
                BitmapFactory.Options options = new BitmapFactory.Options();
                options.InJustDecodeBounds = true;
                BitmapFactory.DecodeStream(pipe.Open(), null, options);
                pipe.Close();
                if (options.OutWidth <= 0 || options.OutHeight <= 0)
                {
                    // It is not a image
                    return null;
                }

                if (options.OutWidth <= mBitmapLimit && options.OutHeight <= mBitmapLimit)
                {
                    // BitmapSource
                    options.InJustDecodeBounds = false;
                    Bitmap bitmap = BitmapFactory.DecodeStream(pipe.Open(), null, options);
                    if (bitmap != null)
                    {
                        return new BitmapSource(bitmap);
                    }
                }
                else
                {
                    // TiledBitmapSource
                    BitmapRegionDecoder Decoder = BitmapRegionDecoder.NewInstance(pipe.Open(), false);
                    if (Decoder != null)
                    {
                        return new TiledBitmapSource(new SkiaRegionDecoder(Decoder, Bitmap.Config.Argb8888));
                    }
                }
            }
            catch (IOException eo)
            {
                return null;
            }
            catch (InsufficientMemoryException ei)
            {
                return null;
            }
            catch(System.Exception ex)
            {

            }
            finally
            {
                pipe.Close();
                pipe.Release();
                mPipe = null;
            }

            return null;
        }

        private class InitTask :AsyncTask<object, object, ImageSource> {

        private WeakReference<AutoSource> mAutoSource;

        public InitTask(AutoSource autoSource)
        {
            mAutoSource = new WeakReference<AutoSource>(autoSource);
        }

            
            protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] native_parms)
            {
                 mAutoSource.TryGetTarget(out AutoSource autoSource);
                if (autoSource != null)
                {
                    return autoSource.Decode();
                }
                else
                {
                    return null;
                }
            }

        

        
        protected override void  OnPostExecute(ImageSource imageSource)
        {
                mAutoSource.TryGetTarget(out AutoSource autoSource);
                if (autoSource == null)
            {
                if (imageSource != null)
                {
                    imageSource.Recycle();
                }
            }
            else
            {
                autoSource.OnInitializeDone(imageSource);
            }
        }

            protected override void OnCancelled(Java.Lang.Object imageSource)
            {
                if (imageSource != null)
                {
                    ((ImageSource)imageSource).Recycle();
                }
            }
            
        

            protected override ImageSource RunInBackground(params object[] @params)
            {
                mAutoSource.TryGetTarget(out AutoSource autoSource);
                if (autoSource != null)
                {
                    return autoSource.Decode();
                }
                else
                {
                    return null;
                }
            }
        }

}
}