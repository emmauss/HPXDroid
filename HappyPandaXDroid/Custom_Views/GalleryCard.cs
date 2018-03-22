using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.App;
using Android.Support.V7.View;
using Com.Bumptech.Glide;


using System.Threading;
using System.Threading.Tasks;
using NLog;


namespace HappyPandaXDroid.Custom_Views
{
    public class GalleryCard : LinearLayout
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        CancellationTokenSource CardCancellationTokenSource = new CancellationTokenSource();
        View galleryCard;
        ImageView img;
        int tries = 0;
        TextView text;
        bool loaded = false;
        TextView text2;
        Core.Gallery.GalleryItem gallery;
        string thumb_path;
        bool Cancelled = false;

        public Core.Gallery.GalleryItem Gallery
        {
            get
            {
                return gallery;
                
            }
            set
            {
                gallery = value;
            }
        }
        public TextView Name
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }
        public ImageView Image
        {
            get
            {
                return img;
            }
            set
            {
                img = value;
            }
        }
        public TextView Artist
        {
            get
            {
                return text2;
            }
            set
            {
                text2 = value;
            }
        }

        public string ThumbnailPath
        {
            get
            {
                return thumb_path; 
            }
        }
        public GalleryCard(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();

        }
        public GalleryCard(Context context) :
            base(context)
        {
            Initialize();
        }

        public GalleryCard(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
            if (Core.App.Settings.IsGrid)
                galleryCard = Inflate(this.Context, Resource.Layout.galleryCardGrid, this);
            else
                galleryCard = Inflate(this.Context, Resource.Layout.galleryCardList, this);

            Name = FindViewById<TextView>(Resource.Id.textViewholder);
            Artist = FindViewById<TextView>(Resource.Id.textViewholder2);
            img = FindViewById<ImageView>(Resource.Id.imageView);
            Clickable = true;
        }

        public void Recycle()
        {
            text.Text = string.Empty;
            text2.Text = string.Empty;
            loaded = false;
            Glide.With(Context).Clear(img);
        }

        public void Reset()
        {
            loaded = false;
            tries = 0;
            thumb_path = string.Empty;
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            CardCancellationTokenSource.Cancel();
            CardCancellationTokenSource = new CancellationTokenSource();

            Refresh();
        }
        

        protected override void OnDetachedFromWindow()
        {
            CardCancellationTokenSource.Cancel();
            if (!loaded)
                Cancelled = true;
            base.OnDetachedFromWindow();
        }

        protected override void OnWindowVisibilityChanged([GeneratedEnum] ViewStates visibility)
        {
            base.OnWindowVisibilityChanged(visibility);
        }

        public async void Refresh()
        {
            bool exists = false;
            if (Gallery == null)
            {
                return;
            }
                var h = new Handler(Looper.MainLooper);
            h.Post(() => {

                Name.Text = Gallery.titles[0].name;
                if (gallery.artists != null)
                    Artist.Text = string.Join(", ", gallery.artists.Select((x) => x.name));
            });

            await Task.Run( async () =>
            {
                try
                {
                    exists = await Core.Gallery.IsSourceExist("gallery", Gallery.id,CardCancellationTokenSource.Token);
                }catch(Exception ex)
                {
                    exists = true;
                }
            });
            if (!exists)
            {
                h.Post(() =>
                {
                    try
                    {
                        Name.Text = Gallery.titles[0].name;
                        if(gallery.artists!=null)
                        Artist.Text = string.Join(", ", gallery.artists.Select((x) => x.name));
                        img.SetImageResource(Resource.Drawable.image_failed);
                    }
                    catch (Exception ex)
                    {

                    }
                });
                return;
            }

            if (tries > 1)
            {
                tries = 0;
                h.Post(() =>
                {
                    try
                    {
                        Name.Text = Gallery.titles[0].name;
                        if (gallery.artists != null)
                            Artist.Text = string.Join(", ", gallery.artists.Select((x) => x.name));
                        img.SetImageResource(Resource.Drawable.image_failed);
                    }
                    catch (Exception ex)
                    {

                    }
                });
                    return;
            }
            logger.Info("Refreshing GalleryCard. GalleryId = {0}", Gallery.id);
            tries++;
            
            await Task.Run(async () =>
            {
                if (!IsCached)
                {
                    h.Post(() =>
                    {
                        Name.Text = Gallery.titles[0].name;
                        if (gallery.artists != null)
                            Artist.Text = string.Join(", ", gallery.artists.Select((x) => x.name));

                    });
                    thumb_path = await gallery.Download(CardCancellationTokenSource.Token);

                }
            });
                LoadThumb(); 

            logger.Info("Refresh {0} Successful", Gallery.id);
        }

        public async  void LoadThumb()
        {
            var h = new Handler(Looper.MainLooper);
            if (IsCached)
            {
                try
                {
                    
                    h.Post(() =>
                    {
                        try
                        {
                            Glide.With(Context)
                             .Load(thumb_path)
                             .Into(img);
                            loaded = true;
                            tries = 0;
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("destroyed"))
                                return;
                        }
                    });
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In GalleryCard.LoadThumb.");

                }

            }

            else {
                bool exists = false;
                await Task.Run(async () =>
                {
                    try
                    {
                        exists = await Core.Gallery.IsSourceExist("gallery", Gallery.id,CardCancellationTokenSource.Token);
                    }
                    catch (Exception ex)
                    {
                        exists = true;
                    }
                });
                if (!exists)
                {
                    h.Post(() =>
                    {
                        try { 
                        Glide.With(this.Context)
                                .Load(Resource.Drawable.image_failed)
                                .Into(img);
                        }
                        catch (Exception ex)
                        {

                        }
                    });
                    return;
                }

                Refresh();

            }
        }

        bool IsCached
        {
            get
            {
                
                
                int item_id = gallery.id;
                try
                {
                    thumb_path = Core.App.Settings.cache + "thumbs/" + Core.App.Server.HashGenerator("medium", "thumb", item_id) + ".jpg";
                    bool check = Core.Media.Cache.IsCached(thumb_path);

                    return check;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In GalleryCard.IsCached. Message : " + ex.Message 
                        + System.Environment.NewLine + ex.StackTrace);

                    return false;
                }

            }
        }

        private void GalleryCard_Touch(object sender, TouchEventArgs e)
        {

        }

        private void GalleryCard_LongClick(object sender, LongClickEventArgs e)
        {

        }

       
        
    }
}