using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
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
    [Activity(Label = "HPXDroid", MainLauncher = true, NoHistory =true,
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

            AppCompatDelegate.CompatVectorFromResourcesEnabled = true;
            SetContentView(Resource.Layout.Main);
            
            await Load();
            var intent = new Intent();
            if (!Core.Net.Connected)
            {
                intent = new Intent(this, typeof(WelcomeActivity));
                StartActivity(intent);
            }
            else
            {
                
                if (Core.App.Settings.IsFirstRun)
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
                if (!File.Exists(Core.App.Settings.BasePath + ".nomedia"))
                    File.Create(Core.App.Settings.BasePath + ".nomedia");
                Task.Run(() => Core.Media.Recents.LoadRecents());
                Task.Run(() => Core.Media.QuickSearch.LoadSearches());
            });
            if(!Core.App.Settings.IsFirstRun)
                if (Core.Net.IsServerReachable())
                {
                    Core.Net.Connect();
                    
                }
            //RequestPermissions();
            return true;
        }

        public override void OnBackPressed()
        {
        }

        public static void InitLogging()
        {
            LogManager.Configuration = new LoggingConfiguration();
            NLog.Targets.FileTarget target = new NLog.Targets.FileTarget("log");
            var now = DateTime.Now;
            string logfile = Path.Combine(Core.App.Settings.Log, $"{now.Year} - {now.Month} - {now.Day} -- {now.ToShortTimeString()} - log.txt".Replace(":","."));
         /*   string logfile = Core.App.Settings.Log + DateTime.Now.ToShortDateString().Replace("/", "-") + " - "
                + DateTime.Now.ToShortTimeString().Replace(":", ".") + " - log.txt";*/
            target.FileName = logfile;
            target.FileNameKind = NLog.Targets.FilePathKind.Absolute;
            LogManager.Configuration.AddTarget(target);

            LogManager.Configuration.AddRuleForAllLevels(target);
            LogManager.ReconfigExistingLoggers();
        }

        public void CreateFolders()
        {
            Directory.CreateDirectory(Core.App.Settings.BasePath);
            Directory.CreateDirectory(Core.App.Settings.CachePath);
            Directory.CreateDirectory(Core.App.Settings.CachePath + "pages/");
            Directory.CreateDirectory(Core.App.Settings.Log);
        }

        public void RequestPermissions()
        {
            if(Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                if (!CheckPermission(Manifest.Permission.ReadExternalStorage) || !CheckPermission(Manifest.Permission.WriteExternalStorage))
                {
                    RequestPermissions(new string[] { Manifest.Permission.ReadExternalStorage,
                                         Manifest.Permission.WriteExternalStorage}, 0);
                }
                else
                    CreateFolders();
            }


            bool CheckPermission(string permission)
            {
                return CheckSelfPermission(permission) == Android.Content.PM.Permission.Granted;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if(grantResults[0] == Permission.Granted)
            {
                CreateFolders();
            }
        }

        //bg thread unhandled exception handler
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (System.Exception)e.ExceptionObject;

            SaveCrashDetails(ex);

            logger.Fatal(ex, "Fatal Exception Thrown : " + ex.Message + System.Environment.NewLine + ex.StackTrace);
            Process.KillProcess(Process.MyPid());
        }

        //ui thread unhandled exception handler
        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            SaveCrashDetails(e.Exception);

            logger.Fatal(e.Exception, "Fatal Exception Thrown : "+ e.Exception.Message + System.Environment.NewLine + e.Exception.StackTrace);
        }

        public void SaveCrashDetails(Exception ex)
        {
            Directory.CreateDirectory(Core.App.Settings.BasePath);

            var crashPath = Path.Combine(Core.App.Settings.BasePath, "crash.txt");

            string crashLog = ex.Message;
            crashLog += "\n" + ex.StackTrace;

            File.WriteAllText(crashPath, crashLog);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Finish();
        }

    }

    
}