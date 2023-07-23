using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    #region ·��

    /// <summary>
    /// ������·�� Unity�洢·��/�洢�ļ������֣�Ĭ��DataStore/��/����.unitysingledata
    /// </summary>
    static string GetSinglePath<T>()
    {
        return Application.persistentDataPath + "/" + dataStoreFolderName + typeof(T).ToString() + ".unitysingledata";
    }
    static string GetDynamicFolderPath()
    {
        return Application.persistentDataPath + "/" + dataStoreFolderName ;
    }

    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region ��ʼ��
    public static Transform root;

    private static string secretKey;

    private static string secretVector;

    /// <summary>
    /// �洢�ļ�������
    /// </summary>
    const string dataStoreFolderName = "DataStore/";

    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        root = GameObject.Find("GlobalRoot").transform.Find("DataStoreRoot");
        if(!Directory.Exists(GetDynamicFolderPath()))
        {
            Directory.CreateDirectory(GetDynamicFolderPath());
            Debug.Log("[SaveLoadManager]:CreateDirectory:"+GetDynamicFolderPath());
        }
        LogManager.Info("[SaveLoadManager]:Init", "FrameworkManagerInit");

    }
    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region ͨ��PlayerPreferences���浽ע���
    /// <summary>
    /// string,int,float�����赥���棨���л����գ����������������ַ���ʽ���л��洢
    /// </summary>
    public static void SaveByPlayerPreferences<T>(string key,T _value,bool encrypt = false)
    {
        string tpye = typeof(T).ToString();
        string value = _value.ToString();
        switch (tpye)
        {
            case "System.String":
                PlayerPrefs.SetString(key, value);
                PlayerPrefs.Save();
                return;

            case "System.Single":
                PlayerPrefs.SetFloat(key, float.Parse(value));
                PlayerPrefs.Save();
                return;

            case "System.Int32":
                PlayerPrefs.SetInt(key, int.Parse(value));
                PlayerPrefs.Save();
                return;
        }
        string json = JsonUtility.ToJson(_value);
        if (encrypt)
        {
            json = Coding_ASH.AES.Encrypt(json, secretKey, secretVector);
        }
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();

    }

    /// <summary>
    /// string,int,float�����赥��ȡ�����л����գ������������ʹ��ַ���ʽ�����л���ȡ
    /// </summary>
    public static T LoadByPlayerPreferences<T>(string key,bool decrypt = false)
    {
        string tpye = typeof(T).ToString();
        switch (tpye)
        {
            case "System.String":
                return (T)(object)PlayerPrefs.GetString(key);

            case "System.Single":
                return (T)(object)PlayerPrefs.GetFloat(key);

            case "System.Int32":
                return (T)(object)PlayerPrefs.GetInt(key);
        }
        string json = PlayerPrefs.GetString(key);
        if (decrypt)
        {
            json = Coding_ASH.AES.Decrypt(json, secretKey, secretVector);
        }
        T Data = JsonUtility.FromJson<T>(json);
        return (T)(object)Data;

    }



    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region ͨ��Json���浽����
    #region �����ౣ��

    /// <summary>
    /// ����������뱾��(�������DataStoreRoot��)
    /// </summary>
    public static void SaveByJsonInLocal<T>(bool encrypt = false)
    {
        T data = root.gameObject.GetComponent<T>();
        string path = GetSinglePath<T>();
        string json = JsonUtility.ToJson(data);

        if (encrypt)
        { 
            json = Coding_ASH.AES.Encrypt(json, secretKey,secretVector); 
        }

        using (FileStream fileStream = new FileStream(path,FileMode.Create))
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
            fileStream.Write(bytes,0, bytes.Length);
        }
        Debug.Log("[SaveLoadManager]:���浥��" + typeof(T).ToString());
    }

    #endregion

    #region ��̬��������

    /// <summary>
    /// ��һ�������������ֲ����뱾��
    /// </summary>
    public static void SaveByJsonInLocal(string name , object data, bool encrypt = false)
    {
        string path = GetDynamicFolderPath() + name + ".unitydynamicdata";
        string json = JsonUtility.ToJson(data);
        if (encrypt)
        {
            json = Coding_ASH.AES.Encrypt(json, secretKey,secretVector);
        }
        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
            fileStream.Write(bytes, 0, bytes.Length);
        }
        Debug.Log("[SaveLoadManager]:�����ļ�" + name);
    }


    #endregion

    #region �������ȡ

    /// <summary>
    /// ��������ӱ��ض�ȡ(�������DataStoreRoot��)
    /// </summary>
    public static T LoadByJsonFromLocal<T>(bool decrypt = false)
    {
        T data = root.gameObject.GetComponent<T>();
        string json;
        string path = GetSinglePath<T>();
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            json = System.Text.Encoding.UTF8.GetString(bytes);
        }
        if(decrypt)
        {
            json = Coding_ASH.AES.Decrypt(json, secretKey,secretVector);
        }
        JsonUtility.FromJsonOverwrite(json, data);
        Debug.Log("[SaveLoadManager]:��ȡ����" + typeof(T).ToString());
        return data;
    }

    #endregion

    #region ��̬���ȡ

    /// <summary>
    /// ��һ���������ֵı����ӱ��ض�ȡ
    /// </summary>
    public static T LoadByJsonFromLocal<T>(string name, bool decrypt = false)
    {
        string json;
        string path = GetDynamicFolderPath() + name + ".unitydynamicdata";
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            json = System.Text.Encoding.UTF8.GetString(bytes);
        }
        if (decrypt)
        {
            json = Coding_ASH.AES.Decrypt(json, secretKey, secretVector);
        }
        T data = JsonUtility.FromJson<T>(json);
        Debug.Log("[SaveLoadManager]:��ȡ�ļ�" + name);
        return data;
    }



    #endregion

    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region ��������
    public static void SetSecretKey(string key)
    {
        secretKey = key;
    }

    public static void SetSecretVector(string vector)
    {
        secretVector = vector;
    }

    #endregion

}
