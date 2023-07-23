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
        

        window = GetWindow<AboutWindow>(true, "����Framework_ASH", true);
        window.Show();
        BaseWindow.LimitWindowSize(300, 510, window);

    }

    //��ʵ��Ҫ��Odin���д�������������ҵĿ�ܾ�Ҫ�������ˣ�����ֲ�Բ��ã������ǵ��õ���Ҫ�����������������ˣ�
    //��ʵ�ҳ����õİ�������unity��ı༭����չ�����һ���Ѿ����鷳����


    //UI���Ǻܻ���������ſ���
    private void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.background = null;    //���ñ������
        fontStyle.normal.textColor = new Color(1, 1, 1);   //����������ɫ
        fontStyle.fontSize = 17;       //�����С
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
                GUILayout.Box("��������֮�ң�", fontStyle);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("BiliBili"))
                {
                    Application.OpenURL("https://space.bilibili.com/1844997136/");
                }
                if (GUILayout.Button("�׻�ʦ"))
                {
                    Application.OpenURL("https://www.mihuashi.com/profiles/2658158/");
                }
                GUILayout.Space(5);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();


        GUILayout.FlexibleSpace();


        GUILayout.BeginHorizontal(); //��ʼˮƽ����
        {
            if (GUILayout.Button("�ر�"))
            {
                window.Close();
            }
        }
        GUILayout.EndHorizontal();
    }

}
