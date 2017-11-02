using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NLog;

namespace HappyPandaXDroid.Core
{
    class Net
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static string session_id = string.Empty;
        public static bool Connected = false;
        

        public class Client
        {
            public bool InUse = false;
            public bool initialise = false;
            public TcpClient client;
            public Client()
            {
                client = new TcpClient(App.Settings.Server_IP, int.Parse(App.Settings.Server_Port));
                string response = Connect(this);
            }
        }

        public static bool Connect()
        {
            
            try
            {
                if(client!=null)
                if (client.client.Connected)
                {
                    return true;
                }
                client = null;
                logger.Info("Connecting to server ...");
                client = new Client();
                Connected = client.client.Connected;
                return Connected;
            }catch(SocketException sex)
            {
                logger.Error(sex, "\n Exception Caught In Net.Connect.");
                client.client.Client.Disconnect(false);
                client.client.Dispose();
                client = null;
                return false;

            }
            catch(Exception ex)
            {
                return false;
            }
        }

        static Client client;

        static string Connect(Client cli)
        {
            var listener = cli.client;
            if (session_id == null || session_id == string.Empty)
            {
                try
                {
                    logger.Info("Connection Successful. Starting Handshake");
                    string response = string.Empty;
                    string payload;
                    var stream = listener.GetStream();
                    byte[] res = new byte[1024];
                    while (true)
                    {
                        stream.Read(res, 0, res.Length);
                        payload = Encoding.UTF8.GetString(res).TrimEnd('\0');
                        response += payload;
                        if (response.Contains("<EOF>"))
                            break;
                        Array.Clear(res, 0, res.Length);
                    }
                    payload = payload.Replace("<EOF>", "");
                    App.Server.info = JSON.Serializer.SimpleSerializer.Deserialize<App.Server.ServerInfo>(payload);
                    List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                    JSON.API.PushKey(ref main, "name", "test");
                    JSON.API.PushKey(ref main, "session", "");
                    JSON.API.PushKey(ref main, "data", "{}");
                    string request = JSON.API.ParseToString(main);
                    byte[] req = Encoding.UTF8.GetBytes(request + "<EOF>");
                    stream.Write(req, 0, req.Length);
                    Array.Clear(res, 0, res.Length);
                    response = string.Empty;
                    while (true)
                    {
                        
                        stream.Read(res, 0, res.Length);
                        payload = Encoding.UTF8.GetString(res).TrimEnd('\0');
                        response += payload;
                        if (response.Contains("<EOF>"))
                            break;
                        Array.Clear(res, 0, res.Length);
                    }
                    payload = response;
                    bool success = false;
                    if (!payload.Contains("Authenticated"))
                    {
                        logger.Info("Handshake Failed");
                        success = true;
                        response = "fail";
                    }
                    else
                    {
                        logger.Info("Handshake Successful");
                        payload = payload.Replace("<EOF>", "");
                        Dictionary<string, string> reply =
                            JSON.Serializer.SimpleSerializer.Deserialize<Dictionary<string, string>>(payload);
                        success = reply.TryGetValue("session", out session_id);
                    }
                    cli.initialise = true;
                    return response;
                }
                catch (SocketException ex)
                {
                    logger.Error(ex, "\n Exception Caught In Net.Connect.");
                    client.client.Client.Disconnect(false);
                    client.client.Dispose();
                    client = null;
                    return "fail";
                }
            }
            else return null;
}


        static Client GetActiveConnection()
        {
            if (client == null)
            {
                client = new Client();
            }

            return client;                
            
        }
        public static object locker = new object();

        public  static string SendPost(string payload)
        {
            logger.Info("Sending Request.\n Request : \n {0} \n", payload);
            string response = "fail";
            lock (locker)
            {
                Client listener = GetActiveConnection();
                listener.client.GetStream().ReadTimeout = 30000;
                listener.client.GetStream().WriteTimeout = 30000;
                try
                {
                    if (session_id == null || session_id == string.Empty || client==null | !client.client.Connected)
                        Connect();
                    if (session_id != null && session_id != string.Empty && client!=null & client.client.Connected)
                    {
                        var stream = listener.client.GetStream();
                        byte[] req = Encoding.UTF8.GetBytes(payload + "<EOF>");
                        byte[] res = new byte[1024 * 10];
                        stream.Write(req, 0, req.Length);
                        Array.Clear(res, 0, res.Length);
                        response = string.Empty;
                        while (true)
                        {
                            stream.Read(res, 0, res.Length);
                            string reply = Encoding.UTF8.GetString(res).TrimEnd('\0');
                            response += reply;
                            if (response.Contains("<EOF>"))
                                break;
                            Array.Clear(res, 0, res.Length);
                        }
                        logger.Info("Received response from server");
                    }

                }
                catch (System.Exception ex)
                {
                    client.client.Client.Disconnect(false);
                    client.client.Dispose();
                    client = null;
                    logger.Error(ex, "\n Exception Caught In Net.SendPost.");

                }
                
            }
            return response;
        }
        
    }
}