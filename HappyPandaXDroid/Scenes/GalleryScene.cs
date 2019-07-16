using Android.App;
using Android.OS;
using Android.Content;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Views;
using Android.Support.Design.Chip;
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
using HappyPandaXDroid.Core;
using Com.Bumptech.Glide.Request;
using ProgressView = XamarinBindings.MaterialProgressBar;
using NLog;
using Android.Content.Res;
using Com.Hippo.Stage;

namespace HappyPandaXDroid.Scenes
{
    public class GalleryScene : HPXScene
    { 
        private TextView mErrorText;
        public TextView title, category, read_action,
            language, pages, time_posted, no_tags, last_read_page;
        public LinearLayout TagLayout;
        CardView ActionCard, ContinueCard;
        public CancellationTokenSource SceneCancellationTokenSource = new CancellationTokenSource();
        public ImageView ThumbView;
        FrameLayout errorFrame;
        public bool isDownloading = false;
        public DialogEventListener dialogeventlistener;
        CustomViews.TagsAdapter tagsAdapter;
        public Core.Gallery.GalleryItem gallery;
        Emmaus.Widget.RecyclerViewPager previewpager;
        ProgressView.MaterialProgressBar mProgressView;
        Chip categoryChip;
        CustomViews.DeleteDialog mDeleteDialog;
        LinearLayout MainLayout;
        LinearLayout ArtistLayout;
        LinearLayout GroupLayout;
        LinearLayout ParodyLayout;
        AppBarLayout AppBarLayout;
        Android.Support.V7.Widget.Toolbar toolbar;
        TextView GalleryStatus;
        RecyclerView tagRecyclerView;
        CustomViews.PreviewPagerAdapter adapter;
        bool loaded = false;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public bool IsRunning = true;
        Helpers.Layouts.ExtraLayoutManager layout;
        private ImageView mErrorImage;
        View MainView;

        protected override View OnCreateView(LayoutInflater p0, ViewGroup p1)
        {
            MainView = p0.Inflate(Resource.Layout.Gallery_Details_Layout, p1, false);
            InitializeViews();
            toolbar.Title = gallery.preferred_title.name;
            logger.Info("Initializing Gallery Detail. GalleryId ={0}", gallery.id);
            if (gallery.Thumb != null || string.IsNullOrWhiteSpace(gallery.Thumb.Uri))
            {
                Glide.With(Context).Load(gallery.Thumb.Uri).Into(ThumbView);
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
            if (!loaded && MainLayout != null)
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
            dialogeventlistener = new DialogEventListener(this);
            mDeleteDialog = new CustomViews.DeleteDialog(this);
            return MainView;
        }

        public GalleryScene(Core.Gallery.GalleryItem gallery)
        {
            if (Core.Media.Cache.TryGetCachedGallery(gallery.BaseId, out Core.Gallery.GalleryItem cachedGallery))
                this.gallery = cachedGallery;
            else
                this.gallery = gallery;
        }

        public async void Load()
        {
            if (RequestToken != null)
            {
                SceneCancellationTokenSource?.Cancel();

                SceneCancellationTokenSource = new CancellationTokenSource();

                RequestToken = new RequestToken(SceneCancellationTokenSource.Token);
            }
            else
                RequestToken = new RequestToken(SceneCancellationTokenSource.Token);

            if (Core.Net.Connect())
            {
                if (gallery.Image == null)
                    gallery.Image = new Media.Image();
                if (string.IsNullOrWhiteSpace(gallery.Image.Uri))
                    gallery.Image.Uri = Core.Gallery.GetThumb(gallery, SceneCancellationTokenSource.Token).Result;

                var h = new Handler(Looper.MainLooper);
                h.Post(() =>
                {
                    try
                    {
                        if (gallery.Image.Uri.Contains("fail"))
                        {
                            GalleryStatus.Text = "Gallery Not Found";
                            GalleryStatus.Visibility = ViewStates.Visible;
                            Glide.With(Context)
                            .Load(Resource.Drawable.image_failed)
                            .Into(ThumbView);
                            gallery.Image.Uri = string.Empty;
                        }
                        else
                            Glide.With(Context)
                                .Load(gallery.Image.Uri)
                                .Into(ThumbView);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "\n Exception Caught In GalleryActivity.Oncreate.");
                    }
                });

                if (SceneCancellationTokenSource.IsCancellationRequested)
                    return;

                h.Post(() =>
                {
                    pages.Text = gallery.ChildCount + $" Page{(gallery.ChildCount > 1 ? "s" : "")}";

                    if (gallery.artists?.Count > 0)
                    {
                        ArtistLayout.Visibility = ViewStates.Visible;
                        ArtistLayout.Orientation = Android.Widget.Orientation.Horizontal;
                        foreach (var artist in gallery.artists)
                        {
                            Chip chip = new Chip(Context);
                            //int color = Context.Resources.GetColor(Resource.Color.purple_a700);
                            chip.SetChipBackgroundColorResource(Resource.Color.loading_indicator_blue);
                            chip.Text = artist.preferred_name.name;
                            chip.Click += Chip_Click;
                            chip.Tag = "artist";

                            ArtistLayout.AddView(chip);
                        }
                    }

                    if (gallery.circles?.Count > 0)
                    {
                        GroupLayout.Visibility = ViewStates.Visible;
                        GroupLayout.Orientation = Android.Widget.Orientation.Horizontal;
                        foreach (var circle in gallery.circles)
                        {
                            Chip chip = new Chip(Context);
                            //int color = Context.Resources.GetColor(Resource.Color.purple_a700);
                            chip.SetChipBackgroundColorResource(Resource.Color.light_green_600);
                            chip.Text = circle.name;
                            chip.Click += Chip_Click;
                            chip.Tag = "group";

                            GroupLayout.AddView(chip);
                        }
                    }

                    if (gallery.parodies?.Count > 0)
                    {
                        ParodyLayout.Visibility = ViewStates.Visible;
                        ParodyLayout.Orientation = Android.Widget.Orientation.Horizontal;
                        foreach (var parody in gallery.parodies)
                        {
                            Chip chip = new Chip(Context);
                            //int color = Context.Resources.GetColor(Resource.Color.purple_a700);
                            chip.SetChipBackgroundColorResource(Resource.Color.purple_500);
                            chip.Text = parody.preferred_name.name;
                            chip.Click += Chip_Click;
                            chip.Tag = "parody";

                            ArtistLayout.AddView(chip);
                        }
                    }
                });


                RequestToken = new RequestToken(SceneCancellationTokenSource.Token);

                RequestToken.FinishedCallback += RequestToken_AsyncCallback;

                RequestToken.FailedCallback += RequestToken_FailedCallback;

                if (gallery.tags == null)
                    Core.Gallery.GetTags(Core.Gallery.ItemType.Gallery, gallery.id, RequestToken);
                else
                    LoadTags();

                Task.Run(() =>
                {
                    LoadPreviews();
                }, SceneCancellationTokenSource.Token);

                loaded = true;

                Media.Cache.CacheGallery(gallery);
            }
            else
            {
                loaded = false;
            }
        }

        private void Chip_Click(object sender, EventArgs e)
        {
            if(sender is Chip chip)
            {
                string query = chip.Tag + ":" + chip.Text;

                LibraryScene scene = new LibraryScene(query, query);
                Stage.PushScene(scene);
            }
        }

        private void RequestToken_FailedCallback(object sender, EventArgs e)
        {
            if (sender is RequestToken token && !token.CancellationToken.IsCancellationRequested && token == RequestToken)
            {
                if (SceneCancellationTokenSource.IsCancellationRequested)
                    return;
                gallery.tags = new Core.Gallery.TagList();
                var h = new Handler(Looper.MainLooper);
                if (!IsDestroyed)
                    h.Post(() =>
                    {
                        mProgressView.Visibility = ViewStates.Invisible;
                        //MainLayout.Visibility = ViewStates.Visible;
                        mErrorText.Text = "Failed to retrieve tags";
                        errorFrame.Visibility = ViewStates.Visible;
                    });
            }
        }

        private void RequestToken_AsyncCallback(object sender, EventArgs e)
        {
            if (sender is RequestToken token && !token.CancellationToken.IsCancellationRequested && token == RequestToken)
            {
                gallery.tags = (Core.Gallery.TagList)token.Result;

                LoadTags();
            }
        }

        void LoadTags()
        {
            ParseData();

            if (SceneCancellationTokenSource.IsCancellationRequested)
                return;

            var h = new Handler(Looper.MainLooper);
            if (!IsDestroyed)
                h.Post(() =>
                {
                    mProgressView.Visibility = ViewStates.Invisible;
                    MainLayout.Visibility = ViewStates.Visible;
                    errorFrame.Visibility = ViewStates.Gone;
                });
        }

        protected override void OnSaveViewState(View p0, Bundle p1)
        {
            var bundle = p1;
            bundle.PutString("gallery", Core.JSON.Serializer.SimpleSerializer.Serialize(gallery));
            base.OnSaveViewState(p0, p1);
        }
        

        protected override void OnRestoreViewState(View p0, Bundle p1)
        {
            base.OnRestoreViewState(p0, p1);
            var bundle = p1;
            gallery = Core.JSON.Serializer.SimpleSerializer.Deserialize
                <Core.Gallery.GalleryItem>(bundle.GetString("gallery"));
        }
        
        protected override void OnDestroyView(View p0)
        {

            SceneCancellationTokenSource.Cancel();
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
            categoryChip = null;
            base.OnDestroyView(p0);
        }

        protected override void OnResume()
        {
            base.OnResume(); 

            Task.Run(async () =>
            {

                var h = new Handler(Looper.MainLooper);
                await Task.Delay(3000);
                if (SceneCancellationTokenSource.IsCancellationRequested)
                    return;
                var item = Core.Media.Recents.GetRecentGallery(gallery.id);
                if (item != null)
                    if (gallery.id == item.id)
                    {
                        gallery.LastPageRead = item.LastPageRead;
                    }

                h.Post(() =>
                {
                    ContinueCard.Enabled = gallery.LastPageRead >= 1;

                    last_read_page.Text = "Last Read Page: " + (gallery.LastPageRead + 1).ToString();
                });
                
            });
        }


        protected override void OnStart()
        {
            base.OnStart();
            if (gallery.Thumb != null)
                if (gallery.Thumb.Uri != string.Empty)
                    Glide.With(Context).Load(gallery.Thumb.Uri).Into(ThumbView);         
        }

        List<List<Core.Gallery.Page>> SplitPageList()
        {
            List<List<Core.Gallery.Page>> pages = new List<List<Core.Gallery.Page>>();
            List<Core.Gallery.Page> current = new List<Core.Gallery.Page>();
            if (gallery.PageList != null)
                if (gallery.PageList.Count > 0)
                    for (int i = 0; i < gallery.PageList.Count; i++)
                    {
                        if (i % 10 == 0)
                            current = new List<Core.Gallery.Page>();
                        current.Add(gallery.PageList[i]);
                        if (i % 10 == 9 || i == gallery.PageList.Count - 1)
                            pages.Add(current);
                    }

            return pages;
        }

        void InitializeViews()
        {
            mProgressView = MainView.FindViewById<ProgressView.MaterialProgressBar>(Resource.Id.progress_view);
            mProgressView.Visibility = ViewStates.Visible;
            MainLayout = MainView.FindViewById<LinearLayout>(Resource.Id.below_header);
            GroupLayout = MainView.FindViewById<LinearLayout>(Resource.Id.groups);
            ArtistLayout = MainView.FindViewById<LinearLayout>(Resource.Id.artists);
            ParodyLayout = MainView.FindViewById<LinearLayout>(Resource.Id.parodies);
            errorFrame = MainView.FindViewById<FrameLayout>(Resource.Id.error_frame);

            GroupLayout.Visibility = ViewStates.Gone;
            ArtistLayout.Visibility = ViewStates.Gone;
            ParodyLayout.Visibility = ViewStates.Gone;
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
            tagsAdapter = new CustomViews.TagsAdapter(this);
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
            adapter = new CustomViews.PreviewPagerAdapter(Context,this);
            mProgressView.Visibility = ViewStates.Visible;
            previewpager.SetAdapter(new Emmaus.Widget.RecyclerViewPagerAdapter(previewpager,adapter));
            categoryChip = MainView.FindViewById<Chip>(Resource.Id.category);
            categoryChip.SetChipBackgroundColorResource(Resource.Color.colorPrimaryDark);
            layout = new Helpers.Layouts.ExtraLayoutManager(this.Context, LinearLayoutManager.Horizontal, false);
            layout.SetExtraLayoutSpace(100);
            previewpager.SetLayoutManager(layout);

            toolbar.Title = gallery.preferred_title.name;
        }

        public void OnCreateOptionsMenu()
        {
            toolbar.InflateMenu(Resource.Menu.galleryTopMenu);

            var menuItem = toolbar.Menu.FindItem(Resource.Id.delete);
            menuItem.SetOnMenuItemClickListener(new DeleteMenuItemClickListener(this));
        }

        class DeleteMenuItemClickListener : Java.Lang.Object, IMenuItemOnMenuItemClickListener
        {
            GalleryScene parent;

            public DeleteMenuItemClickListener(GalleryScene scene)
            {
                parent = scene;
            }

            public bool OnMenuItemClick(IMenuItem item)
            {
               parent.mDeleteDialog.Show(((HPXSceneActivity)parent.MainView.Context).SupportFragmentManager, "DeleteGallery");

                return true;
            }
        }

        public class DialogEventListener : CustomViews.INoticeDialogListener
        {
            GalleryScene parent;
            public DialogEventListener(GalleryScene parent)
            {
                this.parent = parent;
            }
            public void OnDialogNegativeClick(Android.Support.V4.App.DialogFragment dialog)
            {
                //close dialog
            }

            public async void OnDialogPositiveClick(Android.Support.V4.App.DialogFragment dialog)
            {
                bool succeed = false;
                if (dialog is CustomViews.DeleteDialog dd)
                {
                    if (dd.ShouldTrashed)
                    {
                        succeed = await App.Server.SendItemToTrash(parent.gallery, parent.SceneCancellationTokenSource.Token);

                        var h = new Handler(Looper.MainLooper);
                        h.Post(() =>
                        {
                            if (succeed)
                            {
                                Toast.MakeText(parent.Context, "Gallery sent to trash", ToastLength.Short).Show();
                            }
                            else
                            {
                                Toast.MakeText(parent.Context, "Gallery Deletion Failed", ToastLength.Short).Show();
                            }
                        });
                    }
                    else
                    {
                        succeed = await App.Server.DeleteItem(parent.gallery, parent.SceneCancellationTokenSource.Token);

                        var h = new Handler(Looper.MainLooper);
                        h.Post(() =>
                        {
                            if (succeed)
                            {
                                Toast.MakeText(parent.Context, "Gallery Deletion Succeeded", ToastLength.Short).Show();

                                if (parent.Stage.TopScene is GalleryScene scene)
                                {
                                    parent.Stage.PopTopScene();
                                }

                                Media.Cache.RemoveGallery(parent.gallery);
                            }
                            else
                            {
                                Toast.MakeText(parent.Context, "Gallery Deletion Failed", ToastLength.Short).Show();
                            }
                        });
                    }
                }
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

            if (gallery.PageList == null)
                return;
            if (gallery.PageList.Count == 0)
                return;

            Intent intent = new Android.Content.Intent(Context, typeof(GalleryViewer));

            IO.Parcel.PushParcel(gallery);

            intent.PutExtra("no", gallery.LastPageRead + 1);
            StartActivity(intent);
        }

        private void ActionCard_Click(object sender, EventArgs e)
        {
            List<int> pages_ids = new List<int>();

            if (gallery.PageList == null)
                return;
            if (gallery.PageList.Count == 0)
                return;

            Intent intent = new Android.Content.Intent(Context, typeof(GalleryViewer));
            IO.Parcel.PushParcel(gallery);
            StartActivity(intent);
        }

        public void ParseMeta()
        {
            title.Text = gallery.preferred_title.name;
            categoryChip.Text = Core.Gallery.Categories[gallery.category_id].name;
            language.Text = Core.Gallery.Languages[gallery.language_id].name;
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
                        if (gallery.tags != null)
                            if (gallery.tags.Language != null)
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
            if(gallery.PageList == null || gallery.PageList.Count == 0)
            try
            {
                var mdata = new List<Core.Gallery.Page>();

                    int count = App.Server.GetRelatedCount(gallery.id, SceneCancellationTokenSource.Token, Core.Gallery.ItemType.Gallery, Core.Gallery.ItemType.Page);

                    if (count > 0)
                    {
                        gallery.PageList = Core.App.Server.GetRelatedItems<Core.Gallery.Page>(gallery.id, SceneCancellationTokenSource.Token,
                            Core.Gallery.ItemType.Gallery, Core.Gallery.ItemType.Page, count);
                        h.Post(() =>
                        {
                            if (SceneCancellationTokenSource.IsCancellationRequested)
                                return;

                            SetPreviews();
                        });
                    }
            }
            catch (Exception ex)
            {

            }
            else
            {
                h.Post(() =>
                {
                    if (SceneCancellationTokenSource.IsCancellationRequested)
                        return;

                    SetPreviews();
                });
            }
            
        }

        void SetPreviews()
        {
            
            last_read_page.Text = "Last Read Page: " + (gallery.LastPageRead + 1).ToString();
            if (gallery.PageList != null)
            {
                var lists = SplitPageList();
                adapter.SetList(lists);
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
        }

        protected override void OnPause()
        { 
            base.OnPause();
        }
        
        void ParseTags()
        {
            no_tags.Visibility = ViewStates.Gone;
            bool tagsAvailable = IsTagAvailable();

            ((CustomViews.TagsAdapter)tagRecyclerView.GetAdapter()).SetList(new List<Core.Gallery.TagNamespace>());

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

                if (!tagsAvailable)
                {
                    taglists = new List<Core.Gallery.TagNamespace>();
                    var tag = new Core.Gallery.TagNamespace()
                    {
                        name = "tag",

                        tags = new List<Core.Gallery.TagItem>(){ new Core.Gallery.TagItem() { name = "none" } }
                    };

                    taglists.Add(tag);
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
            if (taglist == null)
                return false;
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
