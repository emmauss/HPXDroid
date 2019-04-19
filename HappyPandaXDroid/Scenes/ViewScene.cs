using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProgressView = XamarinBindings.MaterialProgressBar;
using RefreshLayout = Com.Hippo.Refreshlayout;
using FloatingSearchViews;
using FloatingSearchViews.Utils;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using HappyPandaXDroid.Core;
using Com.Miguelcatalan.Materialsearchview;

namespace HappyPandaXDroid.Scenes
{
    public abstract class ViewScene : HPXScene, MaterialSearchView.IOnQueryTextListener, MaterialSearchView.ISearchViewListener
    {
        public abstract Core.Gallery.ViewType ViewType { get;}
        //protected FloatingSearchView searchView;
        Clans.Fab.FloatingActionMenu fam;
        public string title;
        bool isGrid = false;
        protected static Logger logger = LogManager.GetCurrentClassLogger();
        protected Clans.Fab.FloatingActionButton mRefreshFab;
        protected Clans.Fab.FloatingActionButton mJumpFab;
        protected View MainView { get; set; }
        public GalleryLists CurrentLists = new GalleryLists();
        public Core.Gallery.ItemType ItemType = Core.Gallery.ItemType.Gallery;
        public Custom_Views.PageSelector mpageSelector;
        protected EasyRecyclerView.EasyRecyclerView mRecyclerView;
        public bool IsRefreshing = false;
        public Toolbar toolbar;
        public MaterialSearchView searchView;
        PageCheckListener listener;
        bool initialized = false;
        protected CancellationTokenSource SceneCancellationTokenSource = new CancellationTokenSource();
        ProgressView.MaterialProgressBar mProgressView;
        public int columns;
        protected RefreshLayout.RefreshLayout mRefreshLayout;
        Helpers.Layouts.ExtraGridLayoutManager mLayoutManager;
        FrameLayout mErrorFrame;
        ImageView mErrorImage;
        TextView mErrorText;
        protected Custom_Views.CardAdapter.HPXCardAdapter adapter;
        Custom_Views.CardAdapter.GalleryCardAdapter galleryAdapter;
        Custom_Views.CardAdapter.CollectionCardAdapter collectionAdapter;
        CountDown countDown;
        protected FABClickListener fabclick;
        protected bool isLoading = false;
        protected bool IsLoading
        {
            get { return isLoading; }
            set
            {
                if (value == false)
                    Task.Run(() =>
                    {
                        countDown.Start();
                    });
                else
                    isLoading = value;
            }
        }

        public int LastIndex
        {
            get
            {
                return CurrentList.Count == 0 ? 0 : CurrentList.Count;
            }
        }

        public int Count
        {
            get
            {
                switch (ItemType)
                {
                    case Core.Gallery.ItemType.Gallery:
                        return CurrentLists.GalleryCount;
                    case Core.Gallery.ItemType.Collection:
                        CurrentPage = CurrentLists.CollectionPage;
                        return CurrentLists.CollectionCount;
                    default:
                        return 0;

                }
            }

            set
            {
                switch (ItemType)
                {
                    case Core.Gallery.ItemType.Gallery:
                        CurrentLists.GalleryCount = value;
                        break;
                    case Core.Gallery.ItemType.Collection:
                        CurrentLists.CollectionCount = value;
                        break;
                }
            }
        }

        public int CurrentPage
        {
            get
            {
                switch (ItemType)
                {
                    case Core.Gallery.ItemType.Gallery:
                        return CurrentLists.GalleryPage;
                    case Core.Gallery.ItemType.Collection:
                        CurrentPage = CurrentLists.CollectionPage;
                        return CurrentLists.CollectionPage;
                    default:
                        return 0;
                }
            }

            set
            {
                switch (ItemType)
                {
                    case Core.Gallery.ItemType.Gallery:
                        CurrentLists.GalleryPage = value;
                        break;
                    case Core.Gallery.ItemType.Collection:
                        CurrentLists.CollectionPage = value;
                        break;
                }
            }
        }

        protected string current_query = string.Empty;
        public string CurrentQuery => Parse(current_query, false);
        /*{
            get
            {
                return current_query;
            }
            set
            {
                current_query = value;
                SetMainLoading(true);
                if (search != null)
                    search.ActionView.ClearFocus();
                searchView.SetQuery(Parse(current_query,true), false);
                if (toolbar != null)
                    toolbar.Title = Parse(current_query, true);
                Refresh(0);
            }
        }*/
        public DialogEventListener dialogeventlistener;

        protected override void OnCreate(Bundle p0)
        {
            base.OnCreate(p0);
        }

        protected override View OnCreateView(LayoutInflater p0, ViewGroup p1)
        {
            MainView = p0.Inflate(Resource.Layout.viewScene, p1, false);
            Initialize();

            return MainView;
        }

        public void OnCreateOptionsMenu()
        {
            toolbar.InflateMenu(Resource.Menu.gallerySearch);

            var menuItem = toolbar.Menu.FindItem(Resource.Id.action_search);
            searchView.SetMenuItem(menuItem);

            menuItem = toolbar.Menu.FindItem(Resource.Id.addsearch);
        }

        protected virtual void Initialize()
        {
            toolbar = MainView.FindViewById<Toolbar>(Resource.Id.toolbar);
            /*try
            {
                searchView = MainView.FindViewById<FloatingSearchView>(Resource.Id.search);
                searchView.AttachNavigationDrawerToMenuButton(((HPXSceneActivity)Context).navDrawer);
                searchView.SearchAction += SearchView_SearchAction;
                searchView.InflateOverflowMenu(Resource.Menu.gallerySearch);
                searchView.MenuItemClick += SearchView_MenuItemClick;
                foreach(var item in searchView.CurrentMenuItems)
                {
                    if(item.ItemId == Resource.Id.addsearch)
                    {
                        item.ActionView.LongClick += Button_LongClick;
                        item.ActionView.Click += Button_Click;
                    }
                }
                searchView.SetSearchHint(title);
            }catch(Exception ex)
            {

            }*/

            searchView = MainView.FindViewById<MaterialSearchView>(Resource.Id.search_view);
            searchView.SetOnQueryTextListener(this);
            collectionAdapter = new Custom_Views.CardAdapter.CollectionCardAdapter(this.Context, this);
            galleryAdapter = new Custom_Views.CardAdapter.GalleryCardAdapter(this.Context,this);

            mRefreshFab = MainView.FindViewById<Clans.Fab.FloatingActionButton>(Resource.Id.fabRefresh);
            mJumpFab = MainView.FindViewById<Clans.Fab.FloatingActionButton>(Resource.Id.fabJumpTo);
            
            mJumpFab.SetImageResource(Resource.Drawable.v_go_to_dark_x24);
            
            mRefreshFab.SetImageResource(Resource.Drawable.v_refresh_dark_x24);
            fam = MainView.FindViewById<Clans.Fab.FloatingActionMenu>(Resource.Id.fam);
            
            mJumpFab.SetOnClickListener(fabclick);
            mRefreshFab.SetOnClickListener(fabclick);
            countDown = new CountDown(500, 10, this);
            listener = new PageCheckListener(this);
            logger.Info("Initializing HPContent");
            mRecyclerView = MainView.FindViewById<EasyRecyclerView.EasyRecyclerView>(Resource.Id.recyclerView);
            adapter = GetAdapter();
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
            mLayoutManager.SetExtraLayoutSpace(400);

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

            if (string.IsNullOrWhiteSpace(title))
                title = "Search";

            dialogeventlistener = new DialogEventListener(this);
            initialized = true;
            logger.Info("HPContent Initialized");
        }

        private void SearchView_MenuItemClick(object sender, FloatingSearchView.MenuItemClickEventArgs e)
        {
            var menuItem = e.MenuItem;

            /*switch (menuItem.ItemId)
            {
                case Resource.Id.sort:
                    SortClick(menuItem);
                    break;
                case Resource.Id.sort_direction:
                    SortClick(menuItem);
                    break;
            }*/
        }

        public void SortClick(IMenuItem item)
        {
           /*if (item.ItemId == Resource.Id.sort)
            {
                Custom_Views.ListDialog listDialog = new Custom_Views.ListDialog(this, "sort");
                listDialog.Show(((HPXSceneActivity)MainView.Context).FragmentManager, "Sort By");
            }
            else if (item.ItemId == Resource.Id.sort_direction)
            {
                Custom_Views.ListDialog listDialog = new Custom_Views.ListDialog(this, "order");
                listDialog.Show(((HPXSceneActivity)MainView.Context).FragmentManager, "Sort In");
            }*/
        }

        private void SearchView_SearchAction(object sender, FloatingSearchView.SearchActionEventArgs e)
        {
            Refresh(0);
        }

        class MenuClickListener : Java.Lang.Object,FloatingSearchView.IOnMenuItemClickListener
        {
            ViewScene parent;

            public MenuClickListener(ViewScene parent) : base()
            {
                this.parent = parent;
            }

            public void OnActionMenuItemSelected(IMenuItem menuItem)
            {
                switch (menuItem.ItemId)
                {
                    /*case Resource.Id.addsearch:
                        var button = (ImageButton)menuItem.ActionView;

                        button.LongClick += parent.Button_LongClick;
                        button.Click += parent.Button_Click;
                        break;
                    /*se Resource.Id.sort:
                        menuItem.SetOnMenuItemClickListener(new SortMenuClickListener(parent));
                        break;
                    case Resource.Id.sort_direction:
                        menuItem.SetOnMenuItemClickListener(new SortMenuClickListener(parent));
                        break;*/
                }
            }
        }

        private void MRefreshLayout_FooterRefresh(object sender, EventArgs e)
        {
            logger.Info("Swipe Footer Refreshing");
            SetBottomLoading(true);
            ThreadStart load = new ThreadStart(NextPage);
            Thread thread = new Thread(load);
            thread.Start();
        }

        public Custom_Views.CardAdapter.HPXCardAdapter GetAdapter()
        {
            switch (ItemType)
            {
                case Core.Gallery.ItemType.Gallery:
                    return galleryAdapter;
                case Core.Gallery.ItemType.Collection:
                    return collectionAdapter;
                default:
                    return null;
            }
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
                        Refresh(0);
                    }
                }, SceneCancellationTokenSource.Token);
        }

        public ViewScene(string title, string query) : base()
        {
            this.title = title;
        }

        private void AppBarLayout_Drag(object sender, View.DragEventArgs e)
        {
            e.Handled = true;
        }

        void SetColumns()
        {
            isGrid = Core.App.Settings.IsGrid;
            var windo = Context.GetSystemService(Context.WindowService);
            var window = windo.JavaCast<IWindowManager>();
            var display = window.DefaultDisplay; var rotation = display.Rotation;
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

        public List<Core.Gallery.HPXItem> CurrentList 
        {
            get {
                switch (ItemType)
                {
                    case Core.Gallery.ItemType.Gallery:
                        CurrentPage = CurrentLists.GalleryPage;
                        return CurrentLists.Galleries;
                    case Core.Gallery.ItemType.Collection:
                        CurrentPage = CurrentLists.CollectionPage;
                        return CurrentLists.Collections;
                    default:
                        return null;
                }
            }

            set
            {
                switch (ItemType)
                {
                    case Core.Gallery.ItemType.Gallery:
                        CurrentLists.Galleries = value;
                        break;
                    case Core.Gallery.ItemType.Collection:
                        CurrentLists.Collections = value;
                        break;
                }
            }
        }

        public void InitLibrary()
        {
            int tries = 0;
            if (Core.Net.Connected)
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
                                    if(!SceneCancellationTokenSource.IsCancellationRequested)
                                    adapter.ResetList();
                                    SetMainLoading(false);

                                });
                                if (!SceneCancellationTokenSource.IsCancellationRequested)
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
                }, SceneCancellationTokenSource.Token);
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

        protected virtual void SwitchToView(Core.Gallery.ItemType itemType)
        {
           
        }

        private void MErrorFrame_Click(object sender, EventArgs e)
        {
            SetMainLoading(true);
            Refresh(0);
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
            if (SceneCancellationTokenSource.IsCancellationRequested)
                SceneCancellationTokenSource = new CancellationTokenSource();
                Task.Run(async () =>
                {
                    if (CurrentPage != 0)
                    {
                        await Task.Run(() =>
                        {
                            SetBottomLoading(true);
                            if (!SceneCancellationTokenSource.IsCancellationRequested)
                                PreviousPage();
                            SetBottomLoading(false);
                        }, SceneCancellationTokenSource.Token);
                    }
                    else
                    {
                        logger.Info("Swipe Header Refreshing");
                        IsRefreshing = true;
                        await Task.Delay(10);
                        if (!SceneCancellationTokenSource.IsCancellationRequested)
                            Refresh(0);
                    }                    
                });
        }

        class OnRefreshListener : Java.Lang.Object, RefreshLayout.RefreshLayout.IOnRefreshListener
        {
            ViewScene content;
            public OnRefreshListener(ViewScene content)
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
                            content.Refresh(0);
                        }
                    });
            }
        }

        public async void GetLib()
        {
            CurrentList.Clear();
            if (Core.Net.Connect())
            {
                ManualResetEvent manualResetEvent = new ManualResetEvent(false);
                RequestToken token = new RequestToken(manualResetEvent, SceneCancellationTokenSource.Token);
                Core.Gallery.GetPage(ItemType, 0, token, ViewType);

                manualResetEvent.WaitOne();

                if (!token.CancellationToken.IsCancellationRequested)
                {
                    var list = (List<Core.Gallery.HPXItem>)token.Result;
                    CurrentList.AddRange(list);
                    foreach (var item in CurrentList)
                    {
                        item.Thumb = new Media.Image();
                    }
                }
            }
            else
            {
                var h = new Handler(Looper.MainLooper);
                h.Post(() =>
                {
                    SetError(true);
                });
            }
        }

        public abstract void GetTotalCount();

        public abstract void Refresh(int page);

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
            if (page >= (Count / 25))
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
            Refresh(page - 1);
        }


        public class RecyclerViewClickListener : EasyRecyclerView.EasyRecyclerView.IOnItemClickListener
        {
            public void OnItemClick(EasyRecyclerView.EasyRecyclerView parent, RecyclerView.ViewHolder holder)
            {
                if (holder is Custom_Views.CardAdapter.HPXItemHolder vh)
                {
                    if (vh.HPXItem != null && vh.HPXItem.id > 0)
                    {
                        if (vh.HPXItem is Core.Gallery.GalleryItem gallery)
                        {
                            var galleryscene = new Scenes.GalleryScene(gallery);
                            var pscene = (((Custom_Views.CardAdapter.HPXCardAdapter)parent.GetAdapter()).content);
                            pscene.Stage.PushScene(galleryscene);
                        }
                        else if (vh.HPXItem is Core.Gallery.Collection collection)
                        {
                            var collectionscene = new Scenes.CollectionScene(collection, vh.Url);
                            var pscene = (((Custom_Views.CardAdapter.HPXCardAdapter)parent.GetAdapter()).content);
                            pscene.Stage.PushScene(collectionscene);
                        }
                    }
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
                    IsLoading = false;
                    break;
            }
        }

        public class CountDown : CountDownTimer
        {
            ViewScene content;
            public CountDown(long ms, long interval, ViewScene content) : base(ms, interval)
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


        public abstract void NextPage();

        public abstract void PreviousPage();

        protected override void OnStop()
        {
           // SceneCancellationTokenSource.Cancel();
            base.OnStop();
        }

        protected override void OnStart()
        {
            base.OnStart();
           // SceneCancellationTokenSource = new CancellationTokenSource();
        }

        protected override void OnPause()
        {
            if (RequestToken != null)
                RequestToken.IsPaused = true;
            Util.CloseSoftKeyboard((HPXSceneActivity)Context);
            base.OnPause();
            // SceneCancellationTokenSource = new CancellationTokenSource();
        }
        public class DialogEventListener : Custom_Views.INoticeDialogListener
        {
            ViewScene parent;
            public DialogEventListener(ViewScene parent)
            {
                this.parent = parent;
            }
            public void OnDialogNegativeClick(Android.Support.V4.App.DialogFragment dialog)
            {
                //close dialog
            }

            public void OnDialogPositiveClick(Android.Support.V4.App.DialogFragment dialog)
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
            ViewScene content;
            bool isUpdating = false, isScrolling = false;
            public PageCheckListener(ViewScene content)
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

            public override void OnNestedScroll(CoordinatorLayout coordinatorLayout, Java.Lang.Object child, View target, int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed, int type)
            {
                if (child is Clans.Fab.FloatingActionMenu c)
                {
                    base.OnNestedScroll(coordinatorLayout, child, target, dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed,type);

                    if (dyConsumed > 0)
                    {
                        c.HideMenuButton(true);
                    }
                    else if (dyConsumed < 0)
                    {
                        c.ShowMenuButton(true);
                    }
                }
                else if(child is Clans.Fab.FloatingActionButton b)
                {
                    base.OnNestedScroll(coordinatorLayout, child, target, dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed, type);

                    if (dyConsumed > 0)
                    {
                        b.Hide(true);
                    }
                    else if (dyConsumed < 0)
                    {
                        b.Show(true);
                    }
                }
            }

            public override bool OnStartNestedScroll(CoordinatorLayout coordinatorLayout, Java.Lang.Object child, View directTargetChild, View target, int axes, int type)
            {
                  return  axes == ViewCompat.ScrollAxisVertical;
            }
            
        }


        public class SortMenuClickListener : Java.Lang.Object, IMenuItemOnMenuItemClickListener
        {
            ViewScene mparent;
            public SortMenuClickListener(ViewScene parent)
            {
                mparent = parent;
            }
            public bool OnMenuItemClick(IMenuItem item)
            {
                if (item.TitleFormatted.ToString() == "Sort by")
                {
                    Custom_Views.ListDialog listDialog = new Custom_Views.ListDialog(mparent, "sort");
                    listDialog.Show(((HPXSceneActivity)mparent.MainView.Context).FragmentManager, "Sort By");
                }
                else if (item.TitleFormatted.ToString() == "Sort In")
                {
                    Custom_Views.ListDialog listDialog = new Custom_Views.ListDialog(mparent, "order");
                    listDialog.Show(((HPXSceneActivity)mparent.MainView.Context).FragmentManager, "Sort In");
                }

                return true;
            }            
        }


        public class FABClickListener : Java.Lang.Object, View.IOnClickListener
        {
            ViewScene main;
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public FABClickListener(ViewScene main)
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
                        main.mpageSelector?.Show(
                            ((HPXSceneActivity)main.MainView.Context).SupportFragmentManager, "PageSelecter");
                        break;
                    case Resource.Id.fabRefresh:
                        main.Refresh(0);
                        break;
                    case Resource.Id.fabToggle:
                        if (main.ItemType == Core.Gallery.ItemType.Gallery)
                            main.SwitchToView(Core.Gallery.ItemType.Collection);
                        else
                            main.SwitchToView(Core.Gallery.ItemType.Gallery);
                        break;
                }
                main.fam.Close(true);
            }
        }


        protected override void OnDestroyView(View p0)
        {
            SceneCancellationTokenSource.Cancel();
            base.OnDestroyView(p0);
        }

        protected override void OnDestroy()
        {
            SceneCancellationTokenSource.Cancel();
            adapter?.Clear();            
            MainView = null;
            fam = null;
            mJumpFab = null;
            mRefreshFab = null;
            Dispose();
            base.OnDestroy();
        }

        protected override void OnResume()
        {
            if(RequestToken!=null)
            RequestToken.IsPaused = false;

            base.OnResume();
            if (isGrid != Core.App.Settings.IsGrid)
            {
                SetColumns();
                mLayoutManager = new Helpers.Layouts.ExtraGridLayoutManager(this.Context, columns, GridLayoutManager.Vertical, false);
                mLayoutManager.SetExtraLayoutSpace(400);
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

        public static string Parse(string inString, bool IsEscaped)
        {
            string res = string.Empty;
            if (IsEscaped)
                res = inString.Replace("\\\"", "\"");
            else
                res = inString.Replace("\"", "\\\"");

            return res;
        }
        

    public void Button_Click(object sender, EventArgs e)
        {
            Custom_Views.ListDialog listDialog = new Custom_Views.ListDialog(this, "search");
            listDialog.Show(((HPXSceneActivity)MainView.Context).FragmentManager, "Quick Search");

        }

        public void Button_LongClick(object sender, View.LongClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(current_query))
            {
                Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(Context);
                alertDialog.SetTitle("Add quickserch query");
                alertDialog.SetMessage("Do you want to add `" + Parse(CurrentQuery,true) + "` to quick search?");
                alertDialog.SetPositiveButton("Yes", new DialogInterface(this));
                alertDialog.SetNegativeButton("No", new DialogInterface(this));
                alertDialog.Show();
            }
        }

        public bool OnQueryTextSubmit(string p0)
        {
            current_query = p0;
            Refresh(0);

            return false;
        }

        public void OnSearchViewClosed()
        {
            
        }

        public void OnSearchViewShown()
        {
            searchView.SetQuery(current_query, false);
        }

        class DialogInterface : Java.Lang.Object, IDialogInterfaceOnClickListener
        {
            ViewScene LibraryScene;
            public void OnClick(IDialogInterface dialog, int which)
            {
                if((DialogButtonType)which == DialogButtonType.Positive)
                {
                    Core.Media.QuickSearch.AddToQuickSearch(Parse(LibraryScene.CurrentQuery,true));
                    Toast.MakeText(LibraryScene.Context, "Added to Quick Search", ToastLength.Short).Show();
                }
            }

            public DialogInterface(ViewScene libraryScene)
            {
                LibraryScene = libraryScene;
            }
        }

        public class GalleryLists
        {
            public List<Core.Gallery.HPXItem> Galleries = new List<Core.Gallery.HPXItem>();
            public List<Core.Gallery.HPXItem> Collections = new List<Core.Gallery.HPXItem>();
            public int GalleryPage = 0;
            public int CollectionPage = 0;
            public int GalleryCount = 0;
            public int CollectionCount = 0;
        }
    }


}
 