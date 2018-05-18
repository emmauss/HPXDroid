using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using System.Threading;
using Android.Support.V7.View;
using Android.Support.V7.App;
using Android.Support.V4.View;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using ProgressView = XamarinBindings.MaterialProgressBar;
using EasyRecyclerView;
using EasyRecyclerView.Addons;
using NLog;
using Android.Content.Res;
using RefreshLayout = Com.Hippo.Refreshlayout;
using Com.Hippo.Stage;

namespace HappyPandaXDroid.Custom_Views
{
    class CardAdapter
    {
        public abstract class HPXCardAdapter : EasyAdapter
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            CancellationTokenSource AdapterCancellationTokenSource = new CancellationTokenSource();
            public EventHandler<int> ItemClick;
            protected Core.Gallery.ItemType ItemType;
            public Scenes.ViewScene content;
            void OnClick(int position)
            {
                ItemClick?.Invoke(this, position);
            }

            public Scene scene;

            public List<Core.Gallery.HPXItem> mdata;
            Android.Content.Context mcontext;
            public HPXCardAdapter(Context context, Scenes.ViewScene content)
            {
                mcontext = context;
                this.content = content;
                mdata = content.CurrentList;
                AdapterCancellationTokenSource = new CancellationTokenSource();
            }

            public override int ItemCount
            {
                get { return mdata.Count; }
            }

            public void Clear()
            {

                mdata.Clear();
                AdapterCancellationTokenSource.Cancel();
                NotifyDataSetChanged();
            }

            public void ResetList()
            {
                AdapterCancellationTokenSource.Cancel();
                AdapterCancellationTokenSource = new CancellationTokenSource();
                mdata = content.CurrentList;
                this.NotifyDataSetChanged();
            }

            public override void OnViewRecycled(Java.Lang.Object holder)
            {
                base.OnViewRecycled(holder);
                if (holder is HPXCardHolder hold)
                {
                    Task.Run(() =>
                    {
                        hold.gcard.Reset();
                        hold.gcard.Recycle();
                    });
                }

            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                HPXCardHolder vh = holder as HPXCardHolder;
                try
                {
                    Task.Run(() =>
                    vh.Bind(mdata[position]));
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In CardAdapter.OnBindViewHolder.");

                }
            }


            public override RecyclerView.ViewHolder OnCreateViewHolder2(ViewGroup parent, int viewType)
            {
                View itemview = null;
                switch (ItemType)
                {
                    case Core.Gallery.ItemType.Gallery:
                        itemview = new GalleryCard(mcontext);
                        break;
                    case Core.Gallery.ItemType.Collection:
                        itemview = new CollectionCard(mcontext);
                        break;
                }
                HPXCardHolder vh = new HPXCardHolder(itemview);
                return vh;
            }
        }

        public class GalleryCardAdapter : HPXCardAdapter
        {
            public GalleryCardAdapter(Context context, Scenes.ViewScene content)
                : base(context, content)
            {
                ItemType = Core.Gallery.ItemType.Gallery;
            }


        }

        public class CollectionCardAdapter : HPXCardAdapter
        {
            public CollectionCardAdapter(Context context, Scenes.ViewScene content)
                : base(context, content)
            {
                ItemType = Core.Gallery.ItemType.Collection;
            }
        }

        public class HPXCardHolder : RecyclerView.ViewHolder
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public ItemCard gcard;

            public HPXCardHolder(View itemView) : base(itemView)
            {
                gcard = (ItemCard)itemView;
            }

            public void Bind(Core.Gallery.HPXItem item)
            {
                gcard.HPXItem = item;
                Task.Run(() =>
                {
                    gcard.Recycle();
                    gcard.Refresh();
                });
            }

        }
    }
}