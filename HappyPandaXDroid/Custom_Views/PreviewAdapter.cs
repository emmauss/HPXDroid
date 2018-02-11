using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Com.Hippo.Stage;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HappyPandaXDroid.Custom_Views
{
    class PreviewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<PreviewAdapterClickEventArgs> ItemClick;
        public event EventHandler<PreviewAdapterClickEventArgs> ItemLongClick;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<Core.Gallery.Page> mdata;
        Android.Content.Context mcontext;
        int columns = 1;
        Scene previewScene;
        public PreviewAdapter(Context context, Scene scene)
        {
            previewScene = scene;
            mcontext = context;
        }

        public override int ItemCount
        {
            get { return mdata == null ? 0 : mdata.Count; }
        }

        public void SetList(List<Core.Gallery.Page> UrlList)
        {
            mdata = new List<Core.Gallery.Page>();
            if (UrlList != null)
            {
                mdata.AddRange(UrlList);
                NotifyDataSetChanged();
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            Custom_Views.PreviewHolder vh = holder as Custom_Views.PreviewHolder;
            var page = mdata[position];

            Task.Run(async () =>
            {
                try
                {
                    await vh.LoadPreview(page);
                }
                catch (System.Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In GalleryActivity.PreviewAdaptor.OnBindViewHolder.");
                }

            });

            vh.txt.Text = mdata[position].number.ToString();
        }

        

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            base.OnViewRecycled(holder);
            var hold = holder as PreviewHolder;
            if (hold != null)
            {
                hold.Reset();
                hold.Recycle();
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemview = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.preview_template, parent, false);
            Custom_Views.PreviewHolder vh = new Custom_Views.PreviewHolder(itemview, OnClick, OnLongClick, previewScene);
            return vh;
        }

        void OnClick(PreviewAdapterClickEventArgs args)
        {
            args.Gallery = ((Scenes.GalleryScene)previewScene).gallery;
            ItemClick?.Invoke(this, args);
        }
        void OnLongClick(PreviewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);
    }

        public class PreviewPagerAdapter : RecyclerView.Adapter
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public List<List<Core.Gallery.Page>> mdata;
            Android.Content.Context mcontext;
            Scene previewScene;
            public PreviewPagerAdapter(Context context, Scene scene)
            {
                previewScene = scene;
                mcontext = context;
            }

            public override int ItemCount
            {
                get { return mdata == null ? 0 : mdata.Count; }
            }

            public void SetList(List<List<Core.Gallery.Page>> PageList)
            {
                mdata = new List<List<Core.Gallery.Page>>();
                if (PageList != null)
                {
                    mdata.AddRange(PageList);
                    NotifyDataSetChanged();
                }
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                var vh = holder as PageHolder;
                var previews = mdata[position];
            
                    try
                    {
                        vh.SetList(previews);
                    }
                    catch (System.Exception ex)
                    {

                    }
            }

            public override void OnViewRecycled(Java.Lang.Object holder)
            {
                base.OnViewRecycled(holder);
                var hold = holder as PageHolder;
                if (hold != null)
                {
                    hold.Recycle();
                }
            }



            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
            View itemview = LayoutInflater.From(parent.Context)
            .Inflate(Resource.Layout.PreviewsLayout, parent, false);
            PageHolder vh = new PageHolder(itemview,previewScene);
                return vh;
            }
        

            class PageHolder : RecyclerView.ViewHolder
            {
                RecyclerView view;
                List<Core.Gallery.Page> previews;
                PreviewAdapter adapter;
            int columns = 1;
            Scene parentscene;
                public PageHolder(View ItemView,Scene scene) : base(ItemView)
                {
                    view = ItemView.FindViewById<RecyclerView>(Resource.Id.recyclerView);
                parentscene = scene;
                    adapter = new PreviewAdapter(ItemView.Context, scene);
                adapter.ItemClick += Adapter_ItemClick;
                    view.SetAdapter(adapter);
                SetColumns();
                var layout = new Helpers.Layouts.ExtraGridLayoutManager(ItemView.Context, columns);
                view.SetLayoutManager(layout);
                }

            private void Adapter_ItemClick(object sender, PreviewAdapterClickEventArgs e)
            {
                int pos = e.Position;
                List<int> pages_ids = new List<int>();

                if (previews == null)
                    return;
                if (previews.Count < 1)
                    return;
                var page = adapter.mdata[pos];
                    Intent intent = new Intent(ItemView.Context, typeof(GalleryViewer));
                    intent.PutExtra("no", pos);
                    intent.PutExtra("page", Core.JSON.Serializer.SimpleSerializer.Serialize(previews));
                    intent.PutExtra("gallery", Core.JSON.Serializer.SimpleSerializer.Serialize(e.Gallery));
                    ItemView.Context.StartActivity(intent);
                
            }

            void SetColumns()
            {

                var windo = parentscene.Context.GetSystemService(Context.WindowService);
                var window = windo.JavaCast<IWindowManager>();
                var display = window.DefaultDisplay;


                var metrics = new DisplayMetrics();
                display.GetMetrics(metrics);

                float dpwidth = metrics.WidthPixels / metrics.Density;
                columns = (int)dpwidth / 180; ;

            }

            public void SetList(List<Core.Gallery.Page> list)
                {
                    previews = list;
                    adapter.SetList(list);
                adapter.NotifyDataSetChanged();
                }


                public void Recycle()
                {
                    adapter.SetList(new List<Core.Gallery.Page>());
                    adapter.NotifyDataSetChanged();
                    view.GetRecycledViewPool().Clear();
                }
                
            }

        }


        public class PreviewAdapterClickEventArgs : EventArgs
        {
            public View View { get; set; }
            public int Position { get; set; }
        public Core.Gallery.GalleryItem Gallery { get; set; }
        }
    }

