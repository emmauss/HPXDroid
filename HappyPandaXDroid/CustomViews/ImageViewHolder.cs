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

namespace HappyPandaXDroid.CustomViews
{
    public class ImageViewHolder : LinearLayout
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public CancellationTokenSource ImageCancellationTokenSource = new CancellationTokenSource();
        public SubsamplingScaleImageView img;
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
            img.ZoomEnabled = true;
            img.Visibility = ViewStates.Visible;
            img.ImageLoadError += Img_ImageLoadError;
            img.SetMinimumDpi(120);
        }

        private void Img_ImageLoadError(object sender, SubsamplingScaleImageView.ImageLoadErrorEventArgs e)
        {
            Task.Run(() =>
            {
                if (File.Exists(page_path))
                    File.Delete(page_path);
                Refresh();
            });

        }

        public void Release()
        {
            ImageCancellationTokenSource.Cancel();
            img.Recycle();
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
            var h = new Handler(Looper.MainLooper);
            try
            {
                ImageCancellationTokenSource.Cancel();
                ImageCancellationTokenSource = new CancellationTokenSource();
                this.Page = page;
                Load();
            }
            catch (System.Exception ex)
            {
                h.Post(async () =>
                {
                    img.SetImage(ImageSource.InvokeResource(Resource.Drawable.image_failed));
                });
            }


            if(string.IsNullOrWhiteSpace(page_path))
                h.Post(async () =>
                {
                    img.SetImage(ImageSource.InvokeResource(Resource.Drawable.image_failed));
                });
        }

        async void Load()
        {
            var h = new Handler(Looper.MainLooper);
            tries = 0;
            try
            {
                if (IsCached)
                {
                    Core.Gallery.GetCachedPagePath(Page, out page_path, Core.App.Settings.ImageSize);
                    h.Post(async () =>
                    {
                        try
                        {

                            img.SetImage(ImageSource.InvokeUri(page_path));

                        }
                        catch (IllegalArgumentException iex)
                        {
                            if (iex.Message.Contains("destroyed"))
                                return;
                        }
                        catch (System.Exception ex)
                        {

                        }
                        OnLoadEnd();

                    });
                }
                else
                    while (!IsCached)
                    {

                        /*bool exists = await Core.Gallery.IsSourceExist(Core.Gallery.ItemType.Page, Page.id, ImageCancellationTokenSource.Token);
                        if (!exists)
                        {
                            return;
                        }*/
                        tries++;
                        page_path = Page.Download(Core.App.Settings.ImageSize);
                        if (tries > 1)
                            return;
                        else
                            Load();
                    }


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

        bool IsCached
        {
            get
            {
                int item_id = Page.id;
                try
                {
                    return Core.Gallery.IsItemCached(Page, Core.App.Settings.ImageSize);
                }
                catch (System.Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In GalleryCard.IsCached.");

                    return false;
                }
            }
        }
    }
}