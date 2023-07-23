using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

public class LogControlWindow : BaseWindow
{
    static LogControlWindow window;



    string logKind = "";

    public static void CreateLogControlWindow()
    {
        window = GetWindow<LogControlWindow>(false, "日志管理", true);
        window.Show();
        BaseWindow.LimitWindowSize(400, 210, window);
    }

    private void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.background = null;    //设置背景填充
        fontStyle.normal.textColor = new Color(1, 1, 1);   //设置字体颜色
        fontStyle.fontSize = 24;       //字体大小
        fontStyle.alignment = TextAnchor.UpperCenter;
        fontStyle.wordWrap = true;

        GUILayout.BeginVertical();

        GUILayout.Space(10);
        GUILayout.Box("日志管理：", fontStyle);
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        fontStyle.alignment = TextAnchor.UpperLeft;
        fontStyle.fontSize = 17;
        GUILayout.Space(5);
        GUILayout.Box("输出：", fontStyle);
        if(GUILayout.Button("全部启用"))
        {
            LogManager.logControlScriptableObject.testAble = true;
            LogManager.logControlScriptableObject.infoAble = true;
            LogManager.logControlScriptableObject.warningAble = true;
            LogManager.logControlScriptableObject.errorAble = true;
            LogManager.logControlScriptableObject.fatalAble = true;
        }
        if (GUILayout.Button("全部禁用"))
        {
            LogManager.logControlScriptableObject.testAble = false;
            LogManager.logControlScriptableObject.infoAble = false;
            LogManager.logControlScriptableObject.warningAble = false;
            LogManager.logControlScriptableObject.errorAble = false;
            LogManager.logControlScriptableObject.fatalAble = false;
        }
        if(GUILayout.Button("定位标记文件"))
        {
            Selection.activeObject = LogManager.logControlScriptableObject;
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        fontStyle.fontSize = 15;
        if (LogManager.logControlScriptableObject.testAble) 
        {
            logKind = "Text(启用)";
        }
        else
        {
            logKind = "Text(禁用)";
        }
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        GUILayout.Box(logKind, fontStyle);
        GUILayout.FlexibleSpace();
        LogManager.logControlScriptableObject.testAble = GUILayout.Toggle(LogManager.logControlScriptableObject.testAble,GUIContent.none);
        GUILayout.EndHorizontal();

        if (LogManager.logControlScriptableObject.infoAble)
        {
            logKind = "Info(启用)";
        }
        else
        {
            logKind = "Info(禁用)";
        }
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        GUILayout.Box(logKind, fontStyle);
        GUILayout.FlexibleSpace();
        LogManager.logControlScriptableObject.infoAble = GUILayout.Toggle(LogManager.logControlScriptableObject.infoAble, GUIContent.none);
        GUILayout.EndHorizontal();

        if (LogManager.logControlScriptableObject.warningAble)
        {
            logKind = "Warning(启用)";
        }
        else
        {
            logKind = "Warning(禁用)";
        }
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        GUILayout.Box(logKind, fontStyle);
        GUILayout.FlexibleSpace();
        LogManager.logControlScriptableObject.warningAble = GUILayout.Toggle(LogManager.logControlScriptableObject.warningAble, GUIContent.none);
        GUILayout.EndHorizontal();

        if (LogManager.logControlScriptableObject.errorAble)
        {
            logKind = "Error(启用)";
        }
        else
        {
            logKind = "Error(禁用)";
        }
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        GUILayout.Box(logKind, fontStyle);
        GUILayout.FlexibleSpace();
        LogManager.logControlScriptableObject.errorAble = GUILayout.Toggle(LogManager.logControlScriptableObject.errorAble, GUIContent.none);
        GUILayout.EndHorizontal();

        if (LogManager.logControlScriptableObject.fatalAble)
        {
            logKind = "Fatal(启用)";
        }
        else
        {
            logKind = "Fatal(禁用)";
        }
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        GUILayout.Box(logKind, fontStyle);
        GUILayout.FlexibleSpace();
        LogManager.logControlScriptableObject.fatalAble = GUILayout.Toggle(LogManager.logControlScriptableObject.fatalAble, GUIContent.none);
        GUILayout.EndHorizontal();


        GUILayout.EndVertical();
    }

}