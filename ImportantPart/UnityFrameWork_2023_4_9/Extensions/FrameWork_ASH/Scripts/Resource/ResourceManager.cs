using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEditor;

using UnityEngine;

public class ResourceManager
{

    #region 路径
    public static string GetAssetBundlesFolderPath()
    {
        return Application.persistentDataPath + "/" + assetBundlesFolderName;
    }

    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region 初始化

    const string assetBundlesFolderName = "AssetBundles/";

    /// <summary>
    /// 加载的形式
    /// </summary>
    public enum LoadMode
    {
        Synchronous,
        Asynchronous
    }

    /// <summary>
    /// 音乐的形式
    /// </summary>
    public enum MusicKind
    {
        SoundEffect,
        BackgroundMusic
    }

    /// <summary>
    /// 预制体的种类
    /// </summary>
    public enum PrefabKind
    {
        GameObject,
        Effect,
        UI
    }


    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {

        if (!Directory.Exists(GetAssetBundlesFolderPath()))
        {
            Directory.CreateDirectory(GetAssetBundlesFolderPath());
            Debug.Log("[ResourceManager]:CreateDirectory:" + GetAssetBundlesFolderPath());
        }
        LogManager.Info("[ResourceManager]:Init", "FrameworkManagerInit");
    }
    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region 加载
    /// <summary>
    /// 暂未实现异步，默认同步
    /// 预制体种类默认GameObject
    /// </summary>
    public static GameObject LoadPrefab(string path,PrefabKind prefabKind = PrefabKind.GameObject ,LoadMode loadMode = LoadMode.Synchronous)
    {
        path = "Prefabs/" + prefabKind.ToString() + "/" +  path;

        if (loadMode == LoadMode.Synchronous) 
        {
            return Resources.Load<GameObject>(path);
        }
        Debug.LogError("[ResourceManager]:无法加载预制体,路径:"+path);
        return null;
    }
    /// <summary>
    /// 暂未实现异步，默认同步
    /// 音乐形式默认音效
    /// </summary>
    public static AudioClip LoadMusicCilp(string path,MusicKind musicKind = MusicKind.SoundEffect, LoadMode loadMode = LoadMode.Synchronous)
    {
        path = "Audio/Clip/" + musicKind.ToString()+ "/" + path;

        if (loadMode == LoadMode.Synchronous)
        {
            return Resources.Load<AudioClip>(path);
        }
        Debug.LogError("[ResourceManager]:无法加载音频,路径:" + path);
        return null;
    }

    /// <summary>
    ///  暂未实现异步，默认同步
    /// 动态类
    /// </summary>
    public static T LoadScriptableObject<T>(string path, LoadMode loadMode = LoadMode.Synchronous) where T : ScriptableObject
    {
        path = "ScriptableObjects/Dynamic/"  + path;
        if (loadMode == LoadMode.Synchronous)
        {
            return Resources.Load<T>(path);
        }
        Debug.LogError("[ResourceManager]:无法加载动态数据表,类型:" + typeof(T).ToString() + ",路径:" + path);
        return null;
    }


    /// <summary>
    ///  暂未实现异步，默认同步
    /// 单例类
    /// </summary>
    public static T LoadScriptableObject<T>(LoadMode loadMode = LoadMode.Synchronous) where T : ScriptableObject
    {
        string path = "ScriptableObjects/Single/" + typeof(T).Name;
        if (loadMode == LoadMode.Synchronous)
        {
            //return AssetDatabase.LoadAssetAtPath<T>("Assets/Extensions/FrameWork_ASH/Resources/" + path);
            return Resources.Load<T>(path);
        }
        Debug.LogError("[ResourceManager]:无法加载单例数据表,类型:" + typeof(T).ToString() + ",路径:" + path);
        return null;
    }



    /// <summary>
    /// 暂未实现异步，默认同步
    /// </summary>
    public static T LoadResource<T>(string path, LoadMode loadMode = LoadMode.Synchronous) where T:Object
    {
        if (loadMode == LoadMode.Synchronous)
        {
            return Resources.Load<T>(path);
        }
        Debug.LogError("[ResourceManager]:无法加载资源,类型:"+typeof(T).ToString()+",路径:" + path);
        return null;
    }


    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region 卸载
    public static void UnLoadResource(Object resorceToUnLoad)
    {
        Resources.UnloadAsset(resorceToUnLoad);
        Debug.Log("[ResourceManager]:卸载资源" + resorceToUnLoad.ToString());
    }

    public static void UnLoadAllUnUsedResource()
    {
        Resources.UnloadUnusedAssets();
        Debug.Log("[ResourceManager]:已卸载所有未使用资源");
    }

    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region 资产包加载
    public static AssetBundle LoadAssetBundle(string assetBundleName)
    {  
        AssetBundle assetBundle = AssetBundle.LoadFromFile(GetAssetBundlesFolderPath() + assetBundleName);
        Debug.Log("[ResourceManager]:LoadAssetBundle" + assetBundleName);
        return assetBundle;
    }



    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/


}
