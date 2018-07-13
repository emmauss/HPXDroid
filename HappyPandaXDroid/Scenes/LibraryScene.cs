using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HappyPandaXDroid.Core;

namespace HappyPandaXDroid.Scenes
{
    public class LibraryScene : ViewScene
    {
        string query;
        Clans.Fab.FloatingActionButton mToggleFab;

        public LibraryScene(string title, string query) : base(title, query)
        {
            this.query = query;
        }

        protected override void Initialize()
        {
            fabclick = new FABClickListener(this);
            mToggleFab = MainView.FindViewById<Clans.Fab.FloatingActionButton>(Resource.Id.fabToggle);
            mToggleFab.SetImageResource(Resource.Drawable.ic_list_white);
            mToggleFab.SetOnClickListener(fabclick);
            base.Initialize();
            OnCreateOptionsMenu();
            if (query.Trim() != string.Empty)
            {

                toolbar.Title = title.Replace("__namespace__:", "misc:");
                toolbar.Title = title.Replace("\\", "");
            }
            else
                toolbar.Title = "Library";
            Current_Query = Parse(query, false);
        }

        protected override void OnSaveViewState(View p0, Bundle p1)
        {
            var bundle = p1;
            bundle.PutString("query", Current_Query);
            base.OnSaveViewState(p0, p1);
        }

        protected override void OnRestoreViewState(View p0, Bundle p1)
        {
            var bundle = p1;
            query = bundle.GetString("query");
            base.OnRestoreViewState(p0, p1);
        }

        public override async void GetTotalCount()
        {
            Count = await Core.Gallery.GetCount(ItemType, Current_Query, SceneCancellationTokenSource.Token, ViewType);
        }

        public override async void Refresh()
        {
            CurrentList = new List<Core.Gallery.HPXItem>();
            CurrentPage = 0;

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
                    var list = await Core.Gallery.GetPage(ItemType, 0, SceneCancellationTokenSource.Token, ViewType, Core.App.Settings.Default_Sort,
                        Core.App.Settings.Sort_Decending, Current_Query);
                    foreach(var item in list)
                    {
                        item.Image = new Media.Image();
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
                });
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
            adapter.Add((await Core.Gallery.GetPage(ItemType, CurrentPage + 1, SceneCancellationTokenSource.Token, ViewType, Core.App.Settings.Default_Sort,
                Core.App.Settings.Sort_Decending, Current_Query)));
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
            var newitems = await Core.Gallery.GetPage(ItemType, CurrentPage - 1, SceneCancellationTokenSource.Token,
                ViewType, Core.App.Settings.Default_Sort, Core.App.Settings.Sort_Decending, Current_Query);
            int nitems = newitems.Count;
            adapter.Prepend(newitems);
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

        protected override void SwitchToView(Core.Gallery.ItemType itemType)
        {
            if (itemType == ItemType)
                return;
            ItemType = itemType;
            adapter = GetAdapter();
            mRecyclerView.SetAdapter(adapter);
            if (ItemType == Core.Gallery.ItemType.Gallery)
                mToggleFab.SetImageResource(Resource.Drawable.ic_list_white);
            else if (ItemType == Core.Gallery.ItemType.Collection)
                mToggleFab.SetImageResource(Resource.Drawable.ic_import_contacts);
            adapter.ResetList();
            if (CurrentList.Count == 0)
            {
                Refresh();
            }
        }

        public override Core.Gallery.ViewType ViewType => Core.Gallery.ViewType.Library;
    }
}