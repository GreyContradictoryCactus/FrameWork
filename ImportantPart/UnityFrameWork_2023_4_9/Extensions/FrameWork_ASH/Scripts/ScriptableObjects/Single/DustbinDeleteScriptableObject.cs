using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

using UnityEngine;
//取消使用Odin的序列化
//using Sirenix.OdinInspector;

[Serializable]
[CreateAssetMenu(fileName = "DustbinDeleteScriptableObject", menuName = "ScriptableObject/DustbinDeleteScriptableObject", order = 0)]
public class DustbinDeleteScriptableObject : /*Serialized*/ScriptableObject
{
    //[ShowInInspector]
    /// <summary>
    /// 键：文件(object) 值：原路径(string)
    /// </summary>
    public DustbinDeleteSerializableDictionary DeleteRecord = new DustbinDeleteSerializableDictionary();

    //[ShowInInspector]
    /// <summary>
    /// 用于记录文件名，查看重名
    /// </summary>
    public List<string> ObjectsFileName = new List<string>();


}


