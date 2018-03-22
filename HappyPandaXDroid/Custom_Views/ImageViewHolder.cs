using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Java.Lang;
using System.Drawing;
using OpenTK.Graphics;
using Android.Views;
using Android.Widget;
using ProgressView = XamarinBindings.MaterialProgressBar;
using Com.Davemorrissey.Labs.Subscaleview;
using NLog;
using Com.Bumptech.Glide;
using Android.Graphics;
using Com.Bumptech.Glide.Request.Target;

namespace HappyPandaXDroid.Custom_Views
{
    public class ImageViewHolder : LinearLayout
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public CancellationTokenSource ImageCancellationTokenSource = new CancellationTokenSource();
        public SubsamplingScaleImageView  img;
        string page_path;
        Core.Gallery.Page Page { set; get; }
        public bool Loaded = false;
        View view;
        int tries = 0;
        public ImageViewHolder(Context context) : base(context)
        {
            Initialize();
        }

        public ImageViewHolder(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public ImageViewHolder(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
            view = Inflate(this.Context, Resource.Layout.ImageLayout, this);
            img = FindViewById<SubsamplingScaleImageView>(Resource.Id.image);
            img.Visibility = ViewStates.Visible;
            img.ImageLoadError += Img_ImageLoadError;
            img.SetMinimumDpi(240);
        }

        private void Img_ImageLoadError(object sender, SubsamplingScaleImageView.ImageLoadErrorEventArgs e)
        {
            
                Task.Run(() =>
                {
                    if(File.Exists(page_path))
                    File.Delete(page_path);
                    Refresh();
                });
            
        }
        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            ImageCancellationTokenSource.Cancel();
            Release();
        }

        protected override void OnAttachedToWindow()
        {
            ImageCancellationTokenSource = new CancellationTokenSource();
            base.OnAttachedToWindow();
        }

        public void Release()
        {
            ImageCancellationTokenSource.Cancel();
            img.Recycle();
            System.GC.Collect();
            Java.Lang.JavaSystem.Gc();
        }
        public void Refresh(bool delete = false)
        {
            
            if (Page != null)
            {
                if (delete)
                {
                    if (File.Exists(page_path))
                        File.Delete(page_path);
                    page_path = "";
                }
                OnLoadStart(Page);
            }
        }

        public async void OnLoadStart(Core.Gallery.Page page)
        {
                            
                this.Page = page;
                var h = new Handler(Looper.MainLooper);
            bool exists = true;// await Core.Gallery.IsSourceExist("page", page.id);
                if (!exists)
                {
                    h.Post(() =>
                    {
                        try
                        {
                            Glide.With(this.Context)
                                    .Load(Resource.Drawable.image_failed)
                                    .Into(img);
                        }
                        catch (System.Exception ex)
                        {

                        }
                    });
                    return;
                }
                Load();
            
            
        }

        async void Load()
        {
            var h = new Handler(Looper.MainLooper);
            try
                {
                    while (!IsCached())
                    {

                    bool exists =   await Core.Gallery.IsSourceExist("page", Page.id,ImageCancellationTokenSource.Token);
                    if (!exists)
                    {
                        h.Post(() =>
                        {
                            try
                            {
                                Glide.With(this.Context)
                                        .Load(Resource.Drawable.image_failed)
                                        .Into(img);
                            }
                            catch (System.Exception ex)
                            {

                            }
                        });
                        return;
                    }
                    
                    page_path = await Page.Download();

                        if (page_path.Contains("fail"))
                        {

                            if (page_path.Contains("misc"))
                            {
                                tries++;
                                if (tries < 3)
                                {
                                    continue;
                                }

                                return;

                            }
                            return;
                        }
                        else
                        {
                            break;
                        }
                    }

                   if(string.IsNullOrEmpty(page_path))
                {
                    page_path = Core.Gallery.GetCachedPagePath(Page.id);
                }
                h.Post(async () =>
                    {
                        try
                        {
                            
                            img.SetImage(ImageSource.InvokeUri(page_path));
                            img.ImageLoaded += Img_ImageLoaded;
                            
                        }
                        catch (IllegalArgumentException iex)
                        {
                            if (iex.Message.Contains("destroyed"))
                                return;
                        }
                        catch(System.Exception ex)
                        {

                        }
                        OnLoadEnd();

                    });
                    tries=0;
                
                    
                }
                catch (System.Exception ex)
                {

                }

            
        }

        private void Img_ImageLoaded(object sender, EventArgs e)
        {
        }

        public void OnLoadEnd()
        {

            Loaded = true;
            
        }

        bool IsCached()
        {
            int item_id = Page.id;
            try
            {

                return Core.Gallery.IsPageCached(Page);
            }
            catch (System.Exception ex)
            {
                logger.Error(ex, "\n Exception Caught In GalleryCard.IsCached.");

                return false;
            }
        }
    }
}