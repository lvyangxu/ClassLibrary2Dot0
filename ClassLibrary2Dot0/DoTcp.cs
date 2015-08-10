using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;

namespace ClassLibrary2Dot0
{
    /// <summary>
    /// 暂时使用socket类执行此功能
    /// </summary>
    public class DoTcp
    {
        public delegate void stringHandler(string result);
        public delegate void networkStreamHandler(NetworkStream NetworkStream1);

        /// <summary>
        /// 使用tcp协议监听指定的端口及ip
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns>正常返回该监听连接的对象，如果发生异常返回null</returns>
        public TcpListener startTcpListener(string ip, int port)
        {
            TcpListener TcpListener1 = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse(ip);
                TcpListener1 = new TcpListener(localAddr, port);
                TcpListener1.Start();
                return TcpListener1;
            }catch{
                return null;
            }
        }

        public void tcpClientInit(string ip, int port, stringHandler failConnectHandler, networkStreamHandler networkStreamHandler1, stringHandler succReadHandler, stringHandler failReadHandler)
        {
            TcpClient TcpClient1 = new TcpClient();
            try {          
                TcpClient1.Connect(ip, port);
                }
            catch(Exception e){
                    failConnectHandler(e.Message);
                    return;
                }

            Thread clientListenThread = new Thread(new ThreadStart(()=>{           
            NetworkStream NetworkStream1 = TcpClient1.GetStream();
            networkStreamHandler1(NetworkStream1);
            if (NetworkStream1.DataAvailable == true) { 
            readNetworkStream(NetworkStream1, "UTF-8", succReadHandler, failReadHandler);
            }
            }));
            clientListenThread.Start();
        }

        /// <summary>
        /// 建立tcp连接，当接受到挂起的请求后，返回该流
        /// </summary>
        /// <param name="TcpListener1">建立连接的TcpListener对象</param>
        /// <returns>正常返回tcp传输内容的流，出现异常则返回null</returns>
        public NetworkStream startTcpConnect(TcpListener TcpListener1)
        {
            NetworkStream NetworkStream1 = null;
            try
            { 
                TcpClient TcpClient1 = TcpListener1.AcceptTcpClient();
                NetworkStream1 = TcpClient1.GetStream();
             //   TcpClient1.Close();
                return NetworkStream1;
            }
            catch {
                return null;
            }            
        }

        /// <summary>
        /// 使用当前的NetworkStream对象,以指定编码和内容向客户端发送一条消息
        /// </summary>
        /// <param name="TcpListener1"></param>
        /// <param name="NetworkStream1"></param>
        /// <param name="encode"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string tcpSendMessage(TcpListener TcpListener1, NetworkStream NetworkStream1, string encode, string content)
        {
           try
                {
                    byte[] msg = Encoding.GetEncoding(encode).GetBytes(content);
                    NetworkStream1.Write(msg, 0, msg.Length);
                    NetworkStream1.Flush();
                    NetworkStream1.Close();
                    return "succ";
                }
                catch (Exception e)
                {
                    return e.Message;
                }   
        }

        private void readNetworkStream(NetworkStream NetworkStream1, string encode, stringHandler succReadHandler, stringHandler failReadHandler)
        {
            Byte[] bytes = new Byte[1024];
                try
                {
                    int i = NetworkStream1.Read(bytes, 0, bytes.Length);
                    while (i != 0)
                    {
                        String message = Encoding.GetEncoding(encode).GetString(bytes, 0, i);
                        succReadHandler(message);
                        i = NetworkStream1.Read(bytes, 0, bytes.Length);
                    }
                }
                catch (Exception e)
                {
                    failReadHandler(e.Message);
                }            
        }

        /// <summary>
        /// 监听tcp客户端发来的一条消息，并在收到消息后以指定的编码读取数据，调用委托处理结果
        /// </summary>
        /// <param name="TcpListener1">监听的TcpListener对象</param>
        /// <param name="encode">编码格式</param>
        /// <param name="successHandler">成功接收消息后执行的委托,参数为消息的文本内容</param>
        /// <param name="exceptionHandler">发生异常后执行的委托,参数为异常的Message内容</param>
        public void tcpReceiveMessage(TcpListener TcpListener1, NetworkStream NetworkStream1, string encode, stringHandler successHandler, stringHandler exceptionHandler)
        {
            Byte[] bytes = new Byte[1024];
            String data = null;
                try
                {
                    int i = NetworkStream1.Read(bytes, 0, bytes.Length);
                    while (i != 0)
                    {
                        data = Encoding.GetEncoding(encode).GetString(bytes, 0, i);
                        successHandler(data);
                        i = NetworkStream1.Read(bytes, 0, bytes.Length);
                    }
                }
                catch(Exception e) {
                    exceptionHandler(e.Message);
                }
              
        }

        public void listenTcpMessage(TcpListener TcpListener1, List<NetworkStream> NetworkStreamList,stringHandler succMessageHandler,stringHandler errorMessageHandler)
        {
            Thread listenThead = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    NetworkStream NetworkStream1 = startTcpConnect(TcpListener1);
                    NetworkStreamList.Add(NetworkStream1);
                    if (NetworkStream1.DataAvailable == true) { 
                    tcpReceiveMessage(TcpListener1, NetworkStream1, "UTF-8", succMessageHandler,errorMessageHandler);
                    }
                    }
            }));
            listenThead.Start();
        }


    }
}
