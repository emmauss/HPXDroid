using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Net;
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

            public static string username = "test";
            public static string basePath = string.Empty;
            public static string cache = basePath + "cache/";
            public static string Log = basePath + "log/";
            public static bool Refresh = false;
            static string serverip = "127.0.0.1";
            static string port = "7007";
            static int webport = 7008;

            public static string Server_IP => serverip;

            public static int WebClient_Port => webport;

            public static string Server_Port => port;

            static int loopdelay = 500;

            public static int Loop_Delay
            {
                get => loopdelay;
                set => loopdelay = value;
            }
        }

        public class Server
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public static ServerInfo info = new ServerInfo();
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

            public static string StartCommand(int command_id)
            {
                logger.Info("Start Command. commandId={0}", command_id);
                string response = CreateCommand("start_command", command_id);
                response = Net.SendPost(response);
                string state = string.Empty;
                if (GetError(response) == "none")
                {

                    state = JSON.API.GetData(response, 2);
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

            public static string StopCommand(int command_id)
            {
                logger.Info("Stop Command. commandId={0}", command_id);
                string response = CreateCommand("stop_command", command_id);
                response = Net.SendPost(response);
                string state = string.Empty;
                if (GetError(response) == "none")
                {
                    state = JSON.API.GetData(response, 2);
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

            public static string UndoCommand(int command_id)
            {

                string response = CreateCommand("undo_command", command_id);
                response = Net.SendPost(response);
                string state = string.Empty;
                if (GetError(response) == "none")
                {
                    state = JSON.API.GetData(response, 2);
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

                JSON.API.PushKey(ref main, "name", Settings.username);
                JSON.API.PushKey(ref main, "session", Net.session_id);
                JSON.API.PushKey(ref funct, "fname", key);
                JSON.API.PushKey(ref funct, "command_ids", "[" + command_id + "]");
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);
                
                return response;
            }

            public static int GetCommandId(int item_id, string command_response)
            {
                if (command_response.Contains("\"" + item_id + "\""))
                {
                    string command = JSON.API.GetData(command_response, 2);
                    string id = command.Substring(command.IndexOf(":"));
                    id = id.Substring(id.IndexOf(":") + 1, id.IndexOf("}") - 1);
                    id = id.Trim('\"');
                    id = id.Trim(' ');
                    return int.Parse(id);
                }
                else return -1;
            }

            public class ErrorObject
            {
                public int code;
                public string msg;
            }



            public static string GetCommandValue(int command_id, int item_id, string name, string type, bool return_url,bool IsPreview = false)
            {
                logger.Info("Get Command value. commandId={0}, type = {1}, url = {2}, itemID ={3}",
                    command_id, type, return_url.ToString(), item_id.ToString());
                string response = CreateCommand("get_command_value", command_id);
                response = Net.SendPost(response);
                string filename = string.Empty, data = string.Empty;
                
                try
                {
                    if (GetError(response) == "none")
                    {

                        data = JSON.API.GetData(response, 2);
                        data = data.Substring(data.IndexOf(":") + 1);
                        data = data.Remove(data.LastIndexOf("}"));
                        string dir = Settings.cache;
                        switch (type)
                        {
                            case "thumb":
                                dir += "thumbs/";
                                break;
                            case "page":
                                if (IsPreview)
                                    dir += "preview/";
                                else
                                    dir += "pages/";
                                break;

                        }
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                        var profiledata = JSON.Serializer.SimpleSerializer.Deserialize<Gallery.Profile>(data);
                        filename = dir + name + ".jpg";
                        string url = profiledata.data;
                        url = "http://" + App.Settings.Server_IP + ":"+ App.Settings.WebClient_Port + url;
                        if (return_url)
                            return url;
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
                    return "fail";
                }
            }

            public static string GetCommandState(int command_id)
            {
                logger.Info("Get Command State. commandId={0}", command_id);

                string command = CreateCommand("get_command_state", command_id);
                string response = Net.SendPost(command);
                string state = string.Empty;
                string error = GetError(response);
                if (error == "none")
                {
                    state = JSON.API.GetData(response, 2);
                    logger.Info("Get Command State Successful. commandId={0}, state={1}", command_id, state);
                    return state;
                }
                logger.Info("Get Command State Successful. commandId={0}, \n error={1}", command_id, error);
                return (error);

            }

            public static T GetItem<T>(int item_id, string type)
            {
                logger.Info("Get Item. itemId={0}, type = {1}", item_id, type);
                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
                JSON.API.PushKey(ref main, "name", "test");
                JSON.API.PushKey(ref main, "session", Net.session_id);
                JSON.API.PushKey(ref funct, "fname", "get_item");
                JSON.API.PushKey(ref funct, "item_id", "<int>" + item_id);
                JSON.API.PushKey(ref funct, "item_type", type);
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);
                string responsestring = Net.SendPost(response);

                responsestring = responsestring.Remove(responsestring.LastIndexOf("<EOF>"));
                var obj = JSON.Serializer.SimpleSerializer.Deserialize<JSON.ServerObject>(responsestring);
                var array = obj.data as Newtonsoft.Json.Linq.JArray;
                
                    var data = array[0].ToObject<JSON.DataObject>();
                    var sett = data.data as Newtonsoft.Json.Linq.JObject;
                    return sett.ToObject<T>();

            }

            public static List<T> GetRelatedItems<T>(int item_id, string related_type = "Page", int limit = -1)
            {
                logger.Info("Get Item. itemId={0}, related_type = {1}, limit = {2}", item_id, related_type, limit);
                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();
                JSON.API.PushKey(ref main, "name", "test");
                JSON.API.PushKey(ref main, "session", Net.session_id);
                JSON.API.PushKey(ref funct, "fname", "get_related_items");
                JSON.API.PushKey(ref funct, "item_id", "<int>" + item_id);
                JSON.API.PushKey(ref funct, "limit", "<int>" + limit);
                JSON.API.PushKey(ref funct, "related_type", related_type);
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);

                response = Net.SendPost(response);
                string countstring = response;
                countstring = countstring.Remove(countstring.LastIndexOf("<EOF>"));
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

            public int GetRelatedCount(int item_id, string related_type = "Page")
            {
                logger.Info("Get Related Count. itemId={0}, related_type = {1},", item_id, related_type);
                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                List<Tuple<string, string>> funct = new List<Tuple<string, string>>();

                JSON.API.PushKey(ref main, "name", "test");
                JSON.API.PushKey(ref main, "session", Net.session_id);
                JSON.API.PushKey(ref funct, "fname", "get_related_count");
                JSON.API.PushKey(ref funct, "item_id", "<int>" + item_id);
                JSON.API.PushKey(ref funct, "related_type", related_type);
                string response = JSON.API.ParseToString(funct);
                JSON.API.PushKey(ref main, "data", "[\n" + response + "\n]");
                response = JSON.API.ParseToString(main);
                string countstring = Net.SendPost(response);
                string countdata = JSON.API.GetData(countstring, 2);
                countdata = countdata.Substring(countdata.IndexOf(":") + 1, countdata.IndexOf("}") - countdata.IndexOf(":") - 1);
                int.TryParse(countdata, out int count);
                return count;
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
                string feed = info.name;
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


            public struct KeyPair
            {
                public string Key;
                public string Value;
            }
        }
        /*public class Threading
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public static List<Thread> ThreadPool = new List<Thread>();
            public static List<Thread> PendingPool = new List<Thread>();
            public static bool Updating = false;
            public static int ThreadID = 0;
            public static bool RunScheduler = true;
            public class Thread
            {
                public enum ThreadState
                {
                    Created = 1,
                    Pending = 2,
                    Running = 3,
                    Aborting = 4,
                    Aborted = 5,
                    RunToEnd = 6
                }
                public Thread(string Activity)
                {
                    ActivityClass = Activity;
                    ThreadId = ThreadID++;
                }
                public static Random IdGen = new Random(5);
                public System.Threading.Thread thread;
                public int ActitivtyID;
                public string ActivityClass;
                public int ThreadId;
                public bool AbortRequested = false;
                public int Priority = 0;
                public ThreadState Status = ThreadState.Created;
                public void Abort()
                {
                    try
                    {
                        AbortRequested = true;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "\n Exception Caught In App.Threading.Thread.Abort().");
                    }
                }

                public void Start()
                {
                    try
                    {
                        if (!thread.IsAlive)
                            thread.Start();
                    }catch(ThreadStateException ex)
                    {
                       
                    }
                    Status = ThreadState.Running;
                }
            }


            public static Thread CreateThread(Action action, int activityId, string ActivityClass)
            {
                Thread thread = new Thread(ActivityClass);
                ThreadStart thrds = new ThreadStart(action);
                thread.thread = new System.Threading.Thread(thrds);
                thread.ActitivtyID = activityId;
                thread.Status = Thread.ThreadState.Pending;
                return thread;
            }

            public static void StartThread(Thread thread)
            {
                if (thread.Status == Thread.ThreadState.Pending)
                {
                    logger.Info("Thread {0} Dispatched", thread.ThreadId);
                    thread.Start();

                }
            }

            public static void AbortThread(Thread thread)
            {
                if (thread.thread.IsAlive)

                    thread.Abort();
            }

            public static void CleanUp()
            {
                
                if (ThreadPool.Count > 0)
                {

                    int count = ThreadPool.Count;
                    Thread[] threads = new Thread[10];
                    try
                    {
                        threads = ThreadPool.ToArray();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "\n Exception Caught In App.Threading.CleanUp.");
                        return;
                    }
                    foreach (var thread in threads)
                    {
                        if (thread.Status == Thread.ThreadState.Aborted
                            || thread.Status == Thread.ThreadState.RunToEnd)
                        {
                            ThreadPool.Remove(thread);
                        }
                    }
                }
            }

            public static void Schedule(Thread thread)
            {
                logger.Info("Thread {0} added to schedule", thread.ThreadId);
                thread.Status = Thread.ThreadState.Pending;
                thread.Priority = 1;
                while (Updating)
                    System.Threading.Thread.Sleep(200);
                PendingPool.Add(thread);
            }

            public static void ElevateActivityPriority(int activityId)
            {
                if (ThreadPool.Count > 0)
                {
                    int count = ThreadPool.Count;
                    Thread[] threads = new Thread[10];
                    try
                    {
                        threads = ThreadPool.ToArray();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "\n Exception Caught In App.Threading.ElevateActivityPriority.");
                        return;
                    }
                    foreach (var thread in threads)
                    {
                        if (thread.ActitivtyID != activityId)
                        {
                            thread.Priority++;
                            if (thread.Priority > 5)
                                thread.Priority = 5;
                        }
                        else
                        {
                            thread.Priority = 1;
                        }
                    }
                }
            }

            public static void DispatchNextThreads()
            {
                int runningThreads = 0;
                
                if (ThreadPool.Count > 0)
                {
                    int count = ThreadPool.Count;
                    Thread[] threads = new Thread[10];
                    try
                    {
                        threads = ThreadPool.ToArray();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "\n Exception Caught In App.Threading.DispatchNextThreads.");
                        return;
                    }
                    foreach (var thread in threads)
                    {
                        if (thread.Status == Thread.ThreadState.Running)
                            runningThreads++;
                    }
                    if (runningThreads < App.Settings.BackgroundThreadLimit)
                    {
                        int threadsToStart = Settings.BackgroundThreadLimit - runningThreads;
                        for (int i = 1; i <= threadsToStart; i++)
                        {
                            Thread HighestPriorityThread;
                            if (ThreadPool.Count > 0)
                            {
                                HighestPriorityThread = ThreadPool.Find((x) => x.Status == Thread.ThreadState.Pending && !x.thread.IsAlive);
                                if (HighestPriorityThread == null)
                                    break;
                                count = ThreadPool.Count;


                                foreach (var thread in ThreadPool)
                                {
                                    if (thread.Status == Thread.ThreadState.Pending && thread.Priority < HighestPriorityThread.Priority 
                                        )
                                    {
                                        HighestPriorityThread = thread;
                                    }
                                }
                                if (HighestPriorityThread != null)
                                    StartThread(HighestPriorityThread);
                            }
                            else
                                return;
                        }
                    }
                }
            }

            public static void SchedulerTask()
            {
                while (RunScheduler)
                {
                    UpdateStatus();
                    AbortThreads();
                    CleanUp();


                    DispatchNextThreads();
                    UpdateList();
                }
            }

            public static void UpdateList()
            {
                if (PendingPool.Count > 0)
                {
                    Updating = true;
                    System.Threading.Thread.Sleep(100);
                    ThreadPool.AddRange(PendingPool);
                    PendingPool.Clear();
                    Updating = false;
                }
            }

            public static void AbortActivityThreads(int activityId, string ActivityType)
            {
                logger.Info("Abort Activity Child Threads Called. Activity = {0}", ActivityType);
                if (ThreadPool.Count > 0)
                {
                    int count = ThreadPool.Count;
                    Thread[] threads = new Thread[10];
                    try
                    {
                        threads = ThreadPool.ToArray();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "\n Exception Caught In App.Threading.AbortActivityThreads.");
                        return;
                    }
                    foreach (var thread in threads)
                    {
                        if (thread.ActitivtyID == activityId)
                            thread.AbortRequested = true;
                    }
                }
            }

            public static void AbortThreads()
            {
                if (ThreadPool.Count > 0)
                {
                    int count = ThreadPool.Count;
                    Thread[] threads = new Thread[10];
                    try
                    {
                        threads = ThreadPool.ToArray();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "\n Exception Caught In App.Threading.AbortThreads().");
                        return;
                    }
                    foreach (var thread in threads)
                    {
                        if (thread.AbortRequested && thread.Status != Thread.ThreadState.Aborted
                            && thread.thread.IsAlive)
                        {
                            thread.thread.Abort();
                            thread.Status = Thread.ThreadState.Aborting;
                        }
                    }
                }
                PendingPool.Clear();
            }

            public static void UpdateStatus()
            {
                if (ThreadPool.Count > 0)
                {
                    int count = ThreadPool.Count;
                    Thread[] threads = new Thread[10];
                    try
                    {
                        threads = ThreadPool.ToArray();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "\n Exception Caught In App.Threading.UpdateStatus.");
                        return;
                    }
                    foreach (var thread in threads)
                    {
                        switch (thread.Status)
                        {
                            case Thread.ThreadState.Aborting:
                                if (!thread.thread.IsAlive)
                                    thread.Status = Thread.ThreadState.Aborted;
                                break;
                            case Thread.ThreadState.Running:
                                if (!thread.thread.IsAlive || thread.thread.ThreadState == ThreadState.Stopped)
                                    thread.Status = Thread.ThreadState.RunToEnd;
                                break;
                        }
                    }
                }
            }

            public static void InitScheduler()
            {
                logger.Info("Initializing Scheduler");
                ThreadPool.Clear();
                ThreadPool = new List<Thread>();
                ThreadStart thrds = new ThreadStart(SchedulerTask);
                System.Threading.Thread thread = new System.Threading.Thread(thrds);
                //thread.Start();
            }

            public static void Close()
            {
                RunScheduler = false;
                CleanUp();
                AbortThreads();
                ThreadPool.Clear();
                
            }
        }*/

    }
}