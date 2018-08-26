using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
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

            public Scene scene;

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
                    item.Image = new Media.Image();
                }
                Task.Run(()=>UpdateUrls(newItems));
            }

            public async void Prepend(List<Core.Gallery.HPXItem> newItems)
            {
                foreach (var item in newItems)
                {
                    item.Image = new Media.Image();
                }
                var backupData = new List<Core.Gallery.HPXItem>(mdata);
                mdata.Clear();
                var urls = new Dictionary<int, Media.Image>(UrlList);
                UrlList.Clear();
                mdata.AddRange(newItems);
                mdata.AddRange(backupData);
                Task.Run(() =>
                {
                    UpdateUrls(newItems);
                    if (AdapterCancellationTokenSource.IsCancellationRequested)
                        return;
                    if (urls.Count > 0)
                    {
                        foreach (var item in mdata)
                        {
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
                    ids.Add(item.id);
                }
                if (ids.Count > 0)
                {
                    var urls = Core.Gallery.GetImage(newItems, ItemType.ToString(),
                        AdapterCancellationTokenSource.Token).Result;
                    if (AdapterCancellationTokenSource.IsCancellationRequested)
                        return;
                    if (urls.Count > 0)
                    {
                        foreach(var item in newItems)
                        {
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
                        vh.Id = mdata[position].id;
                        if (mdata[position] is Core.Gallery.GalleryItem gallery)
                        {
                            vh.HPXItem = gallery;
                            vh.Name.Text = gallery.titles[0].name;
                            if (gallery.artists.Count > 0)
                                if (gallery.artists[0].Names.Count > 0)
                                    vh.Info.Text = gallery.artists[0].Names[0].name;
                        }
                        else
                         if (mdata[position] is Core.Gallery.Collection collection)
                        {
                            vh.HPXItem = collection;
                            vh.Name.Text = collection.name;
                            if (collection.galleries!=null)
                                    vh.Info.Text = collection.galleries.Length + " galler" + 
                                    (collection.galleries.Length>1?"ies":"y");
                        }

                        vh.Bound = true;
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

            private void Image_Ready(object sender, Media.Image.ImageLoadEvent e)
            {
                LoadImage();
            }

            CancellationTokenSource Token { get; set; }

            public override Core.Gallery.HPXItem HPXItem { get; set; }

            public HPXItemHolder(View itemView) : base(itemView)
            {
                Thumb = itemView.FindViewById<ImageView>(Resource.Id.imageView);
                Name = itemView.FindViewById<TextView>(Resource.Id.name);
                Info = itemView.FindViewById<TextView>(Resource.Id.info);
                Token = new CancellationTokenSource();
            }

            async void LoadImage()
            {
                await Task.Delay(1000);
                var token = CancellationTokenSource.Token;
                string url = App.Server.GetCommandValue(HPXItem.CommandId, HPXItem.id,
                    string.Empty, ref token);
                if (!string.IsNullOrWhiteSpace(url))
                {
                    var h = new Handler(Looper.MainLooper);
                    h.Post(() => Glide.With(Thumb.Context).Load(url).Into(Thumb));
                    HPXItem.Image.Uri = url;
                }
            }

            public class HPXEvent : EventArgs
            {
                public string Url;
                public int Position;
            }

            public void Cancel()
            {
                Token.Cancel();
                Glide.With(ItemView.Context).Clear(Thumb);
                Name.Text = string.Empty;
                Info.Text = string.Empty;
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