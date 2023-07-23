using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    /// <summary>
    /// Ԥ����·����λ�� Resources/Prefabs/UI/ [prefabPath]
    /// </summary>
    public string prefabPath;
    //���ʵ��
    public GameObject panelObject;
    //Ĭ��Ϊ�в�
    public PanelManager.Layer layer = PanelManager.Layer.Middle;

    /// <summary>
    /// ����Ԥ������� ·��ΪResources/Prefabs/UI/ [prefabPath]
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


    //���ڶ�̬
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
