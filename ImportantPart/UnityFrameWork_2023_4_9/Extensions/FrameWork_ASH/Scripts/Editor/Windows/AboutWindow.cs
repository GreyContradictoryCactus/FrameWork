using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AboutWindow : BaseWindow
{
    private static AboutWindow window;
    static TextAsset textFile;

    [MenuItem("Tools/Ash/Help/About FrameWork_ASH")]
    public static void CreatAboutWindow()
    {
        textFile = ResourceManager.LoadResource<TextAsset>("Framework_ASH/AboutFrameworkASH");
        

        window = GetWindow<AboutWindow>(true, "关于Framework_ASH", true);
        window.Show();
        BaseWindow.LimitWindowSize(300, 510, window);

    }

    //其实想要用Odin插件写，但是这样用我的框架就要这个插件了，可移植性不好，（考虑到用的人要买这个插件，还是算了）
    //其实我超想用的啊啊啊，unity你的编辑器扩展真的是一言难尽，麻烦死。


    //UI不是很会调，将就着看吧
    private void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.background = null;    //设置背景填充
        fontStyle.normal.textColor = new Color(1, 1, 1);   //设置字体颜色
        fontStyle.fontSize = 17;       //字体大小
        fontStyle.alignment = TextAnchor.UpperLeft;
        fontStyle.wordWrap = true;


        GUILayout.BeginVertical();
        {
            

            GUILayout.Space(5);


            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(5);
                GUILayout.Box(textFile.text, fontStyle);
                GUILayout.Space(5);                
            }
            GUILayout.EndHorizontal();


            GUILayout.Space(25);

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(5);
                GUILayout.Box("关于虚无之灰：", fontStyle);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("BiliBili"))
                {
                    Application.OpenURL("https://space.bilibili.com/1844997136/");
                }
                if (GUILayout.Button("米画师"))
                {
                    Application.OpenURL("https://www.mihuashi.com/profiles/2658158/");
                }
                GUILayout.Space(5);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();


        GUILayout.FlexibleSpace();


        GUILayout.BeginHorizontal(); //开始水平布局
        {
            if (GUILayout.Button("关闭"))
            {
                window.Close();
            }
        }
        GUILayout.EndHorizontal();
    }

}
