using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HappyPandaXDroid.Services
{
    class DownloadService : Android.App.Service
    {
        public static DownloadService DownloadingService { get; set; }
        Intent Intent;
        public override IBinder OnBind(Intent intent)
        {
            Intent = intent;
            return null;
        }

        public override void OnCreate()
        {
            if (Core.Gallery.DownloadList == null)
                Core.Gallery.DownloadList = new ConcurrentQueue<(Core.Gallery.HPXItem, Core.Gallery.ImageSize)>();
            base.OnCreate();
        }

        public override bool StopService(Intent name)
        {            
            Core.Gallery.StopQueue();
            return base.StopService(name);
        }

        public override void OnDestroy()
        {
            Core.Gallery.StopQueue();
            Core.Gallery.DownloadList.Clear();
            
            base.OnDestroy();
        }

        public DownloadService()
        {
            if (DownloadingService != null)
            {
                DownloadingService.StopSelf();
                DownloadingService.OnDestroy();
            }
            DownloadingService = this;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Thread thread = new Thread(new ThreadStart(Core.Gallery.StartQueue));
            thread.Start();
            return StartCommandResult.NotSticky;
        }

    }
}