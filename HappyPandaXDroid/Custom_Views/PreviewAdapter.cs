using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

using NLog;
using Java.Lang;

namespace HappyPandaXDroid.Custom_Views
{
    class PreviewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<PreviewAdapterClickEventArgs> ItemClick;
        public event EventHandler<PreviewAdapterClickEventArgs> ItemLongClick;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<Core.Gallery.Page> mdata;
        Android.Content.Context mcontext;
        public PreviewAdapter(Context context)
        {
            mcontext = context;
        }

        public override int ItemCount
        {
            get { return mdata == null ? 0 : mdata.Count; }
        }

        public void SetList(List<Core.Gallery.Page> UrlList)
        {
            mdata = new List<Core.Gallery.Page>();
            mdata.AddRange(UrlList);
            NotifyDataSetChanged();
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
            Custom_Views.PreviewHolder vh = new Custom_Views.PreviewHolder(itemview,OnClick,OnLongClick);
            return vh;
        }

        void OnClick(PreviewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(PreviewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    

    public class PreviewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}