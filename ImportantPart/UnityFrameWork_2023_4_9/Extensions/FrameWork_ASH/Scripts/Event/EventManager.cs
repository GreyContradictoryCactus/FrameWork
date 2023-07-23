using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    #region 初始化
    /// <summary>
    /// 事件参数
    /// </summary>
    public delegate void EventData(params object[] args);

    /// <summary>
    /// 事件存储
    /// </summary>
    private static Dictionary<string, EventData> Events;

    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        Events = new Dictionary<string, EventData>();
        LogManager.Info("[EventManager]:Init", "FrameworkManagerInit");

        Debug.LogWarning("[EventManager]:该类还未进行过测试！可能有BUG！");
    }

    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
    #region 事件方法
    /// <summary>
    /// 侦听事件
    /// </summary>
    public void Listen(string name, EventData action)
    {
        if (Events == null)
        {
            Debug.LogError("[EventManager]:!");
            return;
        } 
        if (Events.ContainsKey(name))
        {
            Events[name] += action;
        }
        else
        {
            Events.Add(name, action);
        }
    }

    /// <summary>
    /// 移除事件
    /// </summary>
    public void Remove(string name, EventData action)
    {
        if (Events == null) return;

        if (Events.ContainsKey(name))
        {
            Events[name] -= action;
        }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    public void Trigger(string name, params object[] args)
    {
        if (Events.ContainsKey(name))
        {
            Events[name]?.Invoke(args);
        }
    }

    /// <summary>
    /// 清空所有事件
    /// </summary>
    public void ClearAll()
    {
        Events = null;
    }

    /// <summary>
    /// 清空事件
    /// </summary>
    public void Clear(string name)
    {
        Events[name] = null;
    }


    #endregion
    /*----------------------------------------------------------------------------------------------------------------*/
}


