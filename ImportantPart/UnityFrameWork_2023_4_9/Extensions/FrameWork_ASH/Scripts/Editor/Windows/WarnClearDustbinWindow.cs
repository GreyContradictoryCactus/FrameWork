using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using Codice.CM.Client.Differences;
using UnityEngine.UIElements;

public class WarnClearDustbinWindow : BaseWindow
{
    static WarnClearDustbinWindow window;

    public static void CreatWarnClearDustbinWindow()
    {
        window = GetWindow<WarnClearDustbinWindow>(true, "Warning", true);
        window.Show();
        BaseWindow.LimitWindowSize(350, 110, window);
    }


    public static void ClearDustbin()
    {
        if (!Directory.Exists("Assets/Dustbin"))
        {
            return;
        }

        Directory.Delete("Assets/Dustbin", true);
        Directory.CreateDirectory("Assets/Dustbin");
        AssetDatabase.Refresh();
        //同时清空记录
        DustbinEditor.dustbinDeleteData.DeleteRecord = new DustbinDeleteSerializableDictionary();
        DustbinEditor.dustbinDeleteData.ObjectsFileName = new List<string>();


    }


    private void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.background = null;    //设置背景填充
        fontStyle.normal.textColor = new Color(1, 1, 1);   //设置字体颜色
        fontStyle.fontSize = 21;       //字体大小
        fontStyle.alignment = TextAnchor.MiddleCenter;
        fontStyle.wordWrap = true;


        GUILayout.BeginVertical();
        GUILayout.Box("你确定要清空垃圾桶吗？\n该操作无法撤销", fontStyle);
        GUILayout.EndVertical();


        GUILayout.FlexibleSpace();


        GUILayout.BeginHorizontal();

        GUILayout.Space(10);

        if (GUILayout.Button("确定！", GUILayout.Width(90), GUILayout.Height(40)))
        {
            ClearDustbin();
            window.Close();
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("取消",GUILayout.Width(90), GUILayout.Height(40)))
        {
            window.Close();
        }

        GUILayout.Space(10);

        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.EndVertical();

    }

}
