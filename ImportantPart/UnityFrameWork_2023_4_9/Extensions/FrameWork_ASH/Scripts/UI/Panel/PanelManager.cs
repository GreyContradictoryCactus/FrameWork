using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{

    #region 初始化

    /// <summary>
    /// 层级，决定显示遮挡关系
    /// </summary>
    public enum Layer
    {
        Top = 1,
        High = 2,
        Middle = 3,
        Low = 4,
        Bottom = 5
    }
    /// <summary>
    /// 通过层级查找层级空物体
    /// </summary>
    private static Dictionary<Layer,Transform> layers ;

    /// <summary>
    /// 面板列表
    /// </summary>
    public static Dictionary<string, BasePanel> panels;

    public static Transform root;
    public static Transform canvas;

    /// <summary>
    /// 初始化
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        layers = new Dictionary<Layer, Transform>();
        panels = new Dictionary<string, BasePanel>();
        root = GameObject.Find("GlobalRoot").transform.Find("UIRoot");
        canvas = root.Find("CanvasRoot");

        //这里单独用一个画布放面板，unity内部当画布元素改变后会刷新整个画布，（虽然我做2D游戏不用考虑性能）
        //要尽可能把频繁更新的和不频繁的UI分画布，画布尽量多，不要全部都放在一个画布上

        Transform panel = canvas.Find("PanelCanvas");

        //五个层级应该够用了吧
        layers.Add(Layer.Top, panel.Find("Top"));
        layers.Add(Layer.High, panel.Find("High"));
        layers.Add(Layer.Middle, panel.Find("Middle"));
        layers.Add(Layer.Low, panel.Find("Low"));
        layers.Add(Layer.Bottom, panel.Find("Bottom"));

        LogManager.Info("[PanelManager]:Init", "FrameworkManagerInit");
    }

    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region 面板相关
    public static void Open<T>(params Object[] information) where T:BasePanel
    {
        string name = typeof(T).ToString();

        //已经打开了（一种面板只允许存在一个）
        if (panels.ContainsKey(name))
        {
            return;
        }

        //刚开始学这个UI框架的时候感觉挺难受的，用脚本创建物体本身而不是脚本挂载在物体上，为了代码和资源分离

        //把面板类挂载在 面板Root 上，然后调用脚本
        BasePanel panel = root.gameObject.AddComponent<T>();

        //多态
        panel.OnInit();
        //基类
        panel.Init();

        panel.panelObject.transform.SetParent(layers[panel.layer],false);
        //记录
        panels.Add(name,panel);

        panel.OnShow(information);

    }

    //因为打开用了泛型，关闭不用泛型强迫症不舒服，就封装了一下
    public static void Close<T>()
    {
        string name = typeof(T).ToString();
        Close(name);

    }

    //通过记录关闭
    public static void Close(string name)
    {

        if(!panels.ContainsKey(name))
        {
            return;
        }

        //用名字拿面板
        BasePanel panel = panels[name];

        //多态
        panel.OnClose();

        panels.Remove(name);

        GameObject.Destroy(panel.panelObject);

        //删除组件
        Component.Destroy(panel);

        //话说总是增删组件对资源开销好像有点影响
        //不过我是2D游戏要什么自行车
        //可能可以改成对象池的形式，隐藏
        //但是懒得改
        //交给看到这的你了（）

    }
    #endregion

}
