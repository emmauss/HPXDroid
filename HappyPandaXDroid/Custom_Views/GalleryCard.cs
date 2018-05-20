using Android.Content;
using Android.OS;
using Android.Util;
using Com.Bumptech.Glide;
using NLog;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace HappyPandaXDroid.Custom_Views
{
    public class GalleryCard : ItemCard
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public override CardType CardType => CardType.Gallery;

        public GalleryCard(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {

        }
        public GalleryCard(Context context) :
            base(context)
        {

        }

        public GalleryCard(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {

        }

        protected override void Initialize()
        {
            base.Initialize();

        }

        public override async void Refresh()
        {
            var gallery = HPXItem as Core.Gallery.GalleryItem;
            if (gallery == null)
            {
                return;
            }
            var h = new Handler(Looper.MainLooper);
            h.Post(() =>
            {

                Name.Text = gallery.titles[0].name;
                if (gallery.artists != null)
                    Artist.Text = string.Join(", ", gallery.artists.Select((x) => x.name));
            });
            LoadThumb();

            logger.Info("Refreshing GalleryCard. GalleryId = {0}", HPXItem.id);

            await Task.Run(async () =>
            {
                if (!IsCached)
                {
                    h.Post(() =>
                    {
                        Name.Text = gallery.titles[0].name;
                        if (gallery.artists != null)
                            Artist.Text = string.Join(", ", gallery.artists.Select((x) => x.name));

                    });
                }
            });

            logger.Info("Refresh {0} Successful", HPXItem.id);
        }

        public override async void LoadThumb()
        {
            var gallery = HPXItem as Core.Gallery.GalleryItem;
            if (gallery == null)
            {
                return;
            }
            {
                var h = new Handler(Looper.MainLooper);
                if (IsCached)
                {
                    try
                    {
                        Core.Gallery.GetCachedPagePath(_HPXItem.id,out thumb_path, "Gallery", "medium");
                        h.Post(() =>
                        {
                            try
                            {
                                Glide.With(Context)
                                 .Load(thumb_path)
                                 .Into(img);
                                loaded = true;
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

                else
                {

                    bool exists = false;
                    await Task.Run(async () =>
                    {
                        try
                        {
                            exists = await Core.Gallery.IsSourceExist(Core.Gallery.ItemType.Gallery, HPXItem.id, CardCancellationTokenSource.Token);
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
                            try
                            {
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
                    else
                    {
                        thumb_path = await gallery.Download(CardCancellationTokenSource.Token);
                        h.Post(() =>
                        {
                            try
                            {
                                Glide.With(Context)
                                 .Load(thumb_path)
                                 .Into(img);
                                loaded = true;
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("destroyed"))
                                    return;
                            }
                        });

                    }

                }
            }
        }
    }
}