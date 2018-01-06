using Android.App;
using Android.OS;
using Android.Content;
using Android.Support.V7.App;
using Android.Runtime;
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
using Android.Content.Res;

namespace HappyPandaXDroid
{
    [Activity(Label = "GalleryActivity", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class GalleryActivity : AppCompatActivity
    {
        private TextView mErrorText;
        public TextView title, category, read_action,
            language, pages, time_posted, no_tags;
        public LinearLayout TagLayout;
        CardView ActionCard, ContinueCard;
        public string thumb_path;
        public ImageView ThumbView;
        FrameLayout errorFrame;
        Core.Gallery.GalleryItem gallery;
        RecyclerView grid_layout;
        ProgressView.MaterialProgressBar mProgressView;
        LinearLayout MainView;
        TextView GalleryStatus;
        Custom_Views.PreviewAdapter adapter;
        bool loaded = false;
        ScrollView scrollview;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public bool IsRunning = true;
        GridLayoutManager layout;
        public int PreviewColumns;
        List<Core.Gallery.Page> pagelist, cachedlist;
        private ImageView mErrorImage;

        //public List<Tuple<Task,CancellationTokenSource>> tasklist = new List<Tuple<Task, CancellationTokenSource>>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            SetContentView(Resource.Layout.Gallery_Details_Layout);
            string data = Intent.GetStringExtra("gallery");
            InitializeViews();
            gallery = Core.JSON.Serializer.SimpleSerializer.Deserialize<Core.Gallery.GalleryItem>(data);
            logger.Info("Initializing Gallery Detail. GalleryId ={0}", gallery.id);
            thumb_path = Intent.GetStringExtra("thumb");


            ParseMeta();
            this.Window.SetType(WindowManagerTypes.KeyguardDialog);

            ThreadStart start = new ThreadStart(() =>
            {
                try
                {
                    Load();
                }
                catch (Exception ex)
                {
                    loaded = false;
                }

                if (!loaded)
                {
                    RunOnUiThread(() =>
                    {
                        mErrorText.Text = "Error";
                        MainView.Visibility = ViewStates.Gone;
                        mProgressView.Visibility = ViewStates.Gone;
                        errorFrame.Visibility = ViewStates.Visible;
                    });
                }
            });
            Thread thread = new Thread(start);
            thread.Start();


        }


        //bg thread unhandled exception handler
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (System.Exception)e.ExceptionObject;
            logger.Fatal(ex, ex.Message);
        }

        //ui thread unhandled exception handler
        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            logger.Fatal(e.Exception, "Fatal Exception Thrown : " + e.Exception.Message);
        }



        public void Load()
        {
            if (Core.Net.Connect())
            {
                thumb_path = string.Empty;
                thumb_path = Core.Gallery.GetThumb(gallery).Result;
                RunOnUiThread(() =>
                {
                    try
                    {
                        if (thumb_path.Contains("fail"))
                        {
                            GalleryStatus.Text = "Gallery Not Found";
                            Glide.With(this)
                            .Load(Resource.Drawable.image_failed)
                            .Into(ThumbView);
                        }
                        else
                            Glide.With(this)
                                .Load(thumb_path)
                                .Into(ThumbView);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "\n Exception Caught In GalleryActivity.Oncreate.");
                    }
                });
                gallery.tags = Core.Gallery.GetTags(gallery.id, "Gallery").Result;
                pagelist = Core.App.Server.GetRelatedItems<Core.Gallery.Page>(gallery.id);
                ParseData();
                if (!IsDestroyed)
                    RunOnUiThread(() =>
                    {
                        mProgressView.Visibility = ViewStates.Invisible;
                        MainView.Visibility = ViewStates.Visible;
                        errorFrame.Visibility = ViewStates.Gone;
                    });
                loaded = true;
            }
            else
            {
                loaded = false;
            }
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            SetColumns();
            layout = new GridLayoutManager(this, PreviewColumns);
            grid_layout.SetLayoutManager(layout);
        }

        protected override void OnDestroy()
        {
            IsRunning = false;
            mProgressView = null;
            MainView = null;
            title = null;
            category = null;
            read_action = null;
            language = null;
            pages = null;
            time_posted = null;
            no_tags = null;
            scrollview = null;
            errorFrame.Dispose();
            errorFrame = null;
            if (adapter.mdata != null)
            {
                adapter.mdata.Clear();
                adapter.NotifyDataSetChanged();
            }
            adapter = null;
            grid_layout = null;
            TagLayout = null;
            ActionCard.Click -= ActionCard_Click;
            ActionCard = null;
            GalleryStatus = null;
            ThumbView.SetImageDrawable(null);
            ThumbView = null;
            if (pagelist != null)
            {
                pagelist.Clear();
                pagelist = null;
            }
            GC.Collect();
            Java.Lang.JavaSystem.Gc();
            base.OnDestroy();
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (loaded)
            {
                try
                {
                    if (thumb_path.Contains("fail"))
                    {
                        GalleryStatus.Text = "Gallery Not Found";
                        Glide.With(this)
                        .Load(Resource.Drawable.image_failed)
                        .Into(ThumbView);
                    }
                    else
                        Glide.With(this)
                            .Load(thumb_path)
                            .Into(ThumbView);
                }
                catch (Exception ex)
                {

                }
                pagelist.AddRange(cachedlist);
                cachedlist.Clear();
                adapter.NotifyDataSetChanged();
            }
            Task.Run(async () =>
            {
                await Task.Delay(3000);
                var item = Core.Media.Recents.GetRecentGallery(gallery.id);
                if (item != null)
                    if (gallery.titles[0].name == item.titles[0].name)
                    {
                        gallery.LastPageRead = item.LastPageRead;
                    }

                RunOnUiThread(() =>
                {
                    if (gallery.LastPageRead < 1)
                        ContinueCard.Enabled = false;
                    else
                        ContinueCard.Enabled = true;
                });
            });

        }

        void InitializeViews()
        {

            mProgressView = FindViewById<ProgressView.MaterialProgressBar>(Resource.Id.progress_view);
            mProgressView.Visibility = ViewStates.Visible;
            MainView = FindViewById<LinearLayout>(Resource.Id.below_header);
            errorFrame = FindViewById<FrameLayout>(Resource.Id.error_frame);
            errorFrame.Visibility = ViewStates.Gone;
            errorFrame.Click += ErrorFrame_Click;
            MainView.Visibility = ViewStates.Gone;

            mErrorImage = FindViewById<ImageView>(Resource.Id.error_image);
            mErrorImage.SetImageResource(Resource.Drawable.big_weird_face);
            mErrorText = FindViewById<TextView>(Resource.Id.error_text);


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
            ActionCard = FindViewById<CardView>(Resource.Id.action_card);
            GalleryStatus = FindViewById<TextView>(Resource.Id.status);
            ThumbView = FindViewById<ImageView>(Resource.Id.thumb);
            ContinueCard = FindViewById<CardView>(Resource.Id.continue_card);
            ContinueCard.Click += ContinueCard_Click;
            ActionCard.Clickable = true;
            ActionCard.Click += ActionCard_Click;
            adapter = new Custom_Views.PreviewAdapter(this);
            adapter.ItemClick += Adapter_ItemClick;
            mProgressView.Visibility = ViewStates.Visible;
            grid_layout.SetAdapter(adapter);
            SetColumns();
            layout = new GridLayoutManager(this, PreviewColumns);
            grid_layout.SetLayoutManager(layout);
        }

        private void Adapter_ItemClick(object sender, Custom_Views.PreviewAdapterClickEventArgs e)
        {
            int pos = e.Position;
            List<int> pages_ids = new List<int>();

            if (pagelist == null)
                return;
            if (pagelist == null & pagelist.Count < 1)
                return;
            var page = adapter.mdata[pos];
            Intent intent = new Android.Content.Intent();
            if (page.isPlaceholder)
            {
                intent = new Intent(this, typeof(PreviewActivity));
            }
            else
            {

                intent = new Intent(this, typeof(GalleryViewer));
                intent.PutExtra("no", pos);
            }
            intent.PutExtra("page", Core.JSON.Serializer.SimpleSerializer.Serialize(pagelist));
            intent.PutExtra("gallery", Core.JSON.Serializer.SimpleSerializer.Serialize(gallery));
            StartActivity(intent);
        }

        void SetColumns()
        {
            var windo = GetSystemService(Context.WindowService);
            var window = windo.JavaCast<IWindowManager>();
            var display = window.DefaultDisplay;
            int gridFactor = 0;
            float w = display.Width;
            gridFactor = (int)(Math.Ceiling(w / (160 * 2)));
            if (Resources.Configuration.Orientation == Android.Content.Res.Orientation.Landscape)
            {
                gridFactor = (int)(Math.Ceiling(w / (160 * 3)));
                PreviewColumns = gridFactor * 2;
            }
            else
                PreviewColumns = gridFactor;
        }

        private void ErrorFrame_Click(object sender, EventArgs e)
        {
            ThreadStart start = new ThreadStart(() =>
            {
                try
                {
                    errorFrame.Visibility = ViewStates.Gone;
                    mProgressView.Visibility = ViewStates.Visible;
                    Load();
                }
                catch (Exception ex)
                {
                    loaded = false;
                }

                if (!loaded)
                {
                    RunOnUiThread(() =>
                    {
                        mErrorText.Text = "Error";
                        MainView.Visibility = ViewStates.Gone;
                        mProgressView.Visibility = ViewStates.Gone;
                        errorFrame.Visibility = ViewStates.Visible;
                    });
                }
            });
            Thread thread = new Thread(start);
            thread.Start();
        }

        private void ContinueCard_Click(object sender, EventArgs e)
        {

            if (pagelist == null)
                return;
            if (pagelist == null & pagelist.Count < 1)
                return;

            Intent intent = new Android.Content.Intent(this, typeof(GalleryViewer));
            intent.PutExtra("page", Core.JSON.Serializer.SimpleSerializer.Serialize(pagelist));
            intent.PutExtra("gallery", Core.JSON.Serializer.SimpleSerializer.Serialize(gallery));
            intent.PutExtra("no", gallery.LastPageRead);
            StartActivity(intent);
        }

        private void ActionCard_Click(object sender, EventArgs e)
        {
            List<int> pages_ids = new List<int>();

            if (pagelist == null)
                return;
            if (pagelist == null & pagelist.Count < 1)
                return;

            Intent intent = new Android.Content.Intent(this, typeof(GalleryViewer));
            intent.PutExtra("page", Core.JSON.Serializer.SimpleSerializer.Serialize(pagelist));
            intent.PutExtra("gallery", Core.JSON.Serializer.SimpleSerializer.Serialize(gallery));
            StartActivity(intent);

        }

        public void ParseMeta()
        {
            title.Text = gallery.titles[0].name;
            category.Text = "place_holder";

        }

        async void ParseData()
        {

            try
            {
                RunOnUiThread(() =>
                {
                    try
                    {
                        if (gallery.tags.Language.Count > 0)
                        {
                            if (language == null)
                                language = FindViewById<TextView>(Resource.Id.language);
                            string lan = gallery.tags.Language[0].name;
                            language.Text = System.Globalization.CultureInfo.CurrentCulture
                                .TextInfo.ToTitleCase(lan.ToLower());
                        }
                        else
                            language.Text = "eng";
                        pages.Text = pagelist.Count.ToString() + " Pages";
                        int number = 10;
                        if (pagelist.Count < 10)
                            number = pagelist.Count;
                        var mdata = new List<Core.Gallery.Page>();
                        for (int i = 0; i < number; i++)
                        {
                            mdata.Add(pagelist[i]);
                        }
                        if(pagelist.Count>number)
                        {
                            Core.Gallery.Page loadMore = new Core.Gallery.Page();
                            loadMore.isPlaceholder = true;
                            loadMore.name = "Show More...";
                            mdata.Add(loadMore);
                        }
                        adapter.SetList(mdata);
                        ParseTags();
                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnPause()
        {

            base.OnPause();
        }

        protected override void OnStop()
        {
            base.OnStop();
            Glide.With(this).Clear(ThumbView);
            if (pagelist != null)
            {
                cachedlist = new List<Core.Gallery.Page>(pagelist);
                pagelist.Clear();
            }
            adapter.NotifyDataSetChanged();
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
            foreach (var _namespace in namespaces)
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
                        rtg.Orientation = Android.Widget.Orientation.Horizontal;
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
                            string fullTagName = _namespace.Name + ":" + "\"" + tag.name + "\"";
                            tag_item.Tag = fullTagName;
                            tag_item.Clickable = true;
                            awl.AddView(tag_item);
                        }
                    }
                }
            }

        }

        private void Tag_item_Click(object sender, EventArgs e)
        {
            TextView tag_item = sender as TextView;
            if (tag_item == null)
                return;
            Android.Content.Intent intent = new Android.Content.Intent(this, typeof(LibraryActivity));

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
}