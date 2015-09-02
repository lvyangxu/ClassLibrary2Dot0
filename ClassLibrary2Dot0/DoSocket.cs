using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ClassLibrary2Dot0
{
    public class DoSocket
    {

        public class MySocketClass {
            public string ip { get; set; }
            public int port { get; set; }
            public int listenNum { get; set; }
            public Socket ServerSocket { get; set; }
            public Socket ClientSocket { get; set; }
            public string exceptionString { get; set; }
        }

        /// <summary>
        /// 初始化socket服务器
        /// </summary>
        /// <param name="MySocketClass1"></param>
        public void initSocketServer(MySocketClass MySocketClass1) {
            try
            {
                MySocketClass1.ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                MySocketClass1.ServerSocket.Bind(new IPEndPoint(IPAddress.Parse(MySocketClass1.ip), MySocketClass1.port));
                MySocketClass1.ServerSocket.Listen(MySocketClass1.listenNum);
                MySocketClass1.exceptionString = null;
                AsyncCallback AsyncCallback1 = new AsyncCallback(AcceptCallback);
                MySocketClass1.ServerSocket.BeginAccept(AsyncCallback1, null);
            }
            catch (Exception e)
            {
                MySocketClass1.exceptionString = e.Message;
            }
        }

        public void AcceptCallback(IAsyncResult ar) {
            Socket listener = null;
            Socket handler = null;
            try
            {
                byte[] buffer = new byte[1024];
                listener = (Socket)ar.AsyncState;
                handler = listener.EndAccept(ar);
                handler.NoDelay = true;
                object[] obj = new object[2];
                obj[0] = buffer;
                obj[1] = handler;
                handler.BeginReceive(buffer,0,buffer.Length, SocketFlags.None,new AsyncCallback(ReceiveCallback),obj);
                AsyncCallback aCallback = new AsyncCallback(AcceptCallback);
                listener.BeginAccept(aCallback, listener);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.ToString());
            }
        }

        public static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Fetch a user-defined object that contains information
                object[] obj = new object[2];
                obj = (object[])ar.AsyncState;

                // Received byte array
                byte[] buffer = (byte[])obj[0];

                // A Socket to handle remote host communication.
                Socket handler = (Socket)obj[1];

                // Received message
                string content = string.Empty;

                // The number of bytes received.
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    content += Encoding.Unicode.GetString(buffer, 0,
                        bytesRead);
                    // If message contains "<Client Quit>", finish receiving
                    if (content.IndexOf("<Client Quit>") > -1)
                    {
                        // Convert byte array to string
                        string str =
                            content.Substring(0, content.LastIndexOf("<Client Quit>"));
                        Console.WriteLine(
                            "Read {0} bytes from client.\n Data: {1}",
                            str.Length, str);

                        // Prepare the reply message
                        byte[] byteData =
                            Encoding.Unicode.GetBytes(str);

                        // Sends data asynchronously to a connected Socket
                        handler.BeginSend(byteData, 0, byteData.Length, 0,
                            new AsyncCallback(SendCallback), handler);
                    }
                    else
                    {
                        // Continues to asynchronously receive data
                        byte[] buffernew = new byte[1024];
                        obj[0] = buffernew;
                        obj[1] = handler;
                        handler.BeginReceive(buffernew, 0, buffernew.Length,
                            SocketFlags.None,
                            new AsyncCallback(ReceiveCallback), obj);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Sends data asynchronously to a connected Socket.
        /// </summary>
        /// <param name="ar">
        /// The status of an asynchronous operation
        /// </param> 
        public static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // A Socket which has sent the data to remote host
                Socket handler = (Socket)ar.AsyncState;
                // The number of bytes sent to the Socket
                int bytesSend = handler.EndSend(ar);
                Console.WriteLine(
                    "Sent {0} bytes to Client", bytesSend);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.ToString());
            }
        }

        /// <summary>
        /// 初始化socket客户端
        /// </summary>
        /// <param name="MySocketClass1"></param>
        public void initSocketClient(MySocketClass MySocketClass1) {
            try
            {
                MySocketClass1.ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                MySocketClass1.ClientSocket.Connect(IPAddress.Parse(MySocketClass1.ip), MySocketClass1.port);
                MySocketClass1.exceptionString = null;
            }
            catch (Exception e)
            {
                MySocketClass1.exceptionString = e.Message;
            }
        }

    //    byte[] bytes = new byte[1024];
    //    try
    //    {
    //        // Create one SocketPermission for socket access restrictions
    //        SocketPermission permission = new SocketPermission(
    //            NetworkAccess.Connect,    // Connection permission
    //            TransportType.Tcp,        // Defines transport types
    //            "",                       // Gets the IP addresses
    //            SocketPermission.AllPorts // All ports
    //            );

    //    // Ensures the code to have permission to access a Socket
    //    permission.Demand();

    //        // Resolves a host name to an IPHostEntry instance           
    //        IPHostEntry ipHost = Dns.GetHostEntry("");

    //    // Gets first IP address associated with a localhost
    //    IPAddress ipAddr = ipHost.AddressList[0];

    //    // Creates a network endpoint
    //    IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 4510);

    //    // Create one Socket object to setup Tcp connection
    //    Socket sender = new Socket(
    //        ipAddr.AddressFamily,// Specifies the addressing scheme
    //        SocketType.Stream,   // The type of socket 
    //        ProtocolType.Tcp     // Specifies the protocols 
    //        );

    //    sender.NoDelay = true;   // Using the Nagle algorithm

    //        // Establishes a connection to a remote host
    //        sender.Connect(ipEndPoint);
    //        Console.WriteLine("Socket connected to {0}",
    //            sender.RemoteEndPoint.ToString());

    //        // Sending message
    //        //<Client Quit> is the sign for end of data
    //        string theMessage = "Hello World!";
    //    byte[] msg = Encoding.Unicode.GetBytes(theMessage + "<Client Quit>");

    //    // Sends data to a connected Socket.
    //    int bytesSend = sender.Send(msg);

    //    // Receives data from a bound Socket.
    //    int bytesRec = sender.Receive(bytes);

    //    // Converts byte array to string
    //    theMessage = Encoding.Unicode.GetString(bytes, 0, bytesRec);

    //        // Continues to read the data till data isn't available
    //        while (sender.Available > 0)
    //        {
    //            bytesRec = sender.Receive(bytes);
    //            theMessage += Encoding.Unicode.GetString(bytes, 0, bytesRec);
    //        }
    //Console.WriteLine("The server reply: {0}", theMessage);

    //        // Disables sends and receives on a Socket.
    //        sender.Shutdown(SocketShutdown.Both);

    //        //Closes the Socket connection and releases all resources
    //        sender.Close();
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine("Exception: {0}", ex.ToString());
    //    }
    }
}
