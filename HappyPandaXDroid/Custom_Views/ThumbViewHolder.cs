using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using Com.Hippo.Stage;
using EasyRecyclerView;
using HappyPandaXDroid.Core;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HappyPandaXDroid;

namespace HappyPandaXDroid.Custom_Views
{

    public abstract class ThumbViewHolder : RecyclerView.ViewHolder
    {
        protected CancellationTokenSource CancellationTokenSource;
        public abstract Core.Gallery.HPXItem HPXItem { get; set; }
        public Core.Gallery.ItemType ItemType;
        public ImageView Thumb { get; set; }

        public bool Bound
        {
            set
            {
                if (value)
                {
                    HPXItem.Image.Ready += Image_Ready;
                    CancellationTokenSource = new CancellationTokenSource();
                    if (HPXItem.Image.IsReady)
                        LoadImage();
                }
                else
                {
                    HPXItem.Image.Ready -= Image_Ready;
                    CancellationTokenSource.Cancel();
                }
            }
        }

        private void Image_Ready(object sender, Media.Image.ImageLoadEvent e)
        {
            LoadImage();
        }

        public string Url = string.Empty;
        public int Id = 0;
        CancellationTokenSource Token { get; set; }
        public ThumbViewHolder(View itemView) : base(itemView)
        {
            Token = new CancellationTokenSource();
        }

        async void LoadImage()
        {
            await Task.Delay(1000);
            var token = CancellationTokenSource.Token;
            string url = App.Server.GetCommandValue(HPXItem.CommandId, HPXItem.id,
                string.Empty, ref token);
            if (!string.IsNullOrWhiteSpace(url))
            {
                var h = new Handler(Looper.MainLooper);
                h.Post(() => Glide.With(Thumb.Context).Load(url).Into(Thumb));
                HPXItem.Image.Uri = url;
            }
        }        
    }
}