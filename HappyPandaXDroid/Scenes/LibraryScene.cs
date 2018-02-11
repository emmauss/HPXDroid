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


namespace HappyPandaXDroid.Scenes
{
    public class LibraryScene : HPXScene, Android.Support.V7.Widget.SearchView.IOnQueryTextListener
    {

        Toolbar toolbar;
        Clans.Fab.FloatingActionMenu fam;
        string title, query;
        AppBarLayout appBarLayout;
        bool isGrid = false;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        Clans.Fab.FloatingActionButton mRefreshFab;
        Clans.Fab.FloatingActionButton mJumpFab;
        View MainView { get; set; }
        public List<Core.Gallery.GalleryItem> CurrentList = new List<Core.Gallery.GalleryItem>();
        public Custom_Views.PageSelector mpageSelector;
        EasyRecyclerView.EasyRecyclerView mRecyclerView;
        bool IsRefreshing = false;
        PageCheckListener listener;
        bool initialized = false;

        ProgressView.MaterialProgressBar mProgressView;
        public int count = 0, lastindex = 0, columns;
        RefreshLayout.RefreshLayout mRefreshLayout;
        RecyclerView.LayoutManager mLayoutManager;
        FrameLayout mErrorFrame;
        ImageView mErrorImage;
        TextView mErrorText;
        GalleryCardAdapter adapter;
        CountDown countDown;

        bool isLoading = false;
        bool IsLoading
        {
            get { return isLoading; }
            set
            {
                if (value == false)
                    Task.Run(() => {
                        countDown.Start();
                    });
                else
                    isLoading = value;
            }
        }
        int page = 0;
        public int CurrentPage
        {
            get
            {
                return page;
            }
            set
            {
                page = value;
            }
        }

        string current_query = string.Empty;
        public String Current_Query
        {
            get
            {
                return current_query;
            }
            set
            {
                current_query = value;
                SetMainLoading(true);
                Refresh();
            }
        }
        public DialogEventListener dialogeventlistener;

        protected override void OnCreate(Bundle p0)
        {
            base.OnCreate(p0);
        }

        protected override View OnCreateView(LayoutInflater p0, ViewGroup p1)
        {
            MainView = p0.Inflate(Resource.Layout.LibraryScene, p1,false);
            Initialize();

            return MainView;
        }

        private void Initialize()
        {
            toolbar = MainView.FindViewById<Toolbar>(Resource.Id.toolbar);
            
            appBarLayout = MainView.FindViewById<AppBarLayout>(Resource.Id.appbar);
            appBarLayout.Drag += AppBarLayout_Drag;
            
            mRefreshFab = MainView.FindViewById<Clans.Fab.FloatingActionButton>(Resource.Id.fabRefresh);
            mJumpFab = MainView.FindViewById<Clans.Fab.FloatingActionButton>(Resource.Id.fabJumpTo);
            mJumpFab.SetImageResource(Resource.Drawable.v_go_to_dark_x24);
            mRefreshFab.SetImageResource(Resource.Drawable.v_refresh_dark_x24);
            fam = MainView.FindViewById<Clans.Fab.FloatingActionMenu>(Resource.Id.fam);
            FABClickListener fabclick = new FABClickListener(this);
            mJumpFab.SetOnClickListener(fabclick);
            mRefreshFab.SetOnClickListener(fabclick);
            countDown = new CountDown(500, 10, this);
            listener = new PageCheckListener(this);
            logger.Info("Initializing HPContent");
            mRecyclerView = MainView.FindViewById<EasyRecyclerView.EasyRecyclerView>(Resource.Id.recyclerView);
            adapter = new GalleryCardAdapter(this.Context, this);
            mRecyclerView.SetOnItemClickListener(new RecyclerViewClickListener());
            mRefreshLayout = MainView.FindViewById<RefreshLayout.RefreshLayout>(Resource.Id.refresh_layout);
            mProgressView = MainView.FindViewById<ProgressView.MaterialProgressBar>(Resource.Id.progress_view);
            mProgressView.Visibility = ViewStates.Gone;
            mErrorFrame = MainView.FindViewById<FrameLayout>(Resource.Id.error_frame);
            mErrorFrame.Visibility = ViewStates.Gone;
            mErrorImage = MainView.FindViewById<ImageView>(Resource.Id.error_image);
            mErrorImage.SetImageResource(Resource.Drawable.big_weird_face);
            mErrorText = MainView.FindViewById<TextView>(Resource.Id.error_text);
            mErrorText.Text = "Error";
            mErrorImage.Click += MErrorFrame_Click;
            SetBottomLoading(false);

            
            mRecyclerView.AddOnScrollListener(listener);
            SetColumns();
            mLayoutManager = new Helpers.Layouts.ExtraGridLayoutManager(this.Context, columns, GridLayoutManager.Vertical, false);


            mRecyclerView.SetAdapter(adapter);
            mRefreshLayout.HeaderRefresh += MRefreshLayout_HeaderRefresh;
            mRefreshLayout.FooterRefresh += MRefreshLayout_FooterRefresh;

            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRefreshLayout.SetHeaderColorSchemeResources(new int[] {
                Resource.Color.loading_indicator_red,
                Resource.Color.loading_indicator_purple,
                Resource.Color.loading_indicator_blue,
                Resource.Color.loading_indicator_cyan,
                Resource.Color.loading_indicator_green,
                Resource.Color.loading_indicator_yellow });
            mRefreshLayout.SetFooterColorSchemeResources(
                    Resource.Color.loading_indicator_blue,
                    Resource.Color.loading_indicator_green,
                    Resource.Color.loading_indicator_orange,
                    Resource.Color.loading_indicator_red);


            SetMainLoading(true);
            
            dialogeventlistener = new DialogEventListener(this);
            initialized = true;
            logger.Info("HPContent Initialized");
            OnCreateOptionsMenu();
            if (query.Trim() != string.Empty)
            {
                
                toolbar.Title = title.Replace("__namespace__:","misc:");
            }
            else
                toolbar.Title = "Library";
            Current_Query = Parse(query, false);
        }

        private void MRefreshLayout_FooterRefresh(object sender, EventArgs e)
        {
            logger.Info("Swipe Footer Refreshing");
            SetBottomLoading(true);
            ThreadStart load = new ThreadStart(NextPage);
            Thread thread = new Thread(load);
            thread.Start();
       
            
        }

        private void MRefreshLayout_HeaderRefresh(object sender, EventArgs e)
        {
            var h = new Handler(Looper.MainLooper);
            if (!IsRefreshing)
                Task.Run(async () =>
                {
                    IsRefreshing = true;
                    if (CurrentPage != 0)
                    {
                        await Task.Run(async () =>
                        {
                            PreviousPage();
                        });
                    }
                    else
                    {
                        logger.Info("Swipe Header Refreshing");

                        await Task.Delay(10);
                        Refresh();
                    }

                    h.Post(() =>
                    {
                        mRefreshLayout.HeaderRefreshing = false;
                        mRefreshLayout.FooterRefreshing = false;
                    });
                    IsRefreshing = false;
                });
        }

        public LibraryScene(string title,string query) : base()
        {
            this.title = title;
            this.query = query;
        }

        private void AppBarLayout_Drag(object sender, View.DragEventArgs e)
        {
            e.Handled = true;
        }

        protected override void OnSaveViewState(View p0, Bundle p1)
        {
            var bundle = p1;
            bundle.PutString("query", Current_Query);
            base.OnSaveViewState(p0, p1);
        }

        protected override void OnRestoreViewState(View p0, Bundle p1)
        {
            var bundle = p1;
            query = bundle.GetString("query");
            base.OnRestoreViewState(p0, p1);
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


       
        public void InitLibrary()
        {
            int tries = 0;
            if (Core.Net.Connect())
                Task.Run(async () =>
                {
                    while (tries < 5 && !Core.Net.Connected)
                    {
                        tries++;
                        await Task.Delay(5000);
                    }
                    if (tries >= 5 && !Core.Net.Connected)
                    {
                        {
                            var h = new Handler(Looper.MainLooper);
                            h.Post(() =>
                            {
                                SetMainLoading(false);
                                SetError(true);
                            });
                            return;
                        }
                    }
                    tries = 0;
                    if (Core.Net.Connected)
                    {
                        if (Core.App.Server.Info.session != string.Empty && Core.App.Server.Info.session != null)
                        {
                            {
                                logger.Info("Getting Library");
                                GetLib();
                                var h = new Handler(Looper.MainLooper);
                                h.Post(() =>
                                {
                                    SetMainLoading(false);
                                    adapter.ResetList();
                                    lastindex = CurrentList.Count - 1;
                                    SetMainLoading(false);

                                });
                                GetTotalCount();

                            }
                        }
                    }
                    else
                    {
                        var h = new Handler(Looper.MainLooper);
                        h.Post(() =>
                        {
                            SetMainLoading(false);
                            SetError(true);
                        });
                    }
                });
            else
            {
                var h = new Handler(Looper.MainLooper);
                h.Post(() =>
                {
                    SetMainLoading(false);
                    SetError(true);
                });

            }
        }

        private void MErrorFrame_Click(object sender, EventArgs e)
        {
            SetMainLoading(true);
            Refresh();
        }

        private void MRefreshLayout_OnFooterRefresh(object sender, EventArgs e)
        {

            SetBottomLoading(true);
            ThreadStart load = new ThreadStart(NextPage);
            Thread thread = new Thread(load);
            thread.Start();
        }

        private void MRefreshLayout_OnHeaderRefresh(object sender, EventArgs e)
        {
            if (!IsRefreshing)
                Task.Run(async () =>
                {
                    if (CurrentPage != 0)
                    {
                        await Task.Run(() =>
                        {
                            SetBottomLoading(true);
                            PreviousPage();
                            SetBottomLoading(false);
                        });
                    }
                    else
                    {
                        logger.Info("Swipe Header Refreshing");
                        IsRefreshing = true;
                        await Task.Delay(10);
                        Refresh();
                    }
                    IsRefreshing = false;
                    var h = new Handler(Looper.MainLooper);
                    h.Post(() =>
                    {
                        mRefreshLayout.HeaderRefreshing = false;
                        mRefreshLayout.FooterRefreshing = false;
                    });
                });
        }

        class OnRefreshListener : Java.Lang.Object, RefreshLayout.RefreshLayout.IOnRefreshListener
        {
            LibraryScene content;
            public OnRefreshListener(LibraryScene content)
            {
                this.content = content;
            }
            public void OnFooterRefresh()
            {
                logger.Info("Swipe Footer Refreshing");
                content.SetBottomLoading(true);
                ThreadStart load = new ThreadStart(content.NextPage);
                Thread thread = new Thread(load);
                thread.Start();

            }

            public void OnHeaderRefresh()
            {
                var h = new Handler(Looper.MainLooper);
                if (!content.IsRefreshing)
                    Task.Run(async () =>
                    {
                        content.IsRefreshing = true;
                        if (content.CurrentPage != 0)
                        {
                            await Task.Run(async () =>
                            {
                                content.PreviousPage();
                            });
                        }
                        else
                        {
                            logger.Info("Swipe Header Refreshing");

                            await Task.Delay(10);
                            content.Refresh();
                        }

                        h.Post(() =>
                        {
                            content.mRefreshLayout.HeaderRefreshing = false;
                            content.mRefreshLayout.FooterRefreshing = false;
                        });
                        content.IsRefreshing = false;
                    });
            }
        }

        public async void GetLib()
        {
            CurrentList.Clear();
            if (Core.Net.Connect())
                CurrentList.AddRange(await Core.Gallery.GetPage(0));
            else
            {
                var h = new Handler(Looper.MainLooper);
                h.Post(() =>
                {
                    SetError(true);
                });
            }
        }

        public async void GetTotalCount()
        {
            count = await Core.Gallery.GetCount(Current_Query);
        }

        public async void Refresh()
        {
            var h = new Handler(Looper.MainLooper);
            if (Core.Net.Connect())
            {
                h.Post(() =>
                {
                    SetMainLoading(true);
                });
                await Task.Run(async () =>
                {
                    logger.Info("Refreshing HPContent");
                    CurrentList.Clear();
                    CurrentList.AddRange(await Core.Gallery.GetPage(0, Current_Query));
                    if (CurrentList == null || CurrentList.Count < 1)
                    {
                        h.Post(() =>
                        {
                            SetMainLoading(false);
                            SetError(true);
                        });
                        return;
                    }
                    CurrentPage = 0;
                    h.Post(() =>
                    {
                        adapter.NotifyDataSetChanged();
                        adapter.ResetList();
                        SetMainLoading(false);
                        if (CurrentList.Count > 0)
                            mRecyclerView.ScrollToPosition(0);
                    });
                    GetTotalCount();
                    mpageSelector = new Custom_Views.PageSelector(this);
                    logger.Info("HPContent Refresh Successful");
                });
            }
            else
            {
                h.Post(() =>
                {
                    SetError(true);
                });
            }
        }

        public void SetBottomLoading(bool state)
        {
            switch (state)
            {
                case true:
                    // mBottomProgressBar.Visibility = ViewStates.Visible;
                    IsLoading = true;
                    break;
                case false:
                    // mBottomProgressBar.Visibility = ViewStates.Gone;
                    IsLoading = false;
                    break;
            }
        }

        public new void Dispose()
        {
            CurrentList.Clear();
            
            adapter.NotifyDataSetChanged();
            mRecyclerView.ClearOnScrollListeners();
            mRecyclerView.SetAdapter(null);
            adapter.Dispose();
            adapter = null;
            mLayoutManager.Dispose();
            mpageSelector = null;
            dialogeventlistener = null;
            mRecyclerView = null;
            countDown = null;
            listener = null;
            mProgressView.Dispose();

            GC.Collect();
            Java.Lang.JavaSystem.Gc();
        }

        public void SetError(bool show)
        {
            switch (show)
            {
                case true:
                    mRefreshLayout.Visibility = ViewStates.Gone;
                    mProgressView.Visibility = ViewStates.Gone;
                    mErrorFrame.Visibility = ViewStates.Visible;
                    break;
                case false:
                    mErrorFrame.Visibility = ViewStates.Gone;
                    break;
            }


        }

        int GetFirstItem(int page)
        {
            int mark = -1;
            if (CurrentList.Count > 0)
            {
                var item = CurrentList.FindIndex((x) => x.page == page);
                if (item != -1)
                    mark = item;
            }
            return mark;
        }

        public async void JumpTo(int page)
        {

            IsLoading = true;
            if (page == CurrentPage + 1)
            {
                isLoading = false;
                return;
            }
            var h = new Handler(Looper.MainLooper);
            logger.Info("Loading  Page " + page);
            {
                int cachedPos = GetFirstItem(page - 1);
                if (cachedPos != -1)
                {
                    h.Post(() =>
                    {
                        mRecyclerView.SmoothScrollToPosition(cachedPos);
                    });
                    IsLoading = false;
                    return;

                }
            }

            GetTotalCount();

            h.Post(() =>
            {
                SetMainLoading(true);
            });
            if (page >= (count / 25))
            {
                h.Post(() =>
                {
                    Toast to = Toast.MakeText(this.Context, "Page outside library", ToastLength.Short);
                    to.SetGravity(GravityFlags.Bottom, 0, 10);

                    to.Show();
                    SetMainLoading(false);
                    SetBottomLoading(false);
                    mRefreshLayout.HeaderRefreshing = false;
                    mRefreshLayout.FooterRefreshing = false;
                });
                IsLoading = false;
                return;
            }
            CurrentList.Clear();
            CurrentList.AddRange(await Core.Gallery.GetPage(page - 1, Current_Query));
            if (CurrentList.Count > 0)
            {
                h.Post(() =>
                {
                    adapter.ResetList();
                    if (CurrentList.Count > 0)
                        mRecyclerView.ScrollToPosition(0);

                });
                lastindex = CurrentList.Count - 1;
                GetTotalCount();
                CurrentPage = page - 1;


            }
            h.Post(() =>
            {

                SetMainLoading(false);
            });
            logger.Info("Loading Next Page Successful");

            IsLoading = false;
        }


        public class RecyclerViewClickListener : EasyRecyclerView.EasyRecyclerView.IOnItemClickListener
        {
            public void OnItemClick(EasyRecyclerView.EasyRecyclerView parent, RecyclerView.ViewHolder holder)
            {
                if (holder is GalleryCardHolder vh)
                {
                    string gallerystring = Core.JSON.Serializer.SimpleSerializer.Serialize(vh.gcard.Gallery);
                    var galleryscene = new Scenes.GalleryScene(gallerystring, vh.gcard.ThumbnailPath);
                    var pscene = (((GalleryCardAdapter)parent.GetAdapter()).content);
                    pscene.Stage.PushScene(galleryscene);
                }

            }
        }


        public void SetMainLoading(bool state)
        {
            switch (state)
            {
                case true:
                    mProgressView.Visibility = ViewStates.Visible;
                    SetError(false);
                    mRefreshLayout.Visibility = ViewStates.Gone;
                    mRecyclerView.Visibility = ViewStates.Invisible;
                    IsLoading = true;
                    break;
                case false:
                    mProgressView.Visibility = ViewStates.Gone;
                    mRefreshLayout.Visibility = ViewStates.Visible;
                    mRecyclerView.Visibility = ViewStates.Visible;
                    //mRefreshLayout.EnableSwipeHeader = true;
                    IsLoading = false;
                    break;
            }
        }

        public class CountDown : CountDownTimer
        {
            LibraryScene content;
            public CountDown(long ms, long interval, LibraryScene content) : base(ms, interval)
            {
                this.content = content;
            }

            public override void OnFinish()
            {
                content.IsLoading = false;
            }

            public override void OnTick(long millisUntilFinished)
            {

            }
        }


        public async void NextPage()
        {
            isLoading = true;
            logger.Info("Loading Next Page");
            var h = new Handler(Looper.MainLooper);
            if ((CurrentPage + 1) >= (count / 25))
            {
                h.Post(() =>
                {
                    Toast to = Toast.MakeText(this.Context, "Reached end of library", ToastLength.Short);
                    to.SetGravity(GravityFlags.Bottom, 0, 10);

                    to.Show();
                    SetBottomLoading(false);
                    mRefreshLayout.HeaderRefreshing = false;
                    mRefreshLayout.FooterRefreshing = false;
                    isLoading = false;
                });
                return;
            }
            int lastin = CurrentList.Count - 1;
            CurrentList.AddRange(await Core.Gallery.GetPage(CurrentPage + 1, Current_Query));
            if (CurrentList.Count > 0)
            {
                h.Post(() =>
                {
                    adapter.NotifyItemRangeInserted(lastin + 1, CurrentList.Count - (lastin + 1));

                    mRefreshLayout.HeaderRefreshing = false;
                    mRefreshLayout.FooterRefreshing = false;
                    isLoading = false;
                    SetBottomLoading(false);
                    mRecyclerView.RefreshDrawableState();

                });
                CurrentPage++;
                lastindex = CurrentList.Count - 1;



            }
            logger.Info("Loading Next Page Successful");

        }

        public async void PreviousPage()
        {
            isLoading = true;
            logger.Info("Loading Previous Page");
            var h = new Handler(Looper.MainLooper);
            if (CurrentPage <= 0)
            {
                h.Post(() =>
                {
                    SetBottomLoading(false);
                });
                return;
            }
            if (mRefreshLayout.FooterRefreshing)
            {
                logger.Info("Refresh Operation already in progress");
                isLoading = false;
                return;
            }

            h.Post(() =>
            {
                SetBottomLoading(true);
                mRefreshLayout.HeaderRefreshing = true;
            });
            var oldlist = new List<Core.Gallery.GalleryItem>(CurrentList);
            CurrentList.Clear();
            CurrentList.AddRange(await Core.Gallery.GetPage(CurrentPage - 1, Current_Query));
            int newitems = CurrentList.Count;
            CurrentList.AddRange(oldlist);
            if (newitems > 0)
            {
                h.Post(() =>
                {
                    adapter.NotifyItemRangeInserted(0, 25);
                    mRefreshLayout.HeaderRefreshing = false;
                    isLoading = false;

                });
                lastindex = CurrentList.Count - 1;
                CurrentPage--;
            }
            SetBottomLoading(false);
            logger.Info("Loading Previous Page Successful");

        }



        public class DialogEventListener : Custom_Views.PageSelector.INoticeDialogListener
        {
            LibraryScene parent;
            public DialogEventListener(LibraryScene parent)
            {
                this.parent = parent;
            }
            public void OnDialogNegativeClick(DialogFragment dialog)
            {
                //close dialog
            }

            public void OnDialogPositiveClick(DialogFragment dialog)
            {

                if (dialog is Custom_Views.PageSelector dl)
                {
                    ThreadStart thrds = new ThreadStart(() => { parent.JumpTo(dl.PageSelected); });
                    Thread thread = new Thread(thrds);
                    thread.Start();
                }
            }
        }


        public class PageCheckListener : RecyclerView.OnScrollListener
        {
            LibraryScene content;
            bool isUpdating = false, isScrolling = false;
            public PageCheckListener(LibraryScene content)
            {
                this.content = content;
            }



            public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
            {

                if (!isUpdating)
                    content.CurrentPage = GetPage();
                base.OnScrolled(recyclerView, dx, dy);
            }

            public int GetPage()
            {
                int page = content.CurrentPage;
                int firstposition = ((GridLayoutManager)content.mRecyclerView.GetLayoutManager()).FindFirstVisibleItemPosition();
                if (firstposition > 0)
                {
                    var item = content.CurrentList[firstposition];
                    if (item != null)
                    {
                        page = item.page;
                    }
                }
                return page;
            }

            public override void OnScrollStateChanged(RecyclerView recyclerView, int newState)
            {

                base.OnScrollStateChanged(recyclerView, newState);
            }
        }


        public class GalleryCardAdapter : EasyAdapter
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public EventHandler<int> ItemClick;
            public LibraryScene content;
            void OnClick(int position)
            {
                ItemClick?.Invoke(this, position);
            }

            public Scene scene;

            public List<Core.Gallery.GalleryItem> mdata;
            Android.Content.Context mcontext;
            public GalleryCardAdapter(Context context, LibraryScene content)
            {
                mcontext = context;
                this.content = content;
                mdata = content.CurrentList;
            }

            public override int ItemCount
            {
                get { return mdata.Count; }
            }

            public void ResetList()
            {
                mdata = content.CurrentList;
                this.NotifyDataSetChanged();
            }

            public override void OnViewRecycled(Java.Lang.Object holder)
            {
                base.OnViewRecycled(holder);
                if (holder is GalleryCardHolder hold)
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
                    Task.Run(() =>
                    vh.Bind(mdata[position]));
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

        public class GalleryArgs : EventArgs
        {
            public string GalleryData;
            public string ThumbPath;

        }


        public class HideOnScroll : CoordinatorLayout.Behavior
        {
            public HideOnScroll(Context context, IAttributeSet attrs) : base(context, attrs)
            {

            }



            public override void OnNestedScroll(CoordinatorLayout coordinatorLayout, Java.Lang.Object child, View target, int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed)
            {
                if (child is Clans.Fab.FloatingActionMenu c)
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    base.OnNestedScroll(coordinatorLayout, child, target, dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed);
#pragma warning restore CS0618 // Type or member is obsolete
                    if (dyConsumed > 0)
                    {
                        c.HideMenuButton(true);
                    }
                    else if (dyConsumed < 0)
                    {
                        c.ShowMenuButton(true);
                    }
                }
            }

            public override bool OnStartNestedScroll(CoordinatorLayout coordinatorLayout, Java.Lang.Object child, 
                View directTargetChild, View target, int nestedScrollAxes) 
                => nestedScrollAxes == ViewCompat.ScrollAxisVertical;
        }



        class FABClickListener : Java.Lang.Object, View.IOnClickListener
        {
            LibraryScene main;
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public FABClickListener(LibraryScene main)
            {
                this.main = main;
            }
            public void OnClick(View v)
            {
                var fab = (Clans.Fab.FloatingActionButton)v;
                switch (fab.Id)
                {
                    case Resource.Id.fabJumpTo:
                        logger.Info("Page selector shown");
                        main.mpageSelector.Show(
                            ((HPXSceneActivity)main.MainView.Context).FragmentManager, "PageSelecter");
                        break;
                    case Resource.Id.fabRefresh:
                        //main.Refresh();
                        break;
                }
                main.fam.Close(true);
            }
        }


        protected override void OnDestroyView(View p0)
        {
            
            base.OnDestroyView(p0);
        }

        protected override void OnDestroy()
        {
            MainView = null;
            fam = null;
            appBarLayout = null;
            mJumpFab = null;
            mRefreshFab = null;
            Dispose();
            base.OnDestroy();
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
        public override void OnConfigurationChanged(Configuration newConfig)
        {
            isGrid = Core.App.Settings.IsGrid;
            SetColumns();
            mLayoutManager = new Helpers.Layouts.ExtraGridLayoutManager(this.Context, columns, GridLayoutManager.Vertical, false);
            mRecyclerView.SetLayoutManager(mLayoutManager);
        }

        public bool OnQueryTextChange(string newText)
        {
            //throw new NotImplementedException();
            return true;
        }

        public string Parse(string inString, bool IsEscaped = true)
        {
            string res = string.Empty;
            if (IsEscaped)
                res = inString.Replace("\\\"", "\"");
            else
                res = inString.Replace("\"", "\\\"");

            return res;
        }



        public bool OnQueryTextSubmit(string query)
        {

            //SupportActionBar.InvalidateOptionsMenu();
            toolbar.Title = query;
            logger.Info("Search query submit , query ={0}", query);
            Current_Query = Parse(query, false);
            if (search != null)
                search.ActionView.ClearFocus();
            searchView.SetQuery(query, false);

            return true;
        }



        Android.Support.V7.Widget.SearchView searchView;
        IMenuItem search;
        
        public void OnCreateOptionsMenu()
        {
            toolbar.InflateMenu(Resource.Menu.gallerySearch);
            
            search = toolbar.Menu.FindItem(Resource.Id.search);

            searchView = (Android.Support.V7.Widget.SearchView)search.ActionView;
            searchView.SetOnQueryTextListener(this);
            searchView.SetQuery(Current_Query, false);
        }
        

    }
}