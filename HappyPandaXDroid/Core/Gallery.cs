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
        public static CancellationTokenSource DownloadCancelationToken = new CancellationTokenSource();
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
            Artist = 7,
            Category = 8,
            Circle = 11,
            Collection = 2,
            Gallery = 1,
            GalleryFilter = 3,
            Grouping = 5,
            Language = 9,
            Page = 4,
            Parody = 13,
            Status = 10,
            Title = 6,
            Url = 12
        }

        public enum ViewType
        {
            Favorite = 2,
            Inbox = 3,
            Library = 1,
            ReadLater = 5,
            Trash = 4,
        }

        public enum Sort
        {
            None = 0,
            ArtistName = 20,
            CollectionDate = 52,
            CollectionGalleryCount = 54,
            CollectionName = 51,
            CollectionPublished = 53,
            CollectionRandom = 50,
            GalleryArtist = 3,
            GalleryDate = 4,
            GalleryPageCount = 10,
            GalleryPublished = 5,
            GalleryRandom = 1,
            GalleryRating = 8,
            GalleryRead = 6,
            GalleryReadCount = 9,
            GalleryTitle = 2,
            GalleryUpdated = 7,
            NamespaceTagNamespace = 30,
            NamespaceTagTag = 31,
            ParodyName = 45,
            CircleName = 40
        }

        public class MetaTag
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            bool favorite;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            bool inbox;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            bool readlater;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            bool trash;
        }

        public abstract class HPXItem
        {
            public GalleryItem[] galleries;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int id;
            public string name;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int last_updated;
            public string info;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int pub_date;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int category_id;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int timestamp;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int user_id;
            
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int page;
        }

        public class Collection : HPXItem
        {
            public async Task<string> Download(CancellationToken cancellationToken, string size = "medium")
            {
                return await GetImage(id, ItemType.Collection, false, cancellationToken, size);
            }
        }

        public class GalleryItem : HPXItem
        {
            public List<Collection> collections;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int command_id;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int taggable_id;
            public string last_read;
            public string language_id;
            public List<Artist> artists;
            public List<Page> pages;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<string> filters;
            public string info;
            public bool fetched;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int grouping_id;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int number;
            public bool fav;
            public int rating;
            public List<Profile> profiles;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int times_read;
            public List<Title> titles;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<Url> urls;
            public bool inbox;
            public bool singe_source;
            //public List<string> parodies;
            public List<Circle> circles;
            public bool single_source;
            public MetaTag metaTags;
            public Title preferred_title;

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

            public async Task<string> Download(CancellationToken cancellationToken,string size = "medium")
            {
                return await GetImage(this.id, ItemType.Gallery, false, cancellationToken,size);
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
                return await GetImage(this.id, ItemType.Page, false, DownloadCancelationToken.Token,size);
            }

            public async void DownloadFromQueue()
            {
                await GetImage(this.id, ItemType.Page,false, DownloadCancelationToken.Token,"original");
                RemoveFromQueue();
            }

            public void Queue()
            {
                DownloadList.Add(this);
            }

            public void RemoveFromQueue()
            {
                DownloadList.Remove(this);
                CurrentlyDownloading.Remove(this);
            }
        }

        public class Circle
        {
            public List<Artist> artists;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int id;
            public string name;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int user_id;
        }

        public class Name
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int id;
            public string name;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int language_id;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int alias_for_id;
            public List<Artist> artists;
        } 

        public class Artist
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int id;
            public string name;
            public List<GalleryItem> galleries;
            public List<Circle> circles;
            public string info;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int user_id;
            public MetaTag metatags;
            public List<Name> Names;
            public List<HPXUrl> urls;
            public Name preferred_name;           
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
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int id;
            public string name;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int language_id;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int gallery_id;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int alias_for_id;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int user_id;
            public List<Title> aliases;       
        }

        public class HPXUrl
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int id;
            public string name;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int user_id;
            public List<Artist> artists;
            public List<GalleryItem> galleries;
    }

        public class Url
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
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

        public static async Task<bool> IsSourceExist(ItemType itemType, int id,CancellationToken cancellationToken)
        {
            List<Tuple<string, string>> main = new List<Tuple<string, string>>();
            List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
            JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest? "guest" : Core.App.Settings.Username);
            JSON.API.PushKey(ref main, "session", App.Server.Info.session);
            JSON.API.PushKey(ref funct, "fname", "source_exists");
            JSON.API.PushKey(ref funct, "item_type", itemType.ToString());
            JSON.API.PushKey(ref funct, "item_id","<int>" + id);
            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            string reply = Net.SendPost(response,ref cancellationToken);
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
            StartQueue();
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
                    while (CurrentlyDownloading.Count >= 3)
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

            //Toast.MakeText(Application.Context, "Precaching completed or stopped", ToastLength.Short).Show();
        }

        public static void StopQueue()
        {
            IsDownloading = false;
        }

        public static async void InitiateImageGeneration(int[] ids, ItemType itemType,string size,CancellationToken cancellationToken)
        {
            await Task.Delay(10);
            try
            {
                string item_ids = "[" + String.Join(",", ids) + "]";
                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
                JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest? "guest" : Core.App.Settings.Username);
                JSON.API.PushKey(ref main, "session", App.Server.Info.session);
                JSON.API.PushKey(ref funct, "fname", "get_image");
                JSON.API.PushKey(ref funct, "item_ids", item_ids);
                JSON.API.PushKey(ref funct, "url", "<bool>true");
                JSON.API.PushKey(ref funct, "size", size);
                JSON.API.PushKey(ref funct, "item_type", itemType.ToString());
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);
                string reply = Net.SendPost(response,ref cancellationToken);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "\n Exception Caught In Gallery.InitiateImageGeneration.");
            }
        }

        private static async Task<string> GetImage(int id, ItemType itemType, bool return_url, CancellationToken cancellationToken, string size = "medium")
        {
            string type = string.Empty;
            try
            {
                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
                JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest? "guest" : Core.App.Settings.Username);
                JSON.API.PushKey(ref main, "session", App.Server.Info.session);
                JSON.API.PushKey(ref funct, "fname", "get_image");
                JSON.API.PushKey(ref funct, "item_ids", "[" + id + "]");
                JSON.API.PushKey(ref funct, "url", "<bool>true");
                JSON.API.PushKey(ref funct, "size", size);
                JSON.API.PushKey(ref funct, "item_type", itemType.ToString());
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);
                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;
                string reply = Net.SendPost(response,ref cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;
                int command_id = App.Server.GetCommandId(id, reply);
                while (true)
                {
                    string state = App.Server.GetCommandState(command_id,ref cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                        return string.Empty;
                    if (state.Contains("error"))
                        return "fail: server error";
                    if (state.Contains("failed"))
                        return "fail: command error";
                    if (!state.Contains("finished"))
                        Thread.Sleep(App.Settings.Loop_Delay);
                    else
                        break;
                }

                if (itemType == ItemType.Collection)
                    type = "Collection";
                else
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
                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;
                //get value
                string path = App.Server.GetCommandValue(command_id, id, name, type, return_url,ref cancellationToken);
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

        public static string GetCachedPagePath(int id, ItemType itemType,string size = "original")
        {
            try
            {
                string type = "page";
                if (itemType == ItemType.Collection)
                    type = "Collection";
                else
                    switch (size)
                    {
                        case "medium":
                            type = "thumb";
                            break;
                        case "original":
                            type = "page";
                            break;
                    }

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


        public async static Task<List<HPXItem>> GetPage(ItemType itemType, int page, CancellationToken cancellationToken, 
            ViewType viewType = ViewType.Library, Sort sortCriteria = (Sort)1, bool sortDec = false,
            string searchQuery = "", int limit = 50)
        {
            string sort = sortCriteria.ToString().ToLower();
            if (sortCriteria == Sort.None)
            {
                sort = "null";
            }
            List<Tuple<string, string>> main = new List<Tuple<string, string>>();
            List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
            JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest ? "guest" : Core.App.Settings.Username);
            JSON.API.PushKey(ref main, "session", App.Server.Info.session);
            JSON.API.PushKey(ref funct, "fname", "library_view");
            JSON.API.PushKey(ref funct, "item_type", itemType.ToString());
            JSON.API.PushKey(ref funct, "view_filter", viewType.ToString());
            JSON.API.PushKey(ref funct, "limit", "<int>" + limit.ToString());
            JSON.API.PushKey(ref funct, "search_query", searchQuery);
            JSON.API.PushKey(ref funct, "page", "<int>" + page);
            JSON.API.PushKey(ref funct, "sort_by", (sort == "null" ? "<int>" : "") + sort);
            JSON.API.PushKey(ref funct, "sort_desc", "<bool>" + sortDec.ToString().ToLower());

            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            if (cancellationToken.IsCancellationRequested)
                return null;
            string countstring = Net.SendPost(response, ref cancellationToken);
            if (cancellationToken.IsCancellationRequested)
                return null;
            var obj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(countstring);
            if (obj == null)
            {
                return new List<HPXItem>();
            }
            var array = obj.data as Newtonsoft.Json.Linq.JArray;
            List<HPXItem> list = new List<HPXItem>();
            if (cancellationToken.IsCancellationRequested)
                return null;
            try
            {
                if (array != null & array.Count > 0)
                {
                    var data = array[0].ToObject<JSON.DataObject>();
                    var rdata = data.data as Newtonsoft.Json.Linq.JArray;

                    var arry = rdata.ToArray();
                    switch (itemType)
                    {
                        case ItemType.Gallery:
                            foreach(var token in arry)
                            {
                                if (token.Values<GalleryItem>() != null)
                                {
                                    var item = token.ToObject<GalleryItem>();
                                    list.Add(item);
                                }
                            }
                            break;
                        case ItemType.Collection:
                            foreach (var token in arry)
                            {
                                if (token.Values<Collection>() != null)
                                {
                                    var item = token.ToObject<Collection>();
                                    list.Add(item);
                                }
                            }
                            break;
                    }
                    
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
                if (cancellationToken.IsCancellationRequested)
                    return null;
                Task.Run(() =>
                {
                    InitiateImageGeneration(ids, ItemType.Gallery, "medium", cancellationToken);
                });
            }
            return list;
        }
        
        public static async Task<int> GetCount(ItemType itemType,string query,CancellationToken cancellationToken,ViewType viewType = ViewType.Library)
        {

            List<Tuple<string, string>> main = new List<Tuple<string, string>>();
            List<Tuple<string, string>> funct = new List<Tuple<string, string>>();

            JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest? "guest" : Core.App.Settings.Username);
            JSON.API.PushKey(ref main, "session", App.Server.Info.session);
            JSON.API.PushKey(ref funct, "fname", "get_view_count");
            JSON.API.PushKey(ref funct, "item_type", itemType.ToString());
            JSON.API.PushKey(ref funct, "view_filter", viewType.ToString());
            JSON.API.PushKey(ref funct, "search_query", query);
            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            if (cancellationToken.IsCancellationRequested)
                return 0;
            string countstring = Net.SendPost(response,ref cancellationToken);
            if (cancellationToken.IsCancellationRequested)
                return 0;
            var serverobj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(countstring);
            var dataobj = JSON.API.GetData(serverobj.data, 0);
            var data = ((dataobj as Newtonsoft.Json.Linq.JContainer)["data"])["count"]
                .ToString();
            return int.Parse(data);
        }

        public static async Task<TagList> GetTags(ItemType itemType,int item_id,CancellationToken cancellationToken)
        {
            List<Tuple<string, string>> main = new List<Tuple<string, string>>();
            List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
            JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest? "guest" : Core.App.Settings.Username);
            JSON.API.PushKey(ref main, "session", App.Server.Info.session);
            JSON.API.PushKey(ref funct, "fname", "get_tags");
            JSON.API.PushKey(ref funct, "item_id", "<int>" + item_id);
            JSON.API.PushKey(ref funct, "item_type", itemType.ToString());
            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            if (cancellationToken.IsCancellationRequested)
                return null;
            string responsestring = Net.SendPost(response,ref cancellationToken);
            if (cancellationToken.IsCancellationRequested)
                return null;
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

        public async static Task<string> GetThumb(GalleryItem gallery,CancellationToken cancellationToken)
        {
            int tries = 0;
            string thumb_path = string.Empty;
            while (true)
            {
                if (tries > 3)
                    break;
                
                {
                    if (cancellationToken.IsCancellationRequested)
                        return null;
                    await Task.Run(async () =>
                    {
                        thumb_path = await gallery.Download(cancellationToken);
                    });
                    if (cancellationToken.IsCancellationRequested)
                        return null;
                    gallery = Core.App.Server.GetItem<Core.Gallery.GalleryItem>(gallery.id, ItemType.Gallery,cancellationToken);
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