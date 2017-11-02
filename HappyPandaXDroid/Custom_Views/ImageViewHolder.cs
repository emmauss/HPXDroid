using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }

        public void Refresh()
        {
            if (Page != null)
            {
                OnLoadStart(Page);
            }
        }

        public async void OnLoadStart(Core.Gallery.Page page)
        {
                            
                this.Page = page;
                var h = new Handler(Looper.MainLooper);
                bool exists = await Core.Gallery.IsSourceExist("page", page.id);
                if (!exists)
                {
                    h.Post(() =>
                    {
                        Glide.With(this.Context)
                                .Load(Resource.Drawable.image_failed)
                                .Into(img);
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
                    
                    bool exists = await Core.Gallery.IsSourceExist("page", Page.id);
                    if (!exists)
                    {
                        h.Post(() =>
                        {
                            Glide.With(this.Context)
                                    .Load(Resource.Drawable.image_failed)
                                    .Into(img);
                        });
                        return;
                    }
                    page_path = await Core.Gallery.GetImage(Page, false, "original", false);

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

                    
                h.Post(async () =>
                    {
                        try
                        {
                            bool isLandscape = false;
                           await  Task.Run( async() =>
                            {
                            });
                            
                            img.SetImage(ImageSource.InvokeUri(page_path));

                            
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

        public void OnLoadEnd()
        {

            Loaded = true;
            
                img.Visibility = ViewStates.Visible;
            
        }

        bool IsCached()
        {
            int item_id = Page.id;
            try
            {

                page_path = Core.App.Settings.cache + "pages/" + Core.App.Server.HashGenerator("original", "page", item_id) + ".jpg";
                bool check = Core.Media.Cache.IsCached(page_path);

                return check;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex, "\n Exception Caught In GalleryCard.IsCached.");

                return false;
            }


        }
    }
}