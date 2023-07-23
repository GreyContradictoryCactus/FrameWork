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
        window = GetWindow<LogControlWindow>(false, "��־����", true);
        window.Show();
        BaseWindow.LimitWindowSize(400, 210, window);
    }

    private void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.background = null;    //���ñ������
        fontStyle.normal.textColor = new Color(1, 1, 1);   //����������ɫ
        fontStyle.fontSize = 24;       //�����С
        fontStyle.alignment = TextAnchor.UpperCenter;
        fontStyle.wordWrap = true;

        GUILayout.BeginVertical();

        GUILayout.Space(10);
        GUILayout.Box("��־����", fontStyle);
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        fontStyle.alignment = TextAnchor.UpperLeft;
        fontStyle.fontSize = 17;
        GUILayout.Space(5);
        GUILayout.Box("�����", fontStyle);
        if(GUILayout.Button("ȫ������"))
        {
            LogManager.logControlScriptableObject.testAble = true;
            LogManager.logControlScriptableObject.infoAble = true;
            LogManager.logControlScriptableObject.warningAble = true;
            LogManager.logControlScriptableObject.errorAble = true;
            LogManager.logControlScriptableObject.fatalAble = true;
        }
        if (GUILayout.Button("ȫ������"))
        {
            LogManager.logControlScriptableObject.testAble = false;
            LogManager.logControlScriptableObject.infoAble = false;
            LogManager.logControlScriptableObject.warningAble = false;
            LogManager.logControlScriptableObject.errorAble = false;
            LogManager.logControlScriptableObject.fatalAble = false;
        }
        if(GUILayout.Button("��λ����ļ�"))
        {
            Selection.activeObject = LogManager.logControlScriptableObject;
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        fontStyle.fontSize = 15;
        if (LogManager.logControlScriptableObject.testAble) 
        {
            logKind = "Text(����)";
        }
        else
        {
            logKind = "Text(����)";
        }
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        GUILayout.Box(logKind, fontStyle);
        GUILayout.FlexibleSpace();
        LogManager.logControlScriptableObject.testAble = GUILayout.Toggle(LogManager.logControlScriptableObject.testAble,GUIContent.none);
        GUILayout.EndHorizontal();

        if (LogManager.logControlScriptableObject.infoAble)
        {
            logKind = "Info(����)";
        }
        else
        {
            logKind = "Info(����)";
        }
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        GUILayout.Box(logKind, fontStyle);
        GUILayout.FlexibleSpace();
        LogManager.logControlScriptableObject.infoAble = GUILayout.Toggle(LogManager.logControlScriptableObject.infoAble, GUIContent.none);
        GUILayout.EndHorizontal();

        if (LogManager.logControlScriptableObject.warningAble)
        {
            logKind = "Warning(����)";
        }
        else
        {
            logKind = "Warning(����)";
        }
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        GUILayout.Box(logKind, fontStyle);
        GUILayout.FlexibleSpace();
        LogManager.logControlScriptableObject.warningAble = GUILayout.Toggle(LogManager.logControlScriptableObject.warningAble, GUIContent.none);
        GUILayout.EndHorizontal();

        if (LogManager.logControlScriptableObject.errorAble)
        {
            logKind = "Error(����)";
        }
        else
        {
            logKind = "Error(����)";
        }
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        GUILayout.Box(logKind, fontStyle);
        GUILayout.FlexibleSpace();
        LogManager.logControlScriptableObject.errorAble = GUILayout.Toggle(LogManager.logControlScriptableObject.errorAble, GUIContent.none);
        GUILayout.EndHorizontal();

        if (LogManager.logControlScriptableObject.fatalAble)
        {
            logKind = "Fatal(����)";
        }
        else
        {
            logKind = "Fatal(����)";
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