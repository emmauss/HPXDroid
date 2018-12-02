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
    class CollectionCard : ItemCard
    {
        public override CardType CardType => CardType.Collection;
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public CollectionCard(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {

        }
        public CollectionCard(Context context) :
            base(context)
        {

        }

        public CollectionCard(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {

        }

        protected override void Initialize()
        {
            base.Initialize();
            Artist.Visibility = Android.Views.ViewStates.Gone;
            categoryChip.Visibility = Android.Views.ViewStates.Gone;
        }

        public override async void Refresh()
        {
            var collection = HPXItem as Core.Gallery.Collection;
            if (collection == null)
            {
                return;
            }
            var h = new Handler(Looper.MainLooper);
            h.Post(() =>
            {
                Name.Text = collection.name;
            });
            LoadThumb();

            logger.Info("Refreshing CollectionCard. CollectionId = {0}", HPXItem.id);

            await Task.Run(async () =>
            {
                if (!IsCached)
                {
                    h.Post(() =>
                    {
                        Name.Text = collection.name;

                    });
                }
            });

            logger.Info("Refresh {0} Successful", HPXItem.id);
        }

        public override async void LoadThumb()
        {
            var collection = HPXItem as Core.Gallery.Collection;
            if (collection == null)
            {
                return;
            }
            {
                var h = new Handler(Looper.MainLooper);
                if (IsCached)
                {
                    try
                    {
                        if(Core.Gallery.GetCachedPagePath(_HPXItem, out thumb_path, Core.Gallery.ImageSize.Medium))
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
                        logger.Error(ex, "\n Exception Caught In CollectionCard.LoadThumb.");

                    }

                }

                else
                {
                    
                        thumb_path = await collection.Download(CardCancellationTokenSource.Token);
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