using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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

        static List<Client> ConnectedClients = new List<Client>();

        public static bool IsServerReachable()
        {
            bool res = false;
            try
            {
                var pinger = new System.Net.NetworkInformation.Ping();

                var status = pinger.Send(App.Settings.Server_IP).Status;
                if (status == System.Net.NetworkInformation.IPStatus.Success)
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
                if (response != "fail")
                    initialise = true;
            }

            public void Disconnect()
            {
                client?.Close();
                client?.Dispose();
            }
        }

        public static void DisconnectAllConnections()
        {
            foreach (var client in ConnectedClients)
            {
                client.Disconnect();
            }

            ConnectedClients.Clear();
        }

        public static bool Reconnect()
        {
            App.Server.Info = new App.Server.ServerInfo();
            DisconnectAllConnections();
            return Connect();
        }
        public static bool Connect()
        {
            Client client = null;
            try
            {
                if (ConnectedClients.Exists((x) => x?.client?.Connected == true))
                    return true;
                client = null;
                logger.Info("Connecting to server ...");
                client = new Client();
                Connected = (bool)client.client?.Connected;
                if (Connected)
                {
                    ConnectedClients.Add(client);
                    CreateConnections();
                }

                Media.Cache.InitializeGalleryCache();

                return Connected;
            }
            catch (SocketException sex)
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
            catch (Exception ex)
            {
                return false;
            }
        }

        static string Connect(Client cli)
        {
            var listener = cli.client;
            try
            {
                logger.Info("Connection Successful. Starting Handshake");
                string response = string.Empty, pay = string.Empty;

                var stream = listener.GetStream();
                byte[] decom = Receive(stream);
                pay = Encoding.UTF8.GetString(decom).TrimEnd('\0');
                if (!string.IsNullOrEmpty(App.Server.Info.session))
                    return null;
                App.Server.Info = JSON.Serializer.SimpleSerializer.Deserialize<App.Server.ServerInfo>(pay);
                if ((bool)!App.Server.Info?.data.guest_allowed && App.Settings.IsGuest)
                {
                    DisconnectAllConnections();
                    cli.Disconnect();
                    App.Server.Info = new App.Server.ServerInfo();
                    return "fail";
                }
                List<Tuple<string, string>> main = new List<Tuple<string, string>>();
                JSON.API.PushKey(ref main, "name", Core.App.Settings.IsGuest ? "guest" : Core.App.Settings.Username);
                JSON.API.PushKey(ref main, "session", App.Server.Info.session);
                if (App.Settings.IsGuest)
                    JSON.API.PushKey(ref main, "data", "{}");
                else
                {
                    List<Tuple<string, string>> data = new List<Tuple<string, string>>();
                    JSON.API.PushKey(ref data, "user", App.Settings.Username);
                    JSON.API.PushKey(ref data, "password", App.Settings.Password);
                    string datastring = JSON.API.ParseToString(data);
                    JSON.API.PushKey(ref main, "data", "<int>\n" + datastring + "\n");
                }
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
                    listener.Close();
                    listener.Dispose();
                }
                else
                {
                    logger.Info("Handshake Successful");
                    pay = pay.Replace("<EOF>", "");
                    Dictionary<string, string> reply =
                        JSON.Serializer.SimpleSerializer.Deserialize<Dictionary<string, string>>(pay);
                    success = reply.TryGetValue("session", out App.Server.Info.session);
                    cli.initialise = true;
                }

                return response;
            }
            catch (SocketException ex)
            {
                logger.Error(ex, "\n Exception Caught In Net.Connect.");
                cli.client.Client.Disconnect(false);
                cli.client.Dispose();
                cli = null;
                return "fail";
            }
        }
        static void Send(byte[] packet, NetworkStream stream)
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
            byte[] res = new byte[1024 * 1024 * 10];
            int read = stream.Read(res, 0, res.Length);
            for (int i = 0; i < read; i++)
                payload.Add(res[i]);
            res = new byte[512];
            byte[] eof = Encoding.ASCII.GetBytes("<EOF>");

            Thread.Sleep(App.Settings.Loop_Delay);

            while (stream.DataAvailable)
            {
                read = stream.Read(res, 0, res.Length);
                for (int i = 0; i < read; i++)
                    payload.Add(res[i]);
                Array.Clear(res, 0, res.Length);
                Thread.Sleep(App.Settings.Loop_Delay);
            }

            if (payload.Count == 0)
            {

            }

            byte[] decom = payload.ToArray();

            if (!Encoding.ASCII.GetString(decom).Contains("<EOF>"))
            {

            }
            else
            {
                int index = payload.LastIndexOf(Encoding.ASCII.GetBytes("<")[0]);
                if(index < 10)
                {

                }
                Array.Resize(ref decom, index);
            }
            if(decom.Length == 0)
            {

            }
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
            var eof = Encoding.UTF8.GetBytes("<EOF>");

            Array.Resize(ref payload, payload.Length - eof.Length);
            return payload;
        }

        static void CreateConnections()
        {
            while (ConnectedClients.Count < 5)
            {
                var newclient = new Client();
                if (!newclient.client.Connected)
                    return;
                else if (newclient.initialise)
                    ConnectedClients.Add(newclient);
            }
        }

        static object lockobj = new object();
        static Client GetAvailableConnection(CancellationToken cancellationToken)
        {
            lock (lockobj)
                while (true)
                {
                    if (ConnectedClients.Count < 5)
                        CreateConnections();
                    for (int i = 0; i < ConnectedClients.Count; i++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            return null;
                        if (!ConnectedClients[i].client.Connected)
                        {
                            ConnectedClients.Remove(ConnectedClients[i]);
                            i--;
                            continue;
                        }
                        if (!ConnectedClients[i].InUse)
                        {
                            ConnectedClients[i].InUse = true;
                            return ConnectedClients[i];
                        }
                    }
                    Thread.Sleep(2000);
                    CreateConnections();
                }

        }

        private static (string,bool) SendPost(string request, ref CancellationToken cancellationToken)
        {
            logger.Info("Sending Request.\n Request : \n {0} \n", request);
            string response = "fail", pay = string.Empty;
            bool succeeded = false;
            var payload = new List<byte>();
            Client listener = GetAvailableConnection(cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                if (listener != null)
                    listener.InUse = true;
                return (string.Empty,succeeded);
            }

            listener.client.GetStream().ReadTimeout = 30000;
            listener.client.GetStream().WriteTimeout = 30000;

            try
            {
                if (string.IsNullOrEmpty(App.Server.Info.session))
                    Connect();
                if (!string.IsNullOrEmpty(App.Server.Info.session))
                {
                    var stream = listener.client.GetStream();
                    Send(Encoding.UTF8.GetBytes(request), stream);
                    response = string.Empty;
                    var decomp = Receive(stream);
                    logger.Info("Received response from server");
                    response = Encoding.UTF8.GetString(decomp);
                }
                listener.InUse = false;

                succeeded = true;
            }
            catch (System.Exception ex)
            {
                listener.client.Client.Disconnect(false);
                listener.client.Dispose();
                ConnectedClients.Remove(listener);
                listener = null;
                logger.Error(ex, "\n Exception Caught In Net.SendPost.");

            }

            if (App.Server.GetError(response).Contains("expire"))
            {
                foreach (var client in ConnectedClients)
                    if (client != null)
                        if (client.client != null)
                            client.client.Close();
                ConnectedClients.Clear();
                Connect();
                return SendPost(request, ref cancellationToken);
            }
            else
                return (response,succeeded);
        }

        public static class RequestHandler
        {
            public static ConcurrentQueue<RequestToken> PendingRequests { get; private set; }

            public static ConcurrentQueue<RequestToken> FinishedRequests { get; private set; }

            static bool isRunning;

            static RequestHandler()
            {
                PendingRequests = new ConcurrentQueue<RequestToken>();

                FinishedRequests = new ConcurrentQueue<RequestToken>();
            }

            public static void Start()
            {
                if (isRunning)
                    return;
                isRunning = true;

                while ((PendingRequests.Count>0 || FinishedRequests.Count>0) && isRunning)
                {
                    isRunning = true;

                    if (PendingRequests.TryDequeue(out var request))
                    {

                        if (request.CancellationToken.IsCancellationRequested)
                        {
                            if (request.Mode == RequestToken.RequestMode.Locked)
                                request.RequestResetEvent.Set();
                            continue;
                        }

                        if (request.IsPaused)
                        {
                            PendingRequests.Enqueue(request);

                            continue;
                        }

                        string response = string.Empty;
                        bool succeeded = false;

                        try
                        {
                            (response, succeeded) = SendPost((string)request.Request, ref request.CancellationToken);
                            if (!succeeded)
                                request.Failed = true;
                        }
                        catch (Exception ex)
                        {
                            request.Failed = true;
                            succeeded = false;
                        }
                        if (!request.Failed)
                        {
                            request.SetResult(response);
                        }

                        FinishRequest(request);
                    }

                    List<RequestToken> PausedRequests = FinishedRequests.ToList();

                    foreach(var token in PausedRequests)
                    {
                        if(FinishedRequests.TryDequeue(out request))
                        {
                            FinishRequest(request);
                        }
                    }
                }

                isRunning = false;
            }

            static void FinishRequest(RequestToken token)
            {
                if (token.CancellationToken.IsCancellationRequested)
                    return;
                else if (token.IsPaused)
                    FinishedRequests.Enqueue(token);
                else
                if (!token.Failed)
                {
                    switch (token.Mode)
                    {
                        case RequestToken.RequestMode.Callback:
                            Task.Run(() => token.OnFinished());
                            break;
                        case RequestToken.RequestMode.Locked:
                            token.RequestResetEvent.Set();
                            break;
                    }
                }
                else if (token.Mode == RequestToken.RequestMode.Callback)
                {
                    token.OnFailed();
                }

                if (token.Mode == RequestToken.RequestMode.Locked)
                    token.RequestResetEvent.Set();

            }

            public static void Queue(RequestToken request)
            {
                PendingRequests.Enqueue(request);
            }

        }

    }
}