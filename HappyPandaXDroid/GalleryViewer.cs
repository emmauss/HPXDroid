
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;

using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.View;
using PhotoView = Com.Github.Chrisbanes.Photoview;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Java.Lang;
using Emmaus.Widget;
using Com.Bumptech.Glide.Request;
using Com.Bumptech.Glide.Request.Target;
using NLog;

namespace HappyPandaXDroid
{
    [Activity(Label = "GalleryViewer", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class GalleryViewer : AppCompatActivity
    {
        
        Toolbar toolbar;
        public TextView page_number;
        public RecyclerViewPager galleryPager;
        bool overlayVisible = true;
        public  ImageAdapter adapter;
        public RequestOptions options;
        CancellationTokenSource ViewerCancellationTokenSource = new CancellationTokenSource();
        LinearLayout ScreenFilter;
        FrameLayout lay;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /*List<string> ImageList =
            new List<string>();*/
        List<Core.Gallery.Page> PageList =
            new List<Core.Gallery.Page>();
        SeekBar seekbar,FilterSlider;
        public UICountdown countDown;
        GestureDetector gestureDetector;
        Core.Gallery.GalleryItem gallery;
        
        public int activityID;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.GalleryLayout);
            
                string data = Intent.GetStringExtra("page");
                PageList = Core.JSON.Serializer.SimpleSerializer.Deserialize<List<Core.Gallery.Page>>(data);
            logger.Info("Initializing Gallery Viewer");
            Task.Run(()=>InitPageGen());

            data = Intent.GetStringExtra("gallery");
            gallery = Core.JSON.Serializer.SimpleSerializer.Deserialize<Core.Gallery.GalleryItem>(data);
            int pageno = Intent.GetIntExtra("no", 0);
            options = new RequestOptions()
                .Override(Target.SizeOriginal, Target.SizeOriginal);
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            ScreenFilter = FindViewById<LinearLayout>(Resource.Id.screenFilter);
            
            FilterSlider = FindViewById<SeekBar>(Resource.Id.filterSlider);
            FilterSlider.Max = 255;
            FilterSlider.Progress = 255;
            FilterSlider.ProgressChanged += FilterSlider_ProgressChanged;
            FilterSlider.Click += FilterSlider_Click;
            SetSupportActionBar(toolbar);
            
            lay = FindViewById<FrameLayout>(Resource.Id.frame);
            galleryPager = FindViewById<RecyclerViewPager>(Resource.Id.galleryViewPager);
            var layout = new Helpers.Layouts.ExtraLayoutManager(this, LinearLayoutManager.Horizontal, false);    
            galleryPager.SetLayoutManager(layout);
            gestureDetector = new GestureDetector(this, new TapsDetector(this));
            adapter = new ImageAdapter(PageList,this);
            galleryPager.SetAdapter(new RecyclerViewPagerAdapter(galleryPager, adapter));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            countDown = new UICountdown(5000, 10, this);
            galleryPager.ScrollToPosition(pageno);
            seekbar = FindViewById<SeekBar>(Resource.Id.progress_seekbar);
            seekbar.Max = PageList.Count;
            FilterSlider.Progress = Core.App.Settings.FilterLevel;
            galleryPager.GetAdapter().NotifyDataSetChanged();
            page_number = FindViewById<TextView>(Resource.Id.page_number);
            seekbar.Progress = pageno + 1;
            page_number.Text = seekbar.Progress.ToString();
            Core.Media.Recents.AddToRecents(gallery);
            galleryPager.AddOnPageChangedListener(new PageChangeListener(this));
            seekbar.SetOnSeekBarChangeListener(new SeekBarChangeListener(this));
            logger.Info("Gallery Viewer Initialized");
            

            galleryPager.AddOnItemTouchListener(new ItemTouchListener(this));
            countDown.Start();
            //op.SetOnTouchListener(new ScreenTouchListener(this));
            SupportActionBar.Title = PageList[pageno].name;
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

         }

        public class ItemTouchListener : RecyclerView.SimpleOnItemTouchListener
        {
            GalleryViewer GalleryViewer;
            Timer timer;
            public ItemTouchListener(GalleryViewer viewer)
            {
                GalleryViewer = viewer;
                timer = new Timer(1000, 10);
                timer.Finish += Timer_Finish;

            }

            private void Timer_Finish(object sender, EventArgs e)
            {
                GalleryViewer.ToggleOverlay();
            }

            public override bool OnInterceptTouchEvent(RecyclerView rv, MotionEvent e)
            {
                if (e.Action == MotionEventActions.Down)
                {
                    if (timer.IsRunning)
                        timer.CancelTimer();
                    else
                        timer.StartTimer();
                }
                if(e.Action == MotionEventActions.Move)
                {
                    timer.CancelTimer();
                }
                return base.OnInterceptTouchEvent(rv, e);
            }

            internal class Timer : CountDownTimer
            {
                public event EventHandler Finish;
                public bool IsRunning = false;

                public Timer(long ms, long interval) : base(ms, interval)
                {

                }

                public void StartTimer()
                {
                    IsRunning = true;
                    Start();
                }

                public void CancelTimer()
                {
                    Cancel();
                    IsRunning = false;
                }

                public override void OnFinish()
                {
                    IsRunning = false;
                    Finish.Invoke(null, null);
                }

                public override void OnTick(long millisUntilFinished)
                {

                }
            }
        }


      

        private void FilterSlider_Click(object sender, EventArgs e)
        {
            countDown.Start();
        }


        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            return base.DispatchTouchEvent(ev);
        }

        private void FilterSlider_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            int val = e.Progress;
            countDown.Start();
            SetScreenBrightness(val);
        }

        void SetScreenBrightness(int level)
        {
            int alpha = 255 - level;
            if (alpha < 0)
                alpha = 0;
            if (alpha > 255)
                alpha = 255;
            SetAlpha(ScreenFilter, alpha);
        }

        void SetAlpha(View view, int alpha)
        {
            var bg = view.Background;
            bg.SetAlpha(alpha);
            Core.App.Settings.FilterLevel = FilterSlider.Progress;

        }

        protected override void OnResume()
        {
            
            base.OnResume();
            seekbar.Progress = galleryPager.CurrentPosition + 1;
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnStop()
        {
            countDown.Cancel();
            gallery.LastPageRead = galleryPager.CurrentPosition;
            base.OnStop();

        }

        protected override void OnDestroy()
        {
            adapter.PageList.Clear();
            adapter.NotifyDataSetChanged();
            ViewerCancellationTokenSource.Cancel();
            adapter = null;
            galleryPager.ClearOnScrollListeners();
            galleryPager.ClearOnPageChangedListeners();
            
            galleryPager.RemoveAllViews();
            gallery = null;
            galleryPager = null;
            Core.Media.Recents.SaveRecents();
            seekbar = null;
            lay.RemoveAllViews();
            toolbar = null;
            options = null;
            if (PageList != null)
            {
                PageList.Clear();
                page_number = null;
            }
            FilterSlider = null;
            ScreenFilter = null;
            System.GC.Collect();
            Java.Lang.JavaSystem.Gc();
            logger.Info("Closing Gallery Viewer");
            base.OnDestroy();
        }

        protected override void OnPause()
        {
            gallery.LastPageRead = galleryPager.CurrentPosition;
            Core.Media.Recents.SaveRecents();
            base.OnPause();
        }

        public override bool OnTouchEvent(MotionEvent e)
        {

            return gestureDetector.OnTouchEvent(e);

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.viewer_menu, menu);
            var item = toolbar.Menu.FindItem(Resource.Id.filter);
            FilterClickListerner lis = new FilterClickListerner(this);
            item.SetOnMenuItemClickListener(lis);
            return base.OnCreateOptionsMenu(menu);
        }

        class FilterClickListerner : Java.Lang.Object, IMenuItemOnMenuItemClickListener
        {
            SeekBar filterbar;
            GalleryViewer activity;

            public FilterClickListerner(GalleryViewer act)
            {
                activity = act;
                filterbar = act.FilterSlider;
            }

            public bool OnMenuItemClick(IMenuItem item)
            {
                if (filterbar != null)
                {
                    if (filterbar.Visibility != ViewStates.Visible)
                        filterbar.Visibility = ViewStates.Visible;
                    else
                        filterbar.Visibility = ViewStates.Gone;
                }
                activity.countDown.Start();
                return true;
            }
        }
        

        public class TapsDetector : GestureDetector.SimpleOnGestureListener
        {
            GalleryViewer galleryViewer;
            public TapsDetector(GalleryViewer viewer)
            {
                galleryViewer = viewer;
            }

            public override bool OnSingleTapConfirmed(MotionEvent e)
            {
                galleryViewer.ToggleOverlay();
                return true;
            }

            public override bool OnSingleTapUp(MotionEvent e)
            {
                return base.OnSingleTapUp(e);
            }

            public override bool OnDown(MotionEvent e)
            {
                return true;
            }

        }


        public class SeekBarChangeListener : Java.Lang.Object, SeekBar.IOnSeekBarChangeListener
        {
            GalleryViewer activity;
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public SeekBarChangeListener(GalleryViewer activity)
            {
                this.activity = activity;
            }   
            public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
            {
                //throw new NotImplementedException();
            }

            public void OnStartTrackingTouch(SeekBar seekBar)
            {
                //throw new NotImplementedException();
            }

            public void OnStopTrackingTouch(SeekBar seekBar)
            {
                int new_position = seekBar.Progress;
                if (new_position < 1)
                    new_position = 1;
                seekBar.Progress = new_position;
                activity.galleryPager.ScrollToPosition(new_position - 1);
                activity.page_number.Text = seekBar.Progress.ToString();

            }
        }

        public class PageChangeListener : RecyclerViewPager.OnPageChangedListener
        {
            GalleryViewer mactivity;
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public PageChangeListener(GalleryViewer activity)
            {
                mactivity = activity;
            }
            public void OnPageChanged(int oldPosition, int newPosition)
            {
                mactivity.seekbar.Progress = newPosition + 1;
                mactivity.page_number.Text = (newPosition + 1).ToString() ;
                mactivity.toolbar.Title = mactivity.adapter.PageList[newPosition].name;
                mactivity.gallery.LastPageRead = mactivity.galleryPager.CurrentPosition;
                Core.Media.Recents.SaveRecents();
            }
        }

        

        public List<string> GetPictureList(string imagefile)
        {
            List<string> imgList = new List<string>();
            string folder = Directory.GetParent(imagefile).FullName;
            var list = Directory.EnumerateFiles(folder, "*", SearchOption.TopDirectoryOnly);
            foreach(string file in list)
            {
                if(file.Contains(".jpg")|| file.Contains(".png")|| file.Contains(".gif"))
                {
                    imgList.Add(file);
                }
            }
            return imgList;
        }

        public void InitPageGen()
        {

            Core.Gallery.Page[] pages = PageList.ToArray();
            int[] ids = new int[pages.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = pages[i].id;
            }
            Core.Gallery.InitiateImageGeneration(ids, "page","original",ViewerCancellationTokenSource.Token);
            
        }

        public void ToggleOverlay()
        {
            RunOnUiThread(() =>
            {
                try
                {
                    if (overlayVisible)
                    {
                        overlayVisible = false;
                        toolbar.Visibility = ViewStates.Gone;
                        seekbar.Visibility = ViewStates.Gone;
                        page_number.Visibility = ViewStates.Gone;
                        FilterSlider.Visibility = ViewStates.Gone;
                        countDown.Cancel();
                    }
                    else
                    {
                        overlayVisible = true;
                        toolbar.Visibility = ViewStates.Visible;
                        seekbar.Visibility = ViewStates.Visible;
                        page_number.Visibility = ViewStates.Visible;
                        countDown.Start();

                    }
                }catch(System.Exception ex)
                {

                }
            });

            
        }

        public void NextPage()
        {
            int pos = galleryPager.CurrentPosition;
            if (pos < galleryPager.ItemCount-1)
                galleryPager.SmoothScrollToPosition(pos + 1);

        }

        public void PreviousPage()
        {
            int pos = galleryPager.CurrentPosition;
            if (pos > 0)
                galleryPager.SmoothScrollToPosition(pos - 1);
        }


        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (e.Action == KeyEventActions.Down)
            {
                int pos = galleryPager.CurrentPosition;
                switch (keyCode)
                {
                    case Keycode.VolumeUp:
                        return true;
                    case Keycode.DpadLeft:
                        return true;
                    //break;
                    case Keycode.VolumeDown:
                        return true;
                    case Keycode.DpadRight:
                        return true;
                        //break;
                }
            }
            return false;
        }

        public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {
           
            if (e.Action == KeyEventActions.Up)
            {
                int pos = galleryPager.CurrentPosition;
                switch (keyCode)
                {
                    case Keycode.VolumeUp:
                        PreviousPage();
                        return true;
                    case Keycode.DpadLeft:
                        PreviousPage();
                        return true;
                    //break;
                    case Keycode.VolumeDown:
                        NextPage();
                        return true;
                    case Keycode.DpadRight:
                        NextPage();
                        return true;
                    //break;
                    case Keycode.Back:
                        base.OnBackPressed();
                        return true;
                            
                }
            }
            return false;
        }

        public class UICountdown : CountDownTimer
        {
            GalleryViewer viewer;
            public UICountdown(long ms,long interval, GalleryViewer viewer):base(ms,interval)
            {
                this.viewer = viewer;
            }

            public override void OnFinish()
            {
                viewer.ToggleOverlay();
            }

            public override void OnTick(long millisUntilFinished)
            {
                
            }
        }




        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    return true;
                case Resource.Id.refresh:
                    {
                        var lay = (LinearLayoutManager)galleryPager.GetLayoutManager();
                        int pos = lay.FindFirstCompletelyVisibleItemPosition();
                        if(pos > -1)
                        {
                            var holder = (ImageViewHolder)galleryPager.FindViewHolderForLayoutPosition(pos);
                            holder.imageView.Loaded = false;
                            holder.imageView.Refresh(true);
                        }
                    }
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        

        public  class ImageAdapter : RecyclerView.Adapter , IOnItemChangedListener
        {
            public List<Core.Gallery.Page> PageList;
            private static Logger logger = LogManager.GetCurrentClassLogger();
            Context context;
            CancellationTokenSource AdapterCancellationTokenSource = new CancellationTokenSource();
            IOnRecyclerViewItemClickListener mOnItemClickListener;
            public ImageAdapter(List<Core.Gallery.Page> imagelist, Context context)
            {
                this.context = context;
                PageList = imagelist;
            }

            public override int ItemCount
            {
                get
                {
                    return PageList.Count;
                }
            }

            public override long GetItemId(int position)
            {
                return position;
            }


            public override void OnViewRecycled(Java.Lang.Object holder)
            {
                if (holder is ImageViewHolder hold)
                {
                    hold.imageView.Release();
                }
                base.OnViewRecycled(holder);
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                ImageViewHolder vh = holder as ImageViewHolder;
                var h = new Handler(Looper.MainLooper);
                var activity = (GalleryViewer)context;

               Task.Run(() =>
                {
                    
                        vh.imageView.OnLoadStart(PageList[position]);
                });
            }

            
            public int IndexOf(Core.Gallery.Page item)
            {
                return PageList.IndexOf(item);
            }

            public class Onclick : Java.Lang.Object,View.IOnClickListener
            {
                IOnRecyclerViewItemClickListener ionclick;
                int position;
                RecyclerView.ViewHolder holder;
                public Onclick(ImageAdapter.IOnRecyclerViewItemClickListener onclick,int position,
                    RecyclerView.ViewHolder holder)
                {
                    ionclick = onclick;
                    this.position = position;
                    this.holder = holder;

                }
                public void OnClick(View v)
                {
                    if ( ionclick!= null)
                    {
                        ionclick.OnItemClick(v,position,holder);
                    }
                }
            }

            public ImageAdapter Add(Core.Gallery.Page item)
            {
                PageList.Add(item);

                return this;
            }
            
            public ImageAdapter Insert(int position, Core.Gallery.Page item)
            {
                PageList.Insert(position,item);
                NotifyItemInserted(position);
                return this;
            }

            public ImageAdapter Set(int index, Core.Gallery.Page item)
            {
                if (index > -1)
                    PageList[index] = item;
                NotifyItemChanged(index);
                return this;
            }

            public ImageAdapter Remove(int index)
            {
                PageList.RemoveAt(index);
                if (index == 0)
                {
                    NotifyDataSetChanged();
                }
                else if(index > 0)
                {
                    NotifyItemRemoved(index);
                }
                return this;
            }

            public ImageAdapter Remove(Core.Gallery.Page item)
            {
                int position = PageList.IndexOf(item);
                PageList.Remove(item);
                if (position == 0)
                {
                    NotifyDataSetChanged();
                }
                else if (position > 0)
                {
                    NotifyItemRemoved(position);
                }
                return this;
            }

            public ImageAdapter Clear()
            {
                int size = PageList.Count;
                PageList.Clear();
                NotifyItemRangeRemoved(0, size);
                return this;
            }

            

            public interface IOnRecyclerViewItemClickListener {
                void OnItemClick(View view, int position,RecyclerView.ViewHolder viewHolder);
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                /*var imageview = Android.Views.LayoutInflater.From(parent.Context).
                    Inflate(Resource.Layout.ImageViewTemplate, parent,false);*/
                Custom_Views.ImageViewHolder img = new Custom_Views.ImageViewHolder(context);
                return new ImageViewHolder(img);
            }

            public void Swap(List<Core.Gallery.Page> list, int p1, int p2)
            {
                Core.Gallery.Page temp = list[p1];
                list[p1] = list[p2];
                list[p2] = temp;
            }

            public bool OnItemMove(int fromPosition, int toPosition)
            {
                if (fromPosition < toPosition)
                {
                    for (int i = fromPosition; i < toPosition; i++)
                    {
                        
                        Swap(PageList, i, i + 1);
                    }
                }
                else
                {
                    for (int i = fromPosition; i > toPosition; i--)
                    {
                        Swap(PageList, i, i - 1);
                    }
                }
                NotifyItemMoved(fromPosition, toPosition);
                return true;
            }

            public void OnItemDismiss(int position)
            {
                Remove(position);
            }

            public IOnRecyclerViewItemClickListener GetOnItemClickListener()
            {
                return mOnItemClickListener;
            }

            public ImageAdapter SetOnItemClickListener(
      IOnRecyclerViewItemClickListener itemClickListener)
            {
                mOnItemClickListener = itemClickListener;
                return this;
            }
        }

        public class ImageViewHolder : RecyclerView.ViewHolder
        {
            
            public Custom_Views.ImageViewHolder imageView;
            public bool loaded = false;
            public string page_path = string.Empty;
            public ImageViewHolder(Custom_Views.ImageViewHolder itemView) : base(itemView)
            {
                imageView = itemView;
               //this.IsRecyclable = false;
            }
            
        }



        public interface IOnItemChangedListener
        {

            bool OnItemMove(int fromPosition, int toPosition);

            void OnItemDismiss(int position);
        }

        
    }
}