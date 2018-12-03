using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net;
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
using HappyPandaXDroid;
using HappyPandaXDroid.Core;
using NLog;

namespace HappyPandaXDroid.Core
{
    public class Media
    {
        
        public class Cache
        {
            private static Dictionary<string, Core.Gallery.GalleryItem> GalleryCache { get; set; }

            private static Logger logger = LogManager.GetCurrentClassLogger();

            public static bool IsClearing = false;

            public static bool IsCached(string cacheid)
            {
                
                try
                {
                    string basepath = System.IO.Path.Combine(App.Settings.CachePath, cacheid);

                    bool imagecheck = File.Exists(basepath);

                    bool metacheck = File.Exists(basepath + ".meta");
                    /*if(check)
                    using (var bit = BitmapFactory.DecodeFile(filepath, new BitmapFactory.Options()))
                    {
                        if (bit == null)
                        {
                            File.Delete(filepath);
                            return false;
                        }

                    }*/
                    return imagecheck && metacheck;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In Media.IsCached. Message : " + ex.Message + System.Environment.NewLine + ex.StackTrace);

                    return false;
                }
            }

            public static bool TryGetCachedGallery(string key, out Core.Gallery.GalleryItem gallery)
            {
                return GalleryCache.TryGetValue(key, out gallery);
            }

            public static void CacheGallery(Core.Gallery.GalleryItem gallery)
            {
                GalleryCache.TryAdd(gallery.BaseId, gallery);
            }

            public static void RemoveGallery(Core.Gallery.GalleryItem gallery)
            {
                GalleryCache.Remove(gallery.BaseId);
            }

            public static void InitializeGalleryCache()
            {
                GalleryCache = new Dictionary<string, Gallery.GalleryItem>();
            }

            public static long GetCacheSize()
            {
                Directory.CreateDirectory(App.Settings.CachePath);
                var filelist = new DirectoryInfo(App.Settings.CachePath).EnumerateFiles("*",SearchOption.AllDirectories);
                return filelist.Sum((x) => x.Length);
            }

            public static bool TryGetCachedPath(string cacheid, out string ImagePath)
            {
                ImagePath = string.Empty;
                if (!IsCached(cacheid))
                    return (false);
                else
                {
                    string basepath = System.IO.Path.Combine(App.Settings.CachePath, cacheid);

                    ImagePath = basepath;

                    return true;
                }
            }

            public static string CachePage(string cacheid, string url, bool ForceOverwrite)
            {
                string filename = string.Empty;
                if (!IsCached(cacheid) & ForceOverwrite)
                {
                    filename = System.IO.Path.Combine(App.Settings.CachePath, cacheid);

                    string meta = string.Empty;

                    string extension = url.Substring(url.LastIndexOf("."));

                    using (var client = new WebClient())
                    {
                        logger.Info("Downloading URL. URL : {0}\n, Path : {1}", url, filename);
                        client.DownloadFile(new Uri(url), filename);
                        logger.Info("Download Complete. URL : {0},\n Path : {1},\n Size: {2}", url, filename, new FileInfo(filename).Length);
                        File.WriteAllText(filename + ".meta", extension);
                    }
                }

                if (TryGetCachedPath(cacheid, out filename))
                    return filename;
                else return string.Empty;
            }

            public static void DeleteCachePage(string cacheid)
            {
                string basepath = System.IO.Path.Combine(App.Settings.CachePath, cacheid);
                if (File.Exists(basepath))
                    File.Delete(basepath);
                if (File.Exists(basepath + ".meta"))
                    File.Delete(basepath + ".meta");
            }

            public static async  Task<bool> ClearCache()
            {
                if (IsClearing)
                    return false;
                IsClearing = true;
                await Task.Delay(1000);

                var cachedir = new DirectoryInfo(App.Settings.CachePath);

                cachedir.Delete(true);

                await Task.Delay(1000);

                cachedir.Create();

                IsClearing = false;
                return true;
            }
        }

        public class Image
        {
            public string Uri;
            public bool IsUrl;
            public bool IsReady { get; set; }
            public event EventHandler<ImageLoadEvent> Ready;

            public class ImageLoadEvent : EventArgs
            {
                string Uri;
            }

            public void RequestLoad()
            {
                Ready.Invoke(null, null);
            }
        }

        public class Recents
        {
            public static List<Gallery.GalleryItem> RecentList = new List<Gallery.GalleryItem>();
            static string recentsPath = App.Settings.BasePath + "recents";

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

        public class QuickSearch
        {
            public static List<string> Searches = new List<string>();
            static string recentsPath = App.Settings.BasePath + "searches";

            public static void LoadSearches()
            {
                if (File.Exists(recentsPath))
                {
                    try
                    {
                        string rec = File.ReadAllText(recentsPath);
                        var list = JSON.Serializer.SimpleSerializer.Deserialize<List<string>>(rec);
                        if (list != null)
                        {
                            Searches.Clear();
                            Searches.AddRange(list);
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

            public static void AddToQuickSearch(string searchquery)
            {
                var item = Searches.Find((x) => x == searchquery);
                if (item != null)
                {
                    Searches.Remove(item);
                }
                Searches.Insert(0, searchquery);
                SaveQuickSearch();
            }

            public static void RemoveFromQuicSearch(string searchquery)
            {
                var item = Searches.Find((x) => x == searchquery);
                if (item != null)
                {
                    Searches.Remove(item);
                }
                SaveQuickSearch();
            }

            public static void ClearQuickSearch()
            {
                Searches.Clear();
                SaveQuickSearch();
            }

            public static void SaveQuickSearch()
            {
                lock (Searches)
                {
                    string rec = JSON.Serializer.SimpleSerializer.Serialize(Searches);
                    File.WriteAllText(recentsPath, rec);
                }
            }
        }

    }
}