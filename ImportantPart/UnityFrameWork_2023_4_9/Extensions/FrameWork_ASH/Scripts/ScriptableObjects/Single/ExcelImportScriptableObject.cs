using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "ExcelImportScriptableObject", menuName = "ScriptableObject/ExcelImportScriptableObject", order = 0)]
public class ExcelImportScriptableObject : ScriptableObject
{
    [HideInInspector]
    public bool printLog = false;
    [HideInInspector]
    public string excelFilePath = "";
    [HideInInspector]
    public string excelFolderPath = "";
    [HideInInspector]
    public string full_name = "";
    [HideInInspector]
    public string value_type = "";
    [HideInInspector]
    public string defaultExcelTemporaryStoragePath ;
    [HideInInspector]
    public string exportJsonPath = "";
    [HideInInspector]
    public string exportBinPath = "";
    [HideInInspector]
    public string exportXmlPath = "";
    [HideInInspector]
    public string exportScriptableObjectPath = "";
    [HideInInspector]
    public string exportCodePath = "";
    [HideInInspector]
    public int i = 4;

    [SerializeField]
    public ImportExcelSerializableDictionary excelImportList = new ImportExcelSerializableDictionary();

    public void AddExcelFileToExcelImportList()
    {
        string parameters = full_name + "  ||  " + value_type ;
        excelImportList.Add(excelFilePath, parameters);
        

    }
    public void RemoveExcelFileFromExcelImportList()
    {

        excelImportList.Remove(excelFilePath);

    }
}
