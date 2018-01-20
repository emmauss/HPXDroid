
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

        Custom_Views.ImageViewHolder imageView;
        bool doubl_click = false;
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
            ImageView op = FindViewById<ImageView>(Resource.Id.options);
            lay = FindViewById<FrameLayout>(Resource.Id.frame);
            galleryPager = FindViewById<RecyclerViewPager>(Resource.Id.galleryViewPager);
            var layout = new ExtraLayoutManager(this, LinearLayoutManager.Horizontal, false);    
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
            countDown.Start();
            op.SetOnTouchListener(new ScreenTouchListener(this));
            SupportActionBar.Title = PageList[pageno].name;
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

         }

        public class ScreenTouchListener : Java.Lang.Object, View.IOnTouchListener
        {
            GalleryViewer galleryViewer;
            public ScreenTouchListener(GalleryViewer viewer)
            {
                galleryViewer = viewer;
            }

            public bool OnTouch(View v, MotionEvent e)
            {
                return galleryViewer.gestureDetector.OnTouchEvent(e);
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
            seekbar.Progress = galleryPager.CurrentPosition + 1;
            base.OnResume();
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnStop()
        {
            gallery.LastPageRead = galleryPager.CurrentPosition;
            base.OnStop();

        }

        protected override void OnDestroy()
        {
            adapter.PageList.Clear();
            adapter.NotifyDataSetChanged();
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
        

        public class PhotoImageVIew: PhotoView.PhotoView
        {
            GalleryViewer mactivity;
            public PhotoImageVIew(Context context) : base(context)
            {
                if (context is GalleryViewer)
                    mactivity = (GalleryViewer)context;
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
            }
        }

        public class ExtraLayoutManager : LinearLayoutManager
        {
            private static readonly int DEFAULT_EXTRA_LAYOUT_SPACE = 600;
            private int extraLayoutSpace = -1;
            private Context context;
            

            public ExtraLayoutManager(Context context) : base(context)
            {
                this.context = context;
            }

            public ExtraLayoutManager(Context context, int extraLayoutSpace) : base(context)
            {
                this.context = context;
                this.extraLayoutSpace = extraLayoutSpace;
            }

            

            public ExtraLayoutManager(Context context, int orientation, bool reverseLayout) 
                : base(context,orientation,reverseLayout)
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
            Core.Gallery.InitiateImageGeneration(ids, "page","original");
            
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
                        FilterSlider.Visibility = ViewStates.Gone;
                        countDown.Cancel();
                    }
                    else
                    {
                        overlayVisible = true;
                        toolbar.Visibility = ViewStates.Visible;
                        seekbar.Visibility = ViewStates.Visible;
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

        /*1public override bool DispatchKeyEvent(KeyEvent e)
        {
           
                int pos = galleryPager.CurrentPosition;
                var keyCode = e.KeyCode;
                if(e.Action == KeyEventActions.Up)
                switch (keyCode)
                {
                    case Keycode.VolumeUp:
                        if (e.Action == KeyEventActions.Up)
                        {
                            PreviousPage();
                        }
                        return true;
                    case Keycode.DpadLeft:
                        if (e.Action == KeyEventActions.Up)
                        {
                            PreviousPage();
                        }
                        return true;
                    //break;
                    case Keycode.VolumeDown:
                        if (e.Action == KeyEventActions.Up)
                        {
                            NextPage();
                        }
                        return true;
                    case Keycode.DpadRight:
                        if (e.Action == KeyEventActions.Up)
                        {
                            NextPage();
                        }
                        return true;
                        //break;
                }
            return base.DispatchKeyEvent(e);
        }*/

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
                        if(pos!=null & pos > -1)
                        {
                            var holder = (ImageViewHolder)galleryPager.FindViewHolderForLayoutPosition(pos);
                            holder.imageView.Loaded = false;
                            holder.imageView.Refresh();
                        }
                    }
                    return true;
                    break;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        

        public  class ImageAdapter : RecyclerView.Adapter , IOnItemChangedListener
        {
            public List<Core.Gallery.Page> PageList;
            private static Logger logger = LogManager.GetCurrentClassLogger();
            Context context;
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
                var hold = holder as ImageViewHolder;
                if (hold != null)
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