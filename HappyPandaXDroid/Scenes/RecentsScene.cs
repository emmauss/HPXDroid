using Android.Content;
using Android.Runtime;
using Android.Views;

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

namespace HappyPandaXDroid.Scenes
{
    class RecentsScene : HPXScene
    {

        Toolbar toolbar;
        DrawerLayout navDrawer;
        Clans.Fab.FloatingActionMenu fam;
        EasyRecyclerView.EasyRecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        int columns = 1;
        GalleryCardAdapter adapter;
        AppBarLayout appBarLayout;
        public int activityId;
        public string activityName;
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
            
            activityName = "LibraryActivity " + activityId;
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

            SetColumns();

            mLayoutManager = new ExtraLayoutManager(this.Context, columns, GridLayoutManager.Vertical, false);


            mRecyclerView.SetAdapter(adapter);

            mRecyclerView.SetLayoutManager(mLayoutManager);
        }

        void SetColumns()
        {
            var windo = Context.GetSystemService(Context.WindowService);
            var window = windo.JavaCast<IWindowManager>();
            var display = window.DefaultDisplay;
            int gridFactor = 0;
            float w = display.Width;
            gridFactor = (int)(Math.Ceiling(w / 300d));
            if (Context.Resources.Configuration.Orientation == Android.Content.Res.Orientation.Landscape)
            {
                gridFactor = (int)(Math.Ceiling(w / 400d));
                columns = gridFactor * 2;
            }
            else
                columns = gridFactor;
        }

        private void AppBarLayout_Drag(object sender, View.DragEventArgs e)
        {
            e.Handled = true;
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            SetColumns();
            mLayoutManager = new GridLayoutManager(this.Context, columns);
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

        public class ExtraLayoutManager : GridLayoutManager
        {
            private static readonly int DEFAULT_EXTRA_LAYOUT_SPACE = 800;
            private int extraLayoutSpace = -1;
            private Context context;


            public ExtraLayoutManager(Context context, int columns) : base(context, columns)
            {
                this.context = context;
            }

            public ExtraLayoutManager(Context context, int columns, int extraLayoutSpace) : base(context, columns)
            {
                this.context = context;
                this.extraLayoutSpace = extraLayoutSpace;
            }



            public ExtraLayoutManager(Context context, int columns, int orientation, bool reverseLayout)
                : base(context, columns, orientation, reverseLayout)
            {
                this.context = context;
            }

            public void SetExtraLayoutSpace(int extraLayoutSpace)
            {
                this.extraLayoutSpace = extraLayoutSpace;
            }

            protected override int GetExtraLayoutSpace(RecyclerView.State state)
            {
                if (extraLayoutSpace > 0)
                {
                    return extraLayoutSpace;
                }
                else
                    return DEFAULT_EXTRA_LAYOUT_SPACE;
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
                if (holder is GalleryCardHolder vh)
                {
                    GalleryScene galleryScene = new GalleryScene
                        (Core.JSON.Serializer.SimpleSerializer.Serialize(vh.gcard.Gallery), vh.gcard.ThumbnailPath);
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
            Android.Content.Context mcontext;
            public GalleryCardAdapter(Context context,RecentsScene scene)
            {
                rscene = scene;
                mcontext = context;
                mdata = Core.Media.Recents.RecentList;
            }

            public override int ItemCount
            {
                get { return mdata.Count; }
            }

            public void ResetList()
            {
                mdata = Core.Media.Recents.RecentList;
                this.NotifyDataSetChanged();
            }

            public override void OnViewRecycled(Java.Lang.Object holder)
            {
                base.OnViewRecycled(holder);
                var hold = holder as GalleryCardHolder;
                if (hold != null)
                {
                    hold.gcard.Reset();
                    hold.gcard.Recycle();
                }

            }


            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                GalleryCardHolder vh = holder as GalleryCardHolder;
                try
                {
                    vh.Bind(mdata[position]);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In HPContent.ListViewAdapter.OnBindViewHolder.");

                }
                //vh.gcard.SetOnClickListener(new GalleryCardClickListener());
            }


            public override RecyclerView.ViewHolder OnCreateViewHolder2(ViewGroup parent, int viewType)
            {
                View itemview = new Custom_Views.GalleryCard(mcontext);
                GalleryCardHolder vh = new GalleryCardHolder(itemview);
                return vh;
            }
        }

        public class GalleryCardHolder : RecyclerView.ViewHolder
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public Custom_Views.GalleryCard gcard;

            public GalleryCardHolder(View itemView) : base(itemView)
            {
                gcard = (Custom_Views.GalleryCard)itemView;
            }

            public void Bind(Core.Gallery.GalleryItem item)
            {
                gcard.Gallery = item;
                Task.Run(() =>
                {
                    gcard.Refresh();
                });
            }


        }

    }
}