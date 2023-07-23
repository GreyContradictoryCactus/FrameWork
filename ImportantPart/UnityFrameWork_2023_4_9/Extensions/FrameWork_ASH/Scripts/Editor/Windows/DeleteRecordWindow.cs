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
        window = GetWindow<DeleteRecordWindow>(false, "删除记录", true);
        window.Show();
        //BaseWindow.LimitWindowSize(600, 710, window);
    }

    //这个用Odin写真的是非常合适（悲）
    private void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.background = null;    //设置背景填充
        fontStyle.normal.textColor = new Color(1, 1, 1);   //设置字体颜色
        fontStyle.fontSize = 20;       //字体大小
        fontStyle.alignment = TextAnchor.UpperCenter;
        fontStyle.wordWrap = true;

        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.Box("删除记录：",fontStyle);

        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);

        foreach (Object key in DustbinEditor.dustbinDeleteData.DeleteRecord.Keys)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Box("[文件路径]:" + DustbinEditor.dustbinDeleteData.DeleteRecord[key]);
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("定位"))
            {
                Selection.activeObject = key;
            }

            if (GUILayout.Button("恢复"))
            {
                Selection.activeObject = key;

                DustbinEditor.OverrideUndoDelete();

                //因为操作了DustbinEditor.DeleteRecord，当前在foreach，会报错，要退出
                break;
            }

            if(GUILayout.Button("删除"))
            {
                //路径
                string filePath = AssetDatabase.GetAssetPath(key);
                //切片
                string[] splitString = filePath.Split('/');
                //文件名
                string fileName = splitString[splitString.Length - 1];
                //删除记录，不然会重名
                DustbinEditor.dustbinDeleteData.DeleteRecord.Remove(key);
                DustbinEditor.dustbinDeleteData.ObjectsFileName.Remove(fileName);

                AssetDatabase.DeleteAsset(filePath);

                //因为操作了DustbinEditor.DeleteRecord，当前在foreach，会报错，要退出
                break;
            }

            GUILayout.EndHorizontal();

            
        }

        GUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        if(GUILayout.Button("全部恢复"))
        {
            //恢复时会出现父文件夹找不到（还未恢复）的情况而恢复失败
            //是否需要再循环？
            bool flag = true;
            //控制，防止死循环
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
