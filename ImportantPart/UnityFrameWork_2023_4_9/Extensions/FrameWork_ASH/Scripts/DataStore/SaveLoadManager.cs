using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    #region 路径

    /// <summary>
    /// 单例类路径 Unity存储路径/存储文件夹名字（默认DataStore/）/类型.unitysingledata
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
    #region 初始化
    public static Transform root;

    private static string secretKey;

    private static string secretVector;

    /// <summary>
    /// 存储文件夹名字
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
    #region 通过PlayerPreferences储存到注册表
    /// <summary>
    /// string,int,float类型需单独存（序列化会变空），其它的类型以字符形式序列化存储
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
    /// string,int,float类型需单独取（序列化会变空），其它的类型从字符形式反序列化读取
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
    #region 通过Json储存到本地
    #region 单例类保存

    /// <summary>
    /// 将单例类存入本地(需挂载在DataStoreRoot上)
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
        Debug.Log("[SaveLoadManager]:保存单例" + typeof(T).ToString());
    }

    #endregion

    #region 动态变量保存

    /// <summary>
    /// 将一个变量赋予名字并存入本地
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
        Debug.Log("[SaveLoadManager]:保存文件" + name);
    }


    #endregion

    #region 单例类读取

    /// <summary>
    /// 将单例类从本地读取(需挂载在DataStoreRoot上)
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
        Debug.Log("[SaveLoadManager]:读取单例" + typeof(T).ToString());
        return data;
    }

    #endregion

    #region 动态类读取

    /// <summary>
    /// 将一个赋予名字的变量从本地读取
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
        Debug.Log("[SaveLoadManager]:读取文件" + name);
        return data;
    }



    #endregion

    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region 加密设置
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
