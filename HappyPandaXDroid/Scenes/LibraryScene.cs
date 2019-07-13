using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
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
        Clans.Fab.FloatingActionButton mToggleFab;

        public LibraryScene(string title, string query) : base(title, query)
        {
            this.current_query = query;
        }

        protected override void Initialize()
        {
            fabclick = new FABClickListener(this);
            mToggleFab = MainView.FindViewById<Clans.Fab.FloatingActionButton>(Resource.Id.fabToggle);
            mToggleFab.SetImageResource(Resource.Drawable.ic_list_white);
            mToggleFab.SetOnClickListener(fabclick);
            base.Initialize();

            OnCreateOptionsMenu();
            /*if (query.Trim() != string.Empty)
            {

                toolbar.Title = title.Replace("__namespace__:", "misc:");
                toolbar.Title = title.Replace("\\", "");
            }
            else
                toolbar.Title = "Library";

            Current_Query = Parse(query, false);*/
            toolbar.Title = string.IsNullOrWhiteSpace(current_query) ? "Library" : current_query;
            searchView.SetQuery(current_query, true);
            searchView.SearchViewShown += SearchView_SearchViewShown;
            LoadOptions();
            Refresh(0);
        }

        private void SearchView_SearchViewShown(object sender, EventArgs e)
        {
            searchView.SetQuery(current_query, false);
        }

        protected override void OnSaveViewState(View p0, Bundle p1)
        {
            var bundle = p1;
            bundle.PutString("query", current_query);
            base.OnSaveViewState(p0, p1);
        }

        protected override void OnRestoreViewState(View p0, Bundle p1)
        {
            var bundle = p1;
            current_query = bundle.GetString("query");
            base.OnRestoreViewState(p0, p1);
        }

        public override async void GetTotalCount()
        {
            RequestToken token = new RequestToken(SceneCancellationTokenSource.Token);
            token.FinishedCallback += Token_FinishedCallback;
            token.FailedCallback += Token_FailedCallback;
            Core.Gallery.GetCount(ItemType, CurrentQuery, token, ViewType); 
        }

        private async void Token_FailedCallback(object sender, EventArgs e)
        {
            await Task.Delay(2000);

            GetTotalCount();
        }

        private void Token_FinishedCallback(object sender, EventArgs e)
        {
            if (sender is RequestToken token)
            {
                if (!token.CancellationToken.IsCancellationRequested)
                {
                    Count = (int)token.Result;
                }
            }
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
                    toolbar.Title = current_query;
                    SetMainLoading(true);
                });
                await Task.Run(async () =>
                {
                    SceneCancellationTokenSource.Cancel();

                    SceneCancellationTokenSource = new CancellationTokenSource();

                    RequestToken = new RequestToken(SceneCancellationTokenSource.Token);

                    RequestToken.FinishedCallback += RefreshToken_AsyncCallback;

                    RequestToken.FailedCallback += RefreshToken_FailedCallback;
                    logger.Info("Refreshing HPContent");
                    Core.Gallery.GetPage(ItemType, page, RequestToken, ViewType, Core.App.Settings.Default_Sort,
                        Core.App.Settings.Sort_Descending, CurrentQuery);                    
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

        private void RefreshToken_FailedCallback(object sender, EventArgs e)
        {
            var h = new Handler(Looper.MainLooper);

            if (sender is RequestToken token)
            {
                if (!token.CancellationToken.IsCancellationRequested)
                {
                    h.Post(() =>
                    {
                        SetMainLoading(false);
                        SetError(true);
                    });
                }
            }
        }

        private void RefreshToken_AsyncCallback(object sender, EventArgs e)
        {
            var h = new Handler(Looper.MainLooper);

            if (sender is RequestToken token)
            {
                if (!token.CancellationToken.IsCancellationRequested)
                {
                    List<Core.Gallery.HPXItem> list = (List<Core.Gallery.HPXItem>)token.Result;

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

                    h.Post(() =>
                    {
                        adapter.ResetList();
                        adapter.NotifyDataSetChanged();
                        SetMainLoading(false);

                        h.Post(() =>
                        {
                            mRefreshLayout.HeaderRefreshing = false;
                            mRefreshLayout.FooterRefreshing = false;
                        });

                        IsRefreshing = false;
                        if (CurrentList.Count > 0)
                            mRecyclerView.ScrollToPosition(0);
                    });
                    GetTotalCount();
                    mpageSelector = new Custom_Views.PageSelector(this);
                    logger.Info("HPContent Refresh Successful");
                }
                else if(token == RequestToken && !IsDestroyed)
                    h.Post(() =>
                    {
                        mRefreshLayout.HeaderRefreshing = false;
                        mRefreshLayout.FooterRefreshing = false;
                        IsRefreshing = false;
                    });
            }
        }

        public override async void NextPage()
        {
            isLoading = true;
            logger.Info("Loading Next Page");
            var h = new Handler(Looper.MainLooper);
            if ((CurrentPage + 1) >= (Count / 50))
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

            SceneCancellationTokenSource.Cancel();

            SceneCancellationTokenSource = new CancellationTokenSource();

            RequestToken = new RequestToken(SceneCancellationTokenSource.Token);

            RequestToken.Args = new List<int>() { lastin };

            RequestToken.FinishedCallback += NextPageToken_AsyncCallback;

            RequestToken.FailedCallback += NextPageToken_FailedCallback;

            Core.Gallery.GetPage(ItemType, CurrentPage + 1, RequestToken, ViewType, Core.App.Settings.Default_Sort,
                Core.App.Settings.Sort_Descending, CurrentQuery);
        }

        private void NextPageToken_FailedCallback(object sender, EventArgs e)
        {
            var h = new Handler(Looper.MainLooper);

            if (sender is RequestToken token)
            {
                if (!token.CancellationToken.IsCancellationRequested)
                {
                    h.Post(() =>
                    {
                        mRefreshLayout.HeaderRefreshing = false;
                        mRefreshLayout.FooterRefreshing = false;
                        isLoading = false;
                        SetBottomLoading(false);
                        Toast.MakeText(this.Context, "Failed to retrieve the next page", ToastLength.Long).Show();
                    });
                }
            }
        }

        private void NextPageToken_AsyncCallback(object sender, EventArgs e)
        {
            var h = new Handler(Looper.MainLooper);

            if (sender is RequestToken token)
            {
                if (!token.CancellationToken.IsCancellationRequested)
                {
                    List<Core.Gallery.HPXItem> list = (List<Core.Gallery.HPXItem>)token.Result;
                    int lastin = 0;

                    if(e is RequestToken.ExtraEventArgs args)
                    {
                        if (args.ExtraArgs.Length > 0)
                        {
                            try
                            {
                              lastin = args.ExtraArgs[0];
                            }catch(Exception ex)
                            {

                            }
                        }
                    }

                    adapter.Add(list);

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
                }
            }
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
            //var oldlist = new List<Core.Gallery.HPXItem>(CurrentList);

            SceneCancellationTokenSource.Cancel();

            SceneCancellationTokenSource = new CancellationTokenSource();

            RequestToken = new RequestToken(SceneCancellationTokenSource.Token);

            RequestToken.FinishedCallback += PreviousPageToken_AsyncCallback;

            RequestToken.FailedCallback += PreviousPageToken_FailedCallback;

            Core.Gallery.GetPage(ItemType, CurrentPage - 1, RequestToken,
                ViewType, Core.App.Settings.Default_Sort, Core.App.Settings.Sort_Descending, CurrentQuery);
        }

        private void PreviousPageToken_FailedCallback(object sender, EventArgs e)
        {
            var h = new Handler(Looper.MainLooper);

            if (sender is RequestToken token)
            {
                if (!token.CancellationToken.IsCancellationRequested)
                {
                    h.Post(() =>
                    {
                        mRefreshLayout.HeaderRefreshing = false;
                        mRefreshLayout.FooterRefreshing = false;
                        isLoading = false;
                        SetBottomLoading(false);
                        Toast.MakeText(this.Context, "Failed to retrieve the previous page", ToastLength.Long).Show();
                    });
                }
            }
        }

        private void PreviousPageToken_AsyncCallback(object sender, EventArgs e)
        {
            var h = new Handler(Looper.MainLooper);

            if (sender is RequestToken token)
            {
                if (!token.CancellationToken.IsCancellationRequested)
                {
                    List<Core.Gallery.HPXItem> newitems = (List<Core.Gallery.HPXItem>)token.Result;
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
            }
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
                Refresh(0);
            }
        }

        public override Core.Gallery.ViewType ViewType => Core.Gallery.ViewType.Library;
    }
}