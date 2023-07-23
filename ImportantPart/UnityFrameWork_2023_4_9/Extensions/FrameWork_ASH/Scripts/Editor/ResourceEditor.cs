using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ResourceEditor : MonoBehaviour
{
    [MenuItem("Tools/Ash/Resource/BuildAllAssetBundles",false,50)]
    public static void BuildAllAssetBundles()
    {
        if (!Directory.Exists(ResourceManager.GetAssetBundlesFolderPath()))
        {
            ResourceManager.Init();
        }
        BuildPipeline.BuildAssetBundles(ResourceManager.GetAssetBundlesFolderPath(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        Debug.Log("[ResourceEditor]:BuildAssetBundles");
    }



}
