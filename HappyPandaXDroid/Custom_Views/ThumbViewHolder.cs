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
        public Core.Gallery.ImageSize Size { get; set; }

        public bool Bound
        {
            set
            {
                Media.Image image = Size == Core.Gallery.ImageSize.Small ? HPXItem.Thumb : HPXItem.Image;
                if (value)
                {
                    image.Ready += Image_Ready;
                    CancellationTokenSource = new CancellationTokenSource();
                    if (image.IsReady)
                        LoadImage();
                }
                else
                {
                    image.Ready -= Image_Ready;
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
        public ThumbViewHolder(View itemView, Core.Gallery.ImageSize size) : base(itemView)
        {
            Token = new CancellationTokenSource();
            Size = size;
        }

        async void LoadImage()
        {
            await Task.Delay(1000);
            var token = CancellationTokenSource.Token;
            string url = string.Empty;
            Media.Image image = Size == Core.Gallery.ImageSize.Small ? HPXItem.Thumb : HPXItem.Image;
            if (string.IsNullOrWhiteSpace(image.Uri))
            {
                string cacheid = Core.App.Server.HashGenerator(HPXItem.BaseId, Size, HPXItem.Type);
                url = App.Server.GetCommandValue(HPXItem.CommandId, HPXItem,cacheid, false,ref token);
            }
            else
                url = image.Uri;
            if (!string.IsNullOrWhiteSpace(url))
            {
                var h = new Handler(Looper.MainLooper);
                h.Post(() => Glide.With(Thumb.Context).Load(url).Into(Thumb));
                image.Uri = url;
            }
            else
            {

            }
        }        
    }
}