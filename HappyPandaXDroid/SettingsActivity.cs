using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Support.V7.App;
using Android.Views;
using Java;
using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace HappyPandaXDroid
{
    [Activity(Label = "Settings", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class SettingsActivity : AppCompatActivity
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.settings);
            
            Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            var newFragment = new SettingsFragments();
            newFragment.SetParent(this);
            var ft = FragmentManager.BeginTransaction();
            ft.Add(Resource.Id.fragment_container, newFragment);
            ft.Commit();

            logger.Info("Settings Loaded");
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public  class SettingsFragments : PreferenceFragment, 
            ISharedPreferencesOnSharedPreferenceChangeListener
        {
            string[] sizeCollection;

            CustomViews.OptionDialogPreference cachedialog;
            private static Logger logger = LogManager.GetCurrentClassLogger();
            //Core.App.Settings set = new Core.App.Settings();
            ISharedPreferences sharedPreferences;
            
            Preference server;

            AppCompatActivity parent;

            

            public override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);
                // Create your application here
                AddPreferencesFromResource(Resource.Xml.preferences);
                sizeCollection = Enum.GetNames(typeof(Core.Gallery.ImageSize));
                sharedPreferences = PreferenceScreen.SharedPreferences;
                for (int i = 0; i < PreferenceScreen.PreferenceCount; i++)
                {
                    SetSummary(PreferenceScreen.GetPreference(i));
                }

                PreferenceManager.SetDefaultValues(this.Context, Resource.Xml.preferences, false);

                server = FindPreference("server_section");

                sharedPreferences.RegisterOnSharedPreferenceChangeListener(this);
                cachedialog = (CustomViews.OptionDialogPreference)FindPreference("cachedialog");
                var sizePreference = (ListPreference)FindPreference("image_size");

                sizePreference.SetEntries(sizeCollection);
                sizePreference.SetEntryValues(sizeCollection);
                sizePreference.Summary = Core.App.Settings.ImageSize.ToString();
                sizePreference.SetValueIndex(sizePreference.FindIndexOfValue(Core.App.Settings.ImageSize.ToString()));
                Task.Run(async () =>
                {
                    var h = new Handler(Looper.MainLooper);
                    h.Post(() =>
                    {

                        cachedialog.Title = "Local Cache";
                        cachedialog.Summary = "Caculating Total cache size";
                    });

                    var size = await Core.Media.Cache.GetCacheSize();

                    double cacheSize = Math.Round((double)size / (1024 * 1024), 2);
                    
                    h.Post(() =>
                    {
                        
                        cachedialog.Title = "Local Cache";
                        cachedialog.Summary = cacheSize.ToString() + " MB";
                    });
                });
                cachedialog.OnPositiveClick += Cachedialog_OnPositiveClick;
                server.OnPreferenceClickListener = new Clistener();
            }

            public void SetParent(AppCompatActivity app)
            {
                parent = app;
            }

            private void Server_PreferenceClick(object sender, Preference.PreferenceClickEventArgs e)
            {
                Android.Content.Intent intent = new Intent(parent, typeof(ServerSettingsActivity));

                parent.StartActivity(intent);
            }

            public class Clistener : Java.Lang.Object, Preference.IOnPreferenceClickListener
            {
                public bool OnPreferenceClick(Preference preference)
                {
                    Android.Content.Intent intent = new Intent(preference.Context, typeof(ServerSettingsActivity));

                     preference.Context.StartActivity(intent);
                    return true;
                }
            }

            private async void Cachedialog_OnPositiveClick(object sender, EventArgs e)
            {
                var h = new Handler(Looper.MainLooper);
                h.Post(() =>
                {
                    cachedialog.Summary = "Clearing Cache";
                });
                await Task.Run(async () =>
                {
                    if (await Core.Media.Cache.ClearCache())
                    {
                        string size = Math.Round((double)( await Core.Media.Cache.GetCacheSize()) / (1024 * 1024), 2).ToString() + " MB";
                        
                        h.Post(() =>
                        {
                            cachedialog.Summary = size;
                        });
                    }
                    else
                        h.Post(() =>
                        {
                            cachedialog.Summary = "Cache Clearing already in progress";
                        });
                });
            }

            public override void OnResume()
            {

                base.OnResume();
                sharedPreferences = PreferenceScreen.SharedPreferences;
                for (int i = 0; i < PreferenceScreen.PreferenceCount; i++)
                {
                    SetSummary(PreferenceScreen.GetPreference(i));
                }
                server.PreferenceClick += Server_PreferenceClick;
                sharedPreferences.RegisterOnSharedPreferenceChangeListener(this);

            }

            

            private void SetSummary(Preference pref)
            {
                if (pref is EditTextPreference) {
                    UpdateSummary((EditTextPreference)pref);
                } else if (pref is ListPreference) {
                    UpdateSummary((ListPreference)pref);
                } else if (pref is MultiSelectListPreference) {
                    UpdateSummary((MultiSelectListPreference)pref);
                }
            }

            private void UpdateSummary(MultiSelectListPreference pref)
            {
                //pref.setSummary(Arrays.toString(pref.getValues().toArray()));
            }

            private void UpdateSummary(ListPreference pref)
            {
                pref.Summary=pref.Value;
            }

            private void UpdateSummary(EditTextPreference preference)
            {
                preference.Summary = preference.Text;
            }
            public override void OnPause()
            {
                sharedPreferences = PreferenceScreen.SharedPreferences;
                sharedPreferences.UnregisterOnSharedPreferenceChangeListener(this);
                base.OnPause();
            }

            public override void OnDestroy()
            {
                
                base.OnDestroy();
            }

            public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
            {
                Preference pref = FindPreference(key);
                logger.Info("Change Preference : {0}", key);
                if (pref is EditTextPreference editp)
                {
                    var edit = sharedPreferences.Edit();
                    edit.PutString(key, editp.Text);
                    editp.Summary = editp.Text;
                    edit.Commit();
                }
                else if (pref is TwoStatePreference check)
                {
                    var edit = sharedPreferences.Edit();
                    edit.PutBoolean(key, check.Checked);

                    edit.Commit();

                    if (key == "enable_debugging")
                    {
                        switch (check.Checked)
                        {
                            case true:
                                    if (!Directory.Exists(Core.App.Settings.Log))
                                    {
                                        Directory.CreateDirectory(Core.App.Settings.Log);
                                    }
                                    LogManager.Configuration = new LoggingConfiguration();
                                    NLog.Targets.FileTarget target = new NLog.Targets.FileTarget("log");
                                    string logfile = Core.App.Settings.Log + DateTime.Now.ToShortDateString().Replace("/", "-") + " - "
                                        + DateTime.Now.ToShortTimeString().Replace(":", ".") + " - log.txt";
                                    target.FileName = logfile;
                                    target.FileNameKind = NLog.Targets.FilePathKind.Absolute;
                                    LogManager.Configuration.AddTarget(target);

                                    LogManager.Configuration.AddRuleForAllLevels(target);
                                    LogManager.ReconfigExistingLoggers();
                                
                                break;
                            case false:
                                LogManager.Configuration = null;
                                break;
                        }
                    }
                }
                else if (pref is ListPreference listp)
                {
                    var edit = sharedPreferences.Edit();
                    edit.PutString(key, listp.Entry);
                    listp.Summary = listp.Entry;
                    edit.Commit();
                }
            }


        }

    }
}