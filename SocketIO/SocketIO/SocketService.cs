using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Net.Security;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using Quobject.EngineIoClientDotNet.Client;
using Quobject.EngineIoClientDotNet.Client.Transports;
using Quobject.EngineIoClientDotNet.ComponentEmitter;
using Quobject.SocketIoClientDotNet.Client;
using Socket = Quobject.SocketIoClientDotNet.Client.Socket;

namespace SocketIO
{
    public class SocketService
    {
        private Socket socket;
        private static bool ValidateCert(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public ConcurrentDictionary<string, object> EventsDictionary;

        Dictionary<string, Action<object>> EventsActions = new Dictionary<string, Action<object>>();

        private void ErrorLog(object obj)
        {
            if (obj != null)
            {
                Debug.WriteLine(obj);

            }
            Debug.WriteLine("Exception was thrown");
        }

        public string UserToken { get; set; }

        public SocketService(string server, string userToken = "5601ab2d299cfb5a7698566158fcb2bc350701b9ac77b6f72e52c260fab0da57") : this(server)
        {
            UserToken = userToken;
        }

        public SocketService(string server)
        {
            EventsDictionary = new ConcurrentDictionary<string, object>();
        System.Net.ServicePointManager.ServerCertificateValidationCallback = ValidateCert;
            var options = new IO.Options()
            {
                AutoConnect = true,
                ExtraHeaders = new Dictionary<string, string>() {{"Authorization", UserToken}},
                IgnoreServerCertificateValidation = true,
                Reconnection = true,
                ReconnectionDelay = 10,

            };
            //Transports = ImmutableList.Create<string>( Polling.NAME)

            socket = IO.Socket(server, options);
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                
                socket.Emit("/v1/login");


                

            });
            socket.On(Socket.EVENT_DISCONNECT, () => Debug.WriteLine("Disconnecting"));
            socket.On(
                Socket.EVENT_ERROR, ErrorLog);
            socket.On("hi", (data) =>
            {
                Debug.WriteLine(data);
                socket.Disconnect();
            });

            socket.On(Socket.EVENT_MESSAGE, (data) =>
            {
                Debug.WriteLine("Message recieved");
            });
            socket.On(Socket.EVENT_CONNECT_ERROR, ErrorLog);
            socket.On(Socket.EVENT_RECONNECT_ATTEMPT, () => Debug.WriteLine("Attempting reconnect"));
            socket.On(Socket.EVENT_RECONNECT_ERROR, ErrorLog);
            socket.On(Socket.EVENT_RECONNECT_FAILED, () => Debug.WriteLine("Reconnect failed"));
            //socket.On(Socket, n() {
            //    public void call(Object...args)
            //    {
            //    Transport transport = (Transport)args[0];
            //    // Adding headers when EVENT_REQUEST_HEADERS is called
            //    transport.on(Transport.EVENT_REQUEST_HEADERS, new Emitter.Listener() {
            //        @Override
            //        public void call(Object...args)
            //        {
            //        Log.v(TAG, "Caught EVENT_REQUEST_HEADERS after EVENT_TRANSPORT, adding headers");
            //        Map<String, List<String>> mHeaders = (Map<String, List<String>>)args[0];
            //        mHeaders.put("Authorization", Arrays.asList("Basic bXl1c2VyOm15cGFzczEyMw=="));
            //    }
            //});
            //}
            //});
        }

        protected void MapEventsActions()
        {
            if (socket != null)
                foreach (var eventsAction in EventsActions)
                {
                    socket.On(eventsAction.Key, o =>
                    {
                        Debug.WriteLine($"Socket Event {eventsAction.Key} ");
                        eventsAction.Value?.Invoke(o);
                    });
                }
        }

        public void Connect()
        {
            MapEventsActions();
            try
            {
                Debug.WriteLine("Connecting");
                //socket?.Open();
                Debug.WriteLine("Connected");


            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void Disconnect()
        {
            socket?.Close();
        }
    }
}