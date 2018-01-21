using Android.App;
using Android.OS;
using Android.Content;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.View;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Request;
using ProgressView = XamarinBindings.MaterialProgressBar;
using NLog;
using Android.Content.Res;
using Com.Hippo.Stage;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace HappyPandaXDroid.Scenes
{
    class PreviewScene : HPXScene
    {
        RecyclerView grid_layout;
        Toolbar toolbar;
        Custom_Views.PreviewAdapter adapter;
        GridLayoutManager layout;
        public int PreviewColumns;
        List<Core.Gallery.Page> pagelist;
        Core.Gallery.GalleryItem gallery;
        View MainView;

        protected override View OnCreateView(LayoutInflater p0, ViewGroup p1)
        {
            MainView = p0.Inflate(Resource.Layout.PreviewsLayout, p1, false);
            Init();
            adapter.SetList(pagelist);
            return MainView;
        }


        protected override void OnAttachView(View p0)
        {
            base.OnAttachView(p0);
            toolbar.Title = gallery.titles[0].name;
            toolbar.SetNavigationIcon(Resource.Drawable.navigation_empty_icon);
            toolbar.NavigationClick += Toolbar_NavigationClick;
        }

        private void Toolbar_NavigationClick(object sender, Toolbar.NavigationClickEventArgs e)
        {
            Stage.OnHandleBack();
        }

        public PreviewScene(List<Core.Gallery.Page> list, Core.Gallery.GalleryItem galleryItem)
        {
            pagelist = list;
            gallery = galleryItem;
        }

        protected override void OnSaveViewState(View p0, Bundle p1)
        {
            var bundle = p1;
            bundle.PutString("gallery", Core.JSON.Serializer.SimpleSerializer.Serialize(gallery));
            bundle.PutString("list", Core.JSON.Serializer.SimpleSerializer.Serialize(pagelist));
            base.OnSaveViewState(p0, p1);
        }
        

        protected override void OnRestoreViewState(View p0, Bundle p1)
        {
            base.OnRestoreViewState(p0, p1);
            var bundle = p1;
            gallery = Core.JSON.Serializer.SimpleSerializer.Deserialize
                <Core.Gallery.GalleryItem>(bundle.GetString("gallery"));
            pagelist = Core.JSON.Serializer.SimpleSerializer.DeserializeToList
                <Core.Gallery.Page>(bundle.GetString("gallery"));
        }
        
        void Init()
        {
            toolbar = MainView.FindViewById<Toolbar>(Resource.Id.toolbar);
            grid_layout = MainView.FindViewById<RecyclerView>(Resource.Id.grid_layout);
            adapter = new Custom_Views.PreviewAdapter(this.Context,this);
            adapter.ItemClick += Adapter_ItemClick;
            grid_layout.SetAdapter(adapter);
            SetColumns();
            layout = new GridLayoutManager(this.Context, PreviewColumns);
            grid_layout.SetLayoutManager(layout);
        }



        private void Adapter_ItemClick(object sender, Custom_Views.PreviewAdapterClickEventArgs e)
        {
            int pos = e.Position;
            List<int> pages_ids = new List<int>();

            if (pagelist == null)
                return;
            if (pagelist == null & pagelist.Count < 1)
                return;

            Intent intent = new Android.Content.Intent(this.Context, typeof(GalleryViewer));
            intent.PutExtra("page", Core.JSON.Serializer.SimpleSerializer.Serialize(pagelist));
            intent.PutExtra("gallery", Core.JSON.Serializer.SimpleSerializer.Serialize(gallery));
            intent.PutExtra("no", pos);
            StartActivity(intent);
        }

        void SetColumns()
        {
            var windo = Context.GetSystemService(Context.WindowService);
            var window = windo.JavaCast<IWindowManager>();
            var display = window.DefaultDisplay;
            int gridFactor = 0;
            float w = display.Width;
            gridFactor = (int)(Math.Ceiling(w / (160 * 2)));
            if (Context.Resources.Configuration.Orientation == Android.Content.Res.Orientation.Landscape)
            {
                gridFactor = (int)(Math.Ceiling(w / (160 * 3)));
                PreviewColumns = gridFactor * 2;
            }
            else
                PreviewColumns = gridFactor;
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            SetColumns();
            layout = new GridLayoutManager(this.Context, PreviewColumns);
            grid_layout.SetLayoutManager(layout);
        }
    }
}