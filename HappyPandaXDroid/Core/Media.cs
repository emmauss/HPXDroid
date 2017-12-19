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

        public class Recents
        {
            public static List<Gallery.GalleryItem> RecentList = new List<Gallery.GalleryItem>();
            static string recentsPath = App.Settings.basePath + "recents";

            public static void LoadRecents()
            {
                if (File.Exists(recentsPath))
                {
                    try
                    {
                        string rec = File.ReadAllText(recentsPath);
                        var list = JSON.Serializer.SimpleSerializer.Deserialize<List<Gallery.GalleryItem>>(rec);
                        if (list != null)
                        {
                            RecentList.Clear();
                            RecentList.AddRange(list);
                        }
                    }
                    catch (Exception ex)
                    {
                        File.Delete(recentsPath);
                        File.Create(recentsPath).Close();
                    }
                }
                else
                    File.Create(recentsPath).Close();
            }

            public static void AddToRecents(Gallery.GalleryItem galleryItem)
            {
                var item = RecentList.Find((x) => x.id == galleryItem.id);
                if (item != null)
                {
                    RecentList.Remove(item);
                }
                RecentList.Insert(0, galleryItem);
                SaveRecents();
            }

            public static void RemoveFromRecents(Gallery.GalleryItem galleryItem)
            {
                var item = RecentList.Find((x) => x.id == galleryItem.id);
                if (item != null)
                {
                    RecentList.Remove(item);
                }
                SaveRecents();
            }

            public static void ClearRecents()
            {
                RecentList.Clear();
                SaveRecents();
            }

            public static void SaveRecents()
            {
                lock (RecentList)
                {
                    string rec = JSON.Serializer.SimpleSerializer.Serialize(RecentList);
                    File.WriteAllText(recentsPath, rec);
                }
            }

            public static Gallery.GalleryItem GetRecentGallery(int galleryId)
            {
                var gallery = RecentList.Find((x) => x.id == galleryId);
                if (gallery != null)
                {
                    return gallery;
                }
                else
                    return null;
            }
        }
       
    }
}