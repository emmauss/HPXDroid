using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
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
using Android.Graphics.Drawables;
using Android.Graphics;
using NLog;
using Newtonsoft.Json;

using static HappyPandaXDroid.Core.App.Server;


namespace HappyPandaXDroid.Core
{
    public class Gallery
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static CancellationTokenSource DownloadCancelationToken = new CancellationTokenSource();
        public static ConcurrentQueue<(HPXItem, ImageSize)> DownloadList = new ConcurrentQueue<(HPXItem, ImageSize)>();
        private static List<(HPXItem, ImageSize)> CurrentlyDownloading = new List<(HPXItem, ImageSize)>();
        private static bool IsDownloading = false;
        public static Color[] CategoryColors = new Color[] { };
        public static Dictionary<int, Category> Categories { get; set; }
        public static Dictionary<int, Language> Languages { get; set; }
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
            Filter = 3,
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
            All = 6,
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
            public bool favorite;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool inbox;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool readlater;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool trash;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool follow;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool read;
        }

        static Gallery()
        {
            RefreshServerMeta();
        }

        public static void RefreshServerMeta()
        {
            var categories = GetCategories(new CancellationTokenSource().Token).Result;
            Categories = new Dictionary<int, Category>();
            foreach (var category in categories)
            {
                Categories.Add(category.id, category);
            }

            if (!Categories.ContainsKey(0))
            {
                Categories.Add(0, new Category() { category_id = 0, name = "Doujin" });
            }

            var languages = GetLanguages(new CancellationTokenSource().Token).Result;
            Languages = new Dictionary<int, Language>();
            foreach (var language in languages)
            {
                Languages.Add(language.id, language);
            }

            if (!Languages.ContainsKey(0))
            {
                Languages.Add(0, new Language() { id = 0, name = "English" });
            }
        }

        public class Category : HPXItem
        {
            public override ItemType Type => ItemType.Category;
        }

        public class Language : HPXItem
        {
            public override ItemType Type => ItemType.Language;
        }

        public abstract class HPXItem
        {
            public event EventHandler CountRead;

            [JsonIgnore]
            public virtual int ChildCount { get; set; } = -1;
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

            public Media.Image Thumb;
            public Media.Image Image;
            public int CommandId;

            public MetaTag metatags;

            public abstract ItemType Type { get; }

            public HPXItem Parent;

            public void InvokeCountEvent()
            {
                CountRead.Invoke(this, null);
            }

            public string BaseId
            {
                get
                {
                    string bid = string.Empty;
                    if (Parent != null)
                    {
                        bid += Parent.BaseId;
                    }

                    bid += "- " + id + timestamp;

                    return bid;
                }
            }

            public async Task<string> Download(CancellationToken cancellationToken, ImageSize size = ImageSize.Medium)
            {
                return await GetImage(this, false, cancellationToken, size);
            }

            public void DownloadFromQueue(ImageSize size)
            {
                lock (this)
                {
                    GetImage(this, false, DownloadCancelationToken.Token, size).Wait();
                }

                RemoveFromQueue(size);
            }

            public void RemoveFromQueue(ImageSize size)
            {
                if (CurrentlyDownloading != null)
                    if (CurrentlyDownloading.Count > 0)
                        if (CurrentlyDownloading.Contains((this, size)))
                            CurrentlyDownloading.Remove((this,size));
            }
        }

        public class Collection : HPXItem
        {
            public override ItemType Type => ItemType.Collection;

            public async Task<string> Download(CancellationToken cancellationToken, ImageSize size = ImageSize.Medium)
            {
                return await GetImage(this, false, cancellationToken, size);
            }
        }

        public class PageReadData
        {
            public Page page;
            public bool end;
            public float percent;
            public int count;
            public int timestamp;
        }

        public class GalleryItem : HPXItem
        {
            public List<Collection> collections;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int command_id;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int taggable_id;
            public string last_read;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int language_id;
            public List<Artist> artists;
            public List<Page> pages;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<string> filters;
            public string info;
            public bool fetched;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int grouping_id;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public float number;
            public bool fav;
            public float rating;
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

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public PageReadData pageReadData;

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<Parody> parodies;

            [JsonIgnore()]
            AutoResetEvent AutoResetEvent { get; set; }// = new AutoResetEvent(true);

            

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

            public override ItemType Type => ItemType.Gallery;

            public TagList tags;

            public List<Page> PageList;

            public override int ChildCount {

                get
                {
                    if (base.ChildCount > 0)
                        return base.ChildCount;
                    else if (PageList != null && PageList.Count > 0)
                        return PageList.Count;

                    return 0;
                }
                set => base.ChildCount = value;
            }

            public string Download(CancellationToken cancellationToken,ImageSize size = ImageSize.Original)
            {
                lock (this)
                {
                    if (!GetCachedPagePath(this, out string path, size))
                    {
                        path = GetImage(this, false, DownloadCancelationToken.Token, size).GetAwaiter().GetResult();
                    }

                    return path;
                }
            }
            public void Wait()
            {
                AutoResetEvent.WaitOne(5000);
            }
        }

        public class Parody : HPXItem
        {
            public List<Name> names;

            public Name preferred_name;

            public override ItemType Type => ItemType.Parody;
        }
        
        public class Page : HPXItem
        {
            public int number;
            public int gallery_id;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int taggable_id;
            public bool in_archive;
            public string path;
            public string thumb_url;
            public string image_url;
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int command_id;
            public bool IsPlaceholder = false;
            public bool MoreExists = false;

            public override ItemType Type => ItemType.Page;

            public string Download(ImageSize size = ImageSize.Original)
            {
                lock (this)
                {
                    if (!GetCachedPagePath(this, out string path, size))
                    {
                        path = GetImage(this, false, DownloadCancelationToken.Token, size).GetAwaiter().GetResult();
                    }

                    return path;
                }
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
            ManualResetEvent resetEvent = new ManualResetEvent(false);

            RequestToken request = new RequestToken(resetEvent, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                return false;

            request.Request = response;

            request.Queue();

            request.RequestResetEvent.WaitOne();

            string reply = (string)request.Result;

            if (reply.Contains("exist") && reply.Contains("true"))
                return true;
            else
                return false;
        }

        public static void QueueDownloads(List<HPXItem> downloadList, ItemType type = ItemType.Page, ImageSize size = ImageSize.Original)
        {
            lock (DownloadList)
            {
                List<int> ids = new List<int>();
                try
                {
                    foreach (var item in downloadList)
                    {
                        var tuple = DownloadList.ToList().Find((x) => x.Item1.id == item.id && x.Item2 == size);
                        if (tuple.Item1 == null)
                        {
                            if (item.id > 0)
                                if (!IsItemCached(item, size))
                                {
                                    DownloadList.Enqueue((item, size));
                                    ids.Add(item.id);
                                }
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                var idArray = ids.ToArray();

                InitiateImageGeneration(idArray, type, size, new CancellationTokenSource().Token);
            }

            StartQueue();
        }

        public static void StartQueue()
        {
            var h = new Handler(Looper.MainLooper);
            if (IsDownloading)
            {
                h.Post(() =>
                {
                    Toast.MakeText(Application.Context, "Precaching already running", ToastLength.Short).Show();
                });
                return;
            }

            IsDownloading = true;
            while (DownloadList.Count > 0)
            {
                while (DownloadList.TryDequeue(out (HPXItem,ImageSize) item))
                {
                    if (item != default((HPXItem,ImageSize)))
                    {
                        while (CurrentlyDownloading.Count >= 5)
                        {
                            Thread.Sleep(3000);
                        }

                        if (CurrentlyDownloading.Find(x => x.Item1?.id == item.Item1.id).Item1 != null)
                            continue;

                        if (!IsDownloading)
                        {
                            foreach (var cpage in CurrentlyDownloading)
                                DownloadList.Enqueue(cpage);
                            DownloadList.Enqueue(item);
                            CurrentlyDownloading.Clear();
                            break;
                        }

                        if (IsItemCached(item.Item1, item.Item2))
                            continue;

                        CurrentlyDownloading.Add(item);

                        Task.Run(() => 
                        {
                            item.Item1?.DownloadFromQueue(item.Item2);
                        }
                        );

                        Thread.Sleep(100);
                    }
                }


            }
            
            IsDownloading = false;

            //Toast.MakeText(Application.Context, "Precaching completed or stopped", ToastLength.Short).Show();
        }

        public static void StopQueue()
        {
            IsDownloading = false;
        }

        public static async void InitiateImageGeneration(int[] ids, ItemType itemType,ImageSize size,CancellationToken cancellationToken)
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
                JSON.API.PushKey(ref funct, "size", Enum.GetName(typeof(ImageSize), size));
                JSON.API.PushKey(ref funct, "item_type", Enum.GetName(typeof(ItemType), itemType));
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);
                ManualResetEvent resetEvent = new ManualResetEvent(false);

                RequestToken request = new RequestToken(resetEvent, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return;

                request.Request = response;

                request.Queue();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "\n Exception Caught In Gallery.InitiateImageGeneration.");
            }
        }

        public static async Task<string> GetImage(HPXItem item, bool return_url, CancellationToken cancellationToken, ImageSize size)
        {
            string type = string.Empty;
            try
            {
                if (item.id < 1)
                    return type;

                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
                JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest? "guest" : Core.App.Settings.Username);
                JSON.API.PushKey(ref main, "session", App.Server.Info.session);
                JSON.API.PushKey(ref funct, "fname", "get_image");
                JSON.API.PushKey(ref funct, "item_ids", "[" + item.id + "]");
                JSON.API.PushKey(ref funct, "url", "<bool>true");
                JSON.API.PushKey(ref funct, "size", Enum.GetName(typeof(ImageSize), size));
                JSON.API.PushKey(ref funct, "item_type", Enum.GetName(typeof(ItemType), item.Type));
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);
                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;
                ManualResetEvent resetEvent = new ManualResetEvent(false);

                RequestToken request = new RequestToken(resetEvent, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;

                request.Request = response;

                request.Queue();

                request.RequestResetEvent.WaitOne();

                string reply = (string)request.Result;

                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;
                int command_id = App.Server.GetCommandId(item.id, reply);
                if (command_id == 0 || command_id == -1)
                    return string.Empty;
                while (true)
                {
                    var state = App.Server.GetCommandState(command_id, ref cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                        return "Request Cancelled";
                    if (state == CommandState.Error)
                        return "failed: server error";
                    if (state == CommandState.Failed)
                        return "failed: Command Failed";
                    if (state == CommandState.Stopped)
                        StartCommand(command_id, cancellationToken);
                    else if (state != CommandState.Finished)
                        Thread.Sleep(App.Settings.Loop_Delay);
                    else
                        break;
                }
                string cacheid = App.Server.HashGenerator(item.BaseId, size, item.Type);
                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;
                //get value
                string path = App.Server.GetCommandValue(command_id, item, cacheid, return_url, ref cancellationToken);
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
                     + System.Environment.NewLine + ex.StackTrace, item.id, size, ex.Message,type);
                return "fail: server error";
            }
        }

        public static async Task<Dictionary<int, Media.Image>> GetImage(List<HPXItem> items, ItemType itemType, CancellationToken cancellationToken, ImageSize size = ImageSize.Medium)
        {
            string type = string.Empty;
            List<int> ids = new List<int>();
            foreach (var it in items)
                if (it != null && it.id > 0)
                    ids.Add(it.id);
            try
            {
                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
                JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest ? "guest" : Core.App.Settings.Username);
                JSON.API.PushKey(ref main, "session", App.Server.Info.session);
                JSON.API.PushKey(ref funct, "fname", "get_image");
                JSON.API.PushKey(ref funct, "item_ids", "[" + string.Join(",",ids)+ "]");
                JSON.API.PushKey(ref funct, "url", "<bool>true");
                JSON.API.PushKey(ref funct, "size", Enum.GetName(typeof(ImageSize), size));
                JSON.API.PushKey(ref funct, "item_type", Enum.GetName(typeof(ItemType), itemType));
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);

                ManualResetEvent resetEvent = new ManualResetEvent(false);

                RequestToken request = new RequestToken(resetEvent, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return new Dictionary<int, Media.Image>();

                request.Request = response;

                request.Queue();

                request.RequestResetEvent.WaitOne();

                string reply = (string)request.Result;

                if (cancellationToken.IsCancellationRequested)
                    return new Dictionary<int, Media.Image>();
                var command_ids = App.Server.GetCommandIds(items.ToArray(), reply).ToList();
                command_ids.RemoveAll(x => x == -1 || x == 0);

                List<int> Done;

                while (true)
                {
                    bool state = App.Server.GetCompleted(out Done,command_ids,items.ToArray(), ref cancellationToken, size);
                    if(!state)
                        Thread.Sleep(App.Settings.Loop_Delay);
                    else
                        break;
                }
                if (cancellationToken.IsCancellationRequested)
                    return new Dictionary<int, Media.Image>();
                Dictionary<int, Media.Image> images = new Dictionary<int, Media.Image>();

                Task.Run(() => QueueDownloads(items, itemType, size));

                foreach (var it in items)
                {
                    int id = ids.Find(x => x == it.id);
                    if (id != 0)
                    {
                        Media.Image image = size == Gallery.ImageSize.Small ? it.Thumb : it.Image;

                        images.Add(id, image);
                    }
                    else
                    {

                    }
                    /*foreach (var id in ids)
                    {
                        if (id == it.id)
                        {
                            Media.Image image = size == Gallery.ImageSize.Small ? it.Thumb : it.Image;

                            images.Add(id, image);
                            break;
                        }
                    }*/
                }
                //get value
                //Dictionary<int,Media.Image> path = App.Server.GetCommandValues(command_ids, id, type, ref cancellationToken);
                return images;

            }
            catch (System.Net.Sockets.SocketException sex)
            {
                logger.Error(sex, "\n Exception Caught In Gallery.GetImage.Message {0}\n {1}", sex.Message, sex.StackTrace);
                return new Dictionary<int, Media.Image>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "\n Exception Caught In Gallery.GetImage. type = {3}, itemId = {0}, size = {1},\n Message = {2}"
                     + System.Environment.NewLine + ex.StackTrace, ids, size, ex.Message, type);
                return new Dictionary<int, Media.Image>();
            }
        }

        public static bool IsItemCached(HPXItem item, ImageSize size = ImageSize.Original)
        {
            try
            {
                string cacheid = Core.App.Settings.CachePath + Core.App.Server.HashGenerator(item.BaseId, size, item.Type);
                bool check = Core.Media.Cache.IsCached(cacheid);
                return check;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex, "\n Exception Caught In GalleryCard.IsCached.");

                return false;
            }
        }

        public static async Task<List<Category>> GetCategories(CancellationToken cancellationToken)
        {
            await Task.Delay(10);
            List<Tuple<string, string>> main = new List<Tuple<string, string>>();
            List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
            JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest ? "guest" : Core.App.Settings.Username);
            JSON.API.PushKey(ref main, "session", App.Server.Info.session);
            JSON.API.PushKey(ref funct, "fname", "get_items");
            JSON.API.PushKey(ref funct, "item_type", Enum.GetName(typeof(ItemType), ItemType.Category));

            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            if (cancellationToken.IsCancellationRequested)
                return new List<Category>();
            ManualResetEvent resetEvent = new ManualResetEvent(false);

            RequestToken request = new RequestToken(resetEvent, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                return new List<Category>();

            request.Request = response;

            request.Queue();

            request.RequestResetEvent.WaitOne();

            string reply = (string)request.Result;

            var obj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>((string)request.Result);
            if (obj == null)
            {
                request.Failed = true;
                return new List<Category>();
            }
            var array = obj.data as Newtonsoft.Json.Linq.JArray;
            List<Category> list = new List<Category>();
            if (request.CancellationToken.IsCancellationRequested)
                return new List<Category>();
            try
            {
                if (array != null & array.Count > 0)
                {
                    var data = array[0].ToObject<JSON.DataObject>();
                    var rdata = data.data as Newtonsoft.Json.Linq.JArray;

                    var arry = rdata.ToArray();
                    foreach (var token in arry)
                    {
                        if (token.Values<Category>() != null)
                        {
                            var item = token.ToObject<Category>();
                            if (item != null && item.id > 0)
                                list.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return list;
        }

        public static async Task<List<Language>> GetLanguages(CancellationToken cancellationToken)
        {
            await Task.Delay(10);
            List<Tuple<string, string>> main = new List<Tuple<string, string>>();
            List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
            JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest ? "guest" : Core.App.Settings.Username);
            JSON.API.PushKey(ref main, "session", App.Server.Info.session);
            JSON.API.PushKey(ref funct, "fname", "get_items");
            JSON.API.PushKey(ref funct, "item_type", Enum.GetName(typeof(ItemType), ItemType.Language));

            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            if (cancellationToken.IsCancellationRequested)
                return new List<Language>();
            ManualResetEvent resetEvent = new ManualResetEvent(false);

            RequestToken request = new RequestToken(resetEvent, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                return new List<Language>();

            request.Request = response;

            request.Queue();

            request.RequestResetEvent.WaitOne();

            string reply = (string)request.Result;

            var obj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>((string)request.Result);
            if (obj == null)
            {
                request.Failed = true;
                return new List<Language>();
            }
            var array = obj.data as Newtonsoft.Json.Linq.JArray;
            List<Language> list = new List<Language>();
            if (request.CancellationToken.IsCancellationRequested)
                return new List<Language>();
            try
            {
                if (array != null & array.Count > 0)
                {
                    var data = array[0].ToObject<JSON.DataObject>();
                    var rdata = data.data as Newtonsoft.Json.Linq.JArray;

                    var arry = rdata.ToArray();
                    foreach (var token in arry)
                    {
                        if (token.Values<Language>() != null)
                        {
                            var item = token.ToObject<Language>();
                            if (item != null && item.id > 0)
                                list.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return list;
        }

        public static bool GetCachedPagePath(HPXItem item, out string pagePath, ImageSize size = ImageSize.Original)
        {
            pagePath = string.Empty;
            try
            {
                string cacheid = Core.App.Server.HashGenerator(item.BaseId, size, item.Type);
                bool check = Core.Media.Cache.IsCached(cacheid);

                if (check)
                    Media.Cache.TryGetCachedPath(cacheid, out pagePath);

                return check;
            }
            catch (System.Exception ex)
            {
                logger.Error(ex, "\n Exception Caught In GalleryCard.IsCached.");

                return false;
            }
        }

        public static void RequestPageCount(HPXItem item, CancellationToken cancellationToken, ItemType relatedType)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            else
            {
                int count = App.Server.GetRelatedCount(item.id, cancellationToken, item.Type, relatedType);
                if(count>-1)
                    try
                    {
                        item.ChildCount = count;
                        item.InvokeCountEvent();
                    }
                    catch
                    {

                    }
            }
        }


        public async static void GetPage(ItemType itemType, int page, RequestToken request,
            ViewType viewType = ViewType.Library, Sort sortCriteria = (Sort)1, bool sortDec = false,
            string searchQuery = "")
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
            JSON.API.PushKey(ref funct, "limit", "<int>" + App.Settings.Limit);
            JSON.API.PushKey(ref funct, "search_query", searchQuery);
            JSON.API.PushKey(ref funct, "page", "<int>" + page);
            JSON.API.PushKey(ref funct, "sort_by", (sort == "null" ? "<int>" : "") + sort);
            JSON.API.PushKey(ref funct, "sort_desc", "<bool>" + sortDec.ToString().ToLower());

            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            if (request.CancellationToken.IsCancellationRequested)
                return;

            request.Request = response;

            request.ProcessResult = (x) =>
            {
                if (x.CancellationToken.IsCancellationRequested)
                    return;
                var obj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>((string)x.Result);
                if (obj == null)
                {
                    x.Failed = true;
                    return;
                }
                var array = obj.data as Newtonsoft.Json.Linq.JArray;
                List<HPXItem> list = new List<HPXItem>();
                if (x.CancellationToken.IsCancellationRequested)
                    return;
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
                                foreach (var token in arry)
                                {
                                    if (token.Values<GalleryItem>() != null)
                                    {
                                        var item = token.ToObject<GalleryItem>();
                                        if (item != null && item.id > 0)
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

                                        if (item != null && item.id > 0)
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
                    if (x.CancellationToken.IsCancellationRequested)
                        return;
                    Task.Run(() =>
                    {
                        InitiateImageGeneration(ids, ItemType.Gallery, ImageSize.Small, x.CancellationToken);
                    }, x.CancellationToken);

                    x.SetResult(list);
                }
            };
            request.Queue();
        }
        
        public static async void GetCount(ItemType itemType,string query, RequestToken request,ViewType viewType = ViewType.Library)
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
            if (request.CancellationToken.IsCancellationRequested)
                return;

            request.Request = response;

            request.ProcessResult = (x) =>
            {
                var cancellationToken = x.CancellationToken;

                if (cancellationToken.IsCancellationRequested)
                    return;
                var serverobj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>((string)x.Result);
                var dataobj = JSON.API.GetData(serverobj.data, 0);
                var data = ((dataobj as Newtonsoft.Json.Linq.JContainer)["data"])["count"]
                    .ToString();

                x.SetResult(int.Parse(data));
            };

            request.Queue();

            return;
        }

        public static async void GetTags(ItemType itemType,int item_id,RequestToken request)
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
            if (request.CancellationToken.IsCancellationRequested)
                return;

            request.Request = response;

            request.ProcessResult = (x) =>
            {
                var cancellationToken = x.CancellationToken;

                if (cancellationToken.IsCancellationRequested)
                    return;
                var obj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>((string)request.Result);
                if (obj == null || obj.data == null)
                    return;
                var array = obj.data as Newtonsoft.Json.Linq.JArray;
                var taglist = new TagList();
                if (array != null)
                {
                    var data = array[0].ToObject<JSON.DataObject>();
                    var sett = data.data as Newtonsoft.Json.Linq.JObject;
                    taglist = sett.ToObject<TagList>();
                }

                request.SetResult(taglist);
            };
            request.Queue();

            return;
        }

        public static async void GalleryReadEvent(int item_id, CancellationToken cancellationToken)
        {
            List<Tuple<string, string>> main = new List<Tuple<string, string>>();
            List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
            JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest ? "guest" : Core.App.Settings.Username);
            JSON.API.PushKey(ref main, "session", App.Server.Info.session);
            JSON.API.PushKey(ref funct, "fname", "gallery_read_event");
            JSON.API.PushKey(ref funct, "item_id", "<int>" + item_id);
            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);

            ManualResetEvent resetEvent = new ManualResetEvent(false);

            RequestToken request = new RequestToken(resetEvent, cancellationToken);

            request.Request = response;

            request.Queue();
        }

        public async static Task<string> GetThumb(HPXItem item, CancellationToken cancellationToken)
        {
            int tries = 0;
            string thumb_path = string.Empty;

            if (item.id > 0)
                while (true)
                {
                    if (tries > 3)
                        break;

                    {
                        if (cancellationToken.IsCancellationRequested)
                            return null;
                        thumb_path = await item.Download(cancellationToken);

                        try
                        {
                            if (File.Exists(thumb_path))
                                break;
                        }catch(Exception ex)
                        {

                        }

                        if (cancellationToken.IsCancellationRequested)
                            return null;
                        item = Core.App.Server.GetItem<Core.Gallery.GalleryItem>(item.id, item.Type, cancellationToken);
                    }
                    tries++;
                }


            bool IsCached()
            {
                int item_id = item.id;
                try
                {
                    return Gallery.IsItemCached(item, ImageSize.Medium);
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