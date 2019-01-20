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
using V7 = Android.Support.V7.Widget;
using Com.Bumptech.Glide;
using HappyPandaXDroid.Core;
using Android.Content.Res;

namespace HappyPandaXDroid.Scenes
{
    class CollectionScene : HPXScene
    {
        private TextView mErrorText;
        public TextView title, galleries;
        public CancellationTokenSource SceneCancellationTokenSource = new CancellationTokenSource();
        FrameLayout errorFrame;
        Emmaus.Widget.RecyclerViewPager galleryPager;
        ProgressView.MaterialProgressBar mProgressView;
        Chip categoryChip;
        LinearLayout MainLayout;
        AppBarLayout AppBarLayout;
        Custom_Views.CollectionPagerAdapter adapter;
        bool loaded = false;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public bool IsRunning = true;
        Helpers.Layouts.ExtraLayoutManager layout;
        V7.Toolbar toolbar;
        ImageView thumb;
        Core.Gallery.Collection Collection;
        string url;
        string collectionTitle;
        int count;
        View MainView;

        List<Core.Gallery.HPXItem> Galleries { get; set; }

        public CollectionScene(Core.Gallery.Collection collection,string url)
        {
            Collection = collection;
            collectionTitle = collection.name;
            this.url = url;
        }

        protected override View OnCreateView(LayoutInflater p0, ViewGroup p1)
        {
            MainView = p0.Inflate(Resource.Layout.CollectionScene, p1, false);
            Initialize();

            return MainView;
        }

        protected void Initialize()
        {
            InitializeViews();
            
            RequestToken = new RequestToken(SceneCancellationTokenSource.Token);
            toolbar.Title = Collection.name;
            //initialize header
            
            title.Text = Collection.name;
            Glide.With(Context).Load(Collection.Thumb.Uri).Into(thumb);
            categoryChip.SetChipBackgroundColorResource(Resource.Color.colorPrimaryDark);
            categoryChip.Text = Core.Gallery.Categories[Collection.category_id].name;
            Task.Run(() =>Refresh(0));
        }

        void InitializeViews()
        {
            title = MainView.FindViewById<TextView>(Resource.Id.title);
            galleries = MainView.FindViewById<TextView>(Resource.Id.galleries);
            thumb = MainView.FindViewById<ImageView>(Resource.Id.thumb);
            toolbar = MainView.FindViewById<V7.Toolbar>(Resource.Id.toolbar);
            categoryChip = MainView.FindViewById<Chip>(Resource.Id.category);
            mProgressView = MainView.FindViewById<ProgressView.MaterialProgressBar>(Resource.Id.progress_view);
            mProgressView.Visibility = ViewStates.Visible;
            MainLayout = MainView.FindViewById<LinearLayout>(Resource.Id.below_header);
            errorFrame = MainView.FindViewById<FrameLayout>(Resource.Id.error_frame);
            errorFrame.Visibility = ViewStates.Gone;
            MainLayout.Visibility = ViewStates.Gone;
            galleryPager = MainView.FindViewById<Emmaus.Widget.RecyclerViewPager>(Resource.Id.gallerypager);
            toolbar = MainView.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            mErrorText = MainView.FindViewById<TextView>(Resource.Id.error_text);
            adapter = new Custom_Views.CollectionPagerAdapter(Context, this);
            mProgressView.Visibility = ViewStates.Visible;
            galleryPager.SetAdapter(new Emmaus.Widget.RecyclerViewPagerAdapter(galleryPager, adapter));
            categoryChip = MainView.FindViewById<Chip>(Resource.Id.category);
            categoryChip.SetChipBackgroundColorResource(Resource.Color.colorPrimaryDark);
            layout = new Helpers.Layouts.ExtraLayoutManager(this.Context, LinearLayoutManager.Horizontal, false);
            layout.SetExtraLayoutSpace(100);
            galleryPager.SetLayoutManager(layout);
        }

        public void GetTotalCount()
        {
            if (!SceneCancellationTokenSource.IsCancellationRequested)
                    count = Core.App.Server.GetRelatedCount(Collection.id, SceneCancellationTokenSource.Token, Core.Gallery.ItemType.Collection
                    , Core.Gallery.ItemType.Gallery);
                var h = new Handler(Looper.MainLooper);
            try
            {
                if (!SceneCancellationTokenSource.IsCancellationRequested)
                    h.Post(() => galleries.Text = count.ToString());
            }catch(Exception ex)
            {

            }
        }

        public async void Refresh(int page)
        {
            Galleries = new List<Core.Gallery.HPXItem>();

            if (Collection.Thumb == null)
                Collection.Thumb = new Media.Image();
            if (string.IsNullOrWhiteSpace(Collection.Thumb.Uri))
                Collection.Thumb.Uri = Core.Gallery.GetThumb(Collection, SceneCancellationTokenSource.Token).Result;

            var h = new Handler(Looper.MainLooper);
            h.Post(() =>
            {
                try
                {
                    if (Collection.Thumb.Uri.Contains("fail"))
                    {
                        Glide.With(Context)
                        .Load(Resource.Drawable.image_failed)
                        .Into(thumb);
                        Collection.Thumb.Uri = string.Empty;
                    }
                    else
                        Glide.With(Context)
                            .Load(Collection.Thumb.Uri)
                            .Into(thumb);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In GalleryActivity.Oncreate.");
                }
            });

            if (SceneCancellationTokenSource.IsCancellationRequested)
                return;

            GetTotalCount();

            if (Core.Net.Connect())
            {
                h.Post(() =>
                {
                    mProgressView.Visibility = ViewStates.Visible;
                });
                await Task.Run(async () =>
                {
                    logger.Info("Refreshing HPContent");
                    if (!SceneCancellationTokenSource.IsCancellationRequested)
                    {
                        try
                        {
                            var list = App.Server.GetRelatedItems<Core.Gallery.GalleryItem>(Collection.id, SceneCancellationTokenSource.Token,
                                Core.Gallery.ItemType.Collection, Core.Gallery.ItemType.Gallery, count, 0);
                            if (SceneCancellationTokenSource.IsCancellationRequested)
                                return;
                            foreach (var item in list)
                            {
                                item.Thumb = new Media.Image();
                            }
                            Galleries.AddRange(list);
                            if (Galleries == null || Galleries.Count < 1)
                            {
                                h.Post(() =>
                                {
                                    mProgressView.Visibility = ViewStates.Gone;
                                    errorFrame.Visibility = ViewStates.Visible;
                                });
                                return;
                            }

                            var pages = new List<List<Core.Gallery.HPXItem>>();

                            int i = 0;

                            var currentList = new List<Core.Gallery.HPXItem>();

                            while (i < Galleries.Count)
                            {
                                if (i % 20 == 0)
                                {
                                    currentList = new List<Core.Gallery.HPXItem>();

                                    pages.Add(currentList);
                                }

                                currentList.Add(Galleries[i]);

                                i++;
                            }

                            h.Post(() =>
                            {
                                adapter.SetList(pages);
                                adapter.NotifyDataSetChanged();
                                mProgressView.Visibility = ViewStates.Gone;
                            });
                            logger.Info("HPContent Refresh Successful");

                        }catch(Exception ex)
                        {

                        }
                    }
                },SceneCancellationTokenSource.Token);
            }
            else
            {
                h.Post(() =>
                {
                    mProgressView.Visibility = ViewStates.Gone;
                    errorFrame.Visibility = ViewStates.Visible;
                });
            }
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            layout = new Helpers.Layouts.ExtraLayoutManager(this.Context, LinearLayoutManager.Horizontal, false);
            layout.SetExtraLayoutSpace(300);
            galleryPager.SetLayoutManager(layout);
            galleryPager.GetRecycledViewPool().Clear();
        }
    }
}