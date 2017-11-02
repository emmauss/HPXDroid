using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using NLog;
using NLog.Targets;
using NLog.Config;

namespace HPXTest
{
    class LoggingTest
    {
        static Logger logger = LogManager.GetCurrentClassLogger();
        [Test]
        public void InitLogging()
        {
             LogManager.Configuration = new LoggingConfiguration();
                    NLog.Targets.FileTarget target = new NLog.Targets.FileTarget("log");
                    string logfile = Android.OS.Environment.ExternalStorageDirectory.Path + "/HPX/"
                    + DateTime.Now.ToShortDateString().Replace("/", "-") + " - "
                        + DateTime.Now.ToShortTimeString().Replace(":", ".") + " - log.txt";
           
                    target.FileName = logfile;
                    target.FileNameKind = NLog.Targets.FilePathKind.Absolute;
                    //LogManager.Configuration = new XmlLoggingConfiguration("assets/NLog.config");
                    LogManager.Configuration.AddTarget(target);

                    LogManager.Configuration.AddRuleForAllLevels(target);

                
                LogManager.ReconfigExistingLoggers();
                Assert.True(true);
               
            
            
        }
    }
}