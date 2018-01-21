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
using Com.Hippo.Stage;

namespace HappyPandaXDroid.Scenes
{
    class GalleryScene : HPXScene
    {
        private TextView mErrorText;
        public TextView title, category, read_action,
            language, pages, time_posted, no_tags, last_read_page;
        public LinearLayout TagLayout;
        CardView ActionCard, ContinueCard;
        public string thumb_path, gallerydata;
        public ImageView ThumbView;
        FrameLayout errorFrame;
        Core.Gallery.GalleryItem gallery;
        RecyclerView grid_layout;
        ProgressView.MaterialProgressBar mProgressView;
        LinearLayout MainLayout;
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
        View MainView;

        protected override View OnCreateView(LayoutInflater p0, ViewGroup p1)
        {
            MainView = p0.Inflate(Resource.Layout.Gallery_Details_Layout, p1, false);
            InitializeViews();
            gallery = Core.JSON.Serializer.SimpleSerializer.Deserialize<Core.Gallery.GalleryItem>(gallerydata);
            logger.Info("Initializing Gallery Detail. GalleryId ={0}", gallery.id);
            if (thumb_path != string.Empty)
            {
                Glide.With(Context).Load(thumb_path).Into(ThumbView); 
            }
            ParseMeta();
            
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
                    var h = new Handler(Looper.MainLooper);
                    h.Post(() =>
                    {
                        mErrorText.Text = "Error";
                        MainLayout.Visibility = ViewStates.Gone;
                        mProgressView.Visibility = ViewStates.Gone;
                        errorFrame.Visibility = ViewStates.Visible;
                    });
                }
            });
            Thread thread = new Thread(start);
            thread.Start();
            return MainView;
        }

        public GalleryScene(string gallery,string thumb)
        {
            gallerydata = gallery;
            thumb_path = thumb;
        }

        public void Load()
        {
            if (Core.Net.Connect())
            {
                thumb_path = string.Empty;
                thumb_path = Core.Gallery.GetThumb(gallery).Result;

                var h = new Handler(Looper.MainLooper);
                h.Post(() =>
                {
                    try
                    {
                        if (thumb_path.Contains("fail"))
                        {
                            GalleryStatus.Text = "Gallery Not Found";
                            Glide.With(Context)
                            .Load(Resource.Drawable.image_failed)
                            .Into(ThumbView);
                        }
                        else
                            Glide.With(Context)
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
                    h.Post(() =>
                    {
                        mProgressView.Visibility = ViewStates.Invisible;
                        MainLayout.Visibility = ViewStates.Visible;
                        errorFrame.Visibility = ViewStates.Gone;
                    });
                loaded = true;
            }
            else
            {
                loaded = false;
            }
        }

        protected override void OnSaveViewState(View p0, Bundle p1)
        {
            var bundle = p1;
            bundle.PutString("gallery", Core.JSON.Serializer.SimpleSerializer.Serialize(gallery));
            bundle.PutString("thumb", thumb_path);
            base.OnSaveViewState(p0, p1);
        }
        

        protected override void OnRestoreViewState(View p0, Bundle p1)
        {
            base.OnRestoreViewState(p0, p1);
            var bundle = p1;
            gallery = Core.JSON.Serializer.SimpleSerializer.Deserialize
                <Core.Gallery.GalleryItem>(bundle.GetString("gallery"));
            thumb_path = bundle.GetString("thumb");
        }
        
        protected override void OnDestroyView(View p0)
        {
            IsRunning = false;
            mProgressView = null;
            MainLayout = null;
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
            base.OnDestroyView(p0);
        }

        protected override void OnResume()
        {
            base.OnResume();
          
            Task.Run(async () =>
            {

                var h = new Handler(Looper.MainLooper);
                await Task.Delay(3000);
                var item = Core.Media.Recents.GetRecentGallery(gallery.id);
                if (item != null)
                    if (gallery.titles[0].name == item.titles[0].name)
                    {
                        gallery.LastPageRead = item.LastPageRead;
                    }

                h.Post(() =>
                {
                    if (gallery.LastPageRead < 1)
                        ContinueCard.Enabled = false;
                    else
                        ContinueCard.Enabled = true;
                });
            });
        }


        protected override void OnStart()
        {
            base.OnStart();
            if(thumb_path!=string.Empty)
            Glide.With(Context).Load(thumb_path).Into(ThumbView);
        }

        void InitializeViews()
        {

            mProgressView = MainView.FindViewById<ProgressView.MaterialProgressBar>(Resource.Id.progress_view);
            mProgressView.Visibility = ViewStates.Visible;
            MainLayout = MainView.FindViewById<LinearLayout>(Resource.Id.below_header);
            errorFrame = MainView.FindViewById<FrameLayout>(Resource.Id.error_frame);
            errorFrame.Visibility = ViewStates.Gone;
            errorFrame.Click += ErrorFrame_Click;
            MainLayout.Visibility = ViewStates.Gone;

            mErrorImage = MainView.FindViewById<ImageView>(Resource.Id.error_image);
            mErrorImage.SetImageResource(Resource.Drawable.big_weird_face);
            mErrorText = MainView.FindViewById<TextView>(Resource.Id.error_text);


            title = MainView.FindViewById<TextView>(Resource.Id.title);
           // category = MainView.FindViewById<TextView>(Resource.Id.category);
            read_action = MainView.FindViewById<TextView>(Resource.Id.read);
            language = MainView.FindViewById<TextView>(Resource.Id.language);
            pages = MainView.FindViewById<TextView>(Resource.Id.pages);
            time_posted = MainView.FindViewById<TextView>(Resource.Id.posted);
            last_read_page = MainView.FindViewById<TextView>(Resource.Id.lastReadPage);
            no_tags = MainView.FindViewById<TextView>(Resource.Id.no_tags);
            scrollview = MainView.FindViewById<ScrollView>(Resource.Id.scroll_view);
            grid_layout = MainView.FindViewById<RecyclerView>(Resource.Id.grid_layout);
            TagLayout = MainView.FindViewById<LinearLayout>(Resource.Id.tags);
            ActionCard = MainView.FindViewById<CardView>(Resource.Id.action_card);
            GalleryStatus = MainView.FindViewById<TextView>(Resource.Id.status);
            ThumbView = MainView.FindViewById<ImageView>(Resource.Id.thumb);
            ContinueCard = MainView.FindViewById<CardView>(Resource.Id.continue_card);
            ContinueCard.Click += ContinueCard_Click;
            ActionCard.Clickable = true;
            ActionCard.Click += ActionCard_Click;
            adapter = new Custom_Views.PreviewAdapter(Context,this);
            adapter.ItemClick += Adapter_ItemClick;
            mProgressView.Visibility = ViewStates.Visible;
            grid_layout.SetAdapter(adapter);
            SetColumns();
            layout = new GridLayoutManager(Context, PreviewColumns);
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
            
            if (page.isPlaceholder)
            {
                PreviewScene previewScene = new PreviewScene(pagelist, gallery);
                Stage.PushScene(previewScene);
            }
            else
            {

                Intent intent = new Intent(Context, typeof(GalleryViewer));
                intent.PutExtra("no", pos);
                intent.PutExtra("page", Core.JSON.Serializer.SimpleSerializer.Serialize(pagelist));
                intent.PutExtra("gallery", Core.JSON.Serializer.SimpleSerializer.Serialize(gallery));
                StartActivity(intent);
            }
            
        }

        void SetColumns()
        {
            var windo = Context.GetSystemService(Context.WindowService);
            var window = windo.JavaCast<IWindowManager>();
            var display = window.DefaultDisplay;
            int gridFactor = 0;
            float w = display.Width;
            gridFactor = (int)(Math.Ceiling(w / (160 * 2)));
            if (Context.Resources.Configuration.Orientation == Android.Content.Res.Orientation.Landscape)
            {
                gridFactor = (int)(Math.Ceiling(w / (160 * 3)));
                PreviewColumns = gridFactor * 2;
            }
            else
                PreviewColumns = gridFactor;
        }

        private void ErrorFrame_Click(object sender, EventArgs e)
        {

            var h = new Handler(Looper.MainLooper);
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
                    h.Post(() =>
                    {
                        mErrorText.Text = "Error";
                        MainLayout.Visibility = ViewStates.Gone;
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

            Intent intent = new Android.Content.Intent(Context, typeof(GalleryViewer));
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

            Intent intent = new Android.Content.Intent(Context, typeof(GalleryViewer));
            intent.PutExtra("page", Core.JSON.Serializer.SimpleSerializer.Serialize(pagelist));
            intent.PutExtra("gallery", Core.JSON.Serializer.SimpleSerializer.Serialize(gallery));
            StartActivity(intent);

        }

        public void ParseMeta()
        {
            title.Text = gallery.titles[0].name;

            //category.Text = "place_holder";

        }

        async void ParseData()
        {

            var h = new Handler(Looper.MainLooper);
            try
            {
                h.Post(() =>
                {
                    try
                    {
                        if (language == null)
                            language = MainView.FindViewById<TextView>(Resource.Id.language);
                        language.Text = "Language: ";
                        if (gallery.tags.Language.Count > 0)
                        {
                            
                            string lan = gallery.tags.Language[0].name;
                            language.Text += System.Globalization.CultureInfo.CurrentCulture
                                .TextInfo.ToTitleCase(lan.ToLower());
                        }
                        else
                            language.Text += "English";
                        pages.Text = pagelist.Count.ToString() + " Pages";
                        int number = 10;
                        if (pagelist.Count < 10)
                            number = pagelist.Count;
                        var mdata = new List<Core.Gallery.Page>();
                        last_read_page.Text = "Last Read Page: "+(gallery.LastPageRead + 1).ToString();
                        for (int i = 0; i < number; i++)
                        {
                            mdata.Add(pagelist[i]);
                        }
                        if (pagelist.Count > number)
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

        protected override void OnStop()
        {
            base.OnStop();
            Glide.With(Context).Clear(ThumbView);
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

            var inflater =(LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
            //reclass


            int color_header = Context.Resources.GetColor(Resource.Color.colorPrimary);
            int color_tag = Context.Resources.GetColor(Resource.Color.purple_a700);
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
                        Custom_Views.AutoWrapLayout awl = new Custom_Views.AutoWrapLayout(this.Context);
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
            Stage.PushScene(new LibraryScene((string)tag_item.Tag, (string)tag_item.Tag));

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
            if (taglist.Group != null)
                count += taglist.Group.Count;
            if (taglist.Character != null)
                count += taglist.Character.Count;

            if (count == 0)
                return false;
            else
                return true;
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            SetColumns();
            layout = new GridLayoutManager(this.Context, PreviewColumns);
            grid_layout.SetLayoutManager(layout);
        }
    }
}
