using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Linq;
// using static NetManager;


public class NetManager
{
    #region 定义（服务器通信）

    /// <summary>
    /// 定义套接字
    /// </summary>
    static Socket socket;

    /// <summary>
    /// 接受缓冲区
    /// </summary>
    static ByteArray readBuffer;

    /// <summary>
    /// 写入队列(发送用的)
    /// </summary>
    static Queue<ByteArray> writeQueue = new Queue<ByteArray>();

    /// <summary>
    /// 协议消息列表(接收用的)
    /// </summary>
    static List<MessageBase> messageList = new List<MessageBase>();

    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region 定义（心跳机制）

    /// <summary>
    /// 使用心跳？
    /// </summary>
    public static bool usingPingPong = true;

    /// <summary>
    /// 心跳间隔时间
    /// </summary>
    public static int ppInterval = 30;

    /// <summary>
    /// 最后一次发送PING的时间
    /// </summary>
    static float lastPingTime = 0;

    /// <summary>
    /// 最后一次接收PONG的时间
    /// </summary>
    static float lastPongTime = 0;

    /// <summary>
    /// 收到PONG协议时调用
    /// </summary>
    private static void OnMessagePONG(MessageBase messageBase)
    {
        lastPongTime = Time.time;
        Debug.Log("[客户端]：接收PONG");
    }

    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region 事件监听添加，删除，分发

    //事件
    public enum NetEvent
    {
        ConnectSuccess = 1,
        ConnectFailure = 2,
        ConnectClosing = 3,
    }

    //事件委托类型
    public delegate void EventListener(string err);

    //事件监听列表
    private static Dictionary<NetEvent, EventListener> eventListeners = new Dictionary<NetEvent, EventListener>();

    /// <summary>
    /// 添加事件监听
    /// </summary>
    public static void AddEventListener(NetEvent netEvent, EventListener listener)
    {

        if (eventListeners.ContainsKey(netEvent))
        {
            eventListeners[netEvent] += listener;
        }

        else
        {
            eventListeners.Add(netEvent, listener);
        }
    }

    /// <summary>
    /// 删除事件监听
    /// </summary>
    public static void ReMoveEventListener(NetEvent netEvent, EventListener listener)
    {
        if(eventListeners.ContainsKey(netEvent))
        {
            eventListeners[netEvent] -= listener;
        }

        if (eventListeners[netEvent] == null)
        {
            eventListeners.Remove(netEvent);
        }

    }

    /// <summary>
    /// 分发事件
    /// </summary>
    /// <param name="netEvent"></param>
    /// <param name="err"></param>
    private static void FireEvent(NetEvent netEvent ,string err)
    {
        if(eventListeners.ContainsKey(netEvent))
        {
            eventListeners[netEvent](err);
        }
    }

    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region 协议消息监听添加，删除，分发

    //协议消息委托类型
    public delegate void MessageListener(MessageBase messageBase);

    //协议消息监听列表
    private static Dictionary<string, MessageListener> messageListeners = new Dictionary<string, MessageListener>();

    /// <summary>
    /// 添加协议消息监听
    /// </summary>
    public static void AddMessageListener(string messageName, MessageListener listener)
    {

        if (messageListeners.ContainsKey(messageName))
        {
            messageListeners[messageName] += listener;
        }

        else
        {
            messageListeners.Add(messageName, listener);
        }
    }

    /// <summary>
    /// 删除协议消息监听
    /// </summary>
    public static void ReMoveMessageListener(string messageName, MessageListener listener)
    {
        if (messageListeners.ContainsKey(messageName))
        {
            messageListeners[messageName] -= listener;
        }

        if(messageListeners[messageName] == null)
        {
            messageListeners.Remove(messageName);
        }
    }

    /// <summary>
    /// 分发协议消息
    /// </summary>
    private static void FireMessage(string messageName, MessageBase messageBase)
    {
        if (messageListeners.ContainsKey(messageName))
        {
            messageListeners[messageName](messageBase);
        }
    }

    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region 连接
    /// <summary>
    /// 是否正在连接
    /// </summary>
    static bool isConnecting = false;

    /// <summary>
    /// 连接
    /// </summary>
    public static void Connect(string ip ,int port)
    {
        if(socket != null && socket.Connected)
        {
            Debug.Log("[客户端]：连接失败，已经连接了！");
            return;
        }
        if(isConnecting)
        {
            Debug.Log("[客户端]：连接失败,正在连接！");
            return;
        }

        //初始化成员
        InitState();

       

        //连接
        isConnecting = true;
        socket.BeginConnect(ip, port, ConnectCallBack, socket);
        


    }

    /// <summary>
    /// 连接回调
    /// </summary>
    private static void ConnectCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            FireEvent(NetEvent.ConnectSuccess, "");
            isConnecting = false;
            isClosing = false;
            Debug.Log("[客户端]：连接成功！");

            //开始接收
            socket.BeginReceive(readBuffer.bytes, readBuffer.writeIndex, readBuffer.remainingCapacity, 0, ReceiveCallBack, socket);

        }
        catch(SocketException ex)
        {
            FireEvent(NetEvent.ConnectFailure, ex.ToString());
            isConnecting = false;
        }
    }
    #endregion

    #region 关闭

    //是否正在关闭
    static bool isClosing = false;

    /// <summary>
    /// 关闭连接
    /// </summary>
    public static void Close()
    {
        //状态判断
        if(socket == null || !socket.Connected)
        {
            return;
        }
        if(isClosing)
        {
            return;
        }
        //还有数据
        if(writeQueue.Count>0)
        {
            isClosing = true;
        }
        //没有数据
        else
        {
            socket.Close();
            FireEvent(NetEvent.ConnectClosing, "");
        }

    }

    #endregion

    #region 发送

    /// <summary>
    /// 发送
    /// </summary>
    public static void Send(MessageBase messageBase)
    {
        if(socket == null || !socket.Connected)
        {
            return;
        }
        if(isClosing)
        {
            return;
        }
        if(isConnecting)
        {
            return;
        }

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

        //写入队列
        ByteArray byteArray = new ByteArray(sendBytes);





        int count = 0;
        lock(writeQueue)
        {
            writeQueue.Enqueue(byteArray);
            count = writeQueue.Count;
        }

        //发送
        if(count == 1)
        {
            socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallBack, socket);
        }




    }


    /// <summary>
    /// 发送回调
    /// </summary>
    public static void SendCallBack(IAsyncResult ar)
    {

        Socket socket = (Socket)ar.AsyncState;

        if(socket == null || !socket.Connected)
        {
            return;
        }
        int count = socket.EndSend(ar);

        ByteArray byteArray;
        lock(writeQueue)
        {
            byteArray = writeQueue.First();
        }

        byteArray.readIndex += count;
        if(byteArray.dataLength == 0)
        {
            lock(writeQueue)
            {
                writeQueue.Dequeue();
                byteArray = writeQueue.First();
            }
        }

        if(byteArray != null)
        {
            socket.BeginSend(byteArray.bytes, byteArray.readIndex, byteArray.dataLength, 0, SendCallBack, socket);
        }
        else if(isClosing)
        {
            socket.Close();
        }
    }



    #endregion

    #region 接收


    public static void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int count = socket.EndReceive(ar);
            if(count == 0)
            {
                Close();
                return;
            }
            readBuffer.writeIndex += count;


            readBuffer.CheakAndThenChangeBytes();

            OnReceiveData();

            readBuffer.CheakAndThenChangeBytes();

            socket.BeginReceive(readBuffer.bytes, readBuffer.writeIndex, readBuffer.remainingCapacity, 0, ReceiveCallBack, socket);


        }
        catch(SocketException ex)
        {
            Debug.Log("[客户端]：接收失败，错误：" + ex.ToString());
        }
    }

    public static void OnReceiveData()
    {
        if(readBuffer.dataLength < 2)
        {
            return;
        }

        //获取消息长度
        Int16 bodyLength = readBuffer.ReadInt16();

        if ( bodyLength == 0 ) return;
        if (readBuffer.dataLength < bodyLength + 2) return;

        readBuffer.readIndex += 2;

        //解析协议名
        int nameCount = 0;
        string protoName = MessageBase.DecodeName(readBuffer.bytes, readBuffer.readIndex, out nameCount);

        if(protoName == "")
        {
            return;
        }

        readBuffer.readIndex += nameCount;

        //解析协议体
        int bodyCount = bodyLength - nameCount;
        MessageBase messageBase = MessageBase.Decode(protoName,readBuffer.bytes,readBuffer.readIndex,bodyCount);
        readBuffer.readIndex += bodyCount;
        readBuffer.CheakAndThenChangeBytes();

        lock(messageList)
        {
            messageList.Add(messageBase);
        }
        messageCount++;

        if(readBuffer.dataLength > 2)
        {
            OnReceiveData();
        }


    }

    #endregion

    /*----------------------------------------------------------------------------------------------------------------*/

    #region 消息更新

    //消息列表长度
    static int messageCount = 0;

    //每一帧处理的消息量
    readonly static int MAX_MESSAGE_FIRE = 10;

    public static void MessageUpdate()
    {

        readBuffer.CheakAndThenChangeBytes();

        if(messageCount == 0)
        {
            return;
        }

        for(int i = 0 ; i < MAX_MESSAGE_FIRE; i++)
        {
            MessageBase messageBase = null;
            lock(messageList)
            {
                if(messageList.Count > 0)
                {
                    messageBase = messageList[0];
                    messageList.RemoveAt(0);
                    messageCount--;
                }
            }
            if(messageBase != null)
            {
                FireMessage(messageBase.protoName, messageBase);
            }
            else
            {
                break;
            }

        }




    }










    #endregion

    #region 心跳更新

    private static void PingPongUpdata()
    {
        if(!usingPingPong)
        {
            return;
        }

        //发送PING
        if(Time.time - lastPingTime > ppInterval)
        {
            MessagePING messagePING = new MessagePING();
            Send(messagePING);
            lastPingTime = Time.time;
            // Debug.Log("[客户端]：发送PING协议");




        }

        //检测PONG
        if(Time.time - lastPongTime > ppInterval * 4)
        {
            Close();
            Debug.Log("[客户端]：长时间未收到PONG协议，断开连接");
        }
    }



    #endregion

    #region 统一更新
    /// <summary>
    /// 统一更新
    /// </summary>
    public static void Updata()
    {
        MessageUpdate();
        PingPongUpdata();

    }



    #endregion

    /*----------------------------------------------------------------------------------------------------------------*/


    #region 初始化


    /// <summary>
    /// 初始化状态
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    private static void InitState()
    {
        //初始化socket
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.NoDelay = true;

        //初始化接收缓冲区
        readBuffer = new ByteArray();

        //初始化写入队列
        writeQueue = new Queue<ByteArray>();

        //初始化连接状态
        isConnecting = false;

        //初始化消息列表
        messageList = new List<MessageBase>();

        //初始化消息列表长度
        messageCount = 0;

        //初始化心跳时间
        lastPingTime = Time.time;
        lastPongTime = Time.time;

        //监听PONG（因为一定启用，所以在框架里启用，而不是在其他脚本）
        if(!messageListeners.ContainsKey("MessagePONG"))
        {
            AddMessageListener("MessagePONG",OnMessagePONG);
        }

    }


    #endregion


}







