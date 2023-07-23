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
        //ͬʱ��ռ�¼
        DustbinEditor.dustbinDeleteData.DeleteRecord = new DustbinDeleteSerializableDictionary();
        DustbinEditor.dustbinDeleteData.ObjectsFileName = new List<string>();


    }


    private void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.background = null;    //���ñ������
        fontStyle.normal.textColor = new Color(1, 1, 1);   //����������ɫ
        fontStyle.fontSize = 21;       //�����С
        fontStyle.alignment = TextAnchor.MiddleCenter;
        fontStyle.wordWrap = true;


        GUILayout.BeginVertical();
        GUILayout.Box("��ȷ��Ҫ�������Ͱ��\n�ò����޷�����", fontStyle);
        GUILayout.EndVertical();


        GUILayout.FlexibleSpace();


        GUILayout.BeginHorizontal();

        GUILayout.Space(10);

        if (GUILayout.Button("ȷ����", GUILayout.Width(90), GUILayout.Height(40)))
        {
            ClearDustbin();
            window.Close();
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("ȡ��",GUILayout.Width(90), GUILayout.Height(40)))
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
