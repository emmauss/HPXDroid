﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

using NLog;

namespace HappyPandaXDroid.Custom_Views
{
    class PreviewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<PreviewAdapterClickEventArgs> ItemClick;
        public event EventHandler<PreviewAdapterClickEventArgs> ItemLongClick;
        public int preview_count = 10;
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
            int number = preview_count;
            if (UrlList.Count < 10)
                number = UrlList.Count;
            mdata = new List<Core.Gallery.Page>();
            for (int i = 0; i < number; i++)
            {
                mdata.Add(UrlList[i]);
            }
            //mdata.Capacity = 20;
            mdata.TrimExcess();
            NotifyDataSetChanged();
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            Custom_Views.PreviewHolder vh = holder as Custom_Views.PreviewHolder;
            var page = mdata[position];
            var activity = (GalleryActivity)mcontext;

            Task.Run(async () =>
            {
                try
                {
                    await vh.LoadPreview(page);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In GalleryActivity.PreviewAdaptor.OnBindViewHolder.");
                }

            });

            vh.txt.Text = mdata[position].number.ToString();
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

    public class PreviewAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }


        public PreviewAdapterViewHolder(View itemView, Action<PreviewAdapterClickEventArgs> clickListener,
                            Action<PreviewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            itemView.Click += (sender, e) => clickListener(new PreviewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new PreviewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class PreviewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}