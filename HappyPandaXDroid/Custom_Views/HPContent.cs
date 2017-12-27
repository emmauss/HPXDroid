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
using Toolbar = Android.Support.V7.Widget.Toolbar;
using ProgressView = XamarinBindings.MaterialProgressBar;
using EasyRecyclerView;
using EasyRecyclerView.Addons;
using NLog;

namespace HappyPandaXDroid.Custom_Views
{
    public class HPContent : FrameLayout
    {
        View ContentView;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<Core.Gallery.GalleryItem> CurrentList = new List<Core.Gallery.GalleryItem>();
        public Custom_Views.PageSelector mpageSelector;
        EasyRecyclerView.EasyRecyclerView mRecyclerView;
        bool IsRefreshing = false;
        public string activityName;
        public int activityId;
        PageCheckListener listener;
        bool initialized = false;

        ProgressView.MaterialProgressBar mProgressView;
        public int count = 0, lastindex = 0;
        RefreshLayout.RefreshLayout mRefreshLayout;
        RecyclerView.LayoutManager mLayoutManager;
        FrameLayout mErrorFrame;
        ImageView mErrorImage;
        TextView mErrorText;
        GalleryCardAdapter adapter;
        CountDown countDown;
        
        bool isLoading = false;
        bool IsLoading {
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
        public HPContent(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public HPContent(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        int columns = 0;
        private void Initialize()
        {
            countDown = new CountDown(500, 10,this);
            listener = new PageCheckListener(this);
            logger.Info("Initializing HPContent");
            ContentView = Inflate(Context, Resource.Layout.HPContent, this);
            mRecyclerView = FindViewById<EasyRecyclerView.EasyRecyclerView>(Resource.Id.recyclerView);
            adapter = new GalleryCardAdapter(this.Context,this);
            mRecyclerView.SetOnItemClickListener(new RecyclerViewClickListener());
            mRefreshLayout = FindViewById<RefreshLayout.RefreshLayout>(Resource.Id.refresh_layout);
            mProgressView = FindViewById<ProgressView.MaterialProgressBar>(Resource.Id.progress_view);
            mProgressView.Visibility = ViewStates.Gone;
            mErrorFrame = FindViewById<FrameLayout>(Resource.Id.error_frame);
            mErrorFrame.Visibility = ViewStates.Gone;
            mErrorImage = FindViewById<ImageView>(Resource.Id.error_image);
            mErrorImage.SetImageResource(Resource.Drawable.big_weird_face);
            mErrorText = FindViewById<TextView>(Resource.Id.error_text);
            mErrorText.Text = "Error";
            mErrorImage.Click += MErrorFrame_Click;
            SetBottomLoading(false);
            

            mRecyclerView.AddOnScrollListener(listener);
            SetColumns();
            mLayoutManager = new ExtraLayoutManager(this.Context, columns, GridLayoutManager.Vertical, false);
           
           
            mRecyclerView.SetAdapter(adapter);
            mRefreshLayout.SetOnRefreshListener(new OnRefreshListener(this));

            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRefreshLayout.SetHeaderColorSchemeResources(new int[] {
                Resource.Color.loading_indicator_red,
                Resource.Color.loading_indicator_purple,
                Resource.Color.loading_indicator_blue,
                Resource.Color.loading_indicator_cyan,
                Resource.Color.loading_indicator_green,
                Resource.Color.loading_indicator_yellow });
            mRefreshLayout.SetFooterColorSchemeResources(new int[] {
                    Resource.Color.loading_indicator_red,
                    Resource.Color.loading_indicator_blue,
                    Resource.Color.loading_indicator_green,
                    Resource.Color.loading_indicator_orange });


            SetMainLoading(true);
            mpageSelector = new Custom_Views.PageSelector();
            dialogeventlistener = new DialogEventListener(this);
            initialized = true;
            logger.Info("HPContent Initialized");
        }

        void SetColumns()
        {
            var windo = Context.GetSystemService(Context.WindowService);
            var window = windo.JavaCast<IWindowManager>();
            var display = window.DefaultDisplay;
            int gridFactor = 0;
            float w = display.Width;
            gridFactor = (int)(Math.Ceiling(w / 200d));
            if (Resources.Configuration.Orientation == Android.Content.Res.Orientation.Landscape)
                columns = gridFactor * 2;
            else
                columns = gridFactor;
        }


        public class ExtraLayoutManager : GridLayoutManager
        {
            private static readonly int DEFAULT_EXTRA_LAYOUT_SPACE = 800;
            private int extraLayoutSpace = -1;
            private Context context;


            public ExtraLayoutManager(Context context,int columns) : base(context,columns)
            {
                this.context = context;
            }

            public ExtraLayoutManager(Context context, int columns ,int extraLayoutSpace) : base(context,columns)
            {
                this.context = context;
                this.extraLayoutSpace = extraLayoutSpace;
            }



            public ExtraLayoutManager(Context context, int columns ,int orientation, bool reverseLayout)
                : base(context, columns,orientation, reverseLayout)
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



        public void OrientationChanged(Android.Content.Res.Orientation orientation)
        {
            SetColumns();
            mLayoutManager = new GridLayoutManager(this.Context, columns);
            mRecyclerView.SetLayoutManager(mLayoutManager);
        }

        public class AutoFitGridLayout : GridLayoutManager
        {
            private int mColumnWidth;
            private bool mColumnWidthChanged = true;

            public AutoFitGridLayout(Context context, int columnWidth) : base(context,1)
            {
                /* Initially set spanCount to 1, will be changed automatically later. */
                SetColumnWidth(CheckedColumnWidth(context, columnWidth));
            }

            public AutoFitGridLayout(Context context, int columnWidth, int orientation, bool reverseLayout) : base (context, 1, orientation, reverseLayout)
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
                    int spanCount = Math.Max(1, totalSpace / mColumnWidth);
                    SpanCount = spanCount;
                    mColumnWidthChanged = false;
                }
                base.OnLayoutChildren(recycler, state);
            }
        }


        public  void InitLibrary()
        {
            int tries= 0;
            if(Core.Net.Connect())
            Task.Run(async () =>
             {
                 while (tries <5 && !Core.Net.Connected)
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
            if(!IsRefreshing)
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

        class OnRefreshListener : RefreshLayout.RefreshLayout.IOnRefreshListener
        {
            HPContent content;
            public OnRefreshListener(HPContent content)
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
                            await Task.Run(async() =>
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
            if(Core.Net.Connect())
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
                int cachedPos = GetFirstItem(page-1);
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
            CurrentList.AddRange(await Core.Gallery.GetPage(page-1, Current_Query));
            if (CurrentList.Count > 0)
            {
                h.Post(() =>
                {
                    adapter.ResetList() ;
                    if (CurrentList.Count > 0)
                        mRecyclerView.ScrollToPosition(0);

                });
                lastindex = CurrentList.Count - 1;
                GetTotalCount();
                CurrentPage = page-1;
                

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
                    Intent intent = new Intent(parent.Context, typeof(GalleryActivity));
                    string gallerystring = Core.JSON.Serializer.SimpleSerializer.Serialize(vh.gcard.Gallery);
                    intent.PutExtra("thumb", vh.gcard.ThumbnailPath);
                    intent.PutExtra("gallery", gallerystring);
                    parent.Context.StartActivity(intent);
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
                    mRefreshLayout.EnableSwipeHeader = true;
                    IsLoading = false;
                    break;
            }
        }

        public class CountDown : CountDownTimer
        {
            HPContent content;
            public CountDown(long ms, long interval, HPContent content) : base(ms, interval)
            {
                this.content = content;
            }
            
            public override void OnFinish()
            {
                content.isLoading = false;
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
                    adapter.NotifyItemRangeInserted(lastin+1, CurrentList.Count- (lastin+1));
                    
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
            if (mRefreshLayout.IsFooterRefreshing() )
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
            HPContent parent;
            public DialogEventListener(HPContent parent)
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
            HPContent content;
            bool isUpdating = false, isScrolling = false;
            public PageCheckListener(HPContent content)
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
            HPContent content;
            void OnClick(int position)
            {
                ItemClick?.Invoke(this, position);
            }

            public List<Core.Gallery.GalleryItem> mdata;
            Android.Content.Context mcontext;
            public GalleryCardAdapter(Context context, HPContent content)
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