using Android.App;
using Android.OS;
using Android.Content;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Views;
using Android.Util;
using System.Threading.Tasks;
using System;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using NLog;
using Android.Content.Res;
using Com.Hippo.Stage;

namespace HappyPandaXDroid.Scenes
{
    public class LibraryScene : Scene, Android.Support.V7.Widget.SearchView.IOnQueryTextListener
    {

        Toolbar toolbar;
        public Custom_Views.HPContent ContentView;
        Clans.Fab.FloatingActionMenu fam;
        string title, query;
        AppBarLayout appBarLayout;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        Clans.Fab.FloatingActionButton mRefreshFab;
        Clans.Fab.FloatingActionButton mJumpFab;
        View MainView { get; set; }

        protected override void OnCreate(Bundle p0)
        {
            base.OnCreate(p0);
        }

        protected override View OnCreateView(LayoutInflater p0, ViewGroup p1)
        {
            MainView = p0.Inflate(Resource.Layout.LibraryScene, p1,false);
            Initialize();

            return MainView;
        }

        private void Initialize()
        {
            toolbar = MainView.FindViewById<Toolbar>(Resource.Id.toolbar);


            ContentView = MainView.FindViewById<Custom_Views.HPContent>(Resource.Id.content_view);
            ContentView.ParentScene = this;
            appBarLayout = MainView.FindViewById<AppBarLayout>(Resource.Id.appbar);
            appBarLayout.Drag += AppBarLayout_Drag;
            
            mRefreshFab = MainView.FindViewById<Clans.Fab.FloatingActionButton>(Resource.Id.fabRefresh);
            mJumpFab = MainView.FindViewById<Clans.Fab.FloatingActionButton>(Resource.Id.fabJumpTo);
            mJumpFab.SetImageResource(Resource.Drawable.v_go_to_dark_x24);
            mRefreshFab.SetImageResource(Resource.Drawable.v_refresh_dark_x24);
            fam = MainView.FindViewById<Clans.Fab.FloatingActionMenu>(Resource.Id.fam);
            FABClickListener fabclick = new FABClickListener(this);
            mJumpFab.SetOnClickListener(fabclick);
            mRefreshFab.SetOnClickListener(fabclick);
            OnCreateOptionsMenu();
            if (query.Trim() != string.Empty)
            {
                
                toolbar.Title = title;
            }
            else
                toolbar.Title = "Library";
            ContentView.Current_Query = Parse(query, false);
        }

        public LibraryScene(string title,string query) : base()
        {
            this.title = title;
            this.query = query;
        }

        private void AppBarLayout_Drag(object sender, View.DragEventArgs e)
        {
            e.Handled = true;
        }



        public class HideOnScroll : CoordinatorLayout.Behavior
        {
            public HideOnScroll(Context context, IAttributeSet attrs) : base(context, attrs)
            {

            }



            public override void OnNestedScroll(CoordinatorLayout coordinatorLayout, Java.Lang.Object child, View target, int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed)
            {
                if (child is Clans.Fab.FloatingActionMenu c)
                {
                    base.OnNestedScroll(coordinatorLayout, child, target, dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed);
                    if (dyConsumed > 0)
                    {
                        c.HideMenuButton(true);
                    }
                    else if (dyConsumed < 0)
                    {
                        c.ShowMenuButton(true);
                    }
                }
            }

            public override bool OnStartNestedScroll(CoordinatorLayout coordinatorLayout, Java.Lang.Object child, View directTargetChild, View target, int nestedScrollAxes)
            {
                return nestedScrollAxes == ViewCompat.ScrollAxisVertical;
            }
        }


        class FABClickListener : Java.Lang.Object, View.IOnClickListener
        {
            LibraryScene main;
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public FABClickListener(LibraryScene main)
            {
                this.main = main;
            }
            public void OnClick(View v)
            {
                var fab = (Clans.Fab.FloatingActionButton)v;
                switch (fab.Id)
                {
                    case Resource.Id.fabJumpTo:
                        logger.Info("Page selector shown");
                        main.ContentView.mpageSelector.Show(
                            ((HPXSceneActivity)main.MainView.Context).FragmentManager, "PageSelecter");
                        break;
                    case Resource.Id.fabRefresh:
                        //main.Refresh();
                        break;
                }
                main.fam.Close(true);
            }
        }


        protected override void OnDestroyView(View p0)
        {
            
            base.OnDestroyView(p0);
        }

        protected override void OnDestroy()
        {
            ContentView = null;
            MainView = null;
            fam = null;
            appBarLayout = null;
            mJumpFab = null;
            mRefreshFab = null;
            base.OnDestroy();
        }

        void OnConfigurationChanged(Configuration newConfig)
        {
            ContentView.OrientationChanged(newConfig.Orientation);
        }

        public bool OnQueryTextChange(string newText)
        {
            //throw new NotImplementedException();
            return true;
        }

        public string Parse(string inString, bool IsEscaped = true)
        {
            string res = string.Empty;
            if (IsEscaped)
                res = inString.Replace("\\\"", "\"");
            else
                res = inString.Replace("\"", "\\\"");

            return res;
        }



        public bool OnQueryTextSubmit(string query)
        {

            //SupportActionBar.InvalidateOptionsMenu();
            toolbar.Title = query;
            logger.Info("Search query submit , query ={0}", query);
            ContentView.Current_Query = Parse(query, false);
            if (search != null)
                search.ActionView.ClearFocus();
            searchView.SetQuery(query, false);

            return true;
        }



        Android.Support.V7.Widget.SearchView searchView;
        IMenuItem search;
        
        public void OnCreateOptionsMenu()
        {
            toolbar.InflateMenu(Resource.Menu.gallerySearch);
            
            search = toolbar.Menu.FindItem(Resource.Id.search);

            searchView = (Android.Support.V7.Widget.SearchView)search.ActionView;
            searchView.SetOnQueryTextListener(this);
            searchView.SetQuery(ContentView.Current_Query, false);
        }
        

    }
}