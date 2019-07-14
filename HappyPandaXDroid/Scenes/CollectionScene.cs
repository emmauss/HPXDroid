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
using Android.Util;
using RefreshLayout = Com.Hippo.Refreshlayout;
using FloatingSearchViews;
using FloatingSearchViews.Utils;

namespace HappyPandaXDroid.Scenes
{
    class CollectionScene : HPXScene
    {
        private TextView mErrorText;
        public TextView title, galleries;
        public CancellationTokenSource SceneCancellationTokenSource = new CancellationTokenSource();
        FrameLayout errorFrame;
        FloatingActionButton pageButton;
        Emmaus.Widget.RecyclerViewPager galleryPager;
        ProgressView.MaterialProgressBar mProgressView;
        Chip categoryChip;
        LinearLayout MainLayout;
        CustomViews.CollectionPagerAdapter adapter;
        bool loaded = false;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public bool IsRunning = true;
        Helpers.Layouts.ExtraGridLayoutManager layout;
        ImageView thumb;
        Core.Gallery.Collection Collection;
        //string url;
        int count;
        View MainView;
        int columns = 1;
        List<Core.Gallery.HPXItem> Galleries { get; set; }

        public CollectionScene(Core.Gallery.Collection collection,string url)
        {
            Collection = collection;
        }

        protected override View OnCreateView(LayoutInflater p0, ViewGroup p1)
        {
            MainView = p0.Inflate(Resource.Layout.CollectionLayout, p1, false);
            Initialize();

            return MainView;
        }

        protected void Initialize()
        {
            InitializeViews();
            
            RequestToken = new RequestToken(SceneCancellationTokenSource.Token);
            //initialize header
            
            title.Text = Collection.name;
            Glide.With(Context).Load(Collection.Thumb.Uri).Into(thumb);
            categoryChip.SetChipBackgroundColorResource(Resource.Color.colorPrimaryDark);
            categoryChip.Text = Core.Gallery.Categories[Collection.category_id].name;
            Task.Run(() =>Refresh());
        }

        void InitializeViews()
        {
            title = MainView.FindViewById<TextView>(Resource.Id.title);
            galleries = MainView.FindViewById<TextView>(Resource.Id.galleries);
            thumb = MainView.FindViewById<ImageView>(Resource.Id.thumb);
            categoryChip = MainView.FindViewById<Chip>(Resource.Id.category);
            mProgressView = MainView.FindViewById<ProgressView.MaterialProgressBar>(Resource.Id.progress_view);
            mProgressView.Visibility = ViewStates.Visible;
            pageButton = MainView.FindViewById<FloatingActionButton>(Resource.Id.page_button);
            pageButton.Click += PageButton_Click;
            pageButton.Enabled = false;
            MainLayout = MainView.FindViewById<LinearLayout>(Resource.Id.main_Layout);
            errorFrame = MainView.FindViewById<FrameLayout>(Resource.Id.error_frame);
            errorFrame.Visibility = ViewStates.Gone;
            errorFrame.Click += ErrorFrame_Click;
            MainLayout.Visibility = ViewStates.Gone;
            galleryPager = MainView.FindViewById<Emmaus.Widget.RecyclerViewPager>(Resource.Id.gallerypager);
            mErrorText = MainView.FindViewById<TextView>(Resource.Id.error_text);
            adapter = new CustomViews.CollectionPagerAdapter(Context, this);
            mProgressView.Visibility = ViewStates.Visible;
            galleryPager.SetAdapter(new Emmaus.Widget.RecyclerViewPagerAdapter(galleryPager, adapter));
            categoryChip = MainView.FindViewById<Chip>(Resource.Id.category);
            categoryChip.SetChipBackgroundColorResource(Resource.Color.colorPrimaryDark);
            SetColumns();
            layout = new Helpers.Layouts.ExtraGridLayoutManager(this.Context, columns, LinearLayoutManager.Vertical, false);
            layout.SetExtraLayoutSpace(400);
            galleryPager.SetLayoutManager(layout);
            //galleryPager.Visibility = ViewStates.Gone;
        }

        private void PageButton_Click(object sender, EventArgs e)
        {
            if (count > 50)
            {
                CollectionGalleriesScene scene = new CollectionGalleriesScene(Collection.name, Collection);
                Stage.PushScene(scene);
            }
        }

        void SetColumns()
        {
            var windo = Context.GetSystemService(Context.WindowService);
            var window = windo.JavaCast<IWindowManager>();
            var display = window.DefaultDisplay;
            var rotation = display.Rotation;
            switch (rotation)
            {
                case SurfaceOrientation.Rotation0:
                case SurfaceOrientation.Rotation270:
                    columns = 1;
                    break;
                default:
                    columns = 2;
                    break;

            }
        }

        private void ErrorFrame_Click(object sender, EventArgs e)
        {
            errorFrame.Visibility = ViewStates.Gone;
            MainLayout.Visibility = ViewStates.Gone;
            mProgressView.Visibility = ViewStates.Visible;
            Task.Run(() => Refresh());
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
                    h.Post(() =>
                    {
                        galleries.Text = count.ToString() + $" galler{(count > 1 ? "ies" : "y")}";

                        if (count > 50)
                            pageButton.Enabled = true;
                    });
            }catch(Exception ex)
            {

            }
        }

        public async void Refresh()
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

            if (count < 1)
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
                                Core.Gallery.ItemType.Collection, Core.Gallery.ItemType.Gallery, Core.App.Settings.Limit, 0);
                            if (SceneCancellationTokenSource.IsCancellationRequested)
                                return;
                            foreach (var item in list)
                            {
                                item.Thumb = new Media.Image();
                            }
                            Galleries?.AddRange(list);
                            if (Galleries == null || Galleries.Count < 1)
                            {
                                h.Post(() =>
                                {
                                    mProgressView.Visibility = ViewStates.Gone;
                                    errorFrame.Visibility = ViewStates.Visible;
                                });
                                return;
                            }
                            else
                            {
                                h.Post(() =>
                                {
                                    MainLayout.Visibility = ViewStates.Visible;
                                    galleryPager.Visibility = ViewStates.Visible;
                                    if (Galleries.Count >= count)
                                    {
                                        pageButton.Enabled = false;
                                    }
                                });
                            }

                            var pages = new List<List<Core.Gallery.HPXItem>>();

                            var currentList = new List<Core.Gallery.HPXItem>();

                            foreach(var gallery in Galleries)
                            {
                                currentList.Add(gallery);
                            }

                            pages.Add(currentList);

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
            SetColumns();
            layout = new Helpers.Layouts.ExtraGridLayoutManager(this.Context, columns,LinearLayoutManager.Vertical, false);
            layout.SetExtraLayoutSpace(400);
            galleryPager.SetLayoutManager(layout);
            galleryPager.GetRecycledViewPool().Clear();
        }
    }
}