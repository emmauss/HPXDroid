using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Threading.Tasks;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NLog;
using Newtonsoft.Json;


namespace HappyPandaXDroid.Core
{
    public class Gallery
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static List<Page> DownloadList = new List<Page>();
        private static List<Page> CurrentlyDownloading = new List<Page>();
        private static bool IsDownloading = false;
        public enum ImageSize
        {
            Big = 400,
            Medium = 250,
            Original = 0,
            Small = 100
        }

        public enum ItemType
        {
            Collection = 1,
            Gallery = 0,
            GalleryFilter = 2,
            Grouping = 4,
            Page = 3
        }

        public enum ViewType
        {
            Favorite = 1,
            Inbox = 2,
            Library = 0
        }

        public class GalleryItem
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int command_id;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int taggable_id;
            public string category_id;
            public string last_read;
            public string last_updated;
            public string language_id;
            public List<Artist> artists;
            public List<Page> pages;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int timestamp;
            public string[] filters;
            public string info;
            public bool fetched;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int grouping_id;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int number;
            public bool fav;
            public int id;
            public int rating;
            public List<Profile> profiles;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int times_read;
            public List<Title> titles;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int pub_date;
            public List<URL> urls;
            public bool inbox;
            public bool singe_source;
            public string[] collections;
            public string[] circles;

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int page;
            int lastPageRead =-1;

            public int LastPageRead
            {
                get
                {
                    return lastPageRead;
                }

                set
                {
                    lastPageRead = value;
                }
            }


            public TagList tags;

            public async Task<string> Download(string size = "medium")
            {
                return await GetImage(this.id, "gallery", false, size);
            }

        }
        
        public class Page
        {
            public int number;
            public int gallery_id;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int taggable_id;
            public int id;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int last_updated;
            public bool in_archive;
            public string path;
            public string name;
            public string thumb_url;
            public string image_url;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int command_id;
            public bool IsPlaceholder = false;
            public bool MoreExists = false;

            public async Task<string> Download(string size = "original")
            {
                return await GetImage(this.id,"page", false, size);
            }

            public async void DownloadFromQueue()
            {
                await GetImage(this.id, "page",false, "original");
                RemoveFromQueue();
            }

            public void Queue()
            {
                DownloadList.Add(this);
            }

            public void RemoveFromQueue()
            {
                DownloadList.Remove(this);
            }
        }


        public class Artist
        {
            public int id;
            public string name;
        }
        public class Profile
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int id;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int timestamp;
            public string ext;
            public string size;
            public string data;
        }
        public class Title
        {
            public int id;
            public string name;
            public int language_id;
            public int gallery_id;
        }
        public class URL
        {
            public int id;
            public string url;
            public int gallery_id;
        }

        public class TagList
        {
            public List<TagItem> Artist { get; set; }
            public List<TagItem> Character { get; set; }
            public List<TagItem> Group { get; set; }
            public List<TagItem> Male { get; set; }
            public List<TagItem> Female { get; set; }
            public List<TagItem> Language { get; set; }
            public List<TagItem> __namespace__ { get; set; }
            public List<TagItem> Reclass { get; set; }
            public List<TagItem> Parody { get; set; }
        }

        public class TagNamespace
        {
            public List<TagItem> tags;
            public string name;
        }

        public class TagItem
        {
            public List<string> alias;
            public string name;
        }

        public static async Task<bool> IsSourceExist(string type, int id)
        {
            List<Tuple<string, string>> main = new List<Tuple<string, string>>();
            List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
            JSON.API.PushKey(ref main, "name", "test");
            JSON.API.PushKey(ref main, "session", App.Server.Info.session);
            JSON.API.PushKey(ref funct, "fname", "source_exists");
            JSON.API.PushKey(ref funct, "item_type", type);
            JSON.API.PushKey(ref funct, "item_id","<int>" + id);
            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            string reply = Net.SendPost(response);
            if (reply.Contains("exist") && reply.Contains("true"))
                return true;
            else
                return false;
        }

        public static void QueueDownloads(List<Page> downloadList)
        {
            foreach (var page in downloadList)
                if (DownloadList.Find((x) => x.id == page.id) == null)
                {
                    if (!IsPageCached(page, "original"))
                    DownloadList.Add(page);
                }
            Services.DownloadService.DownloadingService.StartService(null);
        }

        public static void StartQueue()
        {
            if (IsDownloading)
                return;
            IsDownloading = true;
            if(DownloadList.Count>0)
                for (int i = 0; i < DownloadList.Count; i++)
                {
                    if (!IsDownloading)
                        break;
                    while (DownloadList.Count >= 3)
                    {
                        Thread.Sleep(1000);
                        if (!IsDownloading)
                            break;
                    }
                    Gallery.Page page = DownloadList[i];
                    CurrentlyDownloading.Add(page);
                    Task.Run(() =>
                    {
                        page.DownloadFromQueue();

                    });
                    Thread.Sleep(1000);
                }
            IsDownloading = false;
            Services.DownloadService.DownloadingService.StopSelf();
        }

        public static void StopQueue()
        {
            IsDownloading = false;
        }

        public static async void InitiateImageGeneration(int[] ids, string type,string size)
        {
            await Task.Delay(10);
            try
            {
                string item_ids = "[" + String.Join(",", ids) + "]";
                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
                JSON.API.PushKey(ref main, "name", "test");
                JSON.API.PushKey(ref main, "session", App.Server.Info.session);
                JSON.API.PushKey(ref funct, "fname", "get_image");
                JSON.API.PushKey(ref funct, "item_ids", item_ids);
                JSON.API.PushKey(ref funct, "url", "<bool>true");
                JSON.API.PushKey(ref funct, "size", size);
                JSON.API.PushKey(ref funct, "item_type", type);
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);
                string reply = Net.SendPost(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "\n Exception Caught In Gallery.InitiateImageGeneration.");
            }
        }

        private static async Task<string> GetImage(int id, string type, bool return_url, string size = "medium")
        {
            try
            {

                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
                JSON.API.PushKey(ref main, "name", "test");
                JSON.API.PushKey(ref main, "session", App.Server.Info.session);
                JSON.API.PushKey(ref funct, "fname", "get_image");
                JSON.API.PushKey(ref funct, "item_ids", "[" + id + "]");
                JSON.API.PushKey(ref funct, "url", "<bool>true");
                JSON.API.PushKey(ref funct, "size", size);
                JSON.API.PushKey(ref funct, "item_type", type);
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);
                string reply = Net.SendPost(response);
                int command_id = App.Server.GetCommandId(id, reply);
                while (true)
                {
                    string state = App.Server.GetCommandState(command_id);
                    if (state.Contains("error"))
                        return "fail: server error";
                    if (state.Contains("failed"))
                        return "fail: command error";
                    if (!state.Contains("finished"))
                        Thread.Sleep(App.Settings.Loop_Delay);
                    else
                        break;
                }

                switch (size)
                {
                    case "medium":
                        type = "thumb";
                        break;
                    case "original":
                        type = "page";
                        break;
                }
                string name = App.Server.HashGenerator(size, type, id);
                //get value
                string path = App.Server.GetCommandValue(command_id, id, name, type, return_url);
                return path;

            }
            catch (System.Net.Sockets.SocketException sex)
            {
                logger.Error(sex, "\n Exception Caught In Gallery.GetImage.Message {0}\n {1}", sex.Message, sex.StackTrace);
                return "fail: server not found or connection failed error";
            }
            catch (Exception ex)
            {
                logger.Error(ex, "\n Exception Caught In Gallery.GetImage. type = {3}, itemId = {0}, size = {1},\n Message = {2}"
                     + System.Environment.NewLine + ex.StackTrace, id, size, ex.Message,type);
                return "fail: server error";
            }
        }

        public static bool IsPageCached(Core.Gallery.Page Page, string size = "original", string type = "page")
        {
            int item_id = Page.id;
            try
            {

                string page_path = Core.App.Settings.cache + type + "s/" + Core.App.Server.HashGenerator(size, type, item_id) + ".jpg";
                bool check = Core.Media.Cache.IsCached(page_path);

                return check;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex, "\n Exception Caught In GalleryCard.IsCached.");

                return false;
            }
        }

        public static string GetCachedPagePath(int id, string size = "original", string type = "page")
        {
            try
            {

                string page_path = Core.App.Settings.cache + type + "s/" + Core.App.Server.HashGenerator(size, type, id) + ".jpg";
                bool check = Core.Media.Cache.IsCached(page_path);

                return page_path;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex, "\n Exception Caught In GalleryCard.IsCached.");

                return string.Empty;
            }
        }


        public async static Task<List<GalleryItem>> GetPage(int page, string search_query =  "", int limit = 50)
        {
            List<Tuple<string, string>> main = new List<Tuple<string, string>>();
            List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
            JSON.API.PushKey(ref main, "name", "test");
            JSON.API.PushKey(ref main, "session", App.Server.Info.session);
            JSON.API.PushKey(ref funct, "fname", "library_view");
            JSON.API.PushKey(ref funct, "limit", "<int>" + limit.ToString());
            JSON.API.PushKey(ref funct, "search_query", search_query);
            JSON.API.PushKey(ref funct, "page", "<int>" + page);
            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            string countstring = Net.SendPost(response);
            var obj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(countstring);
            if (obj == null)
            {
                return new List<GalleryItem>();
            }
            var array = obj.data as Newtonsoft.Json.Linq.JArray;
            List<GalleryItem> list = new List<GalleryItem>();
            try
            {
                if (array != null & array.Count > 0)
                {
                    var data = array[0].ToObject<JSON.DataObject>();
                    var rdata = data.data as Newtonsoft.Json.Linq.JArray;

                    list = rdata.ToObject<List<GalleryItem>>();
                }
            }
            catch (Exception ex)
            {

            }
            foreach (var g in list)
                g.page = page;
            if (list.Count > 0)
            {
                int[] ids = new int[list.Count];
                for (int i = 0; i < ids.Length; i++)
                {
                    ids[i] = list[i].id;
                }
                Task.Run(() =>
                {
                    InitiateImageGeneration(ids, "gallery", "medium");
                });
            }
            return list;
        }
        
        public static async Task<int> GetCount(string query)
        {

            List<Tuple<string, string>> main = new List<Tuple<string, string>>();
            List<Tuple<string, string>> funct = new List<Tuple<string, string>>();

            JSON.API.PushKey(ref main, "name", "test");
            JSON.API.PushKey(ref main, "session", App.Server.Info.session);
            JSON.API.PushKey(ref funct, "fname", "get_view_count");
            JSON.API.PushKey(ref funct, "item_type", "Gallery");
            JSON.API.PushKey(ref funct, "search_query", query);
            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            string countstring = Net.SendPost(response);
            var serverobj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(countstring);
            var dataobj = JSON.API.GetData(serverobj.data, 0);
            var data = ((dataobj as Newtonsoft.Json.Linq.JContainer)["data"])["count"]
                .ToString();
            return int.Parse(data);
        }

        public static async Task<TagList> GetTags(int item_id, string type)
        {
            List<Tuple<string, string>> main = new List<Tuple<string, string>>();
            List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
            JSON.API.PushKey(ref main, "name", "test");
            JSON.API.PushKey(ref main, "session", App.Server.Info.session);
            JSON.API.PushKey(ref funct, "fname", "get_tags");
            JSON.API.PushKey(ref funct, "item_id", "<int>" + item_id);
            JSON.API.PushKey(ref funct, "item_type", type);
            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            string responsestring = Net.SendPost(response);
            var obj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(responsestring);
            var array = obj.data as Newtonsoft.Json.Linq.JArray;
            var taglist = new TagList();
            if (array != null)
            {
                var data = array[0].ToObject<JSON.DataObject>();
                var sett = data.data as Newtonsoft.Json.Linq.JObject;
                taglist = sett.ToObject<TagList>();
            }
            return taglist;
        }

        public async static Task<string> GetThumb(GalleryItem gallery)
        {
            int tries = 0;
            string thumb_path = string.Empty;
            while (true)
            {
                if (tries > 3)
                    break;
                
                {
                    await Task.Run(async () =>
                    {
                        thumb_path = await gallery.Download();
                    });

                    gallery = Core.App.Server.GetItem<Core.Gallery.GalleryItem>(gallery.id, "Gallery");
                }
                tries++;
            }


            bool IsCached()
            {
                int item_id = gallery.id;
                try
                {
                    thumb_path = Core.App.Settings.cache + "thumbs/" + Core.App.Server.HashGenerator("medium", "thumb",item_id) + ".jpg";
                    return Media.Cache.IsCached(thumb_path);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In Gallery.IsCached.");

                    return false;
                }

            }

            return thumb_path;

        }
    }
}