using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;

using Com.Bumptech.Glide;
using Emmaus.Widget;
using NLog;

namespace HappyPandaXDroid.Core
{
    public class Media
    {
        
        public class Cache
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public static bool IsCached(string filepath)
            {
                
                try
                {
                    bool check = File.Exists(filepath);
                    /*if(check)
                    using (var bit = BitmapFactory.DecodeFile(filepath, new BitmapFactory.Options()))
                    {
                        if (bit == null)
                        {
                            File.Delete(filepath);
                            return false;
                        }

                    }*/
                    return check;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In Media.IsCached. Message : " + ex.Message + System.Environment.NewLine + ex.StackTrace);

                    return false;
                }
            }
            public static long GetCacheSize()
            {
                Directory.CreateDirectory(App.Settings.cache);
                var filelist = new DirectoryInfo(App.Settings.cache).EnumerateFiles("*",SearchOption.AllDirectories);
                return filelist.Sum((x) => x.Length);

            }

            public static async  Task<bool> ClearCache()
            {
                await Task.Delay(10);
                var dirlist = new DirectoryInfo(App.Settings.cache).EnumerateDirectories("*",SearchOption.AllDirectories);
                foreach(var dir in dirlist)
                {
                    dir.Delete(true);
                }
                var filelist = new DirectoryInfo(App.Settings.cache).EnumerateFiles("*", SearchOption.AllDirectories);
                foreach (var file in filelist)
                {
                    file.Delete();
                }
                return true;
            }
        }
       
    }
}