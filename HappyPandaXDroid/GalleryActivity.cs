using Android.App;
using Android.OS;
using Android.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.View;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Request;
using ProgressView = XamarinBindings.MaterialProgressBar;
using NLog;

namespace HappyPandaXDroid
{
    [Activity(Label = "GalleryActivity", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class GalleryActivity : AppCompatActivity
    {
        public TextView title, category, read_action,
            language, pages, time_posted, no_tags;
        public LinearLayout TagLayout, InfoLayout;
        CardView ActionCard;
        public ImageView ThumbView;
        Core.Gallery.GalleryItem gallery;
        RecyclerView grid_layout;
        ProgressView.MaterialProgressBar mProgressView;
        LinearLayout MainView;
        TextView GalleryStatus;
        PreviewAdapter adapter;
        ScrollView scrollview;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public bool IsRunning = true;
        public int activityId;
        List<Core.Gallery.Page> pagelist;
        //public List<Tuple<Task,CancellationTokenSource>> tasklist = new List<Tuple<Task, CancellationTokenSource>>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Gallery_Details_Layout);
            string data = Intent.GetStringExtra("gallery");
            InitializeViews();
            gallery = Core.JSON.Serializer.SimpleSerializer.Deserialize<Core.Gallery.GalleryItem>(data);
            logger.Info("Initializing Gallery Detail. GalleryId ={0}", gallery.id);
            



            ParseMeta();
            this.Window.SetType(WindowManagerTypes.KeyguardDialog);
            string path = string.Empty ;
            Task.Run(async () =>
            {
                path = await Core.Gallery.GetThumb(gallery);
                RunOnUiThread(() =>
                {
                    try
                    {
                        if(path.Contains("fail"))
                        {
                            GalleryStatus.Text = "Gallery Not Found";
                            Glide.With(this)
                            .Load(Resource.Drawable.image_failed)
                            .Into(ThumbView);
                        }
                        else
                        Glide.With(this)
                            .Load(path)
                            .Into(ThumbView);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "\n Exception Caught In GalleryActivity.Oncreate.");
                    }
                });
                gallery.tags = await Core.Gallery.GetTags(gallery.id, "Gallery");
                pagelist = Core.App.Server.GetRelatedItems<Core.Gallery.Page>(gallery.id);
                ParseData();
                RunOnUiThread(() =>
                {
                    mProgressView.Visibility = ViewStates.Invisible;
                    MainView.Visibility = ViewStates.Visible;
                });
            });
            

        }

        protected override void OnDestroy()
        {
            IsRunning = false;
            base.OnDestroy();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        void InitializeViews()
        {
            mProgressView = FindViewById<ProgressView.MaterialProgressBar>(Resource.Id.progress_view);
            mProgressView.Visibility = ViewStates.Visible;
            MainView = FindViewById<LinearLayout>(Resource.Id.below_header);
            MainView.Visibility = ViewStates.Gone;
            title = FindViewById<TextView>(Resource.Id.title);
            category = FindViewById<TextView>(Resource.Id.category);
            read_action = FindViewById<TextView>(Resource.Id.read);
            language = FindViewById<TextView>(Resource.Id.language);
            pages = FindViewById<TextView>(Resource.Id.pages);
            time_posted = FindViewById<TextView>(Resource.Id.posted);
            no_tags = FindViewById<TextView>(Resource.Id.no_tags);
            scrollview = FindViewById<ScrollView>(Resource.Id.scroll_view);
           grid_layout = FindViewById<RecyclerView>(Resource.Id.grid_layout);
            TagLayout = FindViewById<LinearLayout>(Resource.Id.tags);
            InfoLayout = FindViewById<LinearLayout>(Resource.Id.info);
            ActionCard = FindViewById<CardView>(Resource.Id.action_card);
            GalleryStatus = FindViewById<TextView>(Resource.Id.status);
            ThumbView = FindViewById<ImageView>(Resource.Id.thumb);

            ActionCard.Clickable = true;
            ActionCard.Click += ActionCard_Click;
            adapter = new PreviewAdapter(this);

            grid_layout.SetAdapter(adapter);
            var layout = new GridLayoutManager(this, 5);
            grid_layout.SetLayoutManager(layout);
        }

        private void ActionCard_Click(object sender, EventArgs e)
        {
            List<int> pages_ids = new List<int>();

            if (pagelist == null)
                return;
            if ( pagelist==null & pagelist.Count < 1)
                return;

            Intent intent = new Android.Content.Intent(this, typeof(GalleryViewer));
            intent.PutExtra("page", Core.JSON.Serializer.SimpleSerializer.Serialize(pagelist));
            StartActivity(intent);

        }

        public void ParseMeta()
        {
            title.Text = gallery.titles[0].name;
            category.Text = "place_holder";
            
        }

        async void ParseData()
        {
            

            RunOnUiThread(() =>
            {
                if (gallery.tags.Language.Count > 0)
                {
                    string lan = gallery.tags.Language[0].name;
                    language.Text = System.Globalization.CultureInfo.CurrentCulture
                        .TextInfo.ToTitleCase(lan.ToLower());
                }
                else
                    language.Text = "eng";
                pages.Text = pagelist.Count.ToString() + " Pages";
                adapter.SetList(pagelist);
                ParseTags();
            });
        }

        protected override void OnPause()
        {
            base.OnPause();
        }


#pragma warning disable 618
        public void SetTagLayout()
        {

            var inflater = LayoutInflater;
            //reclass


            int color_header = Resources.GetColor(Resource.Color.colorPrimary);
            int color_tag = Resources.GetColor(Resource.Color.purple_a700);
            Type t = gallery.tags.GetType();
            PropertyInfo[] namespaces = t.GetProperties();
            foreach(var _namespace in namespaces)
            {
                object value = _namespace.GetValue(gallery.tags);
                string name = _namespace.Name;
                if (name.Contains("__"))
                    name = "misc";
                if (value != null)
                {
                    var tags = (List<Core.Gallery.TagItem>)value;
                    if (tags.Count > 0)
                    {
                        
                        var rtg = (LinearLayout)inflater.Inflate(Resource.Layout.tag_group_template, TagLayout, false);
                        rtg.Orientation = Orientation.Horizontal;
                        TagLayout.AddView(rtg);
                        TextView tag_header = (TextView)inflater.Inflate(Resource.Layout.tag_template, rtg, false);
                        tag_header.Text = name.ToLower();
                        tag_header.SetBackgroundDrawable(new Custom_Views.RoundSideRectDrawable(color_header));
                        rtg.AddView(tag_header);
                        Custom_Views.AutoWrapLayout awl = new Custom_Views.AutoWrapLayout(this);
                        rtg.AddView(awl, ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                        tags.Sort((a, b) => a.name.CompareTo(b.name));
                        foreach (var tag in tags)
                        {
                            TextView tag_item = (TextView)inflater.Inflate(Resource.Layout.tag_template, awl, false);
                            tag_item.Text = tag.name;                            
                            tag_item.SetBackgroundDrawable(new Custom_Views.RoundSideRectDrawable(color_tag));
                            tag_item.Click += Tag_item_Click;
                            string fullTagName = _namespace.Name + ":" + tag.name;
                            tag_item.Tag = fullTagName;
                            tag_item.Clickable=true;
                            awl.AddView(tag_item);
                        }
                    }
                }InfoLayout.RequestFocus();
            }
            
        }

        private void Tag_item_Click(object sender, EventArgs e)
        {
            TextView tag_item = sender as TextView;
            if (tag_item == null)
                return;
            Android.Content.Intent intent = new Android.Content.Intent(this, typeof(SearchActivity));
            intent.PutExtra("query", (string)tag_item.Tag);
            logger.Info("search init :" + (string)tag_item);
            StartActivity(intent); 

        }

        void ParseTags()
        {
            if (!IsTagAvailable())
                return;
            TagLayout.RemoveAllViews();

            
            
            SetTagLayout();
            TagLayout.Visibility = ViewStates.Visible;
            
        }

        bool IsTagAvailable()
        {
            int count = 0;
            var taglist = gallery.tags;
            if (taglist.Artist != null)
                count += taglist.Artist.Count;
            if (taglist.Female != null)
                count += taglist.Female.Count;
            if (taglist.Language != null)
                count += taglist.Language.Count;
            if (taglist.Male != null)
                count += taglist.Male.Count;
            if (taglist.Reclass != null)
                count += taglist.Reclass.Count;
            if (taglist.__namespace__ != null)
                count += taglist.__namespace__.Count;
            if (taglist.Parody != null)
                count += taglist.Parody.Count;

            if (count == 0)
                return false;
            else
                return true;
        }
    }

    public class PreviewAdapter : RecyclerView.Adapter
    {
        public int preview_count = 10;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<Core.Gallery.Page> mdata;
        Android.Content.Context mcontext;
        public PreviewAdapter(Context context)
        {
            mcontext = context;
        }

        public override int ItemCount
        {
            get { return mdata==null?0:mdata.Count; }
        }

        public void SetList(List<Core.Gallery.Page> UrlList) 
        {
            int number = preview_count;
            if (UrlList.Count < 10)
                number = UrlList.Count;
            mdata = new List<Core.Gallery.Page>();
            for(int i = 0; i < number; i++)
            {
                mdata.Add(UrlList[i]);
            }
            //mdata.Capacity = 20;
            mdata.TrimExcess();
            NotifyDataSetChanged();
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            PreviewViewHolder vh = holder as PreviewViewHolder;
            var page = mdata[position];
            var activity = (GalleryActivity)mcontext;
            var cts = new CancellationTokenSource();

            Task.Run(async () =>
          {
              try
              {
                  await vh.LoadPreview(page);
              }
              catch (Exception ex)
              {
                  logger.Error(ex, "\n Exception Caught In GalleryActivity.PreviewAdaptor.OnBindViewHolder.");
              }

          });
            
            vh.txt.Text = mdata[position].number.ToString();
        }




        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemview = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.preview_template, parent, false);
            PreviewViewHolder vh = new PreviewViewHolder(itemview);
            return vh;
        }
    }

    

    public class PreviewViewHolder : RecyclerView.ViewHolder
    {
        public View preview;
        public ImageView img;
        public TextView txt;
        public int position;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public bool loaded = false;
        public Core.Gallery.Page page;
        string thumb_path;
        public PreviewViewHolder(View itemView) : base(itemView)
        {
            
            preview = itemView;
            img = preview.FindViewById<ImageView>(Resource.Id.preview);
            txt = preview.FindViewById<TextView>(Resource.Id.title);
            preview.SetOnClickListener(new PreviewClickListener());
        }

        bool IsCached
        {
            get
            {
                int item_id = page.id;
                try
                {
                    thumb_path = Core.App.Settings.cache + "preview/" + Core.App.Server.HashGenerator("medium", "preview", item_id) + ".jpg";
                    bool check = Core.Media.Cache.IsCached(thumb_path);

                    return check;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In GalleryCard.IsCached.");

                    return false;
                }
            }
        }

        public async Task<bool> LoadPreview(Core.Gallery.Page page)
        {
            this.page = page;
            int tries = 0;
            try
            {

                var h = new Handler(Looper.MainLooper);
                bool exists = await Core.Gallery.IsSourceExist("page", page.id);
                if (!exists)
                {
                    h.Post(() =>
                    {
                        Glide.With(preview.Context)
                                .Load(Resource.Drawable.image_failed)
                                .Into(img);
                    });
                    return false;
                }
                h.Post(() =>
                {
                try
                {
                    if (((GalleryActivity)(preview.Context)).IsRunning)
                        Glide.With(preview.Context)
                 .Load(Resource.Drawable.loading2)
                 .Into(img);
                    }
                    catch (System.Exception ex)
                    {
                        if (ex.Message.Contains("destroyed"))
                            return;
                    }
                });
                    while (!IsCached)
                    {
                    exists = await Core.Gallery.IsSourceExist("page", page.id);
                    if (!exists)
                    {
                        h.Post(() =>
                        {
                            Glide.With(preview.Context)
                                    .Load(Resource.Drawable.image_failed)
                                    .Into(img);
                        });
                        return false;
                    }

                    page.thumb_url = await Core.Gallery.GetImage(page, false,"medium",true);
                        if (page.thumb_url.Contains("fail"))
                        {
                            if (page.thumb_url.Contains("misc"))
                            {
                                tries++;
                                if (tries < 4)
                                {
                                    continue;
                                }
                            h.Post(() =>
                            {
                                Glide.With(preview.Context)
                                .Load(Resource.Drawable.image_failed)
                                .Into(img);
                            });
                                return false;

                            }
                        h.Post(() =>
                        {
                            Glide.With(preview.Context)
                            .Load(Resource.Drawable.image_failed)
                            .Into(img);
                        });
                        return false;
                        }
                        else
                        {
                            break;
                        }
                    }
                
                h = new Handler(Looper.MainLooper);
                h.Post(() =>
                {
                    if(((GalleryActivity)(preview.Context)).IsRunning)
                    Glide.With(preview.Context)
                         .Load(thumb_path)
                         .Into(img);
                    loaded = true;
                });
                tries = 0;
                return true;
            }
            catch(Exception ex)
            {
                logger.Error(ex, "\n Exception Caught In GalleryActivity.PreviewHolder.LoadPreview.");

                tries = 0;
                return false;
            }
            
        }

        class PreviewClickListener : Java.Lang.Object, View.IOnClickListener
        {
            View preview;
            public void OnClick(View v)
            {
                /*preview = v;
                Intent intent = new Intent(preview.Context, typeof(GalleryActivity));
                /*string gallerystring = Core.JSON.Serializer.simpleSerializer.Serialize(preview.Gallery);
                intent.PutExtra("gallery", gallerystring);
                card.Context.StartActivity(intent);*/
            }
        }


    }
}