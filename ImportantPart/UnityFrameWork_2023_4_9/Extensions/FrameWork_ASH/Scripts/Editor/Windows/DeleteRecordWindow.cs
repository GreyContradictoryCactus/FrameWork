using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;

public class DeleteRecordWindow : BaseWindow
{
    static DeleteRecordWindow window;

    Vector2 scrollPos = new Vector2(0, 0);

    public static void CreateDeleteRecordWindow()
    {
        window = GetWindow<DeleteRecordWindow>(false, "ɾ����¼", true);
        window.Show();
        //BaseWindow.LimitWindowSize(600, 710, window);
    }

    //�����Odinд����Ƿǳ����ʣ�����
    private void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.background = null;    //���ñ������
        fontStyle.normal.textColor = new Color(1, 1, 1);   //����������ɫ
        fontStyle.fontSize = 20;       //�����С
        fontStyle.alignment = TextAnchor.UpperCenter;
        fontStyle.wordWrap = true;

        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.Box("ɾ����¼��",fontStyle);

        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);

        foreach (Object key in DustbinEditor.dustbinDeleteData.DeleteRecord.Keys)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Box("[�ļ�·��]:" + DustbinEditor.dustbinDeleteData.DeleteRecord[key]);
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("��λ"))
            {
                Selection.activeObject = key;
            }

            if (GUILayout.Button("�ָ�"))
            {
                Selection.activeObject = key;

                DustbinEditor.OverrideUndoDelete();

                //��Ϊ������DustbinEditor.DeleteRecord����ǰ��foreach���ᱨ��Ҫ�˳�
                break;
            }

            if(GUILayout.Button("ɾ��"))
            {
                //·��
                string filePath = AssetDatabase.GetAssetPath(key);
                //��Ƭ
                string[] splitString = filePath.Split('/');
                //�ļ���
                string fileName = splitString[splitString.Length - 1];
                //ɾ����¼����Ȼ������
                DustbinEditor.dustbinDeleteData.DeleteRecord.Remove(key);
                DustbinEditor.dustbinDeleteData.ObjectsFileName.Remove(fileName);

                AssetDatabase.DeleteAsset(filePath);

                //��Ϊ������DustbinEditor.DeleteRecord����ǰ��foreach���ᱨ��Ҫ�˳�
                break;
            }

            GUILayout.EndHorizontal();

            
        }

        GUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        if(GUILayout.Button("ȫ���ָ�"))
        {
            //�ָ�ʱ����ָ��ļ����Ҳ�������δ�ָ�����������ָ�ʧ��
            //�Ƿ���Ҫ��ѭ����
            bool flag = true;
            //���ƣ���ֹ��ѭ��
            int times = 0;
            while (flag && times < 100)
            {
                flag = false;
                Object[] keys = DustbinEditor.dustbinDeleteData.DeleteRecord.Keys.ToArray<Object>();
                foreach(Object key in keys)
                {
                    Selection.activeObject = key;

                    string errorInformation = DustbinEditor.OverrideUndoDelete();
                    if(errorInformation != "")
                    {
                        flag = true;
                    }

                }

                times++;
            }
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        
    }
    
}
