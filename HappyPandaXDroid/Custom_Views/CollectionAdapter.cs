using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Com.Hippo.Stage;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Com.Bumptech.Glide;

using HPXHolder = HappyPandaXDroid.Custom_Views.CardAdapter.HPXItemHolder;

namespace HappyPandaXDroid.Custom_Views
{
    class CollectionAdapter : RecyclerView.Adapter
    {
        public event EventHandler<CollectionAdapterClickEventArgs> ItemClick;
        public event EventHandler<CollectionAdapterClickEventArgs> ItemLongClick;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<Core.Gallery.HPXItem> mdata;
        Dictionary<int, Core.Media.Image> URLlist;
        Android.Content.Context mcontext;
        int columns = 1;
        Scene collectionScene;

        public CollectionAdapter(Context context, Scene scene)
        {
            collectionScene = scene;
            mcontext = context;
            URLlist = new Dictionary<int, Core.Media.Image>();
        }

        public override int ItemCount
        {
            get { return mdata == null ? 0 : mdata.Count; }
        }

        public async void SetList(List<Core.Gallery.HPXItem> GalleryList)
        {
            mdata = new List<Core.Gallery.HPXItem>();
            foreach (var page in GalleryList)
                page.Image = new Core.Media.Image();
            if (GalleryList != null)
            {
                mdata.AddRange(GalleryList);
                await Task.Run(() => { UpdateUrls(GalleryList); });
            }
        }

        public void UpdateUrls(List<Core.Gallery.HPXItem> newList)
        {
            var items = new List<Core.Gallery.HPXItem>();
            foreach (var item in newList)
            {
                items.Add(item);
            }
            if (items.Count > 0)
            {
                CancellationToken token = ((Scenes.GalleryScene)collectionScene).SceneCancellationTokenSource.Token;
                var urls = Core.Gallery.GetImage(items, Core.Gallery.ItemType.Gallery,
                    token).Result;
                if (token.IsCancellationRequested)
                    return;
                if (urls.Count > 0)
                {
                    foreach (var item in newList)
                    {
                        if (!URLlist.ContainsKey(item.id))
                        {
                            URLlist.Add(item.id, urls[item.id]);
                        }
                        else
                            URLlist[item.id] = urls[item.id];
                    }
                }
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            HPXHolder vh = holder as HPXHolder;
            var page = mdata[position];
            if (vh != null)
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

                vh.Category.Text = Core.Gallery.Categories[mdata[position].category_id].name;

                vh.Bound = true;

            }
        }

        

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            base.OnViewRecycled(holder);
            if (holder is HPXHolder hold)
            {
                hold.Bound = false;
                hold.Url = string.Empty;

                if (!collectionScene.IsDestroyed && !collectionScene.IsActivityDestroyed)
                    hold.Cancel();
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemview = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.galleryCardList, null);
            HPXHolder vh = new HPXHolder(itemview);
            return vh;
        }

        void OnClick(CollectionAdapterClickEventArgs args)
        {
            ItemClick?.Invoke(this, args);
        }
        void OnLongClick(CollectionAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);
    }


    public class CollectionPagerAdapter : RecyclerView.Adapter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<List<Core.Gallery.HPXItem>> mdata;
        Android.Content.Context mcontext;
        Scene previewScene;
        public CollectionPagerAdapter(Context context, Scene scene)
        {
            previewScene = scene;
            mcontext = context;
        }

        public override int ItemCount
        {
            get { return mdata == null ? 0 : mdata.Count; }
        }

        public void SetList(List<List<Core.Gallery.HPXItem>> PageList)
        {
            mdata = new List<List<Core.Gallery.HPXItem>>();
            if (PageList != null)
            {
                mdata.AddRange(PageList);
                NotifyDataSetChanged();
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var vh = holder as PageHolder;
            var previews = mdata[position];

            try
            {
                vh.SetList(previews);
            }
            catch (System.Exception ex)
            {

            }
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            base.OnViewRecycled(holder);
            var hold = holder as PageHolder;
            if (hold != null)
            {
                hold.Recycle();
            }
        }



        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemview = LayoutInflater.From(parent.Context)
            .Inflate(Resource.Layout.PreviewsLayout, parent, false);
            PageHolder vh = new PageHolder(itemview, previewScene);
            return vh;
        }




        class PageHolder : RecyclerView.ViewHolder
        {
            RecyclerView view;
            List<Core.Gallery.HPXItem> previews;
            CollectionAdapter adapter;
            int columns = 1;
            Scene parentscene;
            public PageHolder(View ItemView, Scene scene) : base(ItemView)
            {
                view = ItemView.FindViewById<RecyclerView>(Resource.Id.recyclerView);
                parentscene = scene;
                adapter = new CollectionAdapter(ItemView.Context, scene);
                adapter.ItemClick += Adapter_ItemClick;
                view.SetAdapter(adapter);

                SetColumns();
                var layout = new Helpers.Layouts.ExtraGridLayoutManager(ItemView.Context, columns);
                view.SetLayoutManager(layout);
            }

            private void Adapter_ItemClick(object sender, CollectionAdapterClickEventArgs e)
            {
                Scenes.GalleryScene galleryScene = new Scenes.GalleryScene
                    (e.Gallery);
                parentscene.Stage.PushScene(galleryScene);
            }

            void SetColumns()
            {
                var windo = parentscene.Context.GetSystemService(Context.WindowService);
                var window = windo.JavaCast<IWindowManager>();
                var display = window.DefaultDisplay;


                var metrics = new DisplayMetrics();
                display.GetMetrics(metrics);

                float dpwidth = metrics.WidthPixels / metrics.Density;
                columns = (int)dpwidth / 180; ;

            }

            public void SetList(List<Core.Gallery.HPXItem> list)
            {
                previews = list;
                adapter.SetList(list);

                adapter.NotifyDataSetChanged();
            }


            public void Recycle()
            {
                adapter.SetList(new List<Core.Gallery.HPXItem>());
                adapter.NotifyDataSetChanged();
                view.GetRecycledViewPool().Clear();
            }

        }

    }


        public class CollectionAdapterClickEventArgs : EventArgs
        {
            public View View { get; set; }
            public int Position { get; set; }
        public Core.Gallery.GalleryItem Gallery { get; set; }
        }
    }

