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
    [Activity(Label = "HPXDroid", MainLauncher = true, Theme = "@style/MyTheme.Splash", NoHistory =true,
        ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation 
        | Android.Content.PM.ConfigChanges.ScreenSize , ScreenOrientation =Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //public List<string> lists = new List<string>();
        //ArrayAdapter<string> adapter;
        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            //set unhandled exception handler
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            
            await Load();

            var intent = new Intent();
            if(Core.App.Settings.IsFirstRun)
            {
                intent = new Intent(this, typeof(WelcomeActivity));
                StartActivity(intent);
                return;
            }
            intent = new Intent(this, typeof(HPXSceneActivity));
            intent.PutExtra("connected", Core.Net.Connected);
            intent.PutExtra("query", string.Empty);
            StartActivity(intent);
            
        }

        async Task<bool> Load()
        {
            
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
            Task.Run(() =>
            {
                InitLogging();
                if (!File.Exists(Core.App.Settings.basePath + ".nomedia"))
                    File.Create(Core.App.Settings.basePath + ".nomedia");
                Task.Run(() => Core.Media.Recents.LoadRecents());
                Task.Run(() => Core.Media.QuickSearch.LoadSearches());
            });
            if(!Core.App.Settings.IsFirstRun)
                if (Core.Net.IsServerReachable())
                {
                    Core.Net.Connect();
                    
                }
            CreateFolders();


            
            
            return true;
        }

        public override void OnBackPressed()
        {
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

        public void CreateFolders()
        {
            Directory.CreateDirectory(Core.App.Settings.basePath);
            Directory.CreateDirectory(Core.App.Settings.cache);
            Directory.CreateDirectory(Core.App.Settings.cache + "pages/");
            Directory.CreateDirectory(Core.App.Settings.Log);
        }
        
        //bg thread unhandled exception handler
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (System.Exception)e.ExceptionObject;
            logger.Fatal(ex, "Fatal Exception Thrown : " + ex.Message + System.Environment.NewLine + ex.StackTrace);
            Process.KillProcess(Process.MyPid());
        }

        //ui thread unhandled exception handler
        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            logger.Fatal(e.Exception, "Fatal Exception Thrown : "+ e.Exception.Message + System.Environment.NewLine + e.Exception.StackTrace);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Finish();
        }

    }

    
}

