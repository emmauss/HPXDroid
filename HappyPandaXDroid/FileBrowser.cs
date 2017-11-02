using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.View;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;

namespace HappyPandaXDroid
{
    [Activity(Label = "FileBrowser")]
    public class FileBrowser : AppCompatActivity
    {
        public List<FileItem> filelist = new List<FileItem>();
        Toolbar toolbar;
        string current_path;
        public RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        ListViewAdapter adapter;
        public struct EventPair
        {
            public View view;
            public MotionEvent motionevent;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FileBrowser);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            // Create your application here
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.filelist);
            adapter = new ListViewAdapter(filelist, this);
            adapter.ItemClick += OnItemClick;
            mRecyclerView.SetAdapter(adapter);
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            current_path = Android.OS.Environment.ExternalStorageDirectory.Path;
        }

        protected override void OnResume()
        {
            base.OnResume();
            RefreshList();

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public class FileItem
        {
            private string filename;
            private string path;
            private string type;

            public string Filename
            {
                get
                {
                    return filename;
                }
                set
                {
                    filename = value;
                }
            }

            public string Path
            {
                get
                {
                    return path;
                }
                set
                {
                    path = value;
                }
            }

            public string Type
            {
                get
                {
                    return type;
                }
                set
                {
                    this.type = value;
                }
            }
        }

        public class ListViewHolder : RecyclerView.ViewHolder
        {
            public TextView filename;
            public ImageView fileimage;
            public View itemView;
            public string path = string.Empty;
            public ListViewHolder(View itemView, Action<int> clicklistener) : base(itemView)
            {
                filename = itemView.FindViewById<TextView>(Resource.Id.file_picker_text);
                fileimage = itemView.FindViewById<ImageView>(Resource.Id.file_picker_image);
                
                TouchListener listen = new TouchListener();
                //itemView.Click += (s, e) => clicklistener(base.AdapterPosition);
                itemView.SetOnTouchListener(listen);
                itemView.Click += (s, e) => clicklistener(base.AdapterPosition);
                this.itemView = itemView;
            }

            public class TouchListener : Java.Lang.Object,View.IOnTouchListener
            {
               
                public bool OnTouch(View v, MotionEvent e)
                {
                    switch (e.Action)
                    {
                        case MotionEventActions.Down:
                            {
                                v.Background =
                                    new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.LightGray);
                                break;
                            }
                        default:
                            {
                                v.Background =
                                    new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent);
                                break;
                            }
                    }
                    return false;
                }
            }
        }

        public void RefreshList()
        {
            filelist.Clear();
            FileItem up = new FileItem
            {
                Filename = "...",
                Type = "UP"
            };
            try
            {
                up.Path = Directory.GetParent(current_path).FullName;
            }catch(Exception ex)
            {
                return;
            }
            filelist.Add(up);
            var directories = Directory.EnumerateDirectories(current_path, "*", SearchOption.TopDirectoryOnly);
            foreach (string dir in directories)
            {
                FileItem dirItem = new FileItem
                {
                    Filename = dir.Substring(dir.LastIndexOf("/") + 1),
                    Type = "folder",
                    Path = dir
                };
                filelist.Add(dirItem);
            }
            var files = Directory.EnumerateFiles(current_path, "*", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                FileItem fileItem = new FileItem
                {
                    Filename = file.Substring(file.LastIndexOf("/") + 1),
                    Type = "file",
                    Path = file
                };
                filelist.Add(fileItem);
            }
            toolbar.Title = current_path;
            adapter.NotifyDataSetChanged();
            mRecyclerView.ScrollToPosition(0);
        }


        void OnItemClick(object sender, int position)
        {
            //get file list and refresh recyclerview
            var madapter = (ListViewAdapter)sender;
            if (madapter.mdata[position].Type != "file")
            {
                current_path = madapter.mdata[position].Path;
                RefreshList();
            }
            else
            {
                if (madapter.mdata[position].Filename.Contains(".jpg") 
                    || madapter.mdata[position].Filename.Contains(".png") 
                    || madapter.mdata[position].Filename.Contains(".gif"))
                {
                    Intent intent = new Android.Content.Intent(this, typeof(GalleryViewer));
                    intent.PutExtra("image", madapter.mdata[position].Path);
                    StartActivity(intent);
                }
            }
            
        }
        
        public class ListViewAdapter : RecyclerView.Adapter 
        {
            public EventHandler<int> ItemClick;
            public List<FileItem> mdata;
           
            Android.Content.Context mcontext;
            public ListViewAdapter(List<FileItem> data, Android.Content.Context context)
            {
                mdata = data;
                mcontext = context;
            }

            public override int ItemCount
            {
                get { return mdata.Count; }
            }

            void OnClick(int position)
            {
                ItemClick?.Invoke(this, position);
            }

            

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                ListViewHolder vh = holder as ListViewHolder;
                vh.filename.Text = mdata[position].Filename;
                vh.path = mdata[position].Path;
                switch (mdata[position].Type)
                {
                    case "file":
                        vh.fileimage.SetImageResource(Resource.Drawable.ic_file);
                        break;
                    case "folder":
                        vh.fileimage.SetImageResource(Resource.Drawable.ic_folder);
                        break;
                    case "UP":
                        vh.fileimage.SetImageResource(Resource.Drawable.ic_up);
                        break;

                }
            }
            

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View itemview = Android.Views.LayoutInflater.From(parent.Context)
                    .Inflate(Resource.Layout.fileListItem, parent, false);
                
                ListViewHolder vh = new ListViewHolder(itemview,OnClick);
                return vh;
            }
        }


    }
}