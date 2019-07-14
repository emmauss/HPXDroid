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
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using NLog;

using Android.Support.V7.Widget;
using Android.Support.V7.App;
using Com.Hippo.Stage;
using HappyPandaXDroid.Core;

namespace HappyPandaXDroid.CustomViews
{
    public class PreviewHolder : ThumbViewHolder
    {
        public View preview;
        public TextView txt;
        public Core.Gallery.Page page;
        Scene scene;

        public override Core.Gallery.HPXItem HPXItem {
            get => page;
            set
            {
                page = (Core.Gallery.Page)value;
            }
        }

        public PreviewHolder(View itemView,Scene scene ) : base(itemView, Core.Gallery.ImageSize.Medium)
        {
            this.scene = scene;
            preview = itemView;
            Thumb = preview.FindViewById<ImageView>(Resource.Id.preview);
            txt = preview.FindViewById<TextView>(Resource.Id.title);
        }

        public void SetClickEvents(Action<PreviewAdapterClickEventArgs> clickListener,
                            Action<PreviewAdapterClickEventArgs> longClickListener)
        {
            preview.Click += (sender, e) => clickListener(new PreviewAdapterClickEventArgs { View = preview, Position = AdapterPosition });
            preview.LongClick += (sender, e) => longClickListener(new PreviewAdapterClickEventArgs { View = preview, Position = AdapterPosition });
        }

        public void RemoveClickEvents(Action<PreviewAdapterClickEventArgs> clickListener,
                            Action<PreviewAdapterClickEventArgs> longClickListener)
        {
            preview.Click -= (sender, e) => clickListener(new PreviewAdapterClickEventArgs { View = preview, Position = AdapterPosition });
            preview.LongClick -= (sender, e) => longClickListener(new PreviewAdapterClickEventArgs { View = preview, Position = AdapterPosition });
        }

        public void Recycle()
        {
            Glide.With(preview.Context).Clear(Thumb);
        }
    }
}