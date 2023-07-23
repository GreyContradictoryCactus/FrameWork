using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例数据表
/// </summary>
public class SingleScriptableObjectManager<T> : ScriptableObject where T : ScriptableObject
{
    
    //单例：文件名和类名一致   
    private static T data;
    public static T Data
    {
        get
        {
            if (data == null)
            {
                //如果为空，首先应该去资源路径下加载对应的数据资源文件
                data = ResourceManager.LoadScriptableObject<T>();
            }
            //如果没有这个文件，直接创建一个数据
            if (data == null)
            {
                LogManager.Error("[SingleScriptableObjectManager]:找不到单例数据表：" + typeof(T).ToString(), "SingleScriptableObjectManager");
            }
            return data;
        }
    }

    public static T GetData()
    {
        return Data;
    }



}
