﻿using System;
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
    public abstract class ItemCard : LinearLayout
    {
        public abstract CardType CardType { get; }
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected CancellationTokenSource CardCancellationTokenSource = new CancellationTokenSource();
        protected View itemCard;
        protected ImageView img;
        protected TextView text;
        protected bool loaded = false;
        protected TextView text2;
        protected Core.Gallery.HPXItem _HPXItem;
        protected string thumb_path;
        bool Cancelled = false;

        public Core.Gallery.HPXItem HPXItem
        {
            get
            {
                return _HPXItem;

            }
            set
            {
                _HPXItem = value;
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
        public ItemCard(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();

        }
        public ItemCard(Context context) :
            base(context)
        {
            Initialize();
        }

        public ItemCard(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            if (Core.App.Settings.IsGrid)
                itemCard = Inflate(this.Context, Resource.Layout.galleryCardGrid, this);
            else
                itemCard = Inflate(this.Context, Resource.Layout.galleryCardList, this);

            Name = FindViewById<TextView>(Resource.Id.textViewholder);
            Artist = FindViewById<TextView>(Resource.Id.textViewholder2);
            img = FindViewById<ImageView>(Resource.Id.imageView);
            Clickable = true;
        }

        public void Recycle()
        {
            Application.SynchronizationContext.Post(_ => {
                text.Text = string.Empty;
                text2.Text = string.Empty;
                loaded = false;
                Glide.With(Context).Clear(img);
            }, null);
        }

        public void Reset()
        {
            loaded = false;
            thumb_path = string.Empty;
            CardCancellationTokenSource.Cancel();
            CardCancellationTokenSource = new CancellationTokenSource();
        }

        protected override void OnWindowVisibilityChanged([GeneratedEnum] ViewStates visibility)
        {
            base.OnWindowVisibilityChanged(visibility);
        }

        public abstract void Refresh();

        public abstract void LoadThumb();

        protected bool IsCached
        {
            get
            {


                int item_id = _HPXItem.id;
                try
                {
                    thumb_path = Core.App.Settings.cache + "thumbs/" + Core.App.Server.HashGenerator("medium", CardType.ToString(), item_id) + ".jpg";
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

    public enum CardType
    {
        Gallery,
        Collection,
    }
}