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
        public CancellationTokenSource SceneCancellationTokenSource = new CancellationTokenSource();
        public string thumb_path, gallerydata;
        public ImageView ThumbView;
        FrameLayout errorFrame;
        public bool isDownloading = false;
        Custom_Views.TagsAdapter tagsAdapter;
        public Core.Gallery.GalleryItem gallery;
        Emmaus.Widget.RecyclerViewPager previewpager;
        ProgressView.MaterialProgressBar mProgressView;
        LinearLayout MainLayout;
        AppBarLayout AppBarLayout;
        Android.Support.V7.Widget.Toolbar toolbar;
        TextView GalleryStatus;
        RecyclerView tagRecyclerView;
        Custom_Views.PreviewPagerAdapter adapter;
        bool loaded = false;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public bool IsRunning = true;
        Helpers.Layouts.ExtraLayoutManager layout;
        public List<Core.Gallery.Page> pagelist, cachedlist;
        private ImageView mErrorImage;
        View MainView;

        protected override View OnCreateView(LayoutInflater p0, ViewGroup p1)
        {
            MainView = p0.Inflate(Resource.Layout.Gallery_Details_Layout, p1, false);
            InitializeViews();
            gallery = Core.JSON.Serializer.SimpleSerializer.Deserialize<Core.Gallery.GalleryItem>(gallerydata);
            toolbar.Title = gallery.titles[0].name;
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

        public async void Load()
        {
            if (Core.Net.Connect())
            {
                Task.Run(() =>
                {
                    LoadPreviews();
                });

                if (thumb_path==string.Empty)
                thumb_path = Core.Gallery.GetThumb(gallery,SceneCancellationTokenSource.Token).Result;

                var h = new Handler(Looper.MainLooper);
                h.Post(() =>
                {
                    try
                    {
                        if (thumb_path.Contains("fail"))
                        {
                            GalleryStatus.Text = "Gallery Not Found";
                            GalleryStatus.Visibility = ViewStates.Visible;
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
                
                gallery.tags = await Core.Gallery.GetTags(Core.Gallery.ItemType.Gallery,gallery.id,SceneCancellationTokenSource.Token);                
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
            SceneCancellationTokenSource.Cancel();
            SceneCancellationTokenSource = null;
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
            errorFrame.Dispose();
            errorFrame = null;
            if (adapter.mdata != null)
            {
                adapter.mdata.Clear();
                adapter.NotifyDataSetChanged();
            }
            adapter = null;
            previewpager = null;
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
                    if (gallery.id == item.id)
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
            if(cachedlist!=null)
                if(cachedlist.Count>0)
                {
                    pagelist = new List<Core.Gallery.Page>(cachedlist);
                    var lists = SplitPageList();
                    adapter.SetList(lists);
                }
        }

        List<List<Core.Gallery.Page>> SplitPageList()
        {
            List<List<Core.Gallery.Page>> pages = new List<List<Core.Gallery.Page>>();
            List<Core.Gallery.Page> current = new List<Core.Gallery.Page>();
            if (pagelist.Count > 0) 
            for (int i = 0; i < pagelist.Count; i++)
            {
                    if (i % 10 == 0)
                        current = new List<Core.Gallery.Page>();
                    current.Add(pagelist[i]);
                    if (i % 10 == 9 || i == pagelist.Count - 1)
                        pages.Add(current);
            }

            return pages;
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

            toolbar = MainView.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            mErrorImage = MainView.FindViewById<ImageView>(Resource.Id.error_image);
            mErrorImage.SetImageResource(Resource.Drawable.big_weird_face);
            mErrorText = MainView.FindViewById<TextView>(Resource.Id.error_text);
            OnCreateOptionsMenu();
            tagRecyclerView = MainView.FindViewById<RecyclerView>(Resource.Id.tagLayout);
            var tlayout = new LinearLayoutManager(Context);
            tagRecyclerView.SetLayoutManager(tlayout);
            tagsAdapter = new Custom_Views.TagsAdapter(this);
            tagRecyclerView.SetAdapter(tagsAdapter);
            title = MainView.FindViewById<TextView>(Resource.Id.title);
           // category = MainView.FindViewById<TextView>(Resource.Id.category);
            read_action = MainView.FindViewById<TextView>(Resource.Id.read);
            language = MainView.FindViewById<TextView>(Resource.Id.language);
            pages = MainView.FindViewById<TextView>(Resource.Id.pages);
            time_posted = MainView.FindViewById<TextView>(Resource.Id.posted);
            last_read_page = MainView.FindViewById<TextView>(Resource.Id.lastReadPage);
            no_tags = MainView.FindViewById<TextView>(Resource.Id.no_tags);
            previewpager = MainView.FindViewById<Emmaus.Widget.RecyclerViewPager>(Resource.Id.previewpager);
            TagLayout = MainView.FindViewById<LinearLayout>(Resource.Id.tags);
            ActionCard = MainView.FindViewById<CardView>(Resource.Id.action_card);
            GalleryStatus = MainView.FindViewById<TextView>(Resource.Id.status);
            GalleryStatus.Visibility = ViewStates.Gone;
            ThumbView = MainView.FindViewById<ImageView>(Resource.Id.thumb);
            ContinueCard = MainView.FindViewById<CardView>(Resource.Id.continue_card);
            ContinueCard.Click += ContinueCard_Click;
            ActionCard.Clickable = true;
            ActionCard.Click += ActionCard_Click;
            adapter = new Custom_Views.PreviewPagerAdapter(Context,this);
            mProgressView.Visibility = ViewStates.Visible;
            previewpager.SetAdapter(new Emmaus.Widget.RecyclerViewPagerAdapter(previewpager,adapter));
            
            layout = new Helpers.Layouts.ExtraLayoutManager(this.Context, LinearLayoutManager.Horizontal, false);
            layout.SetExtraLayoutSpace(100);
            previewpager.SetLayoutManager(layout);
        }

        public void OnCreateOptionsMenu()
        {
            toolbar.InflateMenu(Resource.Menu.galleryTopMenu);

            var menuItem = toolbar.Menu.FindItem(Resource.Id.download);
            menuItem.SetOnMenuItemClickListener(new DownloadMenuItemClickListener(this));
        }

        class DownloadMenuItemClickListener : Java.Lang.Object,IMenuItemOnMenuItemClickListener
        {
            GalleryScene parent;
            List<Core.Gallery.Page> downloadList = new List<Core.Gallery.Page>();
            
            public DownloadMenuItemClickListener(GalleryScene scene)
            {
                parent = scene;
            }

            public bool OnMenuItemClick(IMenuItem item)
            {              
                ThreadStart threadStart = new ThreadStart(StartDownload);
                Thread thread = new Thread(threadStart);
                thread.Start();
                return true;
            }

            void StartDownload()
            {
                Thread.Sleep(100);
                parent.isDownloading = !parent.isDownloading;
                var h = new Handler(Looper.MainLooper);
                if(parent.pagelist!=null)
                Core.Gallery.QueueDownloads(parent.pagelist);
                h.Post(() =>
                {
                        Toast.MakeText(Android.App.Application.Context, "Precaching gallery Completed or was Cancelled", ToastLength.Short).Show();
                });
            }

            
        }

        private void ErrorFrame_Click(object sender, EventArgs e)
        {
            errorFrame.Visibility = ViewStates.Gone;
            mProgressView.Visibility = ViewStates.Visible;
            var h = new Handler(Looper.MainLooper);
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
            await Task.Delay(10);
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

        void LoadPreviews()
        {
            var h = new Handler(Looper.MainLooper);
            try
            {
                var mdata = new List<Core.Gallery.Page>();
               
                    pagelist = Core.App.Server.GetRelatedItems<Core.Gallery.Page>(gallery.id,SceneCancellationTokenSource.Token,
                        Core.Gallery.ItemType.Gallery,Core.Gallery.ItemType.Page);
                h.Post(() =>
                {
                    pages.Text = pagelist.Count.ToString() + " Pages";
                    
                    last_read_page.Text = "Last Read Page: " + (gallery.LastPageRead + 1).ToString();
                    if (pagelist != null)
                    {
                        var lists = SplitPageList();
                        adapter.SetList(lists);
                    }
                    
                });
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnDestroy()
        {
            isDownloading = false;
            base.OnDestroy();
        }

        protected override void OnStop()
        {
            base.OnStop();
            Glide.With(Context).Clear(ThumbView);
            SceneCancellationTokenSource.Cancel();
        }




        void ParseTags()
        {
            if (!IsTagAvailable())
                return;
            no_tags.Visibility = ViewStates.Gone;
            ((Custom_Views.TagsAdapter)tagRecyclerView.GetAdapter()).SetList(new List<Core.Gallery.TagNamespace>());

            {
                List<Core.Gallery.TagNamespace> taglists = new List<Core.Gallery.TagNamespace>();
                Type t = gallery.tags.GetType();
                PropertyInfo[] namespaces = t.GetProperties();
                foreach (var _namespace in namespaces)
                {
                    object value = _namespace.GetValue(gallery.tags);
                    string namespaceName = _namespace.Name;
                    if (namespaceName.Contains("__"))
                        namespaceName = "misc";
                    if (value != null)
                    {
                        var tags = (List<Core.Gallery.TagItem>)value;
                        if (tags.Count > 0)
                        {
                            var tagnamespace = new Core.Gallery.TagNamespace()
                            {
                                name = namespaceName,
                                tags = tags
                            };
                            taglists.Add(tagnamespace);
                            
                        }
                    }
                }

                if(taglists.Count>0)
                {
                    tagsAdapter.SetList(taglists);
                }

            }

            
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
            layout = new Helpers.Layouts.ExtraLayoutManager(this.Context, LinearLayoutManager.Horizontal, false);
            layout.SetExtraLayoutSpace(300);
            previewpager.SetLayoutManager(layout);
            previewpager.GetRecycledViewPool().Clear();
        }
    }
}
