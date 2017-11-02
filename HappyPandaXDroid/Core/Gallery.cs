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
        public static List<GalleryItem> CurrentList = new List<GalleryItem>();
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
            public int command_id;
            public int taggable_id;
            public string category_id;
            public string last_read;
            public string last_updated;
            public string language_id;
            public List<Artist> artists;
            public List<Page> pages;
            public int timestamp;
            public string[] filters;
            public string info;
            public bool fetched;
            public int grouping_id;
            public int number;
            public bool fav;
            public int id;
            public int rating;
            public List<Profile> profiles;
            public int times_read;
            public List<Title> titles;
            public int pub_date;
            public List<URL> urls;
            public bool inbox;
            public bool singe_source;
            public string[] collections;
            public string[] circles;

            public TagList tags;
        }

        public class Page
        {
            public int number;
            public int gallery_id;
            public int taggable_id;
            public int id;
            public int last_updated;
            public bool in_archive;
            public string path;
            public string name;
            public string thumb_url;
            public string image_url;
            public int command_id;
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
            public List<TagItem> Male { get; set; }
            public List<TagItem> Female { get; set; }
            public List<TagItem> Language { get; set; }
            public List<TagItem> __namespace__ { get; set; }
            public List<TagItem> Reclass { get; set; }
            public List<TagItem> Parody { get; set; }
        }

        public class TagNamespace
        {

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
            JSON.API.PushKey(ref main, "session", Net.session_id);
            JSON.API.PushKey(ref funct, "fname", "source_exists");
            JSON.API.PushKey(ref funct, "item_type", type);
            JSON.API.PushKey(ref funct, "item_id","<int>" + id);
            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            string reply = Net.SendPost(response);
            string exist = reply.Substring(reply.IndexOf("exists"));
            bool exists = false;
            if (exist.Contains("true"))
            {
                exists = true;
            }
            return exists;
        }


        public static void GetLibrary()
        {
            List<Tuple<string, string>> main = new List<Tuple<string, string>>();
            List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
            JSON.API.PushKey(ref main, "name", "test");
            JSON.API.PushKey(ref main, "session", Net.session_id);
            JSON.API.PushKey(ref funct, "fname", "library_view");
            JSON.API.PushKey(ref funct, "limit", "<int>25");
            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            string countstring = Net.SendPost(response);
            countstring = countstring.Remove(countstring.LastIndexOf("<EOF>"));
            var obj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(countstring);
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
            }catch(Exception ex)
            {

            }
            CurrentList = list;

            /*
            string data = JSON.API.GetData(countstring, 2);
            if (data.LastIndexOf("]") != data.Length - 1)
                data = data.Remove(data.LastIndexOf("]") + 1);
            CurrentList = JSON.Serializer.SimpleSerializer.DeserializeToList<GalleryItem>(data);
            List<int> g_ids = new List<int>();
            foreach (var gal in CurrentList)
            {
                g_ids.Add(gal.id);
            }
            int[] gids = g_ids.ToArray();*/

            //InitiateImageGeneration(gids, "gallery","medium");
        }

        public static async Task<string> GetImage(GalleryItem gallery, bool return_url, string size = "medium")
        {
            try
            {
                int item_id = gallery.id;
                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
                JSON.API.PushKey(ref main, "name", "test");
                JSON.API.PushKey(ref main, "session", Net.session_id);
                JSON.API.PushKey(ref funct, "fname", "get_image");
                JSON.API.PushKey(ref funct, "item_ids", "[" + item_id + "]");
                JSON.API.PushKey(ref funct, "url", "<bool>true");
                JSON.API.PushKey(ref funct, "size", size);
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);
                string reply = Net.SendPost(response);
                int command_id = App.Server.GetCommandId(item_id, reply);

                while (true)
                {
                    string state = App.Server.GetCommandState(command_id);
                    if (state.Contains("error"))
                        return "fail: server error";
                    if(state.Contains("failed"))
                        return "fail: command error";
                    if (!state.Contains("finished"))
                        Thread.Sleep(App.Settings.Loop_Delay);
                    else
                        break;
                }
                string name = string.Empty;
                //get value
                gallery = App.Server.GetItem<GalleryItem>(gallery.id, "Gallery");
                name = App.Server.HashGenerator(size, "thumb",gallery.id);

                string path = App.Server.GetCommandValue(command_id, gallery.id, name, "thumb", return_url);
                return path;
            }
            catch (System.Net.Sockets.SocketException sex)
            {
                logger.Error(sex, "\n Exception Caught In Gallery.GetImage.Message {0}\n {1}",sex.Message,sex.StackTrace);
                return "fail: server not found or connection failed error";
            }
            catch (Exception ex)
            {
                logger.Error(ex, "\n Exception Caught In Gallery.GetImage. type = Gallery, itemId = {0}, size = {1},\n Message = {2}"
                     + System.Environment.NewLine + ex.StackTrace , gallery.id, size,ex.Message);
                return "fail: server error";
            }
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
                JSON.API.PushKey(ref main, "session", Net.session_id);
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

        public static async Task<string> GetImage(Page page, bool return_url, string size = "medium",bool IsPreview = false)
        {
            try
            {

                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
                JSON.API.PushKey(ref main, "name", "test");
                JSON.API.PushKey(ref main, "session", Net.session_id);
                JSON.API.PushKey(ref funct, "fname", "get_image");
                JSON.API.PushKey(ref funct, "item_ids", "[" + page.id + "]");
                JSON.API.PushKey(ref funct, "url", "<bool>true");
                JSON.API.PushKey(ref funct, "size", size);
                JSON.API.PushKey(ref funct, "item_type", "page");
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);
                string reply = Net.SendPost(response);
                int command_id = App.Server.GetCommandId(page.id, reply);
                while (true)
                {
                    string state = App.Server.GetCommandState(command_id);
                    if (state.Contains("error"))
                        return "fail: server error";
                    if (state.Contains("failed"))
                        return "fail: command error";
                    if (!state.Contains("finished"))
                        Thread.Sleep(3000);
                    else
                        break;
                }
                string type = "";
                switch (size)
                {
                    case "medium":
                        type = "preview";
                        break;
                    case "original":
                        type = "page";
                        break;
                }
                string name = App.Server.HashGenerator(size, type, page.id);
                //get value
                string path = App.Server.GetCommandValue(command_id, page.id, name, "page", return_url,IsPreview);
                return path;

            }
            catch (System.Net.Sockets.SocketException sex)
            {
                logger.Error(sex, "\n Exception Caught In Gallery.GetImage.Message {0}\n {1}", sex.Message, sex.StackTrace);
                return "fail: server not found or connection failed error";
            }
            catch (Exception ex)
            {
                logger.Error(ex, "\n Exception Caught In Gallery.GetImage. type = Page, itemId = {0}, size = {1},\n Message = {2}"
                     + System.Environment.NewLine + ex.StackTrace, page.id, size, ex.Message);
                return "fail: server error";
            }
        }

        public static async Task<bool> SearchGallery(string query)
        {
            try
            {
                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
                JSON.API.PushKey(ref main, "name", "test");
                JSON.API.PushKey(ref main, "session", Net.session_id);
                JSON.API.PushKey(ref funct, "fname", "library_view");
                JSON.API.PushKey(ref funct, "limit", "<int>25");
                JSON.API.PushKey(ref funct, "search_query", query);
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);
                string countstring = Net.SendPost(response);
                countstring = countstring.Remove(countstring.LastIndexOf("<EOF>"));
                var obj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(countstring);
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
                CurrentList = list;

                /*
                string data = JSON.API.GetData(countstring, 2);
                if (data.LastIndexOf("]") != data.Length - 1)
                    data = data.Remove(data.LastIndexOf("]") + 1);
                CurrentList = JSON.Serializer.SimpleSerializer.DeserializeToList<GalleryItem>(data);*/
                if (CurrentList.Count > 0)
                {
                    List<int> g_ids = new List<int>();
                    foreach (var gal in CurrentList)
                    {
                        g_ids.Add(gal.id);
                    }
                    int[] gids = g_ids.ToArray();

                    //InitiateImageGeneration(gids, "gallery", "medium");
                    return true;
                }
                else
                    return false;
            }catch(Exception ex)
            {
                logger.Error(ex, "\n Exception Caught In Gallery.SearchGallery. query = {0}",query);

                return false;
            }
        }

        /// <summary>
        /// jump to a page in the current library
        /// </summary>
        /// <param name="page">zero based page number</param>        
        public static int JumpToPage(int page, string search_query)
        {
            List<Tuple<string, string>> main = new List<Tuple<string, string>>();
            List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
            JSON.API.PushKey(ref main, "name", "test");
            JSON.API.PushKey(ref main, "session", Net.session_id);
            JSON.API.PushKey(ref funct, "fname", "library_view");
            JSON.API.PushKey(ref funct, "limit", "<int>25");
            JSON.API.PushKey(ref funct, "search_query", search_query);
            JSON.API.PushKey(ref funct, "page", "<int>" + page);
            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            string countstring = Net.SendPost(response);
            countstring = countstring.Remove(countstring.LastIndexOf("<EOF>"));
            var obj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(countstring);
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
            CurrentList = list;
            return CurrentList.Count;
        }

        public static int NextPage(int page, string search_query)
        {
            List<Tuple<string, string>> main = new List<Tuple<string, string>>();
            List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
            JSON.API.PushKey(ref main, "name", "test");
            JSON.API.PushKey(ref main, "session", Net.session_id);
            JSON.API.PushKey(ref funct, "fname", "library_view");
            JSON.API.PushKey(ref funct, "limit", "<int>25");
            JSON.API.PushKey(ref funct, "search_query", search_query);
            JSON.API.PushKey(ref funct, "page", "<int>" + page);
            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            string countstring = Net.SendPost(response);
            countstring = countstring.Remove(countstring.LastIndexOf("<EOF>"));
            var obj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(countstring);
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
            CurrentList.AddRange(list);
            return list.Count;
        }

        public static int PreviousPage(int page, string search_query)
        {
            List<Tuple<string, string>> main = new List<Tuple<string, string>>();
            List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
            JSON.API.PushKey(ref main, "name", "test");
            JSON.API.PushKey(ref main, "session", Net.session_id);
            JSON.API.PushKey(ref funct, "fname", "library_view");
            JSON.API.PushKey(ref funct, "limit", "<int>25");
            JSON.API.PushKey(ref funct, "search_query", search_query);
            JSON.API.PushKey(ref funct, "page", "<int>" + page);
            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            string countstring = Net.SendPost(response);
            countstring = countstring.Remove(countstring.LastIndexOf("<EOF>"));
            var obj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(countstring);
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
            list.AddRange(CurrentList);
            CurrentList.Clear();
            CurrentList.AddRange(list);
            return list.Count;
        }

        public static async Task<int> GetCount(string query)
        {

            List<Tuple<string, string>> main = new List<Tuple<string, string>>();
            List<Tuple<string, string>> funct = new List<Tuple<string, string>>();

            JSON.API.PushKey(ref main, "name", "test");
            JSON.API.PushKey(ref main, "session", Net.session_id);
            JSON.API.PushKey(ref funct, "fname", "get_view_count");
            JSON.API.PushKey(ref funct, "item_type", "Gallery");
            JSON.API.PushKey(ref funct, "search_query", query);
            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            string countstring = Net.SendPost(response);
            string countdata = JSON.API.GetData(countstring, 2);
            countdata = countdata.Substring(countdata.IndexOf(":") + 1, countdata.IndexOf("}") - countdata.IndexOf(":") - 1);
            int.TryParse(countdata, out int count);
            return count;
        }

        public static async Task<TagList> GetTags(int item_id, string type)
        {
            List<Tuple<string, string>> main = new List<Tuple<string, string>>();
            List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
            JSON.API.PushKey(ref main, "name", "test");
            JSON.API.PushKey(ref main, "session", Net.session_id);
            JSON.API.PushKey(ref funct, "fname", "get_tags");
            JSON.API.PushKey(ref funct, "item_id", "<int>" + item_id);
            JSON.API.PushKey(ref funct, "item_type", type);
            string response = JSON.API.ParseToString(funct);
            JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
            response = JSON.API.ParseToString(main);
            string responsestring = Net.SendPost(response);
            responsestring = responsestring.Remove(responsestring.LastIndexOf("<EOF>"));
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
                if (IsCached())
                {

                    break;
                }
                else
                {
                    await Task.Run(async () =>
                    {
                        thumb_path = await Core.Gallery.GetImage(gallery, false);
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