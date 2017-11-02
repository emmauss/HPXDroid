using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using System;
using System.IO;
using System.Threading.Tasks;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace HappyPandaXDroid
{
    [Activity(Label = "HPXDroid", MainLauncher = true, Icon = "@drawable/icon",
        ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation 
        | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class MainActivity : AppCompatActivity , Android.Support.V7.Widget.SearchView.IOnQueryTextListener
    {
        
        //public List<string> lists = new List<string>();
        //ArrayAdapter<string> adapter;
        Toolbar toolbar;
        bool RootActivity = true;
        public Custom_Views.HPContent ContentView;
        DrawerLayout navDrawer;
        public bool SwitchedToSettings = false;
        CountDown backTimer;
        AppBarLayout appBarLayout;
        Toast toast;
        public int activityId;
        Clans.Fab.FloatingActionMenu fam;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        Clans.Fab.FloatingActionButton mRefreshFab;
        Clans.Fab.FloatingActionButton mJumpFab;
        bool backPressed = false;
        
        protected override void OnCreate(Bundle bundle)
        {
            Task.Run(()=> {
                Core.Net.Connect();
                });
            base.OnCreate(bundle);
            //set unhandled exception handler
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Task.Run(() => {
                string ds = @"{ 'name' : 'test',
'session' : 'ed352d0aae5b4b11a0e9f64778270087',
'data' : [
{
    'fname' : 'library_view',
'limit' : 25
}
]
}";
                JsonConvert.DeserializeObject(ds);
            });
            CreateFolders();
            //init logger

            InitLogging();
            
            logger.Info("Main Actitvity Created");
            
            toast = Toast.MakeText(this, "Press Back again to exit", ToastLength.Short);
            Android.Support.V7.App.AppCompatDelegate.CompatVectorFromResourcesEnabled = true;
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            appBarLayout = FindViewById<AppBarLayout>(Resource.Id.appbar);
            AppBarLayout.Behavior behavior = new AppBarLayout.Behavior();
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Library";
            ContentView = FindViewById<Custom_Views.HPContent>(Resource.Id.content_view);
            ContentView.activityId = activityId;
            ContentView.activityName = "MainActivity";
            ContentView.InitLibrary();
            var navView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navView.NavigationItemSelected += NavView_NavigationItemSelected;
            navDrawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            var navToggle = new ActionBarDrawerToggle
               (this, navDrawer, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
            navDrawer.AddDrawerListener(navToggle);
            navToggle.SyncState();
            mRefreshFab = FindViewById<Clans.Fab.FloatingActionButton>(Resource.Id.fabRefresh);
            mJumpFab = FindViewById<Clans.Fab.FloatingActionButton>(Resource.Id.fabJumpTo);
            mJumpFab.SetImageResource(Resource.Drawable.v_go_to_dark_x24);
            mRefreshFab.SetImageResource(Resource.Drawable.v_refresh_dark_x24);
            fam = FindViewById<Clans.Fab.FloatingActionMenu>(Resource.Id.fam);
            FABClickListener fabclick = new FABClickListener(this);
            mJumpFab.SetOnClickListener(fabclick);
            mRefreshFab.SetOnClickListener(fabclick);
            
            if(!File.Exists(Core.App.Settings.basePath + ".nomedia"))
            File.Create(Core.App.Settings.basePath + ".nomedia");
            
        }


        public class DragListener : Java.Lang.Object, View.IOnDragListener
        {
            public bool OnDrag(View v, DragEvent e)
            {
                return false;
            }
        }


        public static void InitLogging()
        {
            
            if (Core.App.Settings.Logging_Enabled)
                {
                    LogManager.Configuration = new LoggingConfiguration();
                    NLog.Targets.FileTarget target = new NLog.Targets.FileTarget("log");
                    string logfile = Core.App.Settings.Log + DateTime.Now.ToShortDateString().Replace("/", "-") + " - "
                        + DateTime.Now.ToShortTimeString().Replace(":", ".") + " - log.txt";
                    target.FileName = logfile;
                    target.FileNameKind = NLog.Targets.FilePathKind.Absolute;
                    LogManager.Configuration.AddTarget(target);

                    LogManager.Configuration.AddRuleForAllLevels(target);
                    LogManager.ReconfigExistingLoggers();

                }
           
        }


        

        public class CountDown : CountDownTimer
        {
            MainActivity activity;
            public CountDown(long milliseconds, long interval,MainActivity activity) :base(milliseconds,interval)
            {
               
                this.activity = activity;
                activity.backPressed = true;
                
            }
            public override void OnFinish()
            {
                activity.backPressed = false;
            }

            public override void OnTick(long millisUntilFinished)
            {
                
            }
            
        }
        public override void OnBackPressed()
        {
            if (backPressed)
            {
                toast.Cancel();
               Finish();
            }
            else
            {
                if (navDrawer.IsDrawerOpen(GravityCompat.Start))
                {
                    navDrawer.CloseDrawer(GravityCompat.Start);
                    return;
                }
                if (fam.IsOpened)
                {
                    fam.Close(true);
                    return;
                }
                backTimer = new CountDown(1000, 10, this);
                backTimer.Start();
                
                
                toast.Show();
            }
            
        }


        public void CreateFolders()
        {
            Directory.CreateDirectory(Core.App.Settings.basePath);
            Directory.CreateDirectory(Core.App.Settings.cache);
            Directory.CreateDirectory(Core.App.Settings.cache + "pages/");
            Directory.CreateDirectory(Core.App.Settings.Log);
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
                ContentView.OrientationChanged(newConfig.Orientation);
        }

        //bg thread unhandled exception handler
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (System.Exception)e.ExceptionObject;
            logger.Fatal(ex, "Fatal Exception Thrown : " + ex.Message + System.Environment.NewLine + ex.StackTrace);
        }

        //ui thread unhandled exception handler
        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            logger.Fatal(e.Exception, "Fatal Exception Thrown : "+ e.Exception.Message + System.Environment.NewLine + e.Exception.StackTrace);
        }

        public override bool OnGenericMotionEvent(MotionEvent e)
        {
            /*if(e.Source == InputSourceType.Touchscreen)
            {
                switch (e.Action)
                {
                    case MotionEventActions.PointerIdShift:
                        if (e.GetAxisValue(Axis.Vscroll) < 0.0f)
                        {
                            fam.HideMenuButton(true);
                            SupportActionBar.Hide();
                        }
                        else
                        {
                            fam.ShowMenuButton(true);
                            SupportActionBar.Show();
                        }
                        break;
                }
            }*/
            return base.OnGenericMotionEvent(e);
        }

        public class HideOnScroll : CoordinatorLayout.Behavior
        {
            public HideOnScroll(Context context, IAttributeSet attrs) : base(context,attrs)
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
            MainActivity main;
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public FABClickListener(MainActivity main)
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
                        main.ContentView.mpageSelector.Show(((Activity)main).FragmentManager, "PageSelecter");
                        break;
                    case Resource.Id.fabRefresh:
                        //main.Refresh();
                        break;
                }
                main.fam.Close(true);
            }
        }

        

        protected override void OnDestroy()
        {
            base.OnDestroy();
            backTimer.Dispose();
            ContentView.Dispose();
            ContentView = null;
            
            
           
            
        }

        protected override void OnResume()
        {
            
            base.OnResume();
            if (SwitchedToSettings)
                SwitchedToSettings = false;
            else
                return;
            if (Core.App.Settings.Refresh)
                Core.App.Settings.Refresh = false;
            else
                return;
            try
            {
                Task.Run(async () =>
                {
                    await Task.Delay(10);
                    logger.Info("Refreshing library");
                    RunOnUiThread(() =>
                    {
                        ContentView.SetMainLoading(true);
                    });
                    if (!Core.Net.Connect())
                    {
                        RunOnUiThread(() =>
                        {
                            ContentView.SetMainLoading(false);
                            ContentView.SetError(true);
                        });
                        return;
                    }
                    ContentView.Refresh();
                    logger.Info("Refresh Done");
                });
            }
            catch(System.Exception ex)
            {
                logger.Error(ex, "\n Exception Caught In MainActivity.OnResume");
            }
        }

        private void NavView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {

            var item = e.MenuItem;
            Android.Content.Intent intent;
            switch (item.ItemId)
            {
                case Resource.Id.action_setting:
                    intent = new Android.Content.Intent(this, typeof(SettingsActivity));
                    SwitchedToSettings = true;
                    logger.Info("Settings Openned");
                    StartActivity(intent);
                    break;
                case Resource.Id.action_home:
                    navDrawer.CloseDrawers();
                    break;
            }

        }

            public bool OnQueryTextChange(string newText)
            {
                //throw new NotImplementedException();
                return true;
            }

        public bool OnQueryTextSubmit(string query)
        {
            if (query != string.Empty)
            {
                SupportActionBar.InvalidateOptionsMenu();
                SupportActionBar.Title = query;
                logger.Info("Search query submit , query ={0}", query);
                ContentView.Current_Query = query;
            }
            return false;
        }

        Android.Support.V7.Widget.SearchView searchView;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.gallerySearch, menu);
            var search = toolbar.Menu.FindItem(Resource.Id.search);
            searchView = (Android.Support.V7.Widget.SearchView)search.ActionView;
            searchView.SetOnQueryTextListener(this);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            
            return base.OnOptionsItemSelected(item);
        } 

        public void Switchtotest()
        {
            Android.Content.Intent intent = new Android.Content.Intent(this, typeof(test));
            StartActivity(intent);
        }
        
    }

    
}

