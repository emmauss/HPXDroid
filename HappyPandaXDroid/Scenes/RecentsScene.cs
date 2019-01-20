using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.OS;
using System.Threading;
using Android.App;
using Android.Util;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using NLog;
using Com.Hippo.Stage;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using EasyRecyclerView;
using Android.Content.Res;
using Com.Bumptech.Glide;

namespace HappyPandaXDroid.Scenes
{
    class RecentsScene : HPXScene
    {

        Toolbar toolbar;
        bool isGrid;
        Clans.Fab.FloatingActionMenu fam;
        EasyRecyclerView.EasyRecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        int columns = 1;
        GalleryCardAdapter adapter;
        AppBarLayout appBarLayout;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        Clans.Fab.FloatingActionButton mClearFAB;
        View MainView;

        protected override View OnCreateView(LayoutInflater p0, ViewGroup p1)
        {
            MainView = p0.Inflate(Resource.Layout.RecentsScene, p1, false);
            Initialize();

            return MainView;
        }

        void Initialize()
        {
            toolbar = MainView.FindViewById<Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Recents";
            
            appBarLayout = MainView.FindViewById<AppBarLayout>(Resource.Id.appbar);
            appBarLayout.Drag += AppBarLayout_Drag;
            

            mClearFAB = MainView.FindViewById<Clans.Fab.FloatingActionButton>(Resource.Id.fabClear);
            mClearFAB.SetImageResource(Resource.Drawable.v_clear_all_dark_x24);
            fam = MainView.FindViewById<Clans.Fab.FloatingActionMenu>(Resource.Id.fam);
            FABClickListener fabclick = new FABClickListener(this);
            mClearFAB.SetOnClickListener(fabclick);


            mRecyclerView = MainView.FindViewById<EasyRecyclerView.EasyRecyclerView>(Resource.Id.recyclerView);
            adapter = new GalleryCardAdapter(this.Context,this);
            mRecyclerView.SetOnItemClickListener(new RecyclerViewClickListener());
            isGrid = Core.App.Settings.IsGrid;
            SetColumns();

            mLayoutManager = new Helpers.Layouts.ExtraGridLayoutManager(this.Context, columns, GridLayoutManager.Vertical, false);


            mRecyclerView.SetAdapter(adapter);

            mRecyclerView.SetLayoutManager(mLayoutManager);

            //adapter.ResetList();
        }

        void SetColumns()
        {
            isGrid = Core.App.Settings.IsGrid;
            var windo = Context.GetSystemService(Context.WindowService);
            var window = windo.JavaCast<IWindowManager>();
            var display = window.DefaultDisplay;
            if (isGrid)
            {


                var metrics = new DisplayMetrics();
                display.GetMetrics(metrics);

                float dpwidth = metrics.WidthPixels / metrics.Density;
                columns = (int)dpwidth / 180; ;
            }
            else
            {
                var rotation = display.Rotation;
                switch (rotation)
                {
                    case SurfaceOrientation.Rotation0:
                    case SurfaceOrientation.Rotation270:
                        columns = 1;
                        break;
                    default:
                        columns = 2;
                        break;

                }

            }


        }

        protected override void OnResume()
        {
            base.OnResume();
            if (isGrid != Core.App.Settings.IsGrid)
            {
                SetColumns();
                mLayoutManager = new Helpers.Layouts.ExtraGridLayoutManager(this.Context, columns, GridLayoutManager.Vertical, false);
                mRecyclerView.SetLayoutManager(mLayoutManager);
                mRecyclerView.GetRecycledViewPool().Clear();
            }
        }

        private void AppBarLayout_Drag(object sender, View.DragEventArgs e)
        {
            e.Handled = true;
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            isGrid = Core.App.Settings.IsGrid;
            SetColumns();
            mLayoutManager = new Helpers.Layouts.ExtraGridLayoutManager(this.Context, columns, GridLayoutManager.Vertical, false);
            mRecyclerView.SetLayoutManager(mLayoutManager);
        }

        class FABClickListener : Java.Lang.Object, View.IOnClickListener
        {
            RecentsScene main;
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public FABClickListener(RecentsScene main)
            {
                this.main = main;
            }
            public void OnClick(View v)
            {
                var fab = (Clans.Fab.FloatingActionButton)v;
                switch (fab.Id)
                {
                    case Resource.Id.fabClear:
                        Core.Media.Recents.ClearRecents();
                        main.adapter.NotifyDataSetChanged();
                        break;

                }
                main.fam.Close(true);
            }
        }

        

        public class AutoFitGridLayout : GridLayoutManager
        {
            private int mColumnWidth;
            private bool mColumnWidthChanged = true;

            public AutoFitGridLayout(Context context, int columnWidth) : base(context, 1)
            {
                /* Initially set spanCount to 1, will be changed automatically later. */
                SetColumnWidth(CheckedColumnWidth(context, columnWidth));
            }

            public AutoFitGridLayout(Context context, int columnWidth, int orientation, bool reverseLayout) : base(context, 1, orientation, reverseLayout)
            {
                /* Initially set spanCount to 1, will be changed automatically later. */

                SetColumnWidth(CheckedColumnWidth(context, columnWidth));
            }

            private int CheckedColumnWidth(Context context, int columnWidth)
            {
                if (columnWidth <= 0)
                {
                    /* Set default columnWidth value (48dp here). It is better to move this constant
                    to static constant on top, but we need context to convert it to dp, so can't really
                    do so. */
                    columnWidth = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 48,
                            context.Resources.DisplayMetrics);
                }
                return columnWidth;
            }

            public void SetColumnWidth(int newColumnWidth)
            {
                if (newColumnWidth > 0 && newColumnWidth != mColumnWidth)
                {
                    mColumnWidth = newColumnWidth;
                    mColumnWidthChanged = true;
                }
            }

            public override void OnLayoutChildren(RecyclerView.Recycler recycler, RecyclerView.State state)
            {
                int width = Width;
                int height = Height;
                if (mColumnWidthChanged && mColumnWidth > 0 && width > 0 && height > 0)
                {
                    int totalSpace;
                    if (Orientation == Vertical)
                    {
                        totalSpace = width - PaddingRight - PaddingLeft;
                    }
                    else
                    {
                        totalSpace = height - PaddingTop - PaddingBottom;
                    }
                    int spanCount = System.Math.Max(1, totalSpace / mColumnWidth);
                    SpanCount = spanCount;
                    mColumnWidthChanged = false;
                }
                base.OnLayoutChildren(recycler, state);
            }
        }

        public class RecyclerViewClickListener : EasyRecyclerView.EasyRecyclerView.IOnItemClickListener
        {
            public void OnItemClick(EasyRecyclerView.EasyRecyclerView parent, RecyclerView.ViewHolder holder)
            {
                if (holder is Custom_Views.CardAdapter.HPXItemHolder vh)
                {
                    GalleryScene galleryScene = new GalleryScene
                        (vh.HPXItem as Core.Gallery.GalleryItem);
                    var pscene = (((GalleryCardAdapter)parent.GetAdapter()).rscene);
                    pscene.Stage.PushScene(galleryScene);
                }

            }
        }


        public class GalleryCardAdapter : EasyAdapter
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public EventHandler<int> ItemClick;
            void OnClick(int position)
            {
                ItemClick?.Invoke(this, position);
            }

            public RecentsScene rscene;
            public List<Core.Gallery.GalleryItem> mdata;
            CancellationTokenSource CancellationTokenSource;
            Android.Content.Context mcontext;
            Dictionary<int, Core.Media.Image> UrlList = new Dictionary<int, Core.Media.Image>();
            public GalleryCardAdapter(Context context,RecentsScene scene)
            {
                rscene = scene;
                mcontext = context;
                CancellationTokenSource = new CancellationTokenSource();

                ResetList();
            }

            public override int ItemCount
            {
                get { return mdata.Count; }
            }

            public async void ResetList()
            {
                mdata = Core.Media.Recents.RecentList;
                UrlList.Clear();
                await Task.Run(()=>UpdateUrls(), CancellationTokenSource.Token);                
            }


            public void UpdateUrls()
            {

                var items = new List<Core.Gallery.HPXItem>();

                foreach (var item in mdata)
                {
                    if (item.Thumb == null)
                        item.Thumb = new Core.Media.Image();
                    items.Add(item);
                }

                var h = new Handler(Looper.MainLooper);
                h.Post(() =>
                this.NotifyDataSetChanged());

                if (items.Count > 0)
                {
                    var urls = Core.Gallery.GetImage(items,Core.Gallery.ItemType.Gallery,CancellationTokenSource.Token)
                        .Result;
                    if (urls.Count > 0)
                    {
                        foreach (var item in mdata)
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

            public override void OnViewRecycled(Java.Lang.Object holder)
            {
                base.OnViewRecycled(holder);
                var hold = holder as Custom_Views.CardAdapter.HPXItemHolder;
                if (hold != null)
                {
                    hold.Bound = false;
                    hold.Cancel();
                }

            }


            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                Custom_Views.CardAdapter.HPXItemHolder vh = holder as Custom_Views.CardAdapter.HPXItemHolder;
                try
                {
                    if (vh.ItemView != null)
                    {
                        var gallery = (Core.Gallery.GalleryItem)mdata[position];
                        vh.HPXItem = gallery;
                        vh.Bound = true;
                        vh.Name.Text = gallery.titles[0].name;
                        if (gallery.artists.Count > 0)
                            if (gallery.artists[0].Names.Count > 0)
                                vh.Info.Text = gallery.artists[0].Names[0].name;
                        vh.Category.Text = Core.Gallery.Categories[gallery.category_id].name;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In CardAdapter.OnBindViewHolder.");

                }
            }


            public override RecyclerView.ViewHolder OnCreateViewHolder2(ViewGroup parent, int viewType)
            {
                View itemview = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.galleryCardList, null);
                Custom_Views.CardAdapter.HPXItemHolder vh = new Custom_Views.CardAdapter.HPXItemHolder(itemview);
                return vh;
            }
        }

    }
}