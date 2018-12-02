using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using HappyPandaXDroid.Core;

namespace HappyPandaXDroid.Scenes
{
    class CollectionScene : ViewScene
    {
        TextView titleView, galleries;
        ImageView thumb;
        Core.Gallery.Collection Collection;
        string url;
        public CollectionScene(Core.Gallery.Collection collection,string url) : base(collection.name, null)
        {
            Collection = collection;
            title = collection.name;
            this.url = url;
        }

        protected override View OnCreateView(LayoutInflater p0, ViewGroup p1)
        {
            MainView = p0.Inflate(Resource.Layout.CollectionSceneLayout, p1, false);
            Initialize();

            return MainView;
        }

        protected override void Initialize()
        {
            base.Initialize();
            RequestToken = new RequestToken(SceneCancellationTokenSource.Token);
            //toolbar.Title = Collection.name;
            //initialize header
            titleView = MainView.FindViewById<TextView>(Resource.Id.title);
            // category = MainView.FindViewById<TextView>(Resource.Id.category);
            galleries = MainView.FindViewById<TextView>(Resource.Id.pages);
            thumb = MainView.FindViewById<ImageView>(Resource.Id.thumb);
            titleView.Text = Collection.name;
            Glide.With(Context).Load(url).Into(thumb);
            current_query = string.Empty;
            Refresh(0);
        }

        public readonly new Core.Gallery.ItemType ItemType = Core.Gallery.ItemType.Gallery;

        public override Core.Gallery.ViewType ViewType => Core.Gallery.ViewType.Library;

        public override async void GetTotalCount()
        {
            await Task.Run(() =>
            {
                if (!SceneCancellationTokenSource.IsCancellationRequested)
                    Count = Core.App.Server.GetRelatedCount(Collection.id, SceneCancellationTokenSource.Token, Core.Gallery.ItemType.Collection
                    , Core.Gallery.ItemType.Gallery);
                var h = new Handler(Looper.MainLooper);
                if (!SceneCancellationTokenSource.IsCancellationRequested)
                    h.Post(() => galleries.Text = Count.ToString());
            }, SceneCancellationTokenSource.Token);
        }

        public override async void Refresh(int page)
        {
            CurrentList = new List<Core.Gallery.HPXItem>();
            CurrentPage = page;

            var h = new Handler(Looper.MainLooper);
            if (Core.Net.Connect())
            {
                h.Post(() =>
                {
                    SetMainLoading(true);
                });
                await Task.Run(async () =>
                {
                    logger.Info("Refreshing HPContent");
                    if (!SceneCancellationTokenSource.IsCancellationRequested)
                    {
                        var list = App.Server.GetRelatedItems<Core.Gallery.GalleryItem>(Collection.id, SceneCancellationTokenSource.Token,
                            Core.Gallery.ItemType.Collection, Core.Gallery.ItemType.Gallery, 50, 0);
                        if (SceneCancellationTokenSource.IsCancellationRequested)
                            return;
                            foreach (var item in list)
                        {
                            item.Thumb = new Media.Image();
                        }
                        CurrentList.AddRange(list);
                        if (CurrentList == null || CurrentList.Count < 1)
                        {
                            h.Post(() =>
                            {
                                SetMainLoading(false);
                                SetError(true);
                            });
                            return;
                        }
                        CurrentPage = 0;
                        h.Post(() =>
                        {
                            adapter.ResetList();
                            adapter.NotifyDataSetChanged();
                            SetMainLoading(false);
                            if (CurrentList.Count > 0)
                                mRecyclerView.ScrollToPosition(0);
                        });
                        GetTotalCount();
                        mpageSelector = new Custom_Views.PageSelector(this);
                        logger.Info("HPContent Refresh Successful");
                    }
                },SceneCancellationTokenSource.Token);
            }
            else
            {
                h.Post(() =>
                {
                    SetError(true);
                });
            }
        }

        public override async void NextPage()
        {
            isLoading = true;
            logger.Info("Loading Next Page");
            var h = new Handler(Looper.MainLooper);
            if ((CurrentPage + 1) >= (Count / 25))
            {
                h.Post(() =>
                {
                    Toast to = Toast.MakeText(this.Context, "Reached end of library", ToastLength.Short);
                    to.SetGravity(GravityFlags.Bottom, 0, 10);

                    to.Show();
                    SetBottomLoading(false);
                    mRefreshLayout.HeaderRefreshing = false;
                    mRefreshLayout.FooterRefreshing = false;
                    isLoading = false;
                });
                return;
            }
            int lastin = CurrentList.Count - 1;
            var list = Core.App.Server.GetRelatedItems<Core.Gallery.GalleryItem>(Collection.id, SceneCancellationTokenSource.Token,
                        Core.Gallery.ItemType.Collection, Core.Gallery.ItemType.Gallery, 50, CurrentPage+1);
            adapter.Add(new List<Core.Gallery.HPXItem>(list));
            if (CurrentList.Count > 0)
            {
                h.Post(() =>
                {
                    adapter.NotifyItemRangeInserted(lastin + 1, CurrentList.Count - (lastin + 1));

                    mRefreshLayout.HeaderRefreshing = false;
                    mRefreshLayout.FooterRefreshing = false;
                    isLoading = false;
                    SetBottomLoading(false);
                    mRecyclerView.RefreshDrawableState();

                });
                CurrentPage++;



            }
            logger.Info("Loading Next Page Successful");

        }

        public override async void PreviousPage()
        {
            isLoading = true;
            logger.Info("Loading Previous Page");
            var h = new Handler(Looper.MainLooper);
            if (CurrentPage <= 0)
            {
                h.Post(() =>
                {
                    SetBottomLoading(false);
                });
                return;
            }
            if (mRefreshLayout.FooterRefreshing)
            {
                logger.Info("Refresh Operation already in progress");
                isLoading = false;
                return;
            }

            h.Post(() =>
            {
                SetBottomLoading(true);
                mRefreshLayout.HeaderRefreshing = true;
            });
            var oldlist = new List<Core.Gallery.HPXItem>(CurrentList);
            var newitems = Core.App.Server.GetRelatedItems<Core.Gallery.GalleryItem>(Collection.id, SceneCancellationTokenSource.Token,
                        Core.Gallery.ItemType.Collection, Core.Gallery.ItemType.Gallery, 50, CurrentPage - 1);
            int nitems = newitems.Count;
            adapter.Prepend(new List<Core.Gallery.HPXItem>(newitems));
            if (nitems > 0)
            {
                h.Post(() =>
                {
                    adapter.NotifyItemRangeInserted(0, 25);
                    mRefreshLayout.HeaderRefreshing = false;
                    isLoading = false;

                });
                CurrentPage--;
            }
            SetBottomLoading(false);
            logger.Info("Loading Previous Page Successful");

        }

    }
}