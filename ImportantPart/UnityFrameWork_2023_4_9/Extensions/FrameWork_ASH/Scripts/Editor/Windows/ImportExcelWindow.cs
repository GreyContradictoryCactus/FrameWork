using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using OfficeOpenXml;
using System.IO.Pipes;
using System.Diagnostics;
using UnityEditor.VersionControl;

public class ImportExcelWindow : BaseWindow
{
    static ImportExcelWindow window;

    /// <summary>
    /// 0 导入单个excel文件 ； 1 导入文件夹中的所有excel文件
    /// </summary>
    static int importMode = 0;

    /// <summary>
    /// 0 导出为Json ； 1 导出为Bin ；2 导出为Xml ； 3 导出为ScriptableObject
    /// </summary>
    static int exportMode = 0;

    static ExcelImportScriptableObject excelImportScriptableObject = SingleScriptableObjectManager<ExcelImportScriptableObject>.Data;
    public static void CreateImportExcelWindow()
    {
        window = GetWindow<ImportExcelWindow>(false, "Excel导入", true);
        window.Show();
        excelImportScriptableObject.defaultExcelTemporaryStoragePath = Application.dataPath.Replace("/", "\\") + "\\Extensions\\FrameWork_ASH\\Plugins\\cvs\\Luban\\Configs\\Datas\\TemporaryStorage";
        // BaseWindow.LimitWindowSize(400, 210, window);
        //E:\game\FrameWork\Assets\Extensions\FrameWork_ASH\Plugins\cvs\Luban\Configs\Datas\TemporaryStorage
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
        GUILayout.Box("Excel导入", fontStyle);
        GUILayout.Space(10);

        fontStyle.alignment = TextAnchor.UpperLeft;
        fontStyle.fontSize = 17;

        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        if (GUILayout.Button("选择单个excel文件",GUILayout.MinHeight(40)))
        {
            importMode = 0;
        }
        if (GUILayout.Button("选择一个文件夹中的所有excel文件", GUILayout.MinHeight(40)))
        {
            importMode = 1;
        }
        GUILayout.Space(5);
        GUILayout.EndHorizontal();


        if(importMode == 0)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(5);
            GUILayout.Box("Excel文件路径：", fontStyle);
            excelImportScriptableObject.excelFilePath = GUILayout.TextField(excelImportScriptableObject.excelFilePath, GUILayout.MinWidth(200));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("浏览"))
            {
                string[] filtersStrings = { "Excel","xlsx" };
                excelImportScriptableObject.excelFilePath = EditorUtility.OpenFilePanelWithFilters("选择Excel文件", excelImportScriptableObject.excelFilePath, filtersStrings );
                
                //切片
                string[] splitString = excelImportScriptableObject.excelFilePath.Split('/');
                //文件名
                string fileName = splitString[splitString.Length - 1].Split('.')[0];

                excelImportScriptableObject.full_name = fileName + ".Tb" + fileName[0].ToString().ToUpper() + fileName.Substring(1);
                excelImportScriptableObject.value_type = fileName[0].ToString().ToUpper() + fileName.Substring(1);
            }
            GUILayout.Space(5);

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.Space(5);
            GUILayout.Box("全名(包含模块和名字)：", fontStyle);
            excelImportScriptableObject.full_name = GUILayout.TextField(excelImportScriptableObject.full_name, GUILayout.MinWidth(200));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Box("记录类名：", fontStyle);
            excelImportScriptableObject.value_type = GUILayout.TextField(excelImportScriptableObject.value_type, GUILayout.MinWidth(200));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        if(importMode == 1)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(5);
            GUILayout.Box("Excel文件夹路径：", fontStyle);
            excelImportScriptableObject.excelFolderPath = GUILayout.TextField(excelImportScriptableObject.excelFolderPath, GUILayout.MinWidth(200));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("浏览"))
            {
                excelImportScriptableObject.excelFolderPath = EditorUtility.OpenFolderPanel ("选择Excel文件夹", excelImportScriptableObject.excelFolderPath, "");

            }
            GUILayout.Space(5);

            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);


        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        if (GUILayout.Button("导出为Json", GUILayout.MinHeight(30)))
        {
            exportMode = 0;
        }
        if (GUILayout.Button("导出为Bin", GUILayout.MinHeight(30)))
        {
            exportMode = 1;
        }
        if (GUILayout.Button("导出为Xml", GUILayout.MinHeight(30)))
        {
            exportMode = 2;
        }
        if (GUILayout.Button("导出为ScriptableObject", GUILayout.MinHeight(30)))
        {
            exportMode = 3;
        }
        GUILayout.Space(5);
        GUILayout.EndHorizontal();

        if(exportMode == 0)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(5);
            GUILayout.Box("Json文件导出路径：", fontStyle);
            excelImportScriptableObject.exportJsonPath = GUILayout.TextField(excelImportScriptableObject.exportJsonPath, GUILayout.MinWidth(200));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("浏览"))
            {
                excelImportScriptableObject.exportJsonPath = EditorUtility.OpenFolderPanel("选择Json文件导出文件夹", excelImportScriptableObject.exportJsonPath, "");
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.Space(5);
            GUILayout.Box("Code文件导出路径：", fontStyle);
            excelImportScriptableObject.exportCodePath = GUILayout.TextField(excelImportScriptableObject.exportCodePath, GUILayout.MinWidth(200));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("浏览"))
            {
                excelImportScriptableObject.exportCodePath = EditorUtility.OpenFolderPanel("选择Code文件导出文件夹", excelImportScriptableObject.exportCodePath, "");
            }

            GUILayout.EndHorizontal();
        }

        if (exportMode == 1)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(5);
            GUILayout.Box("Bin文件导出路径：", fontStyle);
            excelImportScriptableObject.exportBinPath = GUILayout.TextField(excelImportScriptableObject.exportBinPath, GUILayout.MinWidth(200));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("浏览"))
            {
                excelImportScriptableObject.exportBinPath = EditorUtility.OpenFolderPanel("选择Bin文件导出文件夹", excelImportScriptableObject.exportBinPath, "");
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.Space(5);
            GUILayout.Box("Code文件导出路径：", fontStyle);
            excelImportScriptableObject.exportCodePath = GUILayout.TextField(excelImportScriptableObject.exportCodePath, GUILayout.MinWidth(200));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("浏览"))
            {
                excelImportScriptableObject.exportCodePath = EditorUtility.OpenFolderPanel("选择Code文件导出文件夹", excelImportScriptableObject.exportCodePath, "");
            }

            GUILayout.EndHorizontal();
        }

        if (exportMode == 2)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(5);
            GUILayout.Box("Xml文件导出路径：", fontStyle);
            excelImportScriptableObject.exportXmlPath = GUILayout.TextField(excelImportScriptableObject.exportXmlPath, GUILayout.MinWidth(200));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("浏览"))
            {
                excelImportScriptableObject.exportXmlPath = EditorUtility.OpenFolderPanel("选择Xml文件导出文件夹", excelImportScriptableObject.exportXmlPath, "");
            }

            GUILayout.EndHorizontal();
        }

        if (exportMode == 3)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(5);
            GUILayout.Box("ScriptableObject文件导出路径：", fontStyle);
            excelImportScriptableObject.exportScriptableObjectPath = GUILayout.TextField(excelImportScriptableObject.exportScriptableObjectPath, GUILayout.MinWidth(200));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("浏览"))
            {
                excelImportScriptableObject.exportScriptableObjectPath = EditorUtility.OpenFolderPanel("选择ScriptableObject文件导出文件夹", excelImportScriptableObject.exportScriptableObjectPath, "");
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.Space(5);
            GUILayout.Box("Code文件导出路径：", fontStyle);
            excelImportScriptableObject.exportCodePath = GUILayout.TextField(excelImportScriptableObject.exportCodePath, GUILayout.MinWidth(200));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("浏览"))
            {
                excelImportScriptableObject.exportCodePath = EditorUtility.OpenFolderPanel("选择Code文件导出文件夹", excelImportScriptableObject.exportCodePath, "");
            }

            GUILayout.EndHorizontal();
        }



        GUILayout.Space(20);
        

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("[添加]选择的excel文件至excel导入列表",GUILayout.MinHeight(25)))
        {
            AddExcelFileToExcelImportList();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("[移除]选择的excel文件从excel导入列表",GUILayout.MinHeight(21)))
        {
            RemoveExcelFileFromExcelImportList();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("[重新导入]所有excel文件", GUILayout.MinHeight(21)))
        {
            ImportExcel();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("[定位]导入列表文件", GUILayout.MinHeight(21)))
        {
            Selection.activeObject = excelImportScriptableObject;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button((excelImportScriptableObject.printLog ?"[开启]" : "[关闭]") + "导入时控制台显示",GUILayout.MinHeight(21)))
        {
            excelImportScriptableObject.printLog = !excelImportScriptableObject.printLog;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("关闭",GUILayout.MinHeight(21)))
        {
            window.Close();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }


    private static void CopyExcelFileToTemporaryStoragePath()
    {
        FileInfo excelFile = new FileInfo(excelImportScriptableObject.excelFilePath);
        
        //Debug.Log(excelFilePath);

        string fileName = Path.GetFileName(excelImportScriptableObject.excelFilePath);

        excelFile.CopyTo(excelImportScriptableObject.defaultExcelTemporaryStoragePath + "\\" + fileName,true);
        
        
    }

    private static void DeleteExcelFileFromTemporaryStoragePath()
    {
        FileInfo excelFile = new FileInfo(excelImportScriptableObject.excelFilePath);

        //Debug.Log(excelFilePath);

        string fileName = Path.GetFileName(excelImportScriptableObject.excelFilePath);

        File.Delete(excelImportScriptableObject.defaultExcelTemporaryStoragePath + "\\" + fileName);

    }


    private static void SetExcelConfig()
    {
        ImportExcelSerializableDictionary excelImportList = excelImportScriptableObject.excelImportList;
        int i = 4;

        FileInfo excelFile = new FileInfo(Application.dataPath.Replace("/", "\\") + "\\Extensions\\FrameWork_ASH\\Plugins\\cvs\\Luban\\Configs\\Datas\\__tables__.xlsx");
        using (ExcelPackage excel = new ExcelPackage(excelFile))
        {
            ExcelWorksheet sheet = excel.Workbook.Worksheets[1];
            foreach (string excelFilePath in excelImportList.Keys)
            {
                sheet.Cells[i, 2].Value = excelImportList[excelFilePath].Replace("  ||  ", "|").Split('|')[0];
                sheet.Cells[i, 3].Value = excelImportList[excelFilePath].Replace("  ||  ", "|").Split('|')[1];
                sheet.Cells[i, 4].Value = "TRUE";
                sheet.Cells[i, 5].Value = "TemporaryStorage\\" + Path.GetFileName(excelFilePath);
                i++;
            }
            for(int t = excelImportScriptableObject.i; t>=i;t--)
            {
                sheet.Cells[t, 2].Value = "";
                sheet.Cells[t, 3].Value = "";
                sheet.Cells[t, 4].Value = "";
                sheet.Cells[t, 5].Value = "";
            }
            excel.Save();
        }
        excelImportScriptableObject.i = i;

    }

    private static void UseBat()
    {
        string batFileName = "";
        string dataDirectory = "";
        string codeAs = "";
        switch (exportMode)
        {
            case 0:
                dataDirectory = excelImportScriptableObject.exportJsonPath.Replace("/", "\\");
                codeAs = "--gen_types code_cs_unity_json,data_json ^\r\n";
                batFileName = "gen_code_json.bat";
                break;
            case 1:
                dataDirectory = excelImportScriptableObject.exportBinPath.Replace("/", "\\");
                codeAs = "--gen_types code_cs_unity_bin,data_bin ^\r\n";
                batFileName = "gen_code_bin.bat";
                break;
            case 2:
                dataDirectory = excelImportScriptableObject.exportXmlPath.Replace("/", "\\");
                codeAs = "--gen_types data_xml ^\r\n";
                batFileName = "gen_data_xml.bat";
                break;
        }


        Directory.SetCurrentDirectory(Application.dataPath.Replace("/", "\\") + "\\Extensions\\FrameWork_ASH\\Plugins\\cvs\\Luban");
        File.Delete(Application.dataPath.Replace("/", "\\") + "\\Extensions\\FrameWork_ASH\\Plugins\\cvs\\Luban\\.cache.meta");
        Process process = new Process();
        string batFilePath = Application.dataPath.Replace("/", "\\") + "\\Extensions\\FrameWork_ASH\\Plugins\\cvs\\Luban\\" + batFileName;
        string batCommandText = 
            "set WORKSPACE=..\\..\\..\\..\\..\\..\r\n\r\n" +
            "set GEN_CLIENT=..\\Luban\\Luban.ClientServer\\Luban.ClientServer.exe\r\n" +
            "set CONF_ROOT=..\\Luban\\Configs\r\n\r\n" +
            "set OUTPUT_CODE_DIR=" + excelImportScriptableObject.exportCodePath + "\r\n" +
            "set OUTPUT_DATA_DIR="+ dataDirectory +"\r\n\r\n" +
            "%GEN_CLIENT% -j cfg --^\r\n " +
            "-d %CONF_ROOT%\\Defines\\__root__.xml ^\r\n " +
            "--input_data_dir %CONF_ROOT%\\Datas ^\r\n " +
            "--output_code_dir %OUTPUT_CODE_DIR% ^\r\n " +
            "--output_data_dir %OUTPUT_DATA_DIR% ^\r\n " +
            codeAs +
            "-s all \r\n\r\n" +
            "pause";
        File.WriteAllText(batFilePath, batCommandText);
        process.StartInfo.FileName = batFilePath;
        if (!excelImportScriptableObject.printLog)
        {
            process.StartInfo.UseShellExecute = false;//运行时隐藏dos窗口
            process.StartInfo.CreateNoWindow = true;//运行时隐藏dos窗口
        }
        process.StartInfo.Verb = "runas";//设置该启动动作，会以管理员权限运行进程
        process.Start();
        process.WaitForExit();

        Directory.SetCurrentDirectory(Application.dataPath.Replace("/", "\\").Replace("\\Assets",""));
    }

    private static void AddExcelFileToExcelImportList()
    {
        if (excelImportScriptableObject.excelFilePath == "" && importMode == 0)
        {
            return;
        }
        if (excelImportScriptableObject.excelFolderPath == "" && importMode == 1)
        {
            return;
        }
        if (importMode == 0)
        {
            CopyExcelFileToTemporaryStoragePath();
            excelImportScriptableObject.AddExcelFileToExcelImportList();
        }
        if (importMode == 1)
        {
            string excelFileFath = excelImportScriptableObject.excelFilePath;
            string full_name = excelImportScriptableObject.full_name;
            string value_type = excelImportScriptableObject.value_type;
            DirectoryInfo directoryInfo = new DirectoryInfo(excelImportScriptableObject.excelFolderPath);
            FileInfo[] excelFiles = directoryInfo.GetFiles();
            foreach (FileInfo excelFile in excelFiles)
            {
                excelImportScriptableObject.excelFilePath = excelFile.FullName.Replace("\\", "/");
                //切片
                string[] splitString = excelImportScriptableObject.excelFilePath.Split('/');
                //文件名
                string fileName = splitString[splitString.Length - 1].Split('.')[0];

                excelImportScriptableObject.full_name = fileName + ".Tb" + fileName[0].ToString().ToUpper() + fileName.Substring(1);
                excelImportScriptableObject.value_type = fileName[0].ToString().ToUpper() + fileName.Substring(1);

                CopyExcelFileToTemporaryStoragePath();
                excelImportScriptableObject.AddExcelFileToExcelImportList();
            }

            excelImportScriptableObject.excelFilePath = excelFileFath;
            excelImportScriptableObject.full_name = full_name;
            excelImportScriptableObject.value_type = value_type;
        }
    }

    private static void RemoveExcelFileFromExcelImportList()
    {
        FileInfo excelTablesFile = new FileInfo(Application.dataPath.Replace("/", "\\") + "\\Extensions\\FrameWork_ASH\\Plugins\\cvs\\Luban\\Configs\\Datas\\__tables__.xlsx");

        if (excelImportScriptableObject.excelFilePath == "" && importMode == 0)
        {
            return;
        }
        if (excelImportScriptableObject.excelFolderPath == "" && importMode == 1)
        {
            return;
        }
        if (importMode == 0)
        {
            DeleteExcelFileFromTemporaryStoragePath();
            excelImportScriptableObject.RemoveExcelFileFromExcelImportList();
            
        }
        if (importMode == 1)
        {
            string excelFileFath = excelImportScriptableObject.excelFilePath;
            string full_name = excelImportScriptableObject.full_name;
            string value_type = excelImportScriptableObject.value_type;
            DirectoryInfo directoryInfo = new DirectoryInfo(excelImportScriptableObject.excelFolderPath);
            FileInfo[] excelFiles = directoryInfo.GetFiles();
            foreach (FileInfo excelFile in excelFiles)
            {
                excelImportScriptableObject.excelFilePath = excelFile.FullName.Replace("\\", "/");
                //切片
                string[] splitString = excelImportScriptableObject.excelFilePath.Split('/');
                //文件名
                string fileName = splitString[splitString.Length - 1].Split('.')[0];

                excelImportScriptableObject.full_name = fileName + ".Tb" + fileName[0].ToString().ToUpper() + fileName.Substring(1);
                excelImportScriptableObject.value_type = fileName[0].ToString().ToUpper() + fileName.Substring(1);

                DeleteExcelFileFromTemporaryStoragePath();
                excelImportScriptableObject.RemoveExcelFileFromExcelImportList();
            }
            excelImportScriptableObject.excelFilePath = excelFileFath;
            excelImportScriptableObject.full_name = full_name;
            excelImportScriptableObject.value_type = value_type;
        }
    }
    private static void ImportExcel()
    {
        SetExcelConfig();
        UseBat();
    }
}