using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class ByteArray 
{
    //默认大小
    const int DEFAULT_SIZE = 1024;

    //初始的大小
    int initSize = 0;

    //缓冲区
    public byte[] bytes;

    //读写的位置
    public int readIndex;
    public int writeIndex;

    //最小剩余容量
    public int minRemainingCapacity = 16;

    //容量
    private int capacity = 0;

    //剩余空间
    public int remainingCapacity
    {
        get { return capacity - writeIndex; }
    }

    //数据长度
    public int dataLength
    {
        get { return writeIndex - readIndex; }
    }

    //初始化
    public ByteArray(int size = DEFAULT_SIZE)
    {
        bytes = new byte[size];
        capacity = size;
        initSize = size;
        readIndex = 0;
        writeIndex = 0;
    }
    public ByteArray(byte[] defaultBytes)
    {
        bytes = defaultBytes;
        capacity = defaultBytes.Length;
        initSize = defaultBytes.Length;
        readIndex = 0;
        writeIndex = defaultBytes.Length;
    }

    /// <summary>
    /// 重设尺寸，同时也会移动数据，清除已读数据
    /// </summary>
    public void ReSize(int size)
    {
        if (size < dataLength) return;
        if (size < initSize) return;
        int n = 1;
        while (n < size) n = n * 2;
        capacity = n;
        byte[] newBytes = new byte[capacity];
        Array.Copy(bytes, readIndex, newBytes,0, dataLength);
        bytes = newBytes;
        writeIndex = dataLength;
        readIndex = 0;
        return;
    }
    /// <summary>
    /// 检查并改变数据
    /// </summary>
    public void CheakAndThenChangeBytes()
    {
        if (dataLength < 16)
        {
            MoveBytes();
            return;
        }
        if (remainingCapacity < minRemainingCapacity)
        {
            MoveBytes();
        }
        if (remainingCapacity < minRemainingCapacity)
        {
            ReSize( dataLength * 2 );
            return;
        }

        return;
    }
    /// <summary>
    /// 移动数据，清空已读数据
    /// </summary>
    public void MoveBytes()
    {
        if(dataLength > 0)
        {
            Array.Copy(bytes, readIndex, bytes, 0, dataLength);
        }
        writeIndex = dataLength;
        readIndex = 0;
        return;
    }

    /// <summary>
    /// 写入数据,把thisBytes的从offset位置开始的count个数据写入bytes
    /// </summary>
    public void Write(byte[] thisBytes,int offset,int count)
    {
        if(remainingCapacity < count)
        {
            ReSize(dataLength + count);
        }
        Array.Copy(thisBytes, offset, bytes, writeIndex, count);
        writeIndex += count;
        return ;

    }

    /// <summary>
    /// 读取数据,把bytes前面的count个数据放入到thisBytes中，从thisBytes的offset开始放入
    /// </summary>
    public void Read(byte[] thisBytes,int offset,int count)
    {
        count = Math.Min(count, dataLength); //防止越界
        Array.Copy(bytes,readIndex, thisBytes, offset, count);
        readIndex += count;
        CheakAndThenChangeBytes();
        return;
    }

    /// <summary>
    /// 读取2个代表数据长度的字节
    /// </summary>
    /// <returns>解析后代表的数据长度</returns>
    public Int16 ReadInt16()
    {
        if (dataLength < 2) return 0;
        Int16 ret = (Int16)((bytes[readIndex + 1] << 8) | bytes[readIndex]);
        // readIndex += 2;
        CheakAndThenChangeBytes();
        return ret;

    }
    /// <summary>
    /// 读取4个代表数据长度的字节
    /// </summary>
    /// <returns>解析后代表的数据长度</returns>
    public Int32 ReadInt32()
    {
        if (dataLength < 4) return 0;
        Int32 ret = (Int32)((bytes[readIndex+3] << 24) | (bytes[readIndex + 2] << 16) | (bytes[readIndex + 1] << 8) | bytes[readIndex]);
        // readIndex += 4;
        CheakAndThenChangeBytes();
        return ret;
    }







}

