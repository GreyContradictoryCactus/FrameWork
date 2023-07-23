using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������ݱ�
/// </summary>
public class SingleScriptableObjectManager<T> : ScriptableObject where T : ScriptableObject
{
    
    //�������ļ���������һ��   
    private static T data;
    public static T Data
    {
        get
        {
            if (data == null)
            {
                //���Ϊ�գ�����Ӧ��ȥ��Դ·���¼��ض�Ӧ��������Դ�ļ�
                data = ResourceManager.LoadScriptableObject<T>();
            }
            //���û������ļ���ֱ�Ӵ���һ������
            if (data == null)
            {
                LogManager.Error("[SingleScriptableObjectManager]:�Ҳ����������ݱ�" + typeof(T).ToString(), "SingleScriptableObjectManager");
            }
            return data;
        }
    }

    public static T GetData()
    {
        return Data;
    }



}
