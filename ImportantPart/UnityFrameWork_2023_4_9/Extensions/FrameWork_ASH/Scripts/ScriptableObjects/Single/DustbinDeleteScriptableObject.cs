using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

using UnityEngine;
//ȡ��ʹ��Odin�����л�
//using Sirenix.OdinInspector;

[Serializable]
[CreateAssetMenu(fileName = "DustbinDeleteScriptableObject", menuName = "ScriptableObject/DustbinDeleteScriptableObject", order = 0)]
public class DustbinDeleteScriptableObject : /*Serialized*/ScriptableObject
{
    //[ShowInInspector]
    /// <summary>
    /// �����ļ�(object) ֵ��ԭ·��(string)
    /// </summary>
    public DustbinDeleteSerializableDictionary DeleteRecord = new DustbinDeleteSerializableDictionary();

    //[ShowInInspector]
    /// <summary>
    /// ���ڼ�¼�ļ������鿴����
    /// </summary>
    public List<string> ObjectsFileName = new List<string>();


}


