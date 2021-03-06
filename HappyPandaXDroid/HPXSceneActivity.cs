﻿using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Com.Hippo.Stage;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Util;
using System.Threading.Tasks;
using Android.Support.V4.View;
using Android.Support.V4.View.Animation;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using NLog;
using Android.Content.Res;
using Com.Hippo.Stage.Curtain;

namespace HappyPandaXDroid
{

    [Activity(Label = "HPXSceneActivity" , ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation 
        | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class HPXSceneActivity : AppCompatActivity, ICurtainSuppler
    {
        Director director;
        Stage stage;
        public DrawerLayout navDrawer;
        StageLayout stageLayout;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Curtains GetCurtain(SceneInfo p0, IList<SceneInfo> p1)
        {
            if(p1!=null)
            if (p1.Count != 0)
            {
                    var curtain = new FadeCurtain();
                    curtain.SetDuration(150);
                    curtain.SetInterpolator(new FastOutSlowInInterpolator());
                    return curtain;
            }
            return NoOpCurtain.Instance;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.HPXStage);
            AppCompatDelegate.CompatVectorFromResourcesEnabled = true;
            // Create your application here
            director = Director.Hire(this, savedInstanceState);
            director.SetCurtainSuppler(this);
            InitializeViews();
            bool IsInitialized = director.Contains(stageLayout.Id);
            stage = director.Direct(stageLayout);

            AppCompatDelegate.CompatVectorFromResourcesEnabled = true;
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            

            if (!IsInitialized)
            {
                stage.PushScene(new Scenes.LibraryScene(string.Empty,string.Empty));
            }


        }

        void InitializeViews()
        {
            navDrawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            stageLayout = FindViewById<StageLayout>(Resource.Id.stage_layout);
            var navView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navView.NavigationItemSelected += NavView_NavigationItemSelected;
            var navToggle = new ActionBarDrawerToggle
               (this, navDrawer, Resource.String.open_drawer, Resource.String.close_drawer);
            navDrawer.AddDrawerListener(navToggle);
            navToggle.SyncState();
        }

        //bg thread unhandled exception handler
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (System.Exception)e.ExceptionObject;
            logger.Fatal(ex, ex.Message);
        }

        //ui thread unhandled exception handler
        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            logger.Fatal(e.Exception, "Fatal Exception Thrown : " + e.Exception.Message);
        }

        public override void OnBackPressed()
        {

            if (navDrawer.IsDrawerOpen(GravityCompat.Start))
            {
                navDrawer.CloseDrawer(GravityCompat.Start);
                return;
            }
            if (navDrawer.IsDrawerOpen(GravityCompat.End))
            {
                navDrawer.CloseDrawer(GravityCompat.End);
                return;
            }
            if (!director.HandleBack() && !director.FocusedStage.TopScene.HandleBack())
            base.OnBackPressed();


        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
        }

        private void NavView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {

            var item = e.MenuItem;
            Android.Content.Intent intent;
            var scene = stage.TopScene;
            switch (item.ItemId)
            {
                case Resource.Id.action_setting:
                    intent = new Android.Content.Intent(this, typeof(SettingsActivity));
                    logger.Info("Settings Opened");
                    StartActivity(intent);
                    break;
                case Resource.Id.action_home:
                    if (!(scene is Scenes.ViewScene)) { 
                        Scenes.ViewScene library = new Scenes.LibraryScene("", "");
                        stage.PushScene(library);
                    }
                    break;
                case Resource.Id.action_recent:
                    if (!(scene is Scenes.RecentsScene))
                    {
                        Scene recents = new Scenes.RecentsScene();
                        stage.PushScene(recents);
                    }
                    break;

            }
            navDrawer.CloseDrawers();

        }

        protected override void OnResume()
        {
            base.OnResume();
            if (Core.App.Settings.Refresh)
            {
                Core.App.Settings.Refresh = false;

                try
                {
                    Task.Run(async () =>
                    {
                        await Task.Delay(10);
                        Core.Net.Connect();
                    },new System.Threading.CancellationToken());
                }
                catch (System.Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In MainActivity.OnResume");
                }
            }
            else if (!Core.Net.Connect())
            {
                RunOnUiThread(() =>
                {
                });
            }


        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            var scene = (Scenes.HPXScene)stage.TopScene;
            scene.OnConfigurationChanged(newConfig);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            stage.Close();
            director.SetCurtainSuppler(null);
        }

    }
}