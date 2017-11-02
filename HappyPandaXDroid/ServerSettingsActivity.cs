using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Support.V7.App;
using Android.Views;
using NLog;
using NLog.Config;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using Toolbar = Android.Support.V7.Widget.Toolbar;
namespace HappyPandaXDroid
{
    [Activity(Label = "ServerSettingsActivity", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class ServerSettingsActivity : AppCompatActivity
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.server_settings);
            Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            var newFragment = new SettingsFragments();
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

        public class SettingsFragments : PreferenceFragment,
            ISharedPreferencesOnSharedPreferenceChangeListener
        {
            
            private static Logger logger = LogManager.GetCurrentClassLogger();
            //Core.App.Settings set = new Core.App.Settings();
            ISharedPreferences sharedPreferences;
            ISharedPreferencesOnSharedPreferenceChangeListener listener;
            Preference pref;
            Stopwatch watch = new Stopwatch();
            public override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);
                // Create your application here
                if (Core.Net.Connected)
                {
                    var set = Core.App.Settings.Server.GetSettings();
                    set.Parse();
                    watch.Start();
                }
                AddPreferencesFromResource(Resource.Xml.server_settings);
                sharedPreferences = PreferenceScreen.SharedPreferences;
                for (int i = 0; i < PreferenceScreen.PreferenceCount; i++)
                {
                    SetSummary(PreferenceScreen.GetPreference(i));
                }
                sharedPreferences.RegisterOnSharedPreferenceChangeListener(this);
                

            }

            public override void OnResume()
            {

                base.OnResume();
                sharedPreferences = PreferenceScreen.SharedPreferences;
                for (int i = 0; i < PreferenceScreen.PreferenceCount; i++)
                {
                    SetSummary(PreferenceScreen.GetPreference(i));
                }

                sharedPreferences.RegisterOnSharedPreferenceChangeListener(this);

            }



            private void SetSummary(Preference pref)
            {
                if (pref is EditTextPreference)
                {
                    UpdateSummary((EditTextPreference)pref);
                }
                else if (pref is ListPreference)
                {
                    UpdateSummary((ListPreference)pref);
                }
                else if (pref is MultiSelectListPreference)
                {
                    UpdateSummary((MultiSelectListPreference)pref);
                }
            }

            private void UpdateSummary(MultiSelectListPreference pref)
            {
                //pref.setSummary(Arrays.toString(pref.getValues().toArray()));
            }

            private void UpdateSummary(ListPreference pref)
            {
                pref.Summary = pref.Value;
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
                    try
                    {
                        var edit = sharedPreferences.Edit();
                        edit.PutString(key, editp.Text);
                        editp.Summary = editp.Text;
                        edit.Commit();
                    }
                    catch(Exception ex)
                    {

                    }
                    if (key == "server_ip" || key == "server_port" || key == "webclient_port")
                    {
                        Core.App.Settings.Refresh = true;
                    }
                    else if (watch.ElapsedMilliseconds>2000 && Core.Net.Connected)
                    {
                        watch.Stop();
                        string v = editp.Text;                        
                        if(editp.EditText.InputType!= Android.Text.InputTypes.ClassNumber)
                        {
                            v = "'" + v + "'";
                        }
                        var bol = Core.App.Settings.Server.SetConfig( key, v);
                    }
                }
                else if (pref is TwoStatePreference check)
                {
                    if (watch.ElapsedMilliseconds > 2000 && Core.Net.Connected)
                    {
                        var edit = sharedPreferences.Edit();
                    edit.PutBoolean(key, check.Checked);
                    edit.Commit();
                    
                        watch.Stop();
                        bool c = check.Checked;
                        string v = c.ToString().ToLower();
                        var bol = Core.App.Settings.Server.SetConfig(key, v);
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