﻿using Android.Content;
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
                        thumb_path = Core.Gallery.GetCachedPagePath(_HPXItem.id, Core.Gallery.ItemType.Collection, CardType.ToString());
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

                    bool exists = false;
                    await Task.Run(async () =>
                    {
                        try
                        {
                            exists = await Core.Gallery.IsSourceExist(Core.Gallery.ItemType.Collection, HPXItem.id, CardCancellationTokenSource.Token);
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
}