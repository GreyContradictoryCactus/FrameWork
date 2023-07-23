using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System;
using System.Linq;
using System.Reflection;
using System.Text;



public class NetManager
{
    #region 定义（服务器通信）

    /// <summary>
    /// 监听Socket
    /// </summary>
    public static Socket listenfd;

    /// <summary>
    /// 客户端Socket及状态信息
    /// </summary>
    public static Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();

    /// <summary>
    /// Select的检查列表
    /// </summary>
    static List<Socket> cheakRead = new List<Socket>();



    #endregion

    #region 开始循环

    public static void StartLoop(string localAddress, int listenPort)
    {
        //Socket
        listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //Bind
        IPAddress iPAddress = IPAddress.Parse(localAddress);
        IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, listenPort);
        listenfd.Bind(iPEndPoint);

        //Listen
        listenfd.Listen(0);
        Console.WriteLine("[服务器]：启动成功");

        //Loop
        while (true)
        {
            ResetCheakRead();
            Socket.Select(cheakRead, null, null, 1000);

            //检查可读对象
            for (int i = cheakRead.Count - 1; i >= 0; i--)
            {
                Socket s = cheakRead[i];
                if (s == listenfd)
                {
                    ReadListenfd(s);
                }
                else
                {
                    ReadClientfd(s);
                }
            }

            //定时器
            Timer();


        }



    }

    #endregion

    #region 接收

    /// <summary>
    /// 填充cheakRead列表
    /// </summary>
    public static void ResetCheakRead()
    {
        cheakRead.Clear();
        cheakRead.Add(listenfd);
        foreach (ClientState s in clients.Values)
        {
            cheakRead.Add(s.socket);
        }
    }

    public static void ReadListenfd(Socket listenfd)
    {
        if (IsSleepTime()) return;
        try
        {
            Socket clientfd = listenfd.Accept();
            Console.WriteLine("[服务器]：通过" + clientfd.RemoteEndPoint.ToString());
            ClientState clientState = new ClientState();
            clientState.socket = clientfd;
            clientState.lastPingTime = GetTimeStamp();
            clients.Add(clientfd, clientState);
        }
        catch (SocketException ex)
        {
            Console.WriteLine("[服务器]：通过错误" + ex.ToString());
        }
    }

    public static void ReadClientfd(Socket clientfd)
    {
        ClientState clientState = clients[clientfd];
        ByteArray readBuffer = clientState.readBuffer;

        //接收
        int count = 0;

        readBuffer.CheakAndThenChangeBytes();

        if (readBuffer.remainingCapacity <= 0)
        {
            Console.WriteLine("[服务器]：接收失败，缓冲区不足");
            Close(clientState);
            return;
        }



        try
        {
            count = clientfd.Receive(readBuffer.bytes, readBuffer.writeIndex, readBuffer.remainingCapacity, 0);

        }
        catch (SocketException sx)
        {
            Console.WriteLine("[服务器]：接收失败，错误：" + sx.ToString());
            Close(clientState);
            return;
        }

        //客户端关闭
        if (count <= 0)
        {
            Close(clientState);
            return;

        }

        //消息处理
        readBuffer.writeIndex += count;

        OnReceiveData(clientState);

        readBuffer.CheakAndThenChangeBytes();




    }

    public static void OnReceiveData(ClientState clientState)
    {
        ByteArray readBuffer = clientState.readBuffer;
        if (readBuffer.dataLength < 2)
        {
            return;
        }

        //获取消息长度
        Int16 bodyLength = readBuffer.ReadInt16();

        if (bodyLength == 0) return;
        if (readBuffer.dataLength < bodyLength + 2) return;

        readBuffer.readIndex += 2;

        //解析协议名
        int nameCount = 0;
        string protoName = MessageBase.DecodeName(readBuffer.bytes, readBuffer.readIndex, out nameCount);

        if (protoName == "")
        {
            Console.WriteLine("[服务器]：接收数据错误");
            Close(clientState);

            return;
        }

        readBuffer.readIndex += nameCount;

        //解析协议体
        int bodyCount = bodyLength - nameCount;
        MessageBase messageBase = MessageBase.Decode(protoName, readBuffer.bytes, readBuffer.readIndex, bodyCount);
        readBuffer.readIndex += bodyCount;
        readBuffer.CheakAndThenChangeBytes();

        //分发消息
        MethodInfo methodInfo = typeof(MessageHandler).GetMethod("On"+protoName);
        object[] o = { clientState, messageBase };
        Console.WriteLine("[服务器]：接收" + protoName+"，来自："+clientState.socket.RemoteEndPoint.ToString());
        if(methodInfo != null)
        {
            methodInfo.Invoke(null, o);
        }
        else
        {
            Console.WriteLine("[服务器]：分发消息失败" + protoName);
        }

        //继续读消息
        if (readBuffer.dataLength > 2)
        {
            OnReceiveData(clientState);
        }


    }

    #endregion

    #region 关闭

    public static void Close(ClientState clientState)
    {
        //事件分发
        MethodInfo methodInfo = typeof(EventHandler).GetMethod("OnClose");
        object[] ob = { clientState };
        methodInfo.Invoke(null, ob);

        //关闭
        clientState.socket.Close();
        clients.Remove(clientState.socket);

    }

    #endregion

    #region 定时器

    public static void Timer()
    {
        MethodInfo methodInfo = typeof(EventHandler).GetMethod("OnTimer");
        object[] ob = { };
        methodInfo.Invoke(null, ob);
    }

    #endregion

    #region 发送

    public static void Send(ClientState clientState,MessageBase messageBase)
    {
        if (clientState == null) return;
        if (!clientState.socket.Connected) return;

        //数据编码
        byte[] nameBytes = MessageBase.EncodeName(messageBase);
        byte[] bodyBytes = MessageBase.Encode(messageBase);
        int len = nameBytes.Length + bodyBytes.Length;
        byte[] sendBytes = new byte[2 + len];

        //组装总长度
        sendBytes[0] = (byte)(len % 256);
        sendBytes[1] = (byte)(len / 256);

        //组装名字
        Array.Copy(nameBytes, 0, sendBytes, 2, nameBytes.Length);

        //组装协议体
        Array.Copy(bodyBytes, 0, sendBytes, 2 + nameBytes.Length, bodyBytes.Length);

        
        try
        {
            clientState.socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, null, null);

        }
        catch(SocketException ex)
        {
            Console.WriteLine("[服务器]：发送失败，错误" + ex.ToString());
        }


    }


    #endregion

    #region 心跳机制

    public static long ppInterval = 30;

    public static long GetTimeStamp()
    {
        TimeSpan timeSpan = DateTime.UtcNow - new DateTime(2020, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(timeSpan.TotalSeconds);
    }


    public static void CheakPing()
    {
        long timeNow = NetManager.GetTimeStamp();

        foreach(ClientState clientState in clients.Values)
        {
            if(timeNow - clientState.lastPingTime > ppInterval * 4)
            {
                Console.WriteLine("[服务器]：长时间未收到PING协议，断开连接" + clientState.socket.RemoteEndPoint.ToString());
                Close(clientState);
                return;
            }
        }

    }




    #endregion

    #region 自动休眠维护

    static string stringStartSleepTime = "02:00";
    static string stringEndSleepTime = "04:00";
    static bool UseAutomaticSleep = true;

    public static bool IsSleepTime()
    {
        float NowTime = MyTime.StringShortTimeToFloatTime(DateTime.Now.ToShortTimeString());
        //Console.WriteLine(NowTime);
        float StartSleepTime = MyTime.StringShortTimeToFloatTime(stringStartSleepTime);
        //Console.WriteLine(StartSleepTime);
        float EndSleepTime = MyTime.StringShortTimeToFloatTime(stringEndSleepTime);
        //Console.WriteLine(EndSleepTime);
        return UseAutomaticSleep && NowTime > StartSleepTime && NowTime < EndSleepTime;
    }

    public static void Sleep()
    {
        foreach(ClientState clientState in clients.Values )
        {
            Close(clientState);
            return;
        }
    }

    public static void CheakSleep()
    {
        if(IsSleepTime())
        {
            Sleep();
        }
    }

    #endregion


}







