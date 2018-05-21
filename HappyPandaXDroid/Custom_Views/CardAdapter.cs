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
    public class CardAdapter
    {
        public abstract class HPXCardAdapter : EasyAdapter
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            CancellationTokenSource AdapterCancellationTokenSource = new CancellationTokenSource();
            public EventHandler<int> ItemClick;
            public event EventHandler<HPXItemHolder.HPXEvent> ImagesReady;
            protected Core.Gallery.ItemType ItemType;
            public Scenes.ViewScene content;
            bool IsImageReady = false;
            void OnClick(int position)
            {
                ItemClick?.Invoke(this, position);
            }

            public Scene scene;

            List<Core.Gallery.HPXItem> mdata;
            public Dictionary<int, string> UrlList;
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
                Task.Run(()=>UpdateUrls(newItems));
            }

            public async void Prepend(List<Core.Gallery.HPXItem> newItems)
            {
                var backupData = new List<Core.Gallery.HPXItem>(mdata);
                mdata.Clear();
                var urls = new Dictionary<int, string>(UrlList);
                UrlList.Clear();
                mdata.AddRange(newItems);
                mdata.AddRange(backupData);
                Task.Run(() =>
                {
                    UpdateUrls(newItems);
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
                
                });
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
                ImagesReady.Invoke(this, null);
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
                        if (IsImageReady) {
                            if (UrlList.ContainsKey(vh.Id))
                            {
                                vh.Url = UrlList[vh.Id];
                                Glide.With(holder.ItemView.Context).Load(vh.Url).Into(vh.Thumb);
                            }
                        }
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
                HPXItemHolder vh = new HPXItemHolder(itemview,this);
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

        public class HPXItemHolder : RecyclerView.ViewHolder
        {
            public event EventHandler<HPXEvent> ImageCompleted;
            HPXCardAdapter HPXCardAdapter;
            public Core.Gallery.HPXItem HPXItem;
            public Core.Gallery.ItemType ItemType;
            public ImageView Thumb { get; set; }
            public TextView Name { get; set; }
            public TextView Info { get; set; }
            public string Url = string.Empty;
            public int Id = 0;
            CancellationTokenSource Token {get;set;}
            public HPXItemHolder(View itemView, HPXCardAdapter adapter) : base(itemView)
            {
                HPXCardAdapter = adapter;
                Thumb = itemView.FindViewById<ImageView>(Resource.Id.imageView);
                Name = itemView.FindViewById<TextView>(Resource.Id.name);
                Info = itemView.FindViewById<TextView>(Resource.Id.info);
                Token = new CancellationTokenSource();
                if(adapter!=null)
                adapter.ImagesReady += HPXItemHolder_ImageCompleted;
            }

            private void HPXItemHolder_ImageCompleted(object sender, HPXEvent e)
            {
                if (Id > 0)
                {
                    if (string.IsNullOrEmpty(Url))
                    {
                        if (HPXCardAdapter.UrlList.ContainsKey(Id))
                        {
                            Url = HPXCardAdapter.UrlList[Id];
                            var h = new Handler(Looper.MainLooper);
                            h.Post(() => Glide.With(ItemView.Context).Load(Url).Into(Thumb));
                        }
                    }
                }
            }

            public class  HPXEvent: EventArgs
            {
                public string Url;
                public int Position;
            }

            public void RegisterImageReadyEvent(EventHandler<HPXEvent> eventHandler)
            {
                
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