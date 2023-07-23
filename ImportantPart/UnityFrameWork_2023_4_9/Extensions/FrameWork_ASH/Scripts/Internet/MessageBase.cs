using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MessageBase
{

    public string protoName = "";

    /// <summary>
    /// 消息处理状态
    /// </summary>
    public enum State
    {
        untreated = -1,
        success = 0,
        fail = 1

    }

    public State result = State.untreated;

    #region 编码与解码

    /// <summary>
    /// 协议体的编码
    /// </summary>
    /// <returns>编码后的协议体</returns>
    public static byte[] Encode(MessageBase messageBase)
    {
        string s = JsonUtility.ToJson(messageBase);
        return System.Text.Encoding.UTF8.GetBytes(s);
    }

    /// <summary>
    /// 协议体的解码
    /// </summary>
    /// <returns>解码后的协议体</returns>
    public static MessageBase Decode(string protoName,byte[] bytes,int offset,int count)
    {
        string s = System.Text.Encoding.UTF8.GetString(bytes,offset,count);
        MessageBase messageBase = (MessageBase)JsonUtility.FromJson(s, Type.GetType(protoName));
        return messageBase;
    }

    /// <summary>
    /// 编码协议名并加上2字节协议名长度在开头
    /// </summary>
    /// <returns>2字节的协议名长度信息+协议名的编码</returns>
    public static byte[] EncodeName(MessageBase messageBase)
    {
        byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(messageBase.protoName);
        //编码协议长度信息
        Int16 len = (Int16)nameBytes.Length;

        byte[] bytes = new byte[2+len];
        bytes[0] = (byte)(len%256);
        bytes[1] = (byte)(len / 256);

        Array.Copy(nameBytes, 0, bytes, 2, len);
        return bytes;
    }

    /// <summary>
    /// 解码码2字节协议名长度并获得协议名
    /// </summary>
    /// <returns>协议名</returns>
    public static string DecodeName(byte[] bytes,int offset,out int count)
    {
        count = 0;

        if(offset + 2 > bytes.Length)
        {
            return "";
        }
        //读取协议长度信息
        Int16 len = (Int16)((bytes[offset + 1] << 8) | bytes[offset]);

        if(len <= 0)
        {
            return "";
        }
        if(offset + 2 + len > bytes.Length)
        {
            return "";
        }

        //解码
        count = 2 + len;
        string protoName = System.Text.Encoding.UTF8.GetString(bytes, offset + 2, len);
        return protoName;




    }



    #endregion
}
