using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Support.V7.App;
using Android.Support.V7.Widget;

using Toolbar = Android.Support.V7.Widget.Toolbar;


namespace HappyPandaXDroid
{
    [Activity(Label = "PreviewActivity")]
    public class PreviewActivity : AppCompatActivity
    {
        RecyclerView grid_layout;
        Toolbar toolbar;
        Custom_Views.PreviewAdapter adapter;
        GridLayoutManager layout;
        public int activityId, PreviewColumns;
        List<Core.Gallery.Page> pagelist;
        Core.Gallery.GalleryItem gallery;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.PreviewsLayout);
            string data = Intent.GetStringExtra("page");
            
            pagelist = Core.JSON.Serializer.SimpleSerializer.DeserializeToList<Core.Gallery.Page>(data);
            data = Intent.GetStringExtra("gallery");
            gallery = Core.JSON.Serializer.SimpleSerializer.Deserialize<Core.Gallery.GalleryItem>(data);
            Init();
            SupportActionBar.Title = gallery.titles[0].name;
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            adapter.SetList(pagelist);
        }


        void Init()
        {
            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            grid_layout = FindViewById<RecyclerView>(Resource.Id.grid_layout);
            adapter = new Custom_Views.PreviewAdapter(this);
            adapter.ItemClick += Adapter_ItemClick;
            grid_layout.SetAdapter(adapter);
            SetColumns();
            layout = new GridLayoutManager(this, PreviewColumns);
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

            Intent intent = new Android.Content.Intent(this, typeof(GalleryViewer));
            intent.PutExtra("page", Core.JSON.Serializer.SimpleSerializer.Serialize(pagelist));
            intent.PutExtra("gallery", Core.JSON.Serializer.SimpleSerializer.Serialize(gallery));
            intent.PutExtra("no", pos);
            StartActivity(intent);
        }

        void SetColumns()
        {
            var windo = GetSystemService(Context.WindowService);
            var window = windo.JavaCast<IWindowManager>();
            var display = window.DefaultDisplay;
            int gridFactor = 0;
            float w = display.Width;
            gridFactor = (int)(Math.Ceiling(w / (160 * 2)));
            if (Resources.Configuration.Orientation == Android.Content.Res.Orientation.Landscape)
            {
                gridFactor = (int)(Math.Ceiling(w / (160 * 3)));
                PreviewColumns = gridFactor * 2;
            }
            else
                PreviewColumns = gridFactor;
        }
    }
}