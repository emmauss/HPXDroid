
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
        FrameLayout lay;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /*List<string> ImageList =
            new List<string>();*/
        List<Core.Gallery.Page> PageList =
            new List<Core.Gallery.Page>();
        SeekBar seekbar;
        UICountdown countDown;

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
            //InitPageGen();

            options = new RequestOptions()
                .Override(Target.SizeOriginal, Target.SizeOriginal);
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            lay = FindViewById<FrameLayout>(Resource.Id.frame);
            galleryPager = FindViewById<RecyclerViewPager>(Resource.Id.galleryViewPager);
            var layout = new ExtraLayoutManager(this, LinearLayoutManager.Horizontal, false);    
            galleryPager.SetLayoutManager(layout);

            adapter = new ImageAdapter(PageList,this);
            galleryPager.SetAdapter(new RecyclerViewPagerAdapter(galleryPager, adapter));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            countDown = new UICountdown(2000, 10, this);

            seekbar = FindViewById<SeekBar>(Resource.Id.progress_seekbar);
            seekbar.Max = PageList.Count;
            seekbar.Progress = galleryPager.CurrentPosition + 1;
            galleryPager.GetAdapter().NotifyDataSetChanged();
            page_number = FindViewById<TextView>(Resource.Id.page_number);
            page_number.Text = seekbar.Progress.ToString();
            galleryPager.AddOnPageChangedListener(new PageChangeListener(this));
            seekbar.SetOnSeekBarChangeListener(new SeekBarChangeListener(this));
            logger.Info("Gallery Viewer Initialized");
            /*int pos = list.IndexOf(data);
            if (pos < 0)
                pos = 0;
            imageHandler.StartReader(galleryPager, this, pos, galleryPager);*/

         }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnDestroy()
        {
            logger.Info("Closing Gallery Viewer");
            base.OnDestroy();
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            {
                ToggleOverlay(true);
            }
            return base.OnTouchEvent(e);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.viewer_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            
            bool res = base.DispatchTouchEvent(ev);
            return res;
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
            private static readonly int DEFAULT_EXTRA_LAYOUT_SPACE = 800;
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

        public async void ToggleOverlay(bool toggle)
        {
            RunOnUiThread(() =>
            {
                if (!toggle)
                {
                    overlayVisible = false;
                    toolbar.Visibility = ViewStates.Gone;
                    seekbar.Visibility = ViewStates.Gone;
                }
                else
                {
                    overlayVisible = true;
                    toolbar.Visibility = ViewStates.Visible;
                    seekbar.Visibility = ViewStates.Visible;
                    countDown.Start();
                    
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
                viewer.ToggleOverlay(false);
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