using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    /// <summary>
    /// 预制体路径，位于 Resources/Prefabs/UI/ [prefabPath]
    /// </summary>
    public string prefabPath;
    //面板实例
    public GameObject panelObject;
    //默认为中层
    public PanelManager.Layer layer = PanelManager.Layer.Middle;

    /// <summary>
    /// 创建预制体面板 路径为Resources/Prefabs/UI/ [prefabPath]
    /// </summary>
    public void Init()
    {
        panelObject = (GameObject)Instantiate(ResourceManager.LoadPrefab(prefabPath,ResourceManager.PrefabKind.UI));
    }


    public void Close()
    {
        string name = this.GetType().ToString();
        PanelManager.Close(name);
    }


    //用于多态
    public virtual void OnInit()
    {

    }
    public virtual void OnShow(params Object[] information)
    {

    }
    public virtual void OnClose()
    {

    }

}
