using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.Preferences;
using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using System.IO;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.View;
using NLog;
using NLog.Config;
using Toolbar = Android.Support.V7.Widget.Toolbar;


namespace HappyPandaXDroid
{
    
    public class SettingsFragment : PreferenceFragment, ISharedPreferencesOnSharedPreferenceChangeListener
    { 
        ISharedPreferences sharedPreferences;
        ISharedPreferencesOnSharedPreferenceChangeListener listener;
        Preference pref;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            AddPreferencesFromResource(Resource.Xml.preferences);
            sharedPreferences = Application.Context.GetSharedPreferences
                ("settings", FileCreationMode.Private);
            //listener = new changelistener(this);

        }
        public override void OnResume()
        {

            base.OnResume();
            

            sharedPreferences.RegisterOnSharedPreferenceChangeListener(this);
            
         }

        public void updateSummary(EditTextPreference pref)
        {

        }

        public override void OnPause()
        {
            sharedPreferences.UnregisterOnSharedPreferenceChangeListener(this);
            base.OnPause();
        }



        public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
        {
            Preference pref = FindPreference(key);

            
            if (pref is EditTextPreference)
            {
                EditTextPreference editp = (EditTextPreference)pref;
                editp.Summary = editp.Text;
            }
            else if(pref is CheckBoxPreference)
            {
                var check = (CheckBoxPreference)pref;
                if (key == "enable_debug")
                {
                    switch (check.Checked)
                    {
                        case true:
                            NLog.Targets.FileTarget target = new NLog.Targets.FileTarget("log");
                            if (!Directory.Exists(Core.App.Settings.Log))
                            {
                                Directory.CreateDirectory(Core.App.Settings.Log);
                            }
                            string logfile = Core.App.Settings.Log + DateTime.Now.ToShortDateString().Replace("/", "-") + " - "
                                + DateTime.Now.ToShortTimeString().Replace(":", ".") + " - log.txt";
                            target.FileName = logfile;
                            target.FileNameKind = NLog.Targets.FilePathKind.Absolute;
                            LogManager.Configuration = new XmlLoggingConfiguration("assets/NLog.config");
                            LogManager.Configuration.AddTarget(target);

                            LogManager.Configuration.AddRuleForAllLevels(target, "*");
                            LogManager.ReconfigExistingLoggers();
                            break;
                        case false:
                            LogManager.Configuration = null;
                            break;
                    }
                }
            }
        }
        

    }
}