using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HappyPandaXDroid.Core;
using static HappyPandaXDroid.Core.Gallery;

namespace HappyPandaXDroid.Scenes
{
    public class CollectionGalleriesScene : ViewScene
    {
        public override Core.Gallery.ViewType ViewType => Core.Gallery.ViewType.Library;

        Clans.Fab.FloatingActionButton mToggleFab;

        Collection collection;

        public CollectionGalleriesScene(string title, Collection collection) : base(title, string.Empty)
        {
            this.collection = collection;
        }

        protected override void Initialize()
        {
            fabclick = new FABClickListener(this);
            mToggleFab = MainView.FindViewById<Clans.Fab.FloatingActionButton>(Resource.Id.fabToggle);
            mToggleFab.Enabled = false;
            mToggleFab.Visibility = ViewStates.Gone;
            base.Initialize();
            /*if (query.Trim() != string.Empty)
            {

                toolbar.Title = title.Replace("__namespace__:", "misc:");
                toolbar.Title = title.Replace("\\", "");
            }
            else
                toolbar.Title = "Library";

            Current_Query = Parse(query, false);*/
            searchView.SetSearchText(title);
            Refresh(0);
        }

        public async override void GetTotalCount()
        {
            while (Count <= 0)
            {
                var result = App.Server.GetRelatedCount(collection.category_id,
                    SceneCancellationTokenSource.Token,
                    ItemType.Collection,
                    ItemType.Gallery);
                if(result > 0)
                {
                    Count = result;

                    return;
                }
                else
                {
                    await Task.Delay(2000);
                }


            }
        }

        public async override void NextPage()
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

            SceneCancellationTokenSource.Cancel();

            SceneCancellationTokenSource = new CancellationTokenSource();

            int nextPage = CurrentPage + 1;

            var list = App.Server.GetRelatedItems<GalleryItem>(collection.id,
                SceneCancellationTokenSource.Token,
                ItemType.Category,
                ItemType.Gallery,
                50, nextPage);

            if (list?.Count > 0)
            {
                List<HPXItem> galleries = new List<HPXItem>();

                foreach(var item in list)
                {
                    galleries.Add(item);
                }

                if (!SceneCancellationTokenSource.Token.IsCancellationRequested)
                {
                    adapter.Add(galleries);

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
            else
            {
                if (!SceneCancellationTokenSource.Token.IsCancellationRequested)
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

        public override void PreviousPage()
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

            int previous = CurrentPage + 1;

            var list = App.Server.GetRelatedItems<GalleryItem>(collection.id,
                SceneCancellationTokenSource.Token,
                ItemType.Category,
                ItemType.Gallery,
                50, previous);

            if (list?.Count > 0)
            {
                List<HPXItem> galleries = new List<HPXItem>();

                foreach (var item in list)
                {
                    galleries.Add(item);
                }

                int nitems = galleries.Count;
                adapter.Prepend(galleries);
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
            else
            {
                if (!SceneCancellationTokenSource.Token.IsCancellationRequested)
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

        public async override void Refresh(int page)
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
                    SceneCancellationTokenSource.Cancel();

                    SceneCancellationTokenSource = new CancellationTokenSource();

                    var list = App.Server.GetRelatedItems<GalleryItem>(collection.id,
                                    SceneCancellationTokenSource.Token,
                                    ItemType.Category,
                                    ItemType.Gallery,
                                    50, page);

                    if (list?.Count > 0)
                    {
                        if (!SceneCancellationTokenSource.Token.IsCancellationRequested)
                        {
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

                            return;
                        }
                        if (!SceneCancellationTokenSource.Token.IsCancellationRequested)
                        {
                            h.Post(() =>
                            {
                                h.Post(() =>
                                {
                                    SetMainLoading(false);
                                    SetError(true);
                                });
                            });
                        }
                    }
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
    }
}