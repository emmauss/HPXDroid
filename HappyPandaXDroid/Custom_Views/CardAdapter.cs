using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Support.Design.Chip;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using Com.Hippo.Stage;
using EasyRecyclerView;
using HappyPandaXDroid.Core;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HappyPandaXDroid.Custom_Views
{
    public class CardAdapter
    {
        public abstract class HPXCardAdapter : EasyAdapter
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public CancellationTokenSource AdapterCancellationTokenSource = new CancellationTokenSource();
            public EventHandler<int> ItemClick;
            protected Core.Gallery.ItemType ItemType;
            public Scenes.ViewScene content;
            bool IsImageReady = false;
            void OnClick(int position)
            {
                ItemClick?.Invoke(this, position);
            }

            List<Core.Gallery.HPXItem> mdata;
            public Dictionary<int,Media.Image> UrlList;
            Android.Content.Context mcontext;
            public HPXCardAdapter(Context context, Scenes.ViewScene content)
            {
                mcontext = context;
                this.content = content;
                mdata = content.CurrentList;
                UrlList = new Dictionary<int, Media.Image>();
                AdapterCancellationTokenSource = new CancellationTokenSource();
            }

            public override int ItemCount
            {
                get { return mdata.Count; }
            }

            public async void Add(List<Core.Gallery.HPXItem> newItems)
            {
                mdata.AddRange(newItems);
                foreach (var item in newItems)
                {
                    item.Thumb = new Media.Image();
                }
                Task.Run(()=>UpdateUrls(newItems));
            }

            public async void Prepend(List<Core.Gallery.HPXItem> newItems)
            {
                foreach (var item in newItems)
                {
                    item.Thumb = new Media.Image();
                }
                var urls = new Dictionary<int, Media.Image>(UrlList);
                UrlList.Clear();
                mdata.InsertRange(0, newItems);
                Task.Run(() =>
                {
                    UpdateUrls(newItems);
                    if (AdapterCancellationTokenSource.IsCancellationRequested)
                        return;
                    if (urls.Count > 0)
                    {
                        foreach (var item in mdata)
                        {
                            if (urls.ContainsKey(item.id))
                                if (!UrlList.ContainsKey(item.id))
                                {
                                    if (urls.ContainsKey(item.id))
                                        UrlList.Add(item.id, urls[item.id]);
                                }
                                else if (urls.ContainsKey(item.id))
                                    UrlList[item.id] = urls[item.id];
                        }
                    }
                
                }, AdapterCancellationTokenSource.Token);
            }

            public void UpdateUrls(List<Core.Gallery.HPXItem> newItems)
            {
                IsImageReady = false;
                List<int> ids = new List<int>();
                foreach (var item in newItems)
                {
                    string cacheid = Core.App.Server.HashGenerator(item.BaseId, Core.Gallery.ImageSize.Small, item.Type);
                    if (Core.Media.Cache.TryGetCachedPath
                        (cacheid, out item.Thumb.Uri))
                    {
                        item.Thumb.IsReady = true;
                        continue;
                    }
                    ids.Add(item.id);
                }
                if (ids.Count > 0)
                {
                    Dictionary<int, Media.Image> urls = new Dictionary<int, Media.Image>();
                    int tries = 0;
                    while(urls.Count == 0)
                    {
                        urls = Core.Gallery.GetImage(newItems, ItemType,
                            AdapterCancellationTokenSource.Token, Core.Gallery.ImageSize.Small).Result;
                        if (AdapterCancellationTokenSource.IsCancellationRequested)
                            return;

                        if (tries > 3)
                            break;
                        tries++;
                    }
                    if (urls.Count > 0)
                    {
                        foreach (var item in newItems)
                        {
                            if (urls.ContainsKey(item.id))
                                if (!UrlList.ContainsKey(item.id))
                                {
                                    UrlList.Add(item.id, urls[item.id]);
                                }
                                else
                                    UrlList[item.id] = urls[item.id];
                        }
                    }
                }
                IsImageReady = true;
            }

            public void Clear()
            {
                UrlList.Clear();
                mdata.Clear();
                AdapterCancellationTokenSource.Cancel();
                NotifyDataSetChanged();
            }

            public void ResetList()
            {
                UrlList.Clear();
                AdapterCancellationTokenSource.Cancel();
                AdapterCancellationTokenSource = new CancellationTokenSource();
                mdata = content.CurrentList;
                Task.Run(()=>UpdateUrls(mdata));
                this.NotifyDataSetChanged();
            }

            public override void OnViewRecycled(Java.Lang.Object holder)
            {
                base.OnViewRecycled(holder);
                if (holder is HPXItemHolder hold)
                {
                    hold.Bound = false;
                    hold.Url = string.Empty;

                    if (!content.IsDestroyed && !content.IsActivityDestroyed)
                        hold.Cancel();
                }
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                HPXItemHolder vh = holder as HPXItemHolder;
                try
                {
                    if (vh.ItemView != null)
                    {
                        var timer = Stopwatch.StartNew();
                        vh.Id = mdata[position].id;
                        if (mdata[position] is Core.Gallery.GalleryItem gallery)
                        {
                            vh.HPXItem = gallery;
                            vh.Name.Text = gallery.preferred_title.name;
                            if (gallery.artists.Count > 0)
                                if (gallery.artists[0].Names.Count > 0)
                                    vh.Info.Text = gallery.artists[0].Names[0].name;
                            if (gallery.ChildCount > 0)
                            {
                                vh.Pages.Text = gallery.ChildCount + " page" + (gallery.ChildCount > 1 ? "s" : "");
                                vh.UnbindCountEvent();
                            }
                            else
                            {
                                vh.BindCountEvent();
                                Task.Run(() => Core.Gallery.RequestPageCount(gallery, AdapterCancellationTokenSource.Token, Core.Gallery.ItemType.Page));
                            }
                        }
                        else
                         if (mdata[position] is Core.Gallery.Collection collection)
                        {
                            vh.HPXItem = collection;
                            vh.Name.Text = collection.name;
                            if (collection.ChildCount > 0)
                            {
                                vh.Info.Text = collection.ChildCount + " galler" +
                                (collection.ChildCount > 1 ? "ies" : "y");
                                vh.UnbindCountEvent();
                            }
                            else
                            {
                                vh.BindCountEvent();
                                Task.Run(() => Core.Gallery.RequestPageCount(collection, AdapterCancellationTokenSource.Token, Core.Gallery.ItemType.Gallery));
                            }
                        }

                        vh.Category.Text = Core.Gallery.Categories[mdata[position].category_id].name;

                        var elapsed = timer.ElapsedMilliseconds;
                        timer.Restart();
                        vh.Bound = true;
                        elapsed = timer.ElapsedMilliseconds;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In CardAdapter.OnBindViewHolder.");

                }
            }
            
            public override RecyclerView.ViewHolder OnCreateViewHolder2(ViewGroup parent, int viewType)
            {
                View itemview = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.galleryCardList,null);
                /*switch (ItemType)
                {
                    case Core.Gallery.ItemType.Gallery:
                        itemview = new GalleryCard(mcontext);
                        break;
                    case Core.Gallery.ItemType.Collection:
                        itemview = new CollectionCard(mcontext);
                        break;
                }*/
                HPXItemHolder vh = new HPXItemHolder(itemview);
                return vh;
            }
        }

        public class HPXItemHolder : ThumbViewHolder
        {
            
            public TextView Name { get; set; }
            public TextView Info { get; set; }
            public Chip Category { get; set; }
            public TextView Pages { get; set; }
           
            CancellationTokenSource Token { get; set; }

            public override Core.Gallery.HPXItem HPXItem { get; set; }

            public HPXItemHolder(View itemView) : base(itemView, Core.Gallery.ImageSize.Small)
            {
                Thumb = itemView.FindViewById<ImageView>(Resource.Id.imageView);
                Name = itemView.FindViewById<TextView>(Resource.Id.name);
                Info = itemView.FindViewById<TextView>(Resource.Id.info);
                Category = itemView.FindViewById<Chip>(Resource.Id.category);
                Category.SetChipBackgroundColorResource(Resource.Color.colorPrimaryDark);
                Pages = itemView.FindViewById<TextView>(Resource.Id.pages);
                Token = new CancellationTokenSource();
            }

            public class HPXEvent : EventArgs
            {
                public string Url;
                public int Position;
            }

            public void BindCountEvent()
            {
                if (HPXItem != null)
                {
                    HPXItem.CountRead += HPXItem_CountRead;
                }
            }

            private void HPXItem_CountRead(object sender, EventArgs e)
            {
                var h = new Handler(Looper.MainLooper);
                h.Post(() =>
                {
                    if (HPXItem is Core.Gallery.GalleryItem gallery)
                    {
                        if (gallery.ChildCount > 0)
                            Pages.Text = gallery.ChildCount + " page" + (gallery.ChildCount > 1 ? "s" : "");
                    }
                    else if (HPXItem is Core.Gallery.Collection collection)
                    {
                        if (collection.ChildCount > 0)
                            Info.Text = collection.ChildCount + " galler" + (collection.ChildCount > 1 ? "ies" : "y");
                    }
                });

                UnbindCountEvent();
            }

            public void UnbindCountEvent()
            {
                try
                {
                    if (HPXItem != null)
                    {
                        HPXItem.CountRead -= HPXItem_CountRead;
                    }
                }
                catch(Exception ex)
                {

                }
            }

            public void Cancel()
            {
                Token.Cancel();
                Glide.With(ItemView.Context).Clear(Thumb);
                Name.Text = string.Empty;
                Info.Text = string.Empty;
                UnbindCountEvent();
            }
        }

        public class GalleryCardAdapter : HPXCardAdapter
        {
            public GalleryCardAdapter(Context context, Scenes.ViewScene content)
                : base(context, content)
            {
                ItemType = Core.Gallery.ItemType.Gallery;
            }


        }

        public class CollectionCardAdapter : HPXCardAdapter
        {
            public CollectionCardAdapter(Context context, Scenes.ViewScene content)
                : base(context, content)
            {
                ItemType = Core.Gallery.ItemType.Collection;
            }
        }        
    }
}