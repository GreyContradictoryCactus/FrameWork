using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine;

public class LogEditor : MonoBehaviour
{
    [MenuItem("Tools/Ash/Log/ShowLogControl", false, 1)]
    public static void ShowLogControl()
    {
        LogControlWindow.CreateLogControlWindow();
    }
}
