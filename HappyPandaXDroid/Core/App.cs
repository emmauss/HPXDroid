using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Net.Http;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using NLog;

namespace HappyPandaXDroid.Core
{
    class App
    {


        /// <summary>
        /// This is the Settings static class that can be used in your Core solution or in any
        /// of your client applications. All settings are laid out the same exact way with getters
        /// and setters. 
        /// </summary>
        public static class Settings
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            
            public static string basePath = Android.OS.Environment.ExternalStorageDirectory.Path + "/HPX/";
            public static string cache = basePath + "cache/";
            public static string Log = basePath + "log/";
            public static bool Refresh = false;
            private static ISettings AppSettings
            {
                get
                {
                    return CrossSettings.Current;
                }
            }

            #region Setting Constants

            private const string SettingsKey = "settings_key";
            private static readonly string SettingsDefault = string.Empty;

            #endregion

            public static int FilterLevel
            {
                get
                {
                    return int.Parse(AppSettings.GetValueOrDefault("filter_level", "255"));
                }
                set
                {
                    AppSettings.AddOrUpdateValue("filter_level", value.ToString());
                }
            }


            public static string Server_IP
            {
                get
                {
                    return AppSettings.GetValueOrDefault("server_ip", "127.0.0.1");
                }
                set
                {
                    if (Server_IP != value)
                        Refresh = true;
                    AppSettings.AddOrUpdateValue("server_ip", value);
                }
            }

            public static string Username
            {
                get
                {
                    return AppSettings.GetValueOrDefault("username", "");
                }
                set
                {
                    if (Username != value)
                        Refresh = true;
                    AppSettings.AddOrUpdateValue("username", value);
                }
            }

            public static string Password
            {
                get
                {
                    return AppSettings.GetValueOrDefault("password", "");
                }
                set
                {
                    if (Password != value)
                        Refresh = true;
                    AppSettings.AddOrUpdateValue("password", value);
                }
            }


            public static int Loop_Delay
            {
                get
                {
                    return int.Parse(AppSettings.GetValueOrDefault("loop_delay", "1000"));
                }
                set
                {
                    if (Loop_Delay != value)
                        Refresh = true;
                    AppSettings.AddOrUpdateValue("loop_delay", value.ToString());
                }
            }

            public static Gallery.Sort Default_Sort
            {
                get
                {
                    return (Gallery.Sort)Enum.Parse(typeof(Gallery.Sort),AppSettings.GetValueOrDefault("default_sort","None"));
                }
                set
                {
                    AppSettings.AddOrUpdateValue("default_sort", value.ToString());
                }
            }

            public static bool IsFirstRun
            {
                get
                {
                    var set = AppSettings.GetValueOrDefault("first_run", true);
                    return set;
                }
                set
                {
                    AppSettings.AddOrUpdateValue("first_run", value);
                }
            }

            public static bool IsGuest
            {
                get
                {
                    var set = AppSettings.GetValueOrDefault("is_guest", true);
                    return set;
                }
                set
                {
                    AppSettings.AddOrUpdateValue("is_guest", value);
                }
            }

            public static bool Sort_Decending
            {
                get
                {
                    var set = AppSettings.GetValueOrDefault("sort_decending", false);
                    return set;
                }
                set
                {
                    AppSettings.AddOrUpdateValue("sort_decending", value);
                }
            }

            public static bool Logging_Enabled
            {
                get
                {
                    var set = AppSettings.GetValueOrDefault("enable_debugging", false);
                    return set;
                }
                set
                {
                    AppSettings.AddOrUpdateValue("enable_debugging", value);
                }
            }

            public static bool IsGrid
            {
                get
                {
                    var set = AppSettings.GetValueOrDefault("grid_layout", false);
                    return set;
                }
                set
                {
                    AppSettings.AddOrUpdateValue("grid_layout", value);
                }
            }


            public static string Server_Port
            {
                get
                {
                    return AppSettings.GetValueOrDefault("server_port", "7007");
                }
                set
                {
                    if (!int.TryParse(value, out int port))
                        value = "7007";
                    if (Server_Port != value)
                        Refresh = true;
                    AppSettings.AddOrUpdateValue("server_port", value);
                }
            }

            public static string WebClient_Port
            {
                get
                {
                    return AppSettings.GetValueOrDefault("webclient_port", "7008");
                }
                set
                {
                    if (!int.TryParse(value, out int port))
                        value = "7008";
                    if (WebClient_Port != value)
                        Refresh = true;
                    AppSettings.AddOrUpdateValue("webclient_port", value);
                }
            }



            public static string GeneralSettings
            {
                get
                {
                    return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
                }
                set
                {
                    AppSettings.AddOrUpdateValue(SettingsKey, value);
                }
            }

            public class Server
            {
                
                public static int Concurrent_Image_Tasks
                {
                    get
                    {
                        return int.Parse(AppSettings.GetValueOrDefault("core.concurrent_image_tasks", "10"));
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("core.concurrent_image_tasks", value.ToString());
                    }
                }

                public static int Database_Timeout
                {
                    get
                    {
                        return int.Parse(AppSettings.GetValueOrDefault("core.sqlite_database_timeout", "60"));
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("core.sqlite_database_timeout", value.ToString());
                    }
                }

                public static bool Debug_Mode
                {
                    get
                    {
                        var set = AppSettings.GetValueOrDefault("core.debug", false);
                        return set;
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("core.debug", value);
                    }
                }


                public static bool Expose_Server
                {
                    get
                    {
                        var set = AppSettings.GetValueOrDefault("server.expose_server", true);
                        return set;
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("server.expose_server", value);
                    }
                }


                public static bool Allow_Guests
                {
                    get
                    {
                        var set = AppSettings.GetValueOrDefault("server.allow_guests", true);
                        return set;
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("server.allow_guests", value);
                    }
                }


                public static bool Disable_Default_User
                {
                    get
                    {
                        var set = AppSettings.GetValueOrDefault("server.disable_default_user", false);
                        return set;
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("server.disable_default_user", value);
                    }
                }

                public static bool Regex
                {
                    get
                    {
                        var set = AppSettings.GetValueOrDefault("search.regex", false);
                        return set;
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("search.regex", value);
                    }
                }

                public static bool Case_Sensitive
                {
                    get
                    {
                        var set = AppSettings.GetValueOrDefault("search.case_sensitive", false);
                        return set;
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("search.case_sensitive", value);
                    }
                }

                public static bool Match_Whole_Words
                {
                    get
                    {
                        var set = AppSettings.GetValueOrDefault("search.match_whole_words", false);
                        return set;
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("search.match_whole_words", value);
                    }
                }

                public static bool Match_All_Terms
                {
                    get
                    {
                        var set = AppSettings.GetValueOrDefault("search.match_all_terms", false);
                        return set;
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("search.match_all_terms", value);
                    }
                }


                public static bool Descendants
                {
                    get
                    {
                        var set = AppSettings.GetValueOrDefault("search.descendants", false);
                        return set;
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("search.descendants", value);
                    }
                }

                public static bool Require_Authentication
                {
                    get
                    {
                        var set = AppSettings.GetValueOrDefault("server.require_auth", false);
                        return set;
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("server.require_auth", value);
                    }
                }


                public static string Secret_Key
                {
                    get
                    {
                        return AppSettings.GetValueOrDefault("server.secret_key", string.Empty);
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("server.secret_key", value);
                    }
                }

                public static string Server_Name
                {
                    get
                    {
                        return AppSettings.GetValueOrDefault("server.server_name", string.Empty);
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("server.server_name", value);
                    }
                }

                public static string Host
                {
                    get
                    {
                        return AppSettings.GetValueOrDefault("server.host", string.Empty);
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("server.host", value);
                    }
                }

                public static string Translation_Locale
                {
                    get
                    {
                        return AppSettings.GetValueOrDefault("client.translation_locale", string.Empty);
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("client.translation_locale", value);
                    }
                }

                public static string Host_Web
                {
                    get
                    {
                        return AppSettings.GetValueOrDefault("server.host_web", string.Empty);
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("server.host_web", value);
                    }
                }

                public static int Port
                {
                    get
                    {
                        return int.Parse(AppSettings.GetValueOrDefault("server.port", "7007"));
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("server.port", value.ToString());
                    }
                }

                public static int Port_Web
                {
                    get
                    {
                        return int.Parse(AppSettings.GetValueOrDefault("server.port_web", "7008"));
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("server.port_web", value.ToString());
                    }
                }

                public static int Session_Span
                {
                    get
                    {
                        return int.Parse(AppSettings.GetValueOrDefault("server.session_span", string.Empty));
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("server.session_span", value.ToString());
                    }
                }

                public static int Torrent_Port
                {
                    get
                    {
                        return int.Parse(AppSettings.GetValueOrDefault("server.torrent_port", "7006"));
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("server.port", value.ToString());
                    }
                }

                public static int Allowed_Clients
                {
                    get
                    {
                        return int.Parse(AppSettings.GetValueOrDefault("server.allowed_clients", "7006"));
                    }
                    set
                    {
                        AppSettings.AddOrUpdateValue("server.allowed_clients", value.ToString());
                    }
                }


                public static ServerSettings GetSettings(CancellationToken cancellationToken)
                {
                    List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                    List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
                    JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest? "guest" : Core.App.Settings.Username);
                    JSON.API.PushKey(ref main, "session", App.Server.Info.session);
                    JSON.API.PushKey(ref funct, "fname", "get_config");

                    string response = JSON.API.ParseToString(funct);
                    JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                    response = JSON.API.ParseToString(main);

                    ManualResetEvent resetEvent = new ManualResetEvent(false);

                    RequestToken request = new RequestToken(resetEvent, cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                        return null;

                    request.Request = response;

                    request.Queue();

                    request.RequestResetEvent.WaitOne();

                    string reply = (string)request.Result;

                    var obj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(reply);
                    var array = obj.data as Newtonsoft.Json.Linq.JArray;
                    var config = new ServerSettings();
                    if (array != null)
                    {
                        var data = array[0].ToObject<JSON.DataObject>();
                        var sett = data.data as Newtonsoft.Json.Linq.JObject;
                        config = sett.ToObject<ServerSettings>();
                    }


                    return config;
                }

                public static bool SetConfig(string key, string value,CancellationToken cancellationToken)
                {
                    List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                    List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
                    JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest? "guest" : Core.App.Settings.Username);
                    JSON.API.PushKey(ref main, "session", App.Server.Info.session);
                    JSON.API.PushKey(ref funct, "fname", "set_config");
                    string set = "{\"" + key + "\": " + value + " }";
                    JSON.API.PushKey(ref funct, "cfg", set);

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

                    if (reply.Contains("updated"))
                        return true;
                    else
                        return false;
                }


                public class ServerSettings
                {
                    public Core core;
                    public Server server;
                    public Search search;
                    public Client client;

                    public class Core
                    {
                        public bool debug;
                        public int concurrent_image_tasks;
                        public int sqlite_database_timeout;
                    }

                    public class Server
                    {
                        public string secret_key;
                        public string host;
                        public string host_web;
                        public int port;
                        public int port_web;
                        public int torrent_port;
                        public bool expose_server;
                        public int allowed_clients;
                        public bool allow_guests;
                        public bool require_auth;
                        public bool disable_default_user;
                        public int session_span;
                        
                    }

                    public class Search
                    {
                        public bool regex;
                        public bool case_sensitive;
                        public bool match_whole_word;
                        public bool match_all_terms;
                        public bool descendents;
                    }

                    public class Client
                    {
                        public string translation_locale;
                    }

                    

                    public void Parse()
                    {
                        App.Settings.Server.Allowed_Clients = server.allowed_clients;
                        Allow_Guests = server.allow_guests;
                        Host = server.host;
                        Host_Web = server.host_web;
                        Port = server.port;
                        Port_Web = server.port_web;
                        Torrent_Port = server.torrent_port;
                        Session_Span = server.torrent_port;
                        Require_Authentication = server.require_auth;
                        Regex = search.regex;
                        Secret_Key = server.secret_key;
                        Expose_Server = server.expose_server;
                        Disable_Default_User = server.disable_default_user;
                        Debug_Mode = core.debug;
                        Concurrent_Image_Tasks = core.concurrent_image_tasks;
                        Case_Sensitive = search.case_sensitive;
                        Match_All_Terms = search.match_all_terms;
                        Match_Whole_Words = search.match_whole_word;
                        Descendants = search.descendents;
                        Translation_Locale = client.translation_locale;
                        Database_Timeout = core.sqlite_database_timeout;

                    }
                }

            }

        }

        public class Server
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public static ServerInfo Info = new ServerInfo();
            public class ServerInfo
            {
                public Data data = new Data();
                public string name = string.Empty;
                public string session = string.Empty;
            }

            public class Data
            {
                public bool guest_allowed;
                public Version version = new Version();

            }

            public class Version
            {
                public int[] torrent = new int[3];
                public int[] db = new int[3];
                public int[] core = new int[3];
            }

            public static void GetServerInfo()
            {
                logger.Info("GetServerInfo() called");
                string fname = "get_version";
                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> data = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
                JSON.API.PushKey(ref main, "id", "1");
                JSON.API.PushKey(ref funct, "fname", fname);
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref data, "function", response);
                response = JSON.API.ParseToString(data);
                JSON.API.PushKey(ref main, "msg", response);
                response = JSON.API.ParseToString(main);

            }

            public static string StartCommand(int command_id,CancellationToken cancellationToken)
            {
                logger.Info("Start Command. commandId={0}", command_id);
                string response = CreateCommand("start_command", command_id);

                ManualResetEvent resetEvent = new ManualResetEvent(false);

                RequestToken request = new RequestToken(resetEvent, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;

                request.Request = response;

                request.Queue();

                request.RequestResetEvent.WaitOne();

                response = (string)request.Result;

                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;

                string state = string.Empty;
                if (GetError(response) == "none")
                {

                    var serverobj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(response);
                    var dataobj = JSON.API.GetData(serverobj.data, 0);
                    var data = ((dataobj as Newtonsoft.Json.Linq.JContainer)["data"])[command_id.ToString()]
                        .ToString();
                    state =data;
                    if (state.Contains("started"))
                        return "started";
                    else
                        return "failed";
                }
                else
                {
                    logger.Info("Command Failed. \n Error: \n {0}", response + "\n");
                    return (GetError(response));
                }
            }

            public static string StopCommand(int command_id,CancellationToken cancellationToken)
            {
                logger.Info("Stop Command. commandId={0}", command_id);
                string response = CreateCommand("stop_command", command_id);

                ManualResetEvent resetEvent = new ManualResetEvent(false);

                RequestToken request = new RequestToken(resetEvent, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;

                request.Request = response;

                request.Queue();

                request.RequestResetEvent.WaitOne();

                response = (string)request.Result;

                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;

                string state = string.Empty;
                if (GetError(response) == "none")
                {
                    var serverobj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(response);
                    var dataobj = JSON.API.GetData(serverobj.data, 0);
                    var data = ((dataobj as Newtonsoft.Json.Linq.JContainer)["data"])[command_id.ToString()]
                        .ToString();
                    state = data;
                    if (state.Contains("stopped"))
                        return "stopped";
                    else
                        return "failed";
                }
                else
                {
                    logger.Info("Command Failed. \n Error: \n {0}", response + "\n");
                    return (GetError(response));
                }
            }

            public static string UndoCommand(int command_id, CancellationToken cancellationToken)
            {

                string response = CreateCommand("undo_command", command_id);

                ManualResetEvent resetEvent = new ManualResetEvent(false);

                RequestToken request = new RequestToken(resetEvent, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;

                request.Request = response;

                request.Queue();

                request.RequestResetEvent.WaitOne();

                response = (string)request.Result;

                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;
                string state = string.Empty;
                if (GetError(response) == "none")
                {
                    var serverobj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(response);
                    var dataobj = JSON.API.GetData(serverobj.data, 0);
                    var data = ((dataobj as Newtonsoft.Json.Linq.JContainer)["data"])[command_id.ToString()]
                        .ToString();
                    state = data;
                    if (state.Contains("s"))
                        return state;
                    else
                        return "failed";
                }
                else return (GetError(response));
            }

            static string CreateCommand(string key, int command_id)
            {
                logger.Info("Create Command. commandName ={0}, commandId ={1}", key, command_id);
                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();

                JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest? "guest" : Core.App.Settings.Username);
                JSON.API.PushKey(ref main, "session", App.Server.Info.session);
                JSON.API.PushKey(ref funct, "fname", key);
                JSON.API.PushKey(ref funct, "command_ids", "[" + command_id + "]");
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);
                
                return response;
            }

            static string CreateCommand(string key, int[] command_id)
            {
                logger.Info("Create Command. commandName ={0}, commandId ={1}", key, command_id);
                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();

                JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest ? "guest" : Core.App.Settings.Username);
                JSON.API.PushKey(ref main, "session", App.Server.Info.session);
                JSON.API.PushKey(ref funct, "fname", key);
                JSON.API.PushKey(ref funct, "command_ids", "[" + string.Join(",",command_id) + "]");
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);

                return response;
            }

            public static int GetCommandId(int item_id, string command_response)
            {
                if (command_response.Contains("\"" + item_id + "\""))
                {
                    var serverobj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(command_response);
                    var dataobj = JSON.API.GetData(serverobj.data, 0);
                    var data = ((dataobj as Newtonsoft.Json.Linq.JContainer)["data"])[item_id.ToString()]
                        .ToString();
                    string id =data;
                    return int.Parse(id);
                }
                else return -1;
            }

            public static int[] GetCommandIds(Gallery.HPXItem[] items, string command_response)
            {
                List<int> commandIds = new List<int>();
                {
                    var serverobj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(command_response);
                    var dataobj = JSON.API.GetData(serverobj.data, 0);
                    foreach(Gallery.HPXItem it in items)
                    {
                        var value = ((dataobj as Newtonsoft.Json.Linq.JContainer)["data"])[it.id.ToString()]
                        .ToString();
                        commandIds.Add(int.Parse(value));
                        it.CommandId = int.Parse(value);
                    }
                }
                /*if (command_response.Contains("\"" + item_id + "\""))
                {
                    var serverobj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(command_response);
                    var dataobj = JSON.API.GetData(serverobj.data, 0);
                    var data = ((dataobj as Newtonsoft.Json.Linq.JContainer)["data"])[item_id.ToString()]
                        .ToString();
                    string id = data;
                    return int.Parse(id);
                }
                else return -1;*/
                return commandIds.ToArray();
            }

            public class ErrorObject
            {
                public int code;
                public string msg;
            }

            public static string GetCommandValue(int command_id, int item_id, string name, string type, bool return_url
                ,ref CancellationToken cancellationToken)
            {
                logger.Info("Get Command value. commandId={0}, type = {1}, url = {2}, itemID ={3}",
                    command_id, type, return_url.ToString(), item_id.ToString());
                string response = CreateCommand("get_command_value", command_id);
                ManualResetEvent resetEvent = new ManualResetEvent(false);

                RequestToken request = new RequestToken(resetEvent, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;

                request.Request = response;

                request.Queue();

                request.RequestResetEvent.WaitOne();

                response = (string)request.Result;

                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;
                string filename = string.Empty;
                Gallery.Profile data = new Gallery.Profile();
                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;
                try
                {
                    if (GetError(response) == "none")
                    {
                        var serverobj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(response);
                        var dataobj = JSON.API.GetData(serverobj.data, 0);
                        data = ((dataobj as Newtonsoft.Json.Linq.JContainer)["data"])[command_id.ToString()]
                            .ToObject(typeof(Gallery.Profile)) as Gallery.Profile;
                        
                        string dir = Settings.cache;                        
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);                        
                        filename = dir + name + ".jpg";
                        string url = data.data;
                        url = "http://" + App.Settings.Server_IP + ":"+ App.Settings.WebClient_Port + url;
                        if (return_url)
                            return url;
                        if (cancellationToken.IsCancellationRequested)
                            return string.Empty;
                        using (var client = new WebClient())
                        {
                            logger.Info("Downloading URL. URL : {0}\n, Path : {1}", url, filename);
                            client.DownloadFile(new Uri(url), filename);
                            logger.Info("Download Complete. URL : {0},\n Path : {1},\n Size: {2}", url, filename, new FileInfo(filename).Length);
                        }
                        return filename;
                    }
                    else return "fail";
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In App.Server.GetCommandValue. Message : " + ex.Message 
                        + System.Environment.NewLine + ex.StackTrace);
                    if (File.Exists(filename))
                        File.Delete(filename);
                    return "fail";
                }
            }

            public static string GetCommandValue(int command_id, int item_id, string type
                , ref CancellationToken cancellationToken)
            {
                logger.Info("Get Command values. commandId={0}, type = {1},  itemIDs =[{2}]",
                    command_id, type, item_id.ToString());
                string response = CreateCommand("get_command_value", command_id);
                ManualResetEvent resetEvent = new ManualResetEvent(false);

                RequestToken request = new RequestToken(resetEvent, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;

                request.Request = response;

                request.Queue();

                request.RequestResetEvent.WaitOne();

                response = (string)request.Result;

                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;
                string filename = string.Empty;
                Gallery.Profile data = new Gallery.Profile();
                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;
                Dictionary<int, string> urls = new Dictionary<int, string>();
                try
                {
                    if (GetError(response) == "none")
                    {
                        var serverobj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(response);
                        var dataobj = JSON.API.GetData(serverobj.data, 0);
                        data = ((dataobj as Newtonsoft.Json.Linq.JContainer)["data"])[command_id.ToString()]
                            .ToObject(typeof(Gallery.Profile)) as Gallery.Profile;

                        string url = data.data;
                        url = "http://" + App.Settings.Server_IP + ":" + App.Settings.WebClient_Port + url;

                        return url;
                    }
                    else return string.Empty;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In App.Server.GetCommandValue. Message : " + ex.Message
                        + System.Environment.NewLine + ex.StackTrace);
                    if (File.Exists(filename))
                        File.Delete(filename);
                    return string.Empty;
                }
            }

            public static Dictionary<int,string> GetCommandValues(int[] command_ids, int[] item_ids, string type,
                ref CancellationToken cancellationToken)
            {
                logger.Info("Get Command values. commandId={0}, type = {1},  itemIDs =[{2}]",
                    command_ids, type,  item_ids.ToString());
                string response = CreateCommand("get_command_value", command_ids);

                ManualResetEvent resetEvent = new ManualResetEvent(false);

                RequestToken request = new RequestToken(resetEvent, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return new Dictionary<int, string>();

                request.Request = response;

                request.Queue();

                request.RequestResetEvent.WaitOne();

                response = (string)request.Result;

                if (cancellationToken.IsCancellationRequested)
                    return new Dictionary<int, string>();

                string filename = string.Empty;
                Gallery.Profile data = new Gallery.Profile();
                if (cancellationToken.IsCancellationRequested)
                    return new Dictionary<int, string>();
                Dictionary<int, string> urls = new Dictionary<int, string>();
                try
                {
                    if (GetError(response) == "none")
                    {
                        var serverobj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(response);
                        var dataobj = JSON.API.GetData(serverobj.data, 0);
                        for(int i =0; i< command_ids.Length;i++)
                        {
                            int id = command_ids[i];
                            data = ((dataobj as Newtonsoft.Json.Linq.JContainer)["data"])[id.ToString()]
                            .ToObject(typeof(Gallery.Profile)) as Gallery.Profile;
                            string url = string.Empty;
                            if (data != null)
                            {
                                url = data.data;
                                url = "http://" + App.Settings.Server_IP + ":" + App.Settings.WebClient_Port + url;
                            }
                            urls.Add(item_ids[i], url);
                        }
                        /*data = ((dataobj as Newtonsoft.Json.Linq.JContainer)["data"])[command_id.ToString()]
                            .ToObject(typeof(Gallery.Profile)) as Gallery.Profile;
                            */
                        
                    }
                    else return new Dictionary<int, string>();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In App.Server.GetCommandValue. Message : " + ex.Message
                        + System.Environment.NewLine + ex.StackTrace);
                    if (File.Exists(filename))
                        File.Delete(filename);
                    return new Dictionary<int, string>();
                }
                return urls;
            }

            public static string GetCommandState(int command_id,ref CancellationToken cancellationToken)
            {
                logger.Info("Get Command State. commandId={0}", command_id);

                string response = CreateCommand("get_command_state", command_id);
                ManualResetEvent resetEvent = new ManualResetEvent(false);

                RequestToken request = new RequestToken(resetEvent, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;

                request.Request = response;

                request.Queue();

                request.RequestResetEvent.WaitOne();

                response = (string)request.Result;

                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;
                string state = string.Empty;
                string error = GetError(response);
                if (error == "none")
                {
                    var serverobj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(response);
                    var dataobj = JSON.API.GetData(serverobj.data, 0);
                    var data = ((dataobj as Newtonsoft.Json.Linq.JContainer)["data"])[command_id.ToString()]
                        .ToString();
                    state = data;
                    logger.Info("Get Command State Successful. commandId={0}, state={1}", command_id, state);
                    return state;
                }
                logger.Info("Get Command State Successful. commandId={0}, \n error={1}", command_id, error);
                return (error);
            }

            public static bool GetCompleted(out List<int> done, List<int> command_ids, Gallery.HPXItem[] hPXItem, ref CancellationToken cancellationToken)
            {
                done = new List<int>();
                List<int> failed = new List<int>();
                logger.Info("Get completed. commandId={0}", command_ids);

                string response = CreateCommand("get_command_state", command_ids.ToArray());
                ManualResetEvent resetEvent = new ManualResetEvent(false);

                RequestToken request = new RequestToken(resetEvent, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return false;

                request.Request = response;

                request.Queue();

                request.RequestResetEvent.WaitOne();

                response = (string)request.Result;

                if (cancellationToken.IsCancellationRequested)
                    return false;
                string state = string.Empty;
                int count = command_ids.Count;
                string error = GetError(response);
                if (error == "none")
                {
                    var serverobj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(response);
                    var dataobj = JSON.API.GetData(serverobj.data, 0);
                    for (int i = 0; i < command_ids.Count; i++)
                    {
                        int id = command_ids[i];
                        var data = ((dataobj as Newtonsoft.Json.Linq.JContainer)["data"])[id.ToString()]
                        .ToString();
                        if (!done.Contains(id))
                        {
                            if (data.ToLower() == "finished")
                            {
                                done.Add(id);
                                foreach (var hp in hPXItem)
                                {
                                    if (id == hp.CommandId)
                                    {
                                        hp.Image.IsReady = true;
                                        Task.Run(() =>
                                        {
                                            hp.Image.RequestLoad();
                                        }, cancellationToken);
                                    }
                                }
                            }
                            else
                              if (data.ToLower().Contains("fail") || data.ToLower().Contains("stopped")
                                || data.ToLower().Contains("out_of_service"))
                                failed.Add(id);
                        }
                    }
                    if((done.Count >= count) || (failed.Count+done.Count >= count))
                    return true;
                   /* 
                    state = data;
                    logger.Info("Get Command State Successful. commandId={0}, state={1}", command_id, state);*/
                    return false;
                }
                return true;
            }

            public static T GetItem<T>(int item_id, Gallery.ItemType itemType,CancellationToken cancellationToken)
            {
                logger.Info("Get Item. itemId={0}, type = {1}", item_id, itemType.ToString());
                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
                JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest? "guest" : Core.App.Settings.Username);
                JSON.API.PushKey(ref main, "session", App.Server.Info.session);
                JSON.API.PushKey(ref funct, "fname", "get_item");
                JSON.API.PushKey(ref funct, "item_id", "<int>" + item_id);
                JSON.API.PushKey(ref funct, "item_type", itemType.ToString());
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);
                ManualResetEvent resetEvent = new ManualResetEvent(false);

                RequestToken request = new RequestToken(resetEvent, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return default(T);

                request.Request = response;

                request.Queue();

                request.RequestResetEvent.WaitOne();

                response = (string)request.Result;

                if (cancellationToken.IsCancellationRequested)
                    return default(T);

                var obj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(response);

                var array = obj.data as Newtonsoft.Json.Linq.JArray;
                
                    var data = array[0].ToObject<JSON.DataObject>();
                    var sett = data.data as Newtonsoft.Json.Linq.JObject;
                    return sett.ToObject<T>();

            }

            public static List<T> GetRelatedItems<T>(int item_id,CancellationToken cancellationToken, Gallery.ItemType itemType
                , Gallery.ItemType relatedType, int limit = -1, int page = 0)
            {
                logger.Info("Get Item. itemId={0}, related_type = {1}, limit = {2}", item_id, relatedType.ToString(), limit);
                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
                JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest? "guest" : Core.App.Settings.Username);
                JSON.API.PushKey(ref main, "session", App.Server.Info.session);
                JSON.API.PushKey(ref funct, "fname", "get_related_items");
                JSON.API.PushKey(ref funct, "item_id", "<int>" + item_id);
                JSON.API.PushKey(ref funct, "limit", "<int>" + limit);
                JSON.API.PushKey(ref funct, "offset", "<int>" + (limit > 0 ? page * limit : 0));
                JSON.API.PushKey(ref funct, "item_type", itemType.ToString());
                JSON.API.PushKey(ref funct, "related_type", relatedType.ToString());
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);

                ManualResetEvent resetEvent = new ManualResetEvent(false);

                RequestToken request = new RequestToken(resetEvent, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return new List<T>();

                request.Request = response;

                request.Queue();

                request.RequestResetEvent.WaitOne();

                response = (string)request.Result;

                if (cancellationToken.IsCancellationRequested)
                    return new List<T>();

                string countstring = response;
                var obj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(countstring);
                var array = obj.data as Newtonsoft.Json.Linq.JArray;
                List<T> list = new List<T>();
                try
                {
                    if (array != null & array.Count > 0)
                    {
                        var data = array[0].ToObject<JSON.DataObject>();
                        var rdata = data.data as Newtonsoft.Json.Linq.JArray;

                        list = rdata.ToObject<List<T>>();
                    }
                }
                catch (Exception ex)
                {

                }

                return list;
            }

            public static int GetRelatedCount(int item_id, CancellationToken cancellationToken, Gallery.ItemType itemType
                , Gallery.ItemType relatedType)
            {
                logger.Info("Get Related Count. itemId={0}, related_type = {1},", item_id, relatedType.ToString());
                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();

                JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest? "guest" : Core.App.Settings.Username);
                JSON.API.PushKey(ref main, "session", App.Server.Info.session);
                JSON.API.PushKey(ref funct, "fname", "get_related_count");
                JSON.API.PushKey(ref funct, "item_id", "<int>" + item_id);
                JSON.API.PushKey(ref funct, "item_type", itemType.ToString());
                JSON.API.PushKey(ref funct, "related_type", relatedType.ToString());
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);
                ManualResetEvent resetEvent = new ManualResetEvent(false);

                RequestToken request = new RequestToken(resetEvent, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return 0;

                request.Request = response;

                request.Queue();

                request.RequestResetEvent.WaitOne();

                response = (string)request.Result;

                if (cancellationToken.IsCancellationRequested)
                    return 0;
                var serverobj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(response);

                if (JSON.API.GetData(serverobj.data, 0) is JSON.ServerObjects.IntegerObject countdata)
                    return countdata.count;
                else return 0;
            }

            public static string ParseItem(string json)
            {
                string d = string.Empty;
                try
                {
                    d = json.Substring(json.IndexOf("[") + 1, json.LastIndexOf("]") - json.IndexOf("[") - 1);
                    d = d.Substring(d.IndexOf(": {") + 1);
                    d = d.Remove(d.LastIndexOf("}"));
                    if (d.LastIndexOf("}") != d.Length - 1)
                        d = d.Remove(d.LastIndexOf("}") + 1);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "\n Exception Caught In App.Server.ParseItem. JSONString = {0}", json);
                }
                return d;
            }

            public static string GetError(string datajson)
            {
                if (datajson.Contains("\"error\""))
                {
                    string error = string.Empty;
                    string temp = datajson.Substring(datajson.IndexOf("error"));
                    temp = temp.Substring(temp.IndexOf("code"));
                    error = temp.Substring(temp.IndexOf(":") + 1, temp.IndexOf("}") - temp.IndexOf(":"));
                    error = error.Trim(' ');
                    error += ":";
                    temp = datajson.Substring(datajson.IndexOf("msg"));
                    error += temp.Substring(temp.IndexOf(":") + 1, temp.IndexOf("}") - temp.IndexOf(":"));
                    error = error.Trim('\n');
                    return "error " + error;
                }
                else return "none";
            }

            public static string HashGenerator(string size,string type, int item_id = 0)
            {
                string feed = Info.name;
                feed += "-" + type;
                feed += "-" + size;
                feed += item_id == 0 ? "" : item_id.ToString();
                byte[] feedbyte = Encoding.Unicode.GetBytes(feed);
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(feedbyte);
                    StringBuilder builder = new StringBuilder();

                    foreach (byte b in hash)
                        builder.Append(b.ToString("x2").ToLower());

                    return builder.ToString();
                }
            }
        }
    } 
}