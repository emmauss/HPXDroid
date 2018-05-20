using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using System.Threading;
using Android.Support.V7.View;
using Android.Support.V7.App;
using Android.Support.V4.View;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using ProgressView = XamarinBindings.MaterialProgressBar;
using EasyRecyclerView;
using EasyRecyclerView.Addons;
using NLog;
using Android.Content.Res;
using RefreshLayout = Com.Hippo.Refreshlayout;
using Com.Hippo.Stage;
using Com.Bumptech.Glide;

namespace HappyPandaXDroid.Custom_Views
{
    class CardAdapter
    {
        public abstract class HPXCardAdapter : EasyAdapter
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            CancellationTokenSource AdapterCancellationTokenSource = new CancellationTokenSource();
            public EventHandler<int> ItemClick;
            protected Core.Gallery.ItemType ItemType;
            public Scenes.ViewScene content;
            void OnClick(int position)
            {
                ItemClick?.Invoke(this, position);
            }

            public Scene scene;

            List<Core.Gallery.HPXItem> mdata;
            Dictionary<int, string> UrlList;
            Android.Content.Context mcontext;
            public HPXCardAdapter(Context context, Scenes.ViewScene content)
            {
                mcontext = context;
                this.content = content;
                mdata = content.CurrentList;
                UrlList = new Dictionary<int, string>();
                AdapterCancellationTokenSource = new CancellationTokenSource();
            }

            public override int ItemCount
            {
                get { return mdata.Count; }
            }

            public async void Add(List<Core.Gallery.HPXItem> newItems)
            {
                mdata.AddRange(newItems);
                await Task.Run(()=>UpdateUrls(newItems));
            }

            public async void Prepend(List<Core.Gallery.HPXItem> newItems)
            {
                var backupData = new List<Core.Gallery.HPXItem>(mdata);
                mdata.Clear();
                var urls = new Dictionary<int, string>(UrlList);
                UrlList.Clear();
                mdata.AddRange(newItems);
                await Task.Run(() => UpdateUrls(newItems));
                mdata.AddRange(backupData);
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
            }

            public void UpdateUrls(List<Core.Gallery.HPXItem> newItems)
            {
                List<int> ids = new List<int>();
                foreach (var item in newItems)
                {
                    ids.Add(item.id);
                }
                if (ids.Count > 0)
                {
                    var urls = Core.Gallery.GetImage(ids.ToArray(), ItemType.ToString(),
                        AdapterCancellationTokenSource.Token).Result;
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
                UpdateUrls(mdata);
                this.NotifyDataSetChanged();
            }

            public override void OnViewRecycled(Java.Lang.Object holder)
            {
                base.OnViewRecycled(holder);
                if (holder is TestHolder hold)
                {
                    hold.Cancel();
                }
                    /*if (holder is HPXCardHolder hold)
                    {
                        Task.Run(() =>
                        {
                            hold.gcard.Reset();
                            hold.gcard.Recycle();
                        });
                    }*/

                }

            public async override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                TestHolder vh = holder as TestHolder;
                try
                {
                    /*Task.Run(() =>
                    vh.Bind(mdata[position]));*/
                    if (vh.ItemView != null)
                    {
                        var gallery = (Core.Gallery.GalleryItem)mdata[position];
                        vh.HPXItem = gallery;
                        vh.Name.Text = gallery.titles[0].name;
                        /*vh.Open -= Vh_Open;
                        vh.Open += Vh_Open;*/
                        if (gallery.artists.Count > 0)
                            if (gallery.artists[0].Names.Count > 0)
                                vh.Info.Text = gallery.artists[0].Names[0].name;
                        Glide.With(holder.ItemView.Context).Load(UrlList[gallery.id]).Into(vh.Thumb);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In CardAdapter.OnBindViewHolder.");

                }
            }

            private void Vh_Open(object sender, TestHolder.HPXEvent e)
            {
               
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
                TestHolder vh = new TestHolder(itemview);
                return vh;
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

        public class HPXCardHolder : RecyclerView.ViewHolder
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public ItemCard gcard;

            public HPXCardHolder(View itemView) : base(itemView)
            {
                gcard = (ItemCard)itemView;
            }

            public void Bind(Core.Gallery.HPXItem item)
            {
                gcard.HPXItem = item;
                Task.Run(() =>
                {
                    gcard.Recycle();
                    gcard.Refresh();
                });
            }
        }

        public class TestHolder : RecyclerView.ViewHolder
        {
            static object lockObj;
            public Core.Gallery.HPXItem HPXItem;
            public ImageView Thumb { get; set; }
            public TextView Name { get; set; }
            public TextView Info { get; set; }
            CancellationTokenSource Token {get;set;}
            public event EventHandler<HPXEvent> Open;
            public TestHolder(View itemView) : base(itemView)
            {
                Thumb = itemView.FindViewById<ImageView>(Resource.Id.imageView);
                Name = itemView.FindViewById<TextView>(Resource.Id.name);
                Info = itemView.FindViewById<TextView>(Resource.Id.info);
                Token = new CancellationTokenSource();
            }            

            public class  HPXEvent: EventArgs
            {
                public Core.Gallery.HPXItem HPXItem;
                public int Position;
            }

            public async Task<bool> LoadThumb(int id, Core.Gallery.ItemType itemType)
            {
                var h = new Handler(Looper.MainLooper);
                if (lockObj == null)
                    lockObj = new object();
                Token = new CancellationTokenSource();
                string path = string.Empty;
                lock (lockObj)
                {
                    if (!Core.Gallery.IsItemCached(id, "medium", itemType.ToString()))
                    {
                        if (Token.IsCancellationRequested)
                            return false;
                        if (!(Core.Gallery.IsSourceExist(Core.Gallery.ItemType.Gallery, id, Token.Token).Result))
                        {
                            if (Token.IsCancellationRequested)
                                return false;
                             h.Post(() => Glide.With(ItemView.Context).Load(Resource.Drawable.image_failed).Into(Thumb));
                            return false;
                        }
                        if (Token.IsCancellationRequested)
                            return false;
                        path = Core.Gallery.GetImage(id, "Gallery", false, Token.Token).Result;
                        if (Token.IsCancellationRequested || string.IsNullOrWhiteSpace(path))
                            return false;
                    }
                    else
                    {
                        if (!Core.Gallery.GetCachedPagePath(id, out path, itemType.ToString(), "medium"))
                            return false;
                    }
                    h.Post(() => Glide.With(ItemView.Context).Load(path).Into(Thumb));
                }
                return true;
            }

            public void Cancel()
            {
                Token.Cancel();
                Glide.With(ItemView.Context).Clear(Thumb);
                Name.Text = string.Empty;
                Info.Text = string.Empty;
            }
        }
    }
}