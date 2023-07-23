using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine;

public class BaseWindow : EditorWindow
{
    /// <summary>
    /// 固定窗口大小
    /// </summary>
    public static void LimitWindowSize(float width,float height,BaseWindow window)
    {
        Vector2 size = new Vector2(width,height);
        window.minSize = size;
        window.maxSize = size;
    }
}
