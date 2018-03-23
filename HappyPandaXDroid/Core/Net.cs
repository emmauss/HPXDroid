using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

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
        public static bool Connected = false;
        

        public static bool IsServerReachable()
        {
            bool res = false;
            try
            {
                var pinger = new System.Net.NetworkInformation.Ping();

                if (pinger.Send(App.Settings.Server_IP).Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return res;
        }

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

        public static void Disconnect()
        {
            if (client != null)
                if (client.client.Connected)
                {
                    client.client.Client.Disconnect(false);
                    client.client.Dispose();
                    client = null;
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
                if (client != null)
                {
                    client.client.Client.Disconnect(false);
                    client.client.Dispose();
                }
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
            if (App.Server.Info.session == null || App.Server.Info.session == string.Empty)
            {
                try
                {
                    logger.Info("Connection Successful. Starting Handshake");
                    string response = string.Empty,pay = string.Empty;
                    
                    var stream = listener.GetStream();
                    byte[] decom = Receive(stream);
                    pay = Encoding.UTF8.GetString(decom).TrimEnd('\0');
                    App.Server.Info = JSON.Serializer.SimpleSerializer.Deserialize<App.Server.ServerInfo>(pay);
                    List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                    JSON.API.PushKey(ref main, "name", "test");
                    JSON.API.PushKey(ref main, "session", "");
                    JSON.API.PushKey(ref main, "data", "{}");
                    string request = JSON.API.ParseToString(main);
                    Send(Encoding.UTF8.GetBytes(request), stream);
                    
                    response = string.Empty;
                    
                    decom = Receive(stream);
                    pay = Encoding.UTF8.GetString(decom).TrimEnd('\0');
                    bool success = false;
                    if (!pay.Contains("Authenticated"))
                    {
                        logger.Info("Handshake Failed");
                        success = true;
                        response = "fail";
                    }
                    else
                    {
                        logger.Info("Handshake Successful");
                        pay = pay.Replace("<EOF>", "");
                        Dictionary<string, string> reply =
                            JSON.Serializer.SimpleSerializer.Deserialize<Dictionary<string, string>>(pay);
                        success = reply.TryGetValue("session", out App.Server.Info.session);
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
        static void Send(byte[] packet,NetworkStream stream)
        {
            byte[] req = IO.Compression.Compress(packet);
            byte[] res = new byte[512];
            var eof = Encoding.UTF8.GetBytes("<EOF>");
            stream.Write(req, 0, req.Length);
            stream.Write(eof, 0, eof.Length);
            Array.Clear(res, 0, res.Length);
        }

        static byte[] Receive(NetworkStream stream)
        {
            List<byte> payload = new List<byte>();
            byte[] res = new byte[4];
            stream.Read(res, 0, res.Length);
            payload.AddRange(res);
            res = new byte[512];
            while (stream.DataAvailable)
            {

                stream.Read(res, 0, res.Length);
                payload.AddRange(TrimEnd(res));
                Array.Clear(res, 0, res.Length);
                Thread.Sleep(100);
            }
            byte[] decom = TrimEOF(payload.ToArray());
            decom = IO.Compression.Decompress(decom);
            return decom;
        } 

        static byte[] TrimEnd(byte[] payload)
        {
            int lastIndex = Array.FindLastIndex(payload, a => a != 0);
            Array.Resize(ref payload, lastIndex + 1);
            return payload;
        }

        static byte[] TrimEOF(byte[] payload)
        {
            Array.Resize(ref payload, payload.Length - 5);
            return payload;
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

        public  static string SendPost(string request,ref CancellationToken cancellationToken)
        {
            logger.Info("Sending Request.\n Request : \n {0} \n", request);
            string response = "fail",pay = string.Empty;
            var payload = new List<byte>();
            lock (locker)
            {
                Client listener = GetActiveConnection();
                listener.client.GetStream().ReadTimeout = 20000;
                listener.client.GetStream().WriteTimeout = 20000;
                if (cancellationToken.IsCancellationRequested)
                    return string.Empty;
                try
                {
                    if (App.Server.Info.session == null || App.Server.Info.session == string.Empty || client==null | !client.client.Connected)
                        Connect();
                    if (App.Server.Info.session != null && App.Server.Info.session != string.Empty && client!=null & client.client.Connected)
                    {
                        var stream = listener.client.GetStream();
                        Send(Encoding.UTF8.GetBytes(request), stream);
                        response = string.Empty;
                        var decomp = Receive(stream);
                        logger.Info("Received response from server");
                        response = Encoding.UTF8.GetString(decomp);
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
            if (App.Server.GetError(response).Contains("expire"))
            {
                Disconnect();
                Connect();
                return SendPost(request,ref cancellationToken);
            }
            else
            return response;
        }
        
    }
}