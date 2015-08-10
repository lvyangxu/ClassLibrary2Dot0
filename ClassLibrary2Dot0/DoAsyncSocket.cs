using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ClassLibrary2Dot0
{
    public class DoAsyncSocket
    {
        DecodeAndEncode DecodeAndEncode1 = new DecodeAndEncode();
        DoJson DoJson1 = new DoJson();
        Regex Regex1 = new Regex("{[^{}]*}");


        /// <summary>
        /// 新建一个Socket的服务器，并监听该端口
        /// </summary>
        /// <param name="ip">本机ip</param>
        /// <param name="port">机端口号</param>
        /// <param name="listenNum">允许的最大客户端连接数</param>
        /// <returns>返回一个2维字符串数组，第一个元素为连接成功后的socket对象，第二个元素为异常的string内容，两者互斥必有一个为null</returns>
        private object[] startSocketServer(string ip,int port,int listenNum)
        {
            object[] result = new object[2] { null, null };
            Socket Socket1 = null;
            try
            {
                Socket1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket1.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
                Socket1.Listen(listenNum);
                result[0] = Socket1;
            }
            catch (Exception e)
            {
                result[1] = e.Message;
            }
            return result;
        }

        /// <summary>
        /// 向socket服务器请求连接，执行socket.connect()方法
        /// </summary>
        /// <param name="ip">服务器ip</param>
        /// <param name="port">服务器端口</param>
        /// <returns>返回一个2维字符串数组，第一个元素为连接成功后的socket对象，第二个元素为异常的string内容，两者互斥必有一个为null</returns>
        private object[] connectToSocketServer(string ip, int port)
        {
            object[] result = new object[2] { null, null };
            Socket Socket1 = null;
            try
            {
                Socket1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket1.Connect(IPAddress.Parse(ip), port);
                result[0] = Socket1;
            }
            catch (Exception e)
            {
                result[1] = e.Message;
            }
            return result;
        }

        /// <summary>
        /// 以指定编码向socket对象发送消息，将内容base64编码后以json串{"message":"content"}发送
        /// </summary>
        /// <param name="Socket1">socket对象</param>
        /// <param name="encode">编码格式</param>
        /// <param name="content">发送的内容</param>
        /// <returns>成功返回succ，否则返回异常信息</returns>
        public string sendMessageToSocket(Socket Socket1, string encode, string content)
        {
            try
            {
                content = DecodeAndEncode1.base64Encode(content, "UTF-8")[0];
                content = "{\"message\":\"" + content + "\"}";
                for (int i = 0; i <= content.Length / 1024; i++)
                {
                    int end = (i == content.Length / 1024) ? content.Length % 1024 : 1024;
                    if (end == 0) {
                        break;
                    }
                    string temp = content.Substring(i * 1024, end);                    
                    Byte[] Byte1 = Encoding.GetEncoding(encode).GetBytes(temp);
                    Socket1.Send(Byte1, SocketFlags.None);
                }
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// 从socket对象接收消息
        /// </summary>
        /// <param name="Socket1">socket对象</param>
        /// <param name="encode">编码</param>
        /// <returns>返回一个2维字符串数组，第一个元素为消息内容，第二个元素为异常内容，两者互斥必有一个为null</returns>
        private string[] receiveMessageFromSocket(List<Socket> ClientSocketList,Socket Socket1, string encode)
        {
            string[] result = new string[2] { null, null };
            Byte[] Byte1 = new Byte[1024];
            try
            {
                int receiveLength = Socket1.Receive(Byte1, SocketFlags.None);
                if (receiveLength <= 0) {
                    Socket1.Close();
                    ClientSocketList.Remove(Socket1);
                    return result;
                }
                result[0] = Encoding.GetEncoding(encode).GetString(Byte1);
            }
            catch (Exception e)
            {
                result[1] = e.Message;
            }
            return result;
        }

        /// <summary>
        /// 从socket对象接收消息
        /// </summary>
        /// <param name="Socket1">socket对象</param>
        /// <param name="encode">编码</param>
        /// <returns>返回一个2维字符串数组，第一个元素为消息内容，第二个元素为异常内容，两者互斥必有一个为null</returns>
        private string[] receiveMessageFromSocket(Socket Socket1, string encode)
        {
            string[] result = new string[2] { null, null };
            Byte[] Byte1 = new Byte[1024];
            try
            {
                int receiveLength = Socket1.Receive(Byte1, SocketFlags.None);
                if (receiveLength <= 0)
                {
                    Socket1.Close();
                    return result;
                }
                result[0] = Encoding.GetEncoding(encode).GetString(Byte1);
            }
            catch (Exception e)
            {
                result[1] = e.Message;
            }
            return result;
        }


        /// <summary>
        /// 在socket客户端新开一个线程,监听来自服务器的socket的消息
        /// </summary>
        /// <param name="Socket1">socket对象</param>
        /// <param name="encode">编码</param>
        /// <param name="threadHandler1">操作当前的线程委托,参数为thread</param>
        /// <param name="stringHandler1">接收消息失败时执行的委托,参数为string</param>
        /// <param name="stringHandler2">收到消息后执行的委托,参数为string</param>
        public void clentListenSocketMessage(Socket Socket1, string encode, threadHandler threadHandler1, stringHandler stringHandler1, stringHandler stringHandler2)
        {
            Thread Thread1 = new Thread(new ThreadStart(() =>
            {
                threadHandler1(Thread.CurrentThread);
                string leaveMessage = string.Empty;
                while (true)
                {

                    string[] result = receiveMessageFromSocket(Socket1, encode);
                    //如果socket连接已断开,则终止监听该消息的线程
                    if (Socket1.Connected == false)
                    {
                        Thread.CurrentThread.Abort();
                        break;
                    }

                    if (result[0] == null)
                    {
                        stringHandler1(result[1]);
                    }
                    else
                    {
                        leaveMessage = leaveMessage + result[0];
                        Match Match1 = Regex1.Match(leaveMessage);
                        //消息未发送完成
                        if (Match1.Value == string.Empty)
                        {

                        }
                        //有完整的一条消息
                        else {
                            string decodeMessage = getMessageFromMyJson(Match1.Value);
                            leaveMessage = leaveMessage.Substring(Match1.Value.Length, leaveMessage.Length - Match1.Value.Length);
                            stringHandler2(decodeMessage);
                        }
                        

                        
                    }
                }
            }));
            Thread1.Start();
        }

        /// <summary>
        /// 新开一个线程监听socket的消息
        /// </summary>
        /// <param name="SocketList1">socket对象集合</param>
        /// <param name="encode">编码</param>
        /// <param name="currentThreadHandler">操作当前的线程委托,参数为thread</param>
        /// <param name="stringHandler1">接收消息失败时执行的委托,参数为string</param>
        /// <param name="stringHandler2">收到消息后执行的委托,参数为string</param>
        public void listenSocketMessage(List<Socket> ClientSocketList, Socket Socket1, string encode, threadHandler currentThreadHandler, stringHandler stringHandler1, stringHandler stringHandler2)
        {
            Thread Thread1 = new Thread(new ThreadStart(() =>
            {
                currentThreadHandler(Thread.CurrentThread);

                Thread receiveThread = new Thread(new ThreadStart(() =>
                {
                    string leaveMessage = string.Empty;
                    while (true)
                    {

                        string[] result = receiveMessageFromSocket(ClientSocketList,Socket1, encode);
                        //如果socket连接已断开,则终止监听该消息的线程
                        if (Socket1.Connected == false)
                        {
                            Thread.CurrentThread.Abort();
                            ClientSocketList.Remove(Socket1);
                            break;
                        }

                        if (result[0] == null)
                        {
                            stringHandler1(result[1]); 
                        }
                        else
                        {
                            leaveMessage = leaveMessage + result[0];
                            Match Match1 = Regex1.Match(leaveMessage);
                            //消息未发送完成
                            if (Match1.Value == string.Empty)
                            {

                            }
                            //有完整的一条消息
                            else
                            {
                                string decodeMessage = getMessageFromMyJson(Match1.Value);
                                leaveMessage = leaveMessage.Substring(Match1.Value.Length, leaveMessage.Length - Match1.Value.Length);
                                stringHandler2(decodeMessage);
                            }
                        }
                    }
                }));
                receiveThread.Start();
                
                
            }));
            Thread1.Start();
        }

        /// <summary>
        /// 新开一个线程管理socket服务器收到的客户端连接
        /// </summary>
        /// <param name="ClientSocketList">所有客户端连接的集合</param>
        /// <param name="StartSocket">服务器监听端口的socket</param>
        /// <param name="manageThread">新开的管理线程</param>
        private void manageConnectClient(List<Socket> ClientSocketList, Socket StartSocket, threadHandler manageThread, threadHandler listenThreadHandler, stringHandler errorMessageHandler, stringHandler succMessageHandler)
        {
            Thread t = new Thread(new ThreadStart(() =>
            {
                manageThread(Thread.CurrentThread);
                while (true)
                {
                    Socket ClientSocket = StartSocket.Accept();
                    ClientSocketList.Add(ClientSocket);
                    listenSocketMessage(ClientSocketList,ClientSocket, "UTF-8", listenThreadHandler, errorMessageHandler, succMessageHandler);
                }
            }));
            t.Start();
        }

        /// <summary>
        /// 以指定ip和端口初始化客户端的socket连接，连接成功后会新增一个线程监听服务器的消息
        /// </summary>
        /// <param name="ip">服务器ip</param>
        /// <param name="port">服务器端口</param>
        /// <param name="connectFailHanlder">socket连接失败时执行的委托,参数为string</param>
        /// <param name="currentSocketHandler">socket连接成功时执行的委托,参数为socket</param>
        /// <param name="listenThreadHandler">监听消息线程的委托,参数为thread</param>
        /// <param name="errorMessageHandler">socket接收消息失败时执行的委托,参数为string</param>
        /// <param name="succMessageHandler">初始化成功后,接收到消息时执行的委托,参数为string</param>
        public void socketClientInit(string ip, int port, string encode, stringHandler connectFailHanlder, socketHandler currentSocketHandler, threadHandler listenThreadHandler, stringHandler errorMessageHandler, stringHandler succMessageHandler)
        {
            object[] object1 = connectToSocketServer(ip, port);
            Socket ClientSocket = (Socket)object1[0];
            //判断建立连接是否失败
            if (ClientSocket == null)
            {
                connectFailHanlder((string)object1[1]);
                return;
            }

            //建立连接成功后返回该socket对象,用于发送和接收消息
            currentSocketHandler(ClientSocket);

            //使用该socket对象监听服务器链接
            clentListenSocketMessage(ClientSocket, encode,
                (listenMessageThread) =>
                {
                    listenThreadHandler(listenMessageThread);
                },
                (receiveExc) =>
                {
                    errorMessageHandler(receiveExc);
                },
            (result) =>
            {
                succMessageHandler(result);
            });
        }

        /// <summary>
        /// 以指定的本机端口初始化服务器socket链接，并分别新开1个所有客户端的socket管理线程和1个所有客户端的socket消息监听线程
        /// </summary>
        /// <param name="ip">本机ip</param>
        /// <param name="port">机端口号</param>
        /// <param name="listenNum">允许的最大客户端连接数</param>
        /// <param name="listenFailHandler">监听端口失败时执行的委托</param>
        /// <param name="manageThreadHandler">管理客户端连接的所有socket集合的线程</param>
        /// <param name="listenThreadHandler">监听客户端消息的线程</param>
        /// <param name="errorMessageHandler">监听消息失败时执行的委托</param>
        /// <param name="succMessageHandler">收到客户端消息时执行的委托</param>
        /// <param name="ClientSocketList">客户端连接的所有socket集合的对象</param>
        public void socketServerInit(string ip, int port, int listenNum, stringHandler listenFailHandler, socketHandler socketHandler1, threadHandler manageThreadHandler, threadHandler listenThreadHandler, stringHandler errorMessageHandler, stringHandler succMessageHandler, List<Socket> ClientSocketList)
        {
            object[] object1 = startSocketServer(ip, port, listenNum);
            Socket StartSocket = (Socket)object1[0];
            //监听端口失败
            if (StartSocket == null)
            {
                listenFailHandler((string)object1[1]);
                return;
            }
            socketHandler1(StartSocket);
            manageConnectClient(ClientSocketList, StartSocket, manageThreadHandler, listenThreadHandler, errorMessageHandler, succMessageHandler);


        }

        /// <summary>
        /// 解码socket传来的消息内容
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string getMessageFromMyJson(string message){
            string result = null;
            string[] jsonResult = DoJson1.getValueByNameFromJsonStr(message, "message");
            if (jsonResult[1] != null) {
                return result;
            }
            result = jsonResult[0];
            result = (string)DecodeAndEncode1.base64Decode(result,"UTF-8")[0];
            return result;
        }

        public delegate void stringHandler(string result);
        public delegate void socketHandler(Socket Socket1);
        public delegate void threadHandler(Thread Thread1);

    }
}
