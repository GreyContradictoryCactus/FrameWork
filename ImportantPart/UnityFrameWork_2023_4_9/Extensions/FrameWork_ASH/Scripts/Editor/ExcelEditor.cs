using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine;

public class ExcelEditor : MonoBehaviour
{
    [MenuItem("Tools/Ash/Table/ImportExcel", false, 1)]
    public static void ShowImportExcelWindow()
    {
        ImportExcelWindow.CreateImportExcelWindow();
    }
}
