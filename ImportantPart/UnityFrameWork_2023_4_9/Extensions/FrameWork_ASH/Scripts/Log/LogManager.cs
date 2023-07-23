using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogManager : MonoBehaviour
{

    public static LogControlScriptableObject logControlScriptableObject = SingleScriptableObjectManager<LogControlScriptableObject>.GetData();

    /// <summary>
    /// 测试
    /// </summary>
    public static void Test(string debugString , string mark = "default")
    {
        if(!logControlScriptableObject.markList.Contains(mark))
        {
            logControlScriptableObject.markList.Add(mark);
        }
        if( (!logControlScriptableObject.globalDisableMark.Contains(mark) && !logControlScriptableObject.textDisableMark.Contains(mark) && logControlScriptableObject.testAble) || (logControlScriptableObject.globalEnableMark.Contains(mark) && !logControlScriptableObject.textEnableMark.Contains(mark) && !logControlScriptableObject.testAble))
        {
            Debug.Log(debugString);
        }
    }

    /// <summary>
    /// 信息
    /// </summary>
    public static void Info(string debugString, string mark = "default" )
    {
        if (!logControlScriptableObject.markList.Contains(mark))
        {
            logControlScriptableObject.markList.Add(mark);
        }
        if ((!logControlScriptableObject.globalDisableMark.Contains(mark) && !logControlScriptableObject.infoDisableMark.Contains(mark) && logControlScriptableObject.infoAble) || (logControlScriptableObject.globalEnableMark.Contains(mark) && !logControlScriptableObject.infoEnableMark.Contains(mark) && !logControlScriptableObject.infoAble))
        {
            Debug.Log(debugString);
        }
    }

    /// <summary>
    /// 警告
    /// </summary>
    public static void Warning(string debugString, string mark = "default")
    {
        if (!logControlScriptableObject.markList.Contains(mark))
        {
            logControlScriptableObject.markList.Add(mark);
        }
        if ((!logControlScriptableObject.globalDisableMark.Contains(mark) && !logControlScriptableObject.warningDisableMark.Contains(mark) && logControlScriptableObject.warningAble) || (logControlScriptableObject.globalEnableMark.Contains(mark) && !logControlScriptableObject.warningEnableMark.Contains(mark) && !logControlScriptableObject.warningAble))
        {
            Debug.LogWarning(debugString);
        }
    }

    /// <summary>
    /// 错误
    /// </summary>
    public static void Error(string debugString, string mark = "default")
    {
        if (!logControlScriptableObject.markList.Contains(mark))
        {
            logControlScriptableObject.markList.Add(mark);
        }
        if ((!logControlScriptableObject.globalDisableMark.Contains(mark) && !logControlScriptableObject.errorDisableMark.Contains(mark) && logControlScriptableObject.errorAble) || (logControlScriptableObject.globalEnableMark.Contains(mark) && !logControlScriptableObject.errorEnableMark.Contains(mark) && !logControlScriptableObject.errorAble))
        {
            Debug.LogError(debugString);
        }
    }

    /// <summary>
    /// 致命错误，会自动暂停游戏
    /// </summary>
    public static void Fatal(string debugString, string mark = "default")
    {
        if (!logControlScriptableObject.markList.Contains(mark))
        {
            logControlScriptableObject.markList.Add(mark);
        }
        if ((!logControlScriptableObject.globalDisableMark.Contains(mark) && !logControlScriptableObject.fatalDisableMark.Contains(mark) && logControlScriptableObject.fatalAble) || (logControlScriptableObject.globalEnableMark.Contains(mark) && !logControlScriptableObject.fatalEnableMark.Contains(mark) && !logControlScriptableObject.fatalAble))
        {
            Debug.LogError(debugString);
            //暂停游戏
            Time.timeScale = 0;
        }
    }
}
