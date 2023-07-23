using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer;
using UnityEditor.PackageManager.UI;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

public class DustbinEditor : MonoBehaviour
{

    public static DustbinDeleteScriptableObject dustbinDeleteData = SingleScriptableObjectManager<DustbinDeleteScriptableObject>.GetData();

    /// <summary>
    /// 显示记录
    /// </summary>
    [MenuItem("Tools/Ash/Asset/ShowDeleteRecord #s", false, 1)]
    public static void ShowDeleteRecord()
    {
        DeleteRecordWindow.CreateDeleteRecordWindow();
    }



    /// <summary>
    /// 清空垃圾桶
    /// </summary>
    [MenuItem("Tools/Ash/Asset/ClearDustbin #c", false,0)]
    public static void ToClearDustbin()
    {
        WarnClearDustbinWindow.CreatWarnClearDustbinWindow();
    }

    /// <summary>
    /// 删除
    /// </summary>
    [MenuItem("Assets/ASH/Delete_ASH #d", false,-100)]
    public static void OverrideDelete()
    {
        //没目录创建目录
        if (!Directory.Exists("Assets/Dustbin"))
        {
            AssetDatabase.CreateFolder("Assets", "Dustbin");
            dustbinDeleteData.DeleteRecord = new DustbinDeleteSerializableDictionary();

        }

        //选中的文件
        Object deleteObject = Selection.activeObject;

        //原始路径
        string originalPath = AssetDatabase.GetAssetPath(deleteObject);

        //已经在垃圾桶中，就不管它
        if (originalPath.Contains("Assets/Dustbin/"))
        {
            Debug.LogWarning("[DustbinEditor]:该文件已经在垃圾桶中了！文件：" + deleteObject.name);
            return;
        }

        //切片
        string[] splitString = originalPath.Split('/');
        //文件名
        string fileName = splitString[splitString.Length - 1];

        //文件
        if (fileName.Contains("."))
        {

            while(dustbinDeleteData.ObjectsFileName.Contains(fileName.Replace(".", "_") + ".unityrubbish"))
            {
                Debug.LogWarning("[DustbinEditor]:重名：" + fileName);

                //文件
                fileName = fileName.Replace(".", "（repetition）.");

            }

            fileName = fileName.Replace(".", "_") + ".unityrubbish";

            //移动文件
            string errorInformation = AssetDatabase.MoveAsset(originalPath, "Assets/Dustbin/" + fileName);
            if (errorInformation != "")
            {
                Debug.LogError(errorInformation);
                return;
            }
        }
        //文件夹
        else
        {
            

            while(dustbinDeleteData.ObjectsFileName.Contains(fileName + "_folder"))
            {
                Debug.LogWarning("[DustbinEditor]:重名：" + fileName);

                fileName = fileName + "（repetition）";

            }

            fileName = fileName + "_folder";

            //移动文件
            string errorInformation = AssetDatabase.MoveAsset(originalPath, "Assets/Dustbin/" + fileName);
            if (errorInformation != "")
            {
                Debug.LogError(errorInformation);
                return;
            }

            Directory.CreateDirectory("Assets/Dustbin/" + fileName + "/cvs");
            DirectoryInfo root = new DirectoryInfo("Assets/Dustbin/" + fileName);
            foreach (FileInfo childFile in root.GetFiles())
            {
                string[] childFilePath = childFile.FullName.Split('\\');
                string sourceFileName = childFilePath[childFilePath.Length - 1];
                if (sourceFileName.Contains(".meta"))
                {
                    File.Delete(childFile.FullName);
                    continue;
                }
                File.Move(childFile.FullName, childFile.FullName.Replace(sourceFileName, "cvs\\" + sourceFileName));
            }
            foreach (DirectoryInfo childDirectory in root.GetDirectories())
            {
                string[] childDirectoryPath = childDirectory.FullName.Split('\\');
                string sourceFileName = childDirectoryPath[childDirectoryPath.Length - 1];
                if (sourceFileName == "cvs")
                {
                    continue;
                }

                Directory.Move(childDirectory.FullName, childDirectory.FullName.Replace(sourceFileName, "cvs\\" + sourceFileName));
            }

            AssetDatabase.Refresh();


        }


        //选中的文件
        deleteObject = Selection.activeObject;

        //记录
        dustbinDeleteData.DeleteRecord.Add(deleteObject, originalPath);
        dustbinDeleteData.ObjectsFileName.Add(fileName);


    }


    /// <summary>
    /// 撤回，返回错误信息
    /// </summary>
    [MenuItem("Assets/ASH/UndoDelete_ASH #f", false, -100)]

    public static string OverrideUndoDelete()
    {
        //撤回记录为空
        if (dustbinDeleteData.DeleteRecord == null)
        {
            //Debug.LogError("[DustbinEditor]:撤回记录为空！");
            return "[DustbinEditor]:撤回记录为空！";
        }
        //选中文件
        Object undoDeleteObject = Selection.activeObject;
        //路径
        string filePath = AssetDatabase.GetAssetPath(undoDeleteObject);
        //切片
        string[] splitString = filePath.Split('/');
        //文件名
        string fileName = splitString[splitString.Length - 1];


        if(fileName.Contains("_folder"))
        {
            if(Directory.Exists(filePath + "\\cvs"))
            {
                DirectoryInfo root = new DirectoryInfo(filePath + "\\cvs");
                foreach (FileInfo file in root.GetFiles())
                {

                    File.Move(file.FullName, file.FullName.Replace("\\cvs\\", "\\"));
                }
                foreach (DirectoryInfo directory in root.GetDirectories())
                {

                    if (directory.FullName.Substring(directory.FullName.Length - 3, 3) == "cvs")
                    {
                        continue;
                    }

                    Directory.Move(directory.FullName, directory.FullName.Replace("\\cvs\\", "\\"));
                }
                root.Delete();
                AssetDatabase.Refresh();
            }
            
        }

        //有记录并且在垃圾桶里
        if (dustbinDeleteData.DeleteRecord.ContainsKey(undoDeleteObject) && filePath.Contains("Assets/Dustbin/") )
        {
            string sourceFilePath = dustbinDeleteData.DeleteRecord[undoDeleteObject];
            //切片
            string[] sourceSplitString = sourceFilePath.Split('/');
            //文件名
            string sourceFileName = sourceSplitString[sourceSplitString.Length - 1];
            if(!Directory.Exists(sourceFilePath.Replace("/" + sourceFileName, "")))
            {
                return "[DustbinEditor]:路径不存在！";
            }
            DirectoryInfo root = new DirectoryInfo(sourceFilePath.Replace("/"+sourceFileName,""));           
            bool flag = true;
            int times = 0;
            if(fileName.Contains("_folder"))
            {
                while (flag && times <100)
                {
                    times++;
                    flag = false;
                    foreach (DirectoryInfo directory in root.GetDirectories())
                    {
                        //Debug.Log(directory.FullName);
                        //Debug.Log(Directory.GetCurrentDirectory()+"\\"+sourceFilePath.Replace("/", "\\"));
                        if (directory.FullName == Directory.GetCurrentDirectory()+"\\"+ sourceFilePath.Replace("/","\\"))
                        {
                            sourceFilePath = sourceFilePath + "（repetition）";
                            flag = true;
                        }

                    }
                }
            }
            else
            {
                while (flag && times < 100)
                {
                    times++;
                    flag = false;
                    foreach (FileInfo file in root.GetFiles())
                    {
                        //Debug.Log(file.FullName);
                        //Debug.Log(Directory.GetCurrentDirectory()+"\\"+sourceFilePath.Replace("/", "\\"));
                        if (file.FullName == Directory.GetCurrentDirectory() + "\\" + sourceFilePath.Replace("/", "\\"))
                        {
                            string _sourceFileName = sourceFileName.Replace(".", "（repetition）.");
                            sourceFilePath = sourceFilePath.Replace(sourceFileName, _sourceFileName);
                            flag = true;
                        }

                    }
                }
            }


            //移动
            string errorString = AssetDatabase.MoveAsset(filePath, sourceFilePath);
            if(errorString != "")
            {
                return errorString;
            }
            //删除记录，不然会重名
            dustbinDeleteData.DeleteRecord.Remove(undoDeleteObject);
            dustbinDeleteData.ObjectsFileName.Remove(fileName);
            return "";
        }

        return "[DustbinEditor]:文件没有记录或不在垃圾桶中！";
    }



}
