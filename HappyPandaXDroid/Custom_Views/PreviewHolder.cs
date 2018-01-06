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
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using NLog;

using Android.Support.V7.Widget;
using Android.Support.V7.App;

namespace HappyPandaXDroid.Custom_Views
{
    public class PreviewHolder : RecyclerView.ViewHolder
    {
        public View preview;
        public ImageView img;
        public TextView txt;
        public int position;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public bool loaded = false;
        public Core.Gallery.Page page;
        string thumb_path;
        public PreviewHolder(View itemView, Action<PreviewAdapterClickEventArgs> clickListener,
                            Action<PreviewAdapterClickEventArgs> longClickListener) : base(itemView)
        {

            preview = itemView;
            img = preview.FindViewById<ImageView>(Resource.Id.preview);
            txt = preview.FindViewById<TextView>(Resource.Id.title);
            itemView.Click += (sender, e) => clickListener(new PreviewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new PreviewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });

        }

        bool IsCached
        {
            get
            {
                int item_id = page.id;
                try
                {
                    thumb_path = Core.App.Settings.cache + "preview/" + Core.App.Server.HashGenerator("medium", "preview", item_id) + ".jpg";
                    bool check = Core.Media.Cache.IsCached(thumb_path);

                    return check;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In GalleryCard.IsCached.");

                    return false;
                }
            }
        }



        public async Task<bool> LoadPreview(Core.Gallery.Page page)
        {
            this.page = page;
            int tries = 0;
            var h = new Handler(Looper.MainLooper);
            try
            {
                if (page.isPlaceholder)
                {
                    h.Post(() =>
                    {
                        try
                        {
                            Glide.With(preview.Context)
                                    .Load(Resource.Drawable.ic_add_white)
                                    .Into(img);
                            txt.Text = page.name;
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex, "\n Exception Caught In GalleryActivity.PreviewHolder.LoadPreview." +
                                        "Failure in \'source exist check'. Exception Message\n" + ex.Message);
                        }
                    });
                    return true;
                }
               
                bool exists = await Core.Gallery.IsSourceExist("page", page.id);
                if (!exists)
                {

                    h.Post(() =>
                    {
                        try
                        {
                            Glide.With(preview.Context)
                                    .Load(Resource.Drawable.image_failed)
                                    .Into(img);
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex, "\n Exception Caught In GalleryActivity.PreviewHolder.LoadPreview." +
                                        "Failure in \'source exist check'. Exception Message\n" + ex.Message);
                        }
                    });
                    return false;
                }
                h.Post(() =>
                {
                    try
                    {
                        if (((GalleryActivity)(preview.Context)).IsRunning)
                            Glide.With(preview.Context)
                     .Load(Resource.Drawable.loading2)
                     .Into(img);
                    }
                    catch (System.Exception ex)
                    {
                        logger.Error(ex, "\n Exception Caught In GalleryActivity.PreviewHolder.LoadPreview." +
                                        "Failure in setting koadung image. Exception Message\n" + ex.Message);
                        if (ex.Message.Contains("destroyed"))
                            return;
                    }
                });
                while (!IsCached)
                {
                    exists = await Core.Gallery.IsSourceExist("page", page.id);
                    if (!exists)
                    {
                        h.Post(() =>
                        {
                            try
                            {
                                Glide.With(preview.Context)
                                        .Load(Resource.Drawable.image_failed)
                                        .Into(img);
                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex, "\n Exception Caught In GalleryActivity.PreviewHolder.LoadPreview." +
                                        "Failure in setting failed source. Exception Message\n" + ex.Message);
                            }
                        });
                        return false;
                    }

                    page.thumb_url = await Core.Gallery.GetImage(page, false, "medium", true);
                    if (page.thumb_url.Contains("fail"))
                    {
                        if (page.thumb_url.Contains("misc"))
                        {
                            tries++;
                            if (tries < 4)
                            {
                                continue;
                            }
                            h.Post(() =>
                            {
                                try
                                {
                                    Glide.With(preview.Context)
                                    .Load(Resource.Drawable.image_failed)
                                    .Into(img);
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex, "\n Exception Caught In GalleryActivity.PreviewHolder.LoadPreview." +
                                        "Failure in getting Image. Exception Message\n" + ex.Message);
                                }
                            });
                            return false;

                        }
                        h.Post(() =>
                        {
                            try
                            {
                                Glide.With(preview.Context)
                                .Load(Resource.Drawable.image_failed)
                                .Into(img);
                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex, "\n Exception Caught In GalleryActivity.PreviewHolder.LoadPreview" +
                                    ". Exception Message\n" + ex.Message);
                            }
                        });
                        return false;
                    }
                    else
                    {
                        break;
                    }
                }

                h = new Handler(Looper.MainLooper);
                h.Post(() =>
                {
                        Glide.With(preview.Context)
                             .Load(thumb_path)
                             .Into(img);
                    loaded = true;
                });
                tries = 0;
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "\n Exception Caught In GalleryActivity.PreviewHolder.LoadPreview. Exception Message\n" + ex.Message);

                tries = 0;
                return false;
            }

        }
        
    }
}